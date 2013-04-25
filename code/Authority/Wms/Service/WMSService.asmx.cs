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
            ServiceFactory factory = new ServiceFactory();

            string result = null;
            string strResult = string.Empty;
            bool b = false;

            if (!string.IsNullOrEmpty(xml))
            {
                XElement doc = XElement.Parse(xml);

                #region XML<head>
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

                #region XML<data>
                var datas = from data in doc.Descendants("data")
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
                //using (var scope = new TransactionScope())
                //{
                for (int i = 0; i < datas.Count(); i++)
                {
                    var dataArray = datas.ToArray()[i];
                    BillMaster bm = new BillMaster();
                    bm.BillType = dataArray.BillType;
                    bm.UUID = dataArray.UUID;
                    bm.BillDate = Convert.ToDateTime(dataArray.BillDate);
                    bm.MakerName = dataArray.MakerName;
                    bm.OperateDate = Convert.ToDateTime(dataArray.OperateDate);
                    bm.CigaretteType = dataArray.CigaretteType;
                    bm.BillCompanyCode = dataArray.BillCompanyCode;
                    bm.SupplierCode = dataArray.SupplierCode;
                    bm.SupplierType = dataArray.SupplierType;
                    bm.State = dataArray.State;

                    b = factory.GetService<IBillMasterService>().Add(bm, out strResult);

                    for (int j = 0; j < dataArray.BillDetail.Count(); j++)
                    {
                        var data1Array = dataArray.BillDetail.ToArray()[j];
                        BillDetail bd = new BillDetail();
                        //bd.MasterID = dataArray.;
                        bd.PieceCigarCode = data1Array.PieceCigarCode;
                        bd.BoxCigarCode = data1Array.BoxCigarCode;
                        bd.BillQuantity = Convert.ToInt32(data1Array.BillQuantity);
                        bd.FixedQuantity = Convert.ToInt32(data1Array.FixedQuantity);
                        bd.RealQuantity = Convert.ToInt32(data1Array.RealQuantity);

                        b = factory.GetService<IBillDetailService>().Add(bd, out strResult);
                    }

                    //if (!string.IsNullOrEmpty(result))
                    //{
                    //    scope.Complete();
                    //    break;
                    //}
                }
                //}
                if (b == true)
                {
                    result = string.Format(returnMsg, headList.MsgId, headList.StateCode, headList.StateDesc, headList.WsMark, headList.WsMethod, headList.WsParam, headList.CurrTime, headList.CurrUser);
                }

                //foreach (var bd in bm.BillDetail)
                //{
                //    BillDetail entityBD = new BillDetail();
                //    entityBD.PieceCigarCode = bd.PieceCigarCode;
                //    entityBD.BoxCigarCode = bd.BoxCigarCode;
                //    entityBD.BillQuantity = Convert.ToInt32(bd.BillQuantity);
                //    entityBD.FixedQuantity = Convert.ToInt32(bd.FixedQuantity);
                //    entityBD.RealQuantity = Convert.ToInt32(bd.RealQuantity);
                //}
                //}
                //}            
                #endregion

                #region Contract
                //var contract = from c in doc.Descendants("CONTRACT_BASE_1")
                //               select new
                //               {
                //                   ContractCode = c.Element("CONTRACT_NO").Value,
                //                   SupplySideCode = c.Element("A_NO").Value,
                //                   DemandSideCode = c.Element("B_NO").Value,
                //                   ContractDate = c.Element("CONTRACT_DATE").Value,
                //                   StartDade = c.Element("START_DATE").Value,
                //                   EndDate = c.Element("END_DATE").Value,
                //                   SendPlaceCode = c.Element("SEND_CODE").Value,
                //                   SendAddress = c.Element("SEND_ADD").Value,
                //                   ReceivePlaceCode = c.Element("RECEIVE_CODE").Value,
                //                   ReceiveAddress = c.Element("RECEIVE_ADD").Value,
                //                   SaleDate = c.Element("SALE_DATE").Value,
                //                   State = c.Element("USE_STATE").Value,
                //                   ContractDetail = from cb1 in doc.Descendants("CONTRACT_BASE_1")
                //                                    select new
                //                                    {
                //                                        ContractCode = c.Element("CONTRACT_NO").Value,
                //                                        BrandCode = c.Element("BRAND_CODE").Value,
                //                                        Quantity = c.Element("NUM").Value,
                //                                        Price = c.Element("PRICE").Value,
                //                                        Amount = c.Element("SUM").Value,
                //                                        TaxAmount = c.Element("SUM_TAX").Value
                //                                    }
                //               };
                //foreach (var con in contract)
                //{
                //    Contract cc = new Contract();
                //    cc.ContractCode = con.ContractCode.ToString();
                //    b = ContractService.Add(cc, out strResult);
                //}
                #endregion

                #region Navicert
                //var navicert = from zb1 in doc.Descendants("ZYZ_BASE_1")
                //               select new
                //               {
                //                   NavicertCode = zb1.Element("PERM_ID").Value,
                //                   NavicertDate = zb1.Element("PERM_DATE").Value,
                //                   TruckPlateNo = zb1.Element("TRUCK_NO").Value,
                //                   NavicertDetail = from nc1 in doc.Descendants("ZYZ_CONTRACT_1")
                //                                    select new
                //                                    {
                //                                        ContractCode = nc1.Element("CONTRACT_CODE").Value
                //                                    }
                //               };
                //foreach (var navi in navicert)
                //{
                //    Navicert nn = new Navicert();
                //    nn.NavicertCode = navi.NavicertCode.ToString();
                //    b = NavicertService.Add(nn, out strResult);
                //}
                #endregion
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
