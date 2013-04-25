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

                #region BillMaster
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
                //using (var scope = new TransactionScope())
                //{
                for (int i = 0; i < billMaster.Count(); i++)
                {
                    var bmArray = billMaster.ToArray()[i];
                    BillMaster bm = new BillMaster();
                    bm.ID = Guid.NewGuid();
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
                    //if (!string.IsNullOrEmpty(result))
                    //{
                    //    scope.Complete();
                    //    break;
                    //}
                }
                //} 
                #endregion

                #region Contract
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
                for (int i = 0; i < contract.Count(); i++)
                {
                    var cArray = contract.ToArray()[i];
                    Contract con = new Contract();                    
                    con.ContractCode = cArray.ContractCode;
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

                    for (int j = 0; j < cArray.ContractDetail.Count(); j++)
                    {
                        var cdArray = cArray.ContractDetail.ToArray()[j];
                        ContractDetail cd = new ContractDetail();
                        cd.ContractCode = cdArray.ContractCode;
                        cd.BrandCode = cdArray.BrandCode;
                        cd.Quantity = cdArray.Quantity;
                        cd.Price = cdArray.Price;
                        cd.Amount = Convert.ToInt32(cdArray.Amount);
                        cd.TaxAmount = Convert.ToInt32(cdArray.TaxAmount);

                        b = factory.GetService<IContractDetailService>().Add(cd, out strResult);
                    }
                }
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

                if (b == true)
                {
                    result = string.Format(returnMsg, headList.MsgId, headList.StateCode, headList.StateDesc, headList.WsMark, headList.WsMethod, headList.WsParam, headList.CurrTime, headList.CurrUser);
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
