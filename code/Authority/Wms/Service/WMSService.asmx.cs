using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using THOK.Security;
using System.Transactions;
using System.Xml;

namespace Wms.Service
{
    /// <summary>
    /// WMSBillService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://www.skyseaok.com/webservices/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class WMSService : System.Web.Services.WebService
    {
        [Dependency]
        public IBillMasterService BillMasterService { get; set; }
        [Dependency]
        public IBillDetailService BillDetailService { get; set; }
        [Dependency]
        public IContractService ContractService { get; set; }
        [Dependency]
        public INavicertService NavicertService { get; set; }

        #region 常量
        private const string returnMsg = @"<?xml version='1.0' encoding='GBK'>
                                            <dataset>
	                                            <head>
	                                                <msg_id>{0}</msg_id>
		                                            <state_code>{1}</state_code>
		                                            <state_desc>{2}</state_desc>
		                                            <ws_mark>{3}</ws_mark>
		                                            <ws_method>{4}</ws_method>
		                                            <ws_param>{5}</ws_param>
		                                            <curr_time>{6}</curr_time>
		                                            <curr_user>{7}</curr_user>
	                                            </head>
                                            </dataset>";
        #endregion

        [WebMethod]
        public string WMSBillService(string xml)
        {
            #region XML
            xml = @"<?xml version='1.0' encoding='GBK'?>
                    <dataset>
                        <head>
                            <msg_id>000</msg_id>
                            <state_code>000</state_code>
                            <state_desc>状态描述</state_desc>
	                        <ws_mark>WMSBillService</ws_mark>
	                        <ws_method>BillCreate</ws_method>
	                        <ws_param>1</ws_param>
                            <curr_time>2013-04-26 09:32:29</curr_time>
	                        <curr_user>2</curr_user>
                        </head>
                        <datalist>
	                        <data>
                                <bb_type>1111</bb_type>
　　　　　                      <bb_uuid>1</bb_uuid>
	                            <bb_input_date>2013-04-24 16:04:50</bb_input_date>
	                            <bb_input_person>1</bb_input_person>
	                            <bb_oper_date>2013-04-24</bb_oper_date>
	                            <bb_cig_type>0</bb_cig_type>
	                            <bb_commerce_code>1</bb_commerce_code>
                                <bb_flow_code>1</bb_flow_code>
	                            <bb_flow_type>0</bb_flow_type>
                                <BB_STATE>0</BB_STATE>
                                <list>
	                                <data_1>
		                                <bd_pcig_code>1</bd_pcig_code>
		                                <bd_bcig_code>1</bd_bcig_code>
		                                <bd_bill_all_num1>1001</bd_bill_all_num1>
		                                <bd_bill_fixed_all_num1>1002</bd_bill_fixed_all_num1>
　　　　　　　                          <BD_BILL_ALL_SCAN_NUM1>1003</BD_BILL_ALL_SCAN_NUM1>
	                                </data_1>
	                                <data_1>
                                        <bd_pcig_code>pcig_code</bd_pcig_code>
		                                <bd_bcig_code>bcig_code</bd_bcig_code>
		                                <bd_bill_all_num1>2001</bd_bill_all_num1>
		                                <bd_bill_fixed_all_num1>2002</bd_bill_fixed_all_num1>
　　　　　　　                          <BD_BILL_ALL_SCAN_NUM1>2003</BD_BILL_ALL_SCAN_NUM1>
	                                </data_1>
                                </list>
                                <CONTRACT_LIST>
                                    <CONTRACT_BASE_1>
                                        <CONTRACT_NO>01</CONTRACT_NO>
                                        <A_NO>1</A_NO>
                                        <B_NO>2</B_NO>
                                        <CONTRACT_DATE>2013-04-26 09:13:57</CONTRACT_DATE>
                                        <START_DATE>2013-04-26 09:14:03</START_DATE>
                                        <END_DATE>2013-04-26 09:14:09</END_DATE>
                                        <SEND_CODE>002</SEND_CODE>
                                        <SEND_ADD>2121</SEND_ADD>
                                        <RECEIVE_CODE>65421</RECEIVE_CODE>
                                        <RECEIVE_ADD>1212</RECEIVE_ADD>
                                        <SALE_DATE>2013-04-26 09:14:33</SALE_DATE>
                                        <USE_STATE>1</USE_STATE>
                                        <CONTRACT_DETAIL_1>
                                            <CONTRACT_NO>01</CONTRACT_NO>
                                            <BRAND_CODE>24545</BRAND_CODE>
                                            <NUM>12</NUM>
                                            <PRICE>54</PRICE>
                                            <SUM>96</SUM>
                                            <SUM_TAX>60</SUM_TAX>
                                        </CONTRACT_DETAIL_1>
                                    </CONTRACT_BASE_1>
                                </CONTRACT_LIST>
                            <ZYZ_LIST>
                                <ZYZ_BASE_1>
                                    <PERM_ID>微机编号</PERM_ID>
                                    <PERM_DATE>2013-04-26 10:00:07</PERM_DATE>
                                    <TRUCK_NO>车牌号</TRUCK_NO>
                                    <ZYZ_CONTRACT_1>
                                        <CONTRACT_CODE>01</CONTRACT_CODE>
                                    </ZYZ_CONTRACT_1>
                                </ZYZ_BASE_1>
                            </ZYZ_LIST>
                        </data>
                        </datalist>
                    </dataset>";
            #endregion

            ServiceFactory factory = new ServiceFactory();

            string result = null;
            string strResult = string.Empty;
            bool b = false;

            Guid billMasterID = Guid.NewGuid();

            if (!string.IsNullOrEmpty(xml))
            {
                XElement doc = XElement.Parse(xml);

                #region Head
                var heads = from head in doc.Descendants("head")
                            select new
                            {
                                MsgId = head.Element("msg_id").Value,
                                StateCode = head.Element("state_code").Value,
                                StateDesc = head.Element("state_desc").Value,
                                WsMark = head.Element("ws_mark").Value,
                                WsMethod = head.Element("ws_method").Value,
                                WsParam = head.Element("ws_param").Value ?? "",
                                CurrTime = head.Element("curr_time").Value ?? "",
                                CurrUser = head.Element("curr_user").Value ?? ""
                            };
                var headList = heads.ToArray()[0];
                #endregion

                #region var billMaster
                var billMaster = from data in doc.Descendants("data")
                                 select new
                                 {
                                     BillType = data.Element("bb_type").Value,
                                     UUID = data.Element("bb_uuid").Value,
                                     BillDate = data.Element("bb_input_date").Value,
                                     MakerName = data.Element("bb_input_person").Value,
                                     OperateDate = data.Element("bb_oper_date").Value,
                                     CigaretteType = data.Element("bb_cig_type").Value,
                                     BillCompanyCode = data.Element("bb_commerce_code").Value,
                                     SupplierCode = data.Element("bb_flow_code").Value,
                                     SupplierType = data.Element("bb_flow_type").Value,
                                     State = data.Element("BB_STATE").Value,
                                     BillDetail = from data_1 in doc.Descendants("data_1")
                                                  select new
                                                  {
                                                      PieceCigarCode = data_1.Element("bd_pcig_code").Value,
                                                      BoxCigarCode = data_1.Element("bd_bcig_code").Value,
                                                      BillQuantity = data_1.Element("bd_bill_all_num1").Value,
                                                      FixedQuantity = data_1.Element("bd_bill_fixed_all_num1").Value,
                                                      RealQuantity = data_1.Element("BD_BILL_ALL_SCAN_NUM1").Value
                                                  }
                                 };
                #endregion

                #region var contract
                var contract = from cb1 in doc.Descendants("CONTRACT_BASE_1")
                               select new
                               {
                                   ContractCode = cb1.Element("CONTRACT_NO").Value,
                                   SupplySideCode = cb1.Element("A_NO").Value,
                                   DemandSideCode = cb1.Element("B_NO").Value,
                                   ContractDate = cb1.Element("CONTRACT_DATE").Value,
                                   StartDade = cb1.Element("START_DATE").Value,
                                   EndDate = cb1.Element("END_DATE").Value,
                                   SendPlaceCode = cb1.Element("SEND_CODE").Value,
                                   SendAddress = cb1.Element("SEND_ADD").Value,
                                   ReceivePlaceCode = cb1.Element("RECEIVE_CODE").Value,
                                   ReceiveAddress = cb1.Element("RECEIVE_ADD").Value,
                                   SaleDate = cb1.Element("SALE_DATE").Value,
                                   State = cb1.Element("USE_STATE").Value,
                                   ContractDetail = from cd1 in doc.Descendants("CONTRACT_DETAIL_1")
                                                    select new
                                                    {
                                                        ContractCode = cd1.Element("CONTRACT_NO").Value,
                                                        BrandCode = cd1.Element("BRAND_CODE").Value,
                                                        Quantity = cd1.Element("NUM").Value,
                                                        Price = cd1.Element("PRICE").Value,
                                                        Amount = cd1.Element("SUM").Value,
                                                        TaxAmount = cd1.Element("SUM_TAX").Value
                                                    }
                               };
                #endregion

                #region var navicert
                var navicert = from zb1 in doc.Descendants("ZYZ_BASE_1")
                               select new
                               {
                                   NavicertCode = zb1.Element("PERM_ID").Value,
                                   NavicertDate = zb1.Element("PERM_DATE").Value,
                                   TruckPlateNo = zb1.Element("TRUCK_NO").Value,
                               };
                var contractCode = from nc1 in doc.Descendants("ZYZ_CONTRACT_1")
                                   select new
                                   {
                                       ContractCode = nc1.Element("CONTRACT_CODE").Value
                                   };
                #endregion

                using (var scope = new TransactionScope())
                {
                    #region for (int i = 0; i < billMaster.Count(); i++)
                    for (int i = 0; i < billMaster.Count(); i++)
                    {
                        var bmArray = billMaster.ToArray()[i];
                        BillMaster bm = new BillMaster();
                        bm.ID = billMasterID;
                        bm.BillType = bmArray.BillType;
                        bm.UUID = bmArray.UUID;
                        bm.BillDate = Convert.ToDateTime(bmArray.BillDate);
                        bm.MakerName = bmArray.MakerName;
                        bm.OperateDate = Convert.ToDateTime(bmArray.OperateDate);
                        bm.CigaretteType = bmArray.CigaretteType;
                        bm.BillCompanyCode = bmArray.BillCompanyCode;
                        bm.SupplierCode = bmArray.SupplierCode;
                        bm.SupplierType = bmArray.SupplierType;
                        bm.State = bmArray.State;
                        b = factory.GetService<IBillMasterService>().Add(bm, out strResult);
                        for (int j = 0; j < bmArray.BillDetail.Count(); j++)
                        {
                            var bdArray = bmArray.BillDetail.ToArray()[j];
                            BillDetail bd = new BillDetail();
                            bd.MasterID = bm.ID;
                            bd.PieceCigarCode = bdArray.PieceCigarCode;
                            bd.BoxCigarCode = bdArray.BoxCigarCode;
                            bd.BillQuantity = Convert.ToInt32(bdArray.BillQuantity);
                            bd.FixedQuantity = Convert.ToInt32(bdArray.FixedQuantity);
                            bd.RealQuantity = Convert.ToInt32(bdArray.RealQuantity);
                            b = factory.GetService<IBillDetailService>().Add(bd, out strResult);
                        }
                        if (b == false)
                        {
                            break;
                        }
                        else
                        {
                            #region for (int i = 0; i < contract.Count(); i++)
                            for (int k = 0; k < contract.Count(); k++)
                            {
                                var cArray = contract.ToArray()[k];
                                Contract con = new Contract();
                                con.ContractCode = cArray.ContractCode;
                                con.MasterID = billMasterID;
                                con.SupplySideCode = cArray.SupplySideCode;
                                con.DemandSideCode = cArray.DemandSideCode;
                                con.ContractDate = Convert.ToDateTime(cArray.ContractDate);
                                con.StartDade = Convert.ToDateTime(cArray.StartDade);
                                con.EndDate = Convert.ToDateTime(cArray.EndDate);
                                con.SendPlaceCode = cArray.SendPlaceCode;
                                con.SendAddress = cArray.SendAddress;
                                con.ReceivePlaceCode = cArray.ReceivePlaceCode;
                                con.ReceiveAddress = cArray.ReceiveAddress;
                                con.SaleDate = cArray.SaleDate;
                                con.State = cArray.State;
                                b = factory.GetService<IContractService>().Add(con, out strResult);
                                for (int l = 0; l < cArray.ContractDetail.Count(); l++)
                                {
                                    var cdArray = cArray.ContractDetail.ToArray()[l];
                                    ContractDetail cd = new ContractDetail();
                                    cd.ContractCode = cdArray.ContractCode;
                                    cd.BrandCode = cdArray.BrandCode;
                                    cd.Quantity = cdArray.Quantity;
                                    cd.Price = cdArray.Price;
                                    cd.Amount = Convert.ToInt32(cdArray.Amount);
                                    cd.TaxAmount = Convert.ToInt32(cdArray.TaxAmount);
                                    b = factory.GetService<IContractDetailService>().Add(cd, out strResult);
                                }
                                if (b == false)
                                {
                                    break;
                                }
                                else
                                {
                                    #region for (int i = 0; i < contractCode.Count(); i++)
                                    for (int m = 0; m < contractCode.Count(); m++)
                                    {
                                        var ncArray = contractCode.ToArray()[m];
                                        Navicert na = new Navicert();
                                        na.ID = Guid.NewGuid();
                                        na.MasterID = billMasterID;
                                        na.ContractCode = ncArray.ContractCode;
                                        for (int n = 0; n < contractCode.Count(); n++)
                                        {
                                            var nArray = navicert.ToArray()[n];
                                            na.NavicertCode = nArray.NavicertCode;
                                            na.NavicertDate = Convert.ToDateTime(nArray.NavicertDate);
                                            na.TruckPlateNo = nArray.TruckPlateNo;
                                        }
                                        b = factory.GetService<INavicertService>().Add(na, out strResult);
                                        if (b == false)
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            scope.Complete();
                                            result = string.Format(returnMsg, headList.MsgId, headList.StateCode, headList.StateDesc, headList.WsMark, headList.WsMethod, headList.WsParam, headList.CurrTime, headList.CurrUser);
                                        }
                                    }
                                    #endregion
                                }
                            }
                            #endregion
                        }
                    }
                    #endregion
                }
            }
            return result;
        }


        [WebMethod]
        public string WMSBillService_ZipBase64(string xml)
        {
            try
            {
                return WMSBillService(Unzip(xml));
            }
            catch (Exception)
            {
                return string.Format(returnMsg);
            }
        }

        [WebMethod]
        public string WMSPalletInfo(string xml)
        {
            return returnMsg;
        }

        [WebMethod]
        public string WMSPalletInfo_ZipBase64(string xml)
        {
            try
            {
                return WMSPalletInfo(Unzip(xml));
            }
            catch (Exception)
            {
                return string.Format(returnMsg);
            }
        }

        public string Unzip(string xml)
        {
            return xml;
        }
    }
}
