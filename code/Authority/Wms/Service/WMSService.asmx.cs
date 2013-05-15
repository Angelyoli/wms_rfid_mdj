using System;
using System.Linq;
using System.Web.Services;
using System.Xml.Linq;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using THOK.Security;
using System.Transactions;
using System.IO;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using System.Collections.Generic;
using System.Xml;
using THOK.Wms.DownloadWms.Bll;

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

        public AddXmlValueBll bll = new AddXmlValueBll();

        #region 常量
        ServiceFactory factory = new ServiceFactory();
        private const string returnMsg = @"<?xml version='1.0' encoding='GBK'?>
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

        bool b = false;

        public string[] MessageInfo(string message)
        {
            DateTime timeNow = System.DateTime.Now;
            string[] info = { "", "001", message, "", "", "", timeNow.ToShortTimeString(), "" };
            return info;
        }

        [WebMethod]
        public string WMSBillService(string xml)
        {
            ServiceFactory factory = new ServiceFactory();

            string result = null;
            string strResult = string.Empty;
            bool b = false;
            
            if (!string.IsNullOrEmpty(xml))
            {
                XElement doc = null;
                try
                {
                    doc = XElement.Parse(xml);
                    bll.insert("Bill", xml);
                }
                catch (Exception)
                {
                    result = ZipBase64(string.Format(returnMsg, MessageInfo("XML的数据格式不正确！")));
                    return result;
                }
                try
                {
                    #region var heads
                    var heads = from head in doc.Descendants("head")
                                select new
                                {
                                    MsgId = (head.Element("msg_id") ?? null) == null ? null : head.Element("msg_id").Value,
                                    StateCode = (head.Element("state_code") ?? null) == null ? null : head.Element("state_code").Value,
                                    StateDesc = (head.Element("state_desc") ?? null) == null ? null : head.Element("state_desc").Value,
                                    WsMark = (head.Element("ws_mark") ?? null) == null ? null : head.Element("ws_mark").Value,
                                    WsMethod = (head.Element("ws_method") ?? null) == null ? null : head.Element("ws_method").Value,
                                    WsParam = (head.Element("ws_param") ?? null) == null ? null : head.Element("ws_param").Value ?? "",
                                    CurrTime = (head.Element("curr_time") ?? null) == null ? null : head.Element("curr_time").Value ?? "",
                                    CurrUser = (head.Element("curr_user") ?? null) == null ? null : head.Element("curr_user").Value ?? ""
                                };
                    #endregion

                    #region var billMaster
                    var billMaster = from data in doc.Descendants("data")
                                     select new
                                     {
                                         BillType = (data.Element("bb_type") ?? null) == null ? null : data.Element("bb_type").Value,
                                         UUID = (data.Element("bb_uuid") ?? null) == null ? null : data.Element("bb_uuid").Value,
                                         BillDate = (data.Element("bb_input_date") ?? null) == null ? null : data.Element("bb_input_date").Value,
                                         MakerName = (data.Element("bb_input_person") ?? null) == null ? null : data.Element("bb_input_person").Value,
                                         OperateDate = (data.Element("bb_oper_date") ?? null) == null ? null : data.Element("bb_oper_date").Value,
                                         CigaretteType = (data.Element("bb_cig_type") ?? null) == null ? null : data.Element("bb_cig_type").Value,
                                         BillCompanyCode = (data.Element("bb_commerce_code") ?? null) == null ? null : data.Element("bb_commerce_code").Value,
                                         SupplierCode = (data.Element("bb_flow_code") ?? null) == null ? null : data.Element("bb_flow_code").Value,
                                         SupplierType = (data.Element("bb_flow_type") ?? null) == null ? null : data.Element("bb_flow_type").Value,
                                         State = (data.Element("bb_state") ?? null) == null ? null : data.Element("bb_state").Value,
                                         BillDetail = from data_1 in doc.Descendants("data_1")
                                                      select new
                                                      {
                                                          PieceCigarCode = (data_1.Element("bd_pcig_code") ?? null) == null ? null : data_1.Element("bd_pcig_code").Value,
                                                          BoxCigarCode = (data_1.Element("bd_bcig_code") ?? null) == null ? null : data_1.Element("bd_bcig_code").Value,
                                                          BillQuantity = (data_1.Element("bd_bill_all_num1") ?? null) == null ? null : data_1.Element("bd_bill_all_num1").Value,
                                                          FixedQuantity = (data_1.Element("bd_bill_fixed_all_num1") ?? null) == null ? null : data_1.Element("bd_bill_fixed_all_num1").Value,
                                                          RealQuantity = (data_1.Element("bd_bill_all_scan_num1") ?? null) == null ? null : data_1.Element("bd_bill_all_scan_num1").Value
                                                      }
                                     };
                    #endregion

                    #region var contract
                    var contract = from cb1 in doc.Descendants("contract_base_1")
                                   select new
                                   {
                                       ContractCode = (cb1.Element("contract_no") ?? null) == null ? null : cb1.Element("contract_no").Value,
                                       SupplySideCode = (cb1.Element("a_no") ?? null) == null ? null : cb1.Element("a_no").Value,
                                       DemandSideCode = (cb1.Element("b_no") ?? null) == null ? null : cb1.Element("b_no").Value,
                                       ContractDate = (cb1.Element("contract_date") ?? null) == null ? null : cb1.Element("contract_date").Value,
                                       StartDade = (cb1.Element("start_date") ?? null) == null ? null : cb1.Element("start_date").Value,
                                       EndDate = (cb1.Element("start_date") ?? null) == null ? null : cb1.Element("end_date").Value,
                                       SendPlaceCode = (cb1.Element("send_code") ?? null) == null ? null : cb1.Element("send_code").Value,
                                       SendAddress = (cb1.Element("send_add") ?? null) == null ? null : cb1.Element("send_add").Value,
                                       ReceivePlaceCode = (cb1.Element("send_add") ?? null) == null ? null : cb1.Element("receive_code").Value,
                                       ReceiveAddress = (cb1.Element("receive_add") ?? null) == null ? null : cb1.Element("receive_add").Value,
                                       SaleDate = (cb1.Element("sale_date") ?? null) == null ? null : cb1.Element("sale_date").Value,
                                       State = (cb1.Element("use_state") ?? null) == null ? null : cb1.Element("use_state").Value,
                                       ContractDetail = from cd1 in doc.Descendants("contract_detail_1")
                                                        select new
                                                        {
                                                            ContractCode = (cd1.Element("contract_no") ?? null) == null ? null : cd1.Element("contract_no").Value,
                                                            BrandCode = (cd1.Element("brand_code") ?? null) == null ? null : cd1.Element("brand_code").Value,
                                                            Quantity = (cd1.Element("num") ?? null) == null ? null : cd1.Element("num").Value,
                                                            Price = (cd1.Element("price") ?? null) == null ? null : cd1.Element("price").Value,
                                                            Amount = (cd1.Element("sum") ?? null) == null ? null : cd1.Element("sum").Value,
                                                            TaxAmount = (cd1.Element("sum_tax") ?? null) == null ? null : cd1.Element("sum_tax").Value
                                                        }
                                   };
                    #endregion

                    #region var navicert
                    var navicert = from zb1 in doc.Descendants("zyz_base_1")
                                   select new
                                   {
                                       NavicertCode = (zb1.Element("perm_id") ?? null) == null ? null : zb1.Element("perm_id").Value,
                                       NavicertDate = (zb1.Element("perm_date") ?? null) == null ? null : zb1.Element("perm_date").Value,
                                       TruckPlateNo = (zb1.Element("truck_no") ?? null) == null ? null : zb1.Element("truck_no").Value
                                   };
                    var contractCodes = from nc1 in doc.Descendants("contract_code")
                                        select new
                                        {
                                            ContractCode = nc1.Value ?? null
                                        };
                    #endregion

                    var headList = heads.ToArray()[0];

                    if (headList.WsMethod == "BillCreate" || headList.WsMethod == "BillModify" || headList.WsMethod == "BillStart" || headList.WsMethod == "BillScan" || headList.WsMethod == "BillConfirm" || headList.WsMethod == "BillDelete")
                    {
                        using (var scope = new TransactionScope())
                        {
                            BillMaster bm = new BillMaster();
                            BillDetail bd = new BillDetail();
                            Contract con = new Contract();
                            ContractDetail cd = new ContractDetail();
                            Navicert na = new Navicert();

                            Guid billMasterID = Guid.NewGuid();

                            #region BillMaster and BillDetail
                            if (doc.Descendants("data") != null)
                            {
                                try
                                {
                                    for (int i = 0; i < billMaster.Count(); i++)
                                    {
                                        var bmArray = billMaster.ToArray()[i];
                                        bm.ID = billMasterID;
                                        bm.BillType = bmArray.BillType;
                                        bm.UUID = bmArray.UUID;
                                        bm.BillDate = Convert.ToDateTime(bmArray.BillDate);
                                        bm.MakerName = bmArray.MakerName;
                                        bm.OperateDate = Convert.ToDateTime(bmArray.OperateDate == "" ? null : bmArray.OperateDate);
                                        bm.CigaretteType = bmArray.CigaretteType;
                                        bm.BillCompanyCode = bmArray.BillCompanyCode;
                                        bm.SupplierCode = bmArray.SupplierCode;
                                        bm.SupplierType = bmArray.SupplierType;
                                        bm.State = bmArray.State;
                                        if (headList.WsMethod == "BillModify" || headList.WsMethod == "BillStart" || headList.WsMethod == "BillScan" || headList.WsMethod == "BillConfirm")
                                        {
                                            b = factory.GetService<IBillMasterService>().Delete(con.ContractCode, bm.UUID, out strResult);
                                        }
                                        if (headList.WsMethod == "BillCreate" || b == true)
                                        {
                                            b = factory.GetService<IBillMasterService>().Add(bm, out strResult);
                                        }
                                        #region BillDetail
                                        if (doc.Descendants("data_1") != null)
                                        {
                                            for (int j = 0; j < bmArray.BillDetail.Count(); j++)
                                            {
                                                var bdArray = bmArray.BillDetail.ToArray()[j];
                                                bd.ID = Guid.NewGuid();
                                                bd.MasterID = bm.ID;
                                                bd.PieceCigarCode = bdArray.PieceCigarCode;
                                                bd.BoxCigarCode = bdArray.BoxCigarCode;
                                                bd.BillQuantity = Convert.ToDecimal(bdArray.BillQuantity);
                                                bd.FixedQuantity = Convert.ToDecimal(bdArray.FixedQuantity);
                                                bd.RealQuantity = Convert.ToDecimal(bdArray.RealQuantity);
                                                bd.BillMaster = bm;
                                                if (headList.WsMethod == "BillCreate" || headList.WsMethod == "BillModify" || headList.WsMethod == "BillStart" || headList.WsMethod == "BillScan" || headList.WsMethod == "BillConfirm")
                                                {
                                                    b = factory.GetService<IBillDetailService>().Add(bd, out strResult);
                                                }
                                                if (headList.WsMethod == "BillDelete")
                                                {
                                                    b = true;
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                    if (b == true)
                                    {
                                        result = ZipBase64(string.Format(returnMsg, MessageInfo("操作出入库单成功！")));
                                    }
                                    else
                                    {
                                        result = ZipBase64(string.Format(returnMsg, MessageInfo("操作出入库单失败！")));
                                        return result;
                                    }
                                }
                                catch (Exception)
                                {
                                    result = ZipBase64(string.Format(returnMsg, MessageInfo("出入库单节点不匹配！")));
                                    return result;
                                }
                            }
                            #endregion

                            #region Contract and ContractDetail
                            if (doc.Descendants("contract_base_1") != null)
                            {
                                try
                                {
                                    for (int k = 0; k < contract.Count(); k++)
                                    {
                                        var cArray = contract.ToArray()[k];
                                        con.ContractCode = cArray.ContractCode;
                                        con.MasterID = bm.ID;
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
                                        if (headList.WsMethod == "BillCreate" || headList.WsMethod == "BillModify" || headList.WsMethod == "BillStart" || headList.WsMethod == "BillScan" || headList.WsMethod == "BillConfirm")
                                        {
                                            b = factory.GetService<IContractService>().Add(con, out strResult);
                                        }
                                        #region ContractDetail
                                        if (doc.Descendants("contract_detail_1") != null)
                                        {
                                            for (int l = 0; l < cArray.ContractDetail.Count(); l++)
                                            {
                                                var cdArray = cArray.ContractDetail.ToArray()[l];
                                                cd.ContractCode = cdArray.ContractCode;
                                                cd.BrandCode = cdArray.BrandCode;
                                                cd.Quantity = Convert.ToDecimal(cdArray.Quantity);
                                                cd.Price = Convert.ToDecimal(cdArray.Price);
                                                cd.Amount = Convert.ToInt32(cdArray.Amount);
                                                cd.TaxAmount = Convert.ToInt32(cdArray.TaxAmount);
                                                cd.Contract = con;
                                                if (headList.WsMethod == "BillCreate" || headList.WsMethod == "BillModify" || headList.WsMethod == "BillStart" || headList.WsMethod == "BillScan" || headList.WsMethod == "BillConfirm")
                                                {
                                                    b = factory.GetService<IContractDetailService>().Add(cd, out strResult);
                                                }
                                                if (headList.WsMethod == "BillDelete")
                                                {
                                                    b = true;
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                    if (b == true)
                                    {
                                        result = ZipBase64(string.Format(returnMsg, MessageInfo("操作合同表单成功！")));
                                    }
                                    else
                                    {
                                        result = ZipBase64(string.Format(returnMsg, MessageInfo("操作合同表单失败！")));
                                        return result;
                                    }
                                }
                                catch (Exception)
                                {
                                    result = ZipBase64(string.Format(returnMsg, MessageInfo("合同表单节点不匹配！")));
                                    return result;
                                }
                            }
                            #endregion

                            #region Navicert for contractCode
                            if (doc.Descendants("zyz_base_1") != null && doc.Descendants("contract_code") != null)
                            {
                                try
                                {
                                    for (int m = 0; m < contractCodes.Count(); m++)
                                    {
                                        var nArray = navicert.ToArray()[m];
                                        foreach (var item in contractCodes)
                                        {
                                            na.ContractCode = item.ContractCode;
                                            na.ID = Guid.NewGuid();
                                            na.MasterID = bm.ID;
                                            na.NavicertCode = nArray.NavicertCode;
                                            na.NavicertDate = Convert.ToDateTime(nArray.NavicertDate);
                                            na.TruckPlateNo = nArray.TruckPlateNo;
                                            na.Contract = con;
                                            if (headList.WsMethod == "BillCreate" || headList.WsMethod == "BillModify" || headList.WsMethod == "BillStart" || headList.WsMethod == "BillScan" || headList.WsMethod == "BillConfirm")
                                            {
                                                b = factory.GetService<INavicertService>().Add(na, out strResult);
                                            }
                                            if (headList.WsMethod == "BillDelete")
                                            {
                                                b = true;
                                            }
                                        }
                                    }
                                    if (b == true)
                                    {
                                        result = ZipBase64(string.Format(returnMsg, MessageInfo("操作准运证成功！")));
                                    }
                                    else
                                    {
                                        result = ZipBase64(string.Format(returnMsg, MessageInfo("操作准运证失败！")));
                                        return result;
                                    }
                                }
                                catch (Exception)
                                {
                                    result = ZipBase64(string.Format(returnMsg, MessageInfo("准运证节点不匹配！")));
                                    return result;
                                }
                            }
                            #endregion

                            #region Delete
                            if (headList.WsMethod == "BillDelete")
                            {
                                b = factory.GetService<IBillMasterService>().Delete(con.ContractCode, bm.UUID, out strResult);
                            }
                            if (b == true)
                            {
                                scope.Complete();
                                result = ZipBase64(string.Format(returnMsg, headList.MsgId, headList.StateCode, headList.StateDesc, headList.WsMark, headList.WsMethod, headList.WsParam, headList.CurrTime, headList.CurrUser));
                            }
                            else
                            {
                                result = ZipBase64(string.Format(returnMsg, MessageInfo("删除失败！")));
                                return result;
                            }
                            #endregion
                        }
                    }
                    else if (headList.WsMethod == "PalletInfo")
                    {
                        result = WMSPalletInfo(xml);
                    }
                    else
                    {
                        result = ZipBase64(string.Format(returnMsg, MessageInfo("<ws_method></ws_method>标签内字段不匹配！")));
                        return result;
                    }
                }
                catch (Exception)
                {
                    result = ZipBase64(string.Format(returnMsg, MessageInfo("有几种可能性：1.XML标签不正确;2.MSDTC服务未开启;3.Hosts配置错误！")));
                    return result;
                }
            }
            else
            {
                return ZipBase64(string.Format(returnMsg, MessageInfo("XML参数是空的！")));
            }
            
            return result;
        }


        [WebMethod]
        public string WMSBillService_ZipBase64(string xml)
        {
            try
            {
                return WMSBillService(UpZipBase64(xml));
            }
            catch (Exception ex)
            {
                return ZipBase64(string.Format(returnMsg, MessageInfo("解压失败！错误消息："+ex.Message)));
            }
        }

        [WebMethod]
        public string WMSPalletInfo(string xml)
        {
            string result = "";
            XElement doc = null;
            try
            {
                doc = XElement.Parse(xml);
                bll.insert("Pallet", xml);
            }
            catch
            {
                return ZipBase64(string.Format(returnMsg, "", "001", "发送失败：不是有效的XML格式字符串", "", "", "", "", ""));
            }
            var queryHead = from d in doc.Descendants("head")
                            select new
                            {
                                msg_id = (d.Element("msg_id") ?? null) == null ? null : d.Element("msg_id").Value,
                                ws_mark = (d.Element("ws_mark") ?? null) == null ? null : d.Element("ws_mark").Value,
                                ws_method = (d.Element("ws_method") ?? null) == null ? null : d.Element("ws_method").Value,
                                ws_param = (d.Element("ws_param") ?? null) == null ? null : d.Element("ws_param").Value,
                                client_ip = (d.Element("client_ip") ?? null) == null ? null : d.Element("client_ip").Value,
                                curr_time = (d.Element("curr_time") ?? null) == null ? null : d.Element("curr_time").Value,
                                curr_user = (d.Element("curr_user") ?? null) == null ? null : d.Element("curr_user").Value
                            };
            var head = queryHead.ToArray()[0];
            var queryData = from d in doc.Descendants("data")
                            select new
                            {
                                bb_type = (d.Element("bb_type") ?? null) == null ? null : d.Element("bb_type").Value,
                                bb_uuid = (d.Element("bb_uuid") ?? null) == null ? null : d.Element("bb_uuid").Value,
                                bb_ticket_no = (d.Element("bb_ticket_no") ?? null) == null ? null : d.Element("bb_ticket_no").Value,
                                bb_contact_no = (d.Element("bb_contact_no") ?? null) == null ? null : d.Element("bb_contact_no").Value,
                                bb_oper_date = (d.Element("bb_oper_date") ?? null) == null ? null : d.Element("bb_oper_date").Value,
                                barcode_type = (d.Element("barcode_type") ?? null) == null ? null : d.Element("barcode_type").Value,
                                pallet_id = (d.Element("pallet_id") ?? null) == null ? null : d.Element("pallet_id").Value,
                                brand_info = (d.Element("brand_info") ?? null) == null ? null : d.Element("brand_info").Value,
                                ok_brand_info = (d.Element("ok_brand_info") ?? null) == null ? null : d.Element("ok_brand_info").Value,
                                RFIDAntCode = (d.Element("rfidantcode") ?? null) == null ? null : d.Element("rfidantcode").Value,
                                scan_time = (d.Element("scan_time") ?? null) == null ? null : d.Element("scan_time").Value
                            };
            using (var scope = new TransactionScope())
            {
                for (int i = 0; i < queryData.Count(); i++)
                {
                    string palletid = string.Empty;
                    string barcodetype = string.Empty;
                    string bbtickedo = string.Empty;
                    string bbcontactno = string.Empty;
                    string bboperdate = DateTime.Now.ToString();
                    var qhead = queryHead.ToArray()[0];
                    var data = queryData.ToArray()[i];
                    string[] brandInfo = null;
                    //brandInfo = (data.ok_brand_info).ToString().Split(';');
                    if (qhead.ws_mark == "WMSPalletInfo")
                    {
                        brandInfo = (data.brand_info).ToString().Split(';');
                        barcodetype = data.barcode_type;
                    }
                    else
                    {
                        brandInfo = (data.ok_brand_info).ToString().Split(';');
                        bbtickedo = data.bb_ticket_no;
                        bbcontactno = data.bb_contact_no;
                        bboperdate = data.bb_oper_date;
                    }                   
                    Pallet palletAdd = new Pallet();

                    if (brandInfo.Length < 4)
                    {
                        brandInfo = (data.brand_info).ToString().Split('；');
                    }
                                      
                    try
                    {
                        palletAdd.PalletID = data.pallet_id;
                        palletAdd.WmsUUID = "";//
                        palletAdd.UUID = data.bb_uuid;
                        palletAdd.TicketNo = bbtickedo;
                        palletAdd.OperateDate = Convert.ToDateTime(bboperdate);
                        palletAdd.OperateType = data.bb_type;
                        palletAdd.BarCodeType = barcodetype;
                        palletAdd.RfidAntCode = (data.RFIDAntCode).ToString();
                        palletAdd.PieceCigarCode = brandInfo[0];
                        palletAdd.BoxCigarCode = brandInfo[3];
                        palletAdd.CigaretteName = brandInfo[1];
                        palletAdd.Quantity = Convert.ToDecimal(brandInfo[2]);
                        palletAdd.ScanTime = Convert.ToDateTime(data.scan_time);
                    }
                    catch (Exception ex)
                    {
                        return ZipBase64(string.Format(returnMsg, "", "001", "发送失败：数据不符合要求"+ex.Message, "", "", "", "", ""));
                    }
                    result = factory.GetService<IPalletService>().Add(palletAdd);
                    if (result != "")
                    {
                        break;
                    }
                }
                if (result == "")
                {
                    scope.Complete();
                }
            }
            if (result == "")
            {
                return ZipBase64(string.Format(returnMsg, head.msg_id.ToString(), "000", "发送成功", head.ws_mark.ToString(), head.ws_method.ToString(), head.ws_param.ToString(), head.curr_time.ToString(), head.curr_user.ToString()));
            }
            else
            {
                return ZipBase64(string.Format(returnMsg, head.msg_id, "001", result, head.ws_mark, head.ws_method, head.ws_param, head.curr_time, head.curr_user));
            }
        }

        [WebMethod]
        public string WMSPalletInfo_ZipBase64(string xml)
        {
            try
            {
                return WMSPalletInfo(UpZipBase64(xml));
            }
            catch (Exception ex)
            {
                return ZipBase64(string.Format(returnMsg, MessageInfo("解压失败！错误消息:" + ex.Message)));
            }
        }

        public static string ZipBase64(string strSource)
        {
            try
            {
                //将明文字符串STRA转化为字节数组BYTEA。
                byte[] bytesDecode = Encoding.UTF8.GetBytes(strSource);
                //将字节数组BYTEA压缩成BYTEB。
                bytesDecode = ZipBytes(bytesDecode);
                //对BYTEB采用BASE64编码，形成密文字符串STRB。
                return Convert.ToBase64String(bytesDecode);
            }
            catch (Exception er)
            {
                throw er;
            }
        }

        public static byte[] ZipBytes(byte[] bytesZip)
        {
            try
            {
                MemoryStream mMemory = new MemoryStream();
                ZipOutputStream mStream = new ZipOutputStream(mMemory);
                ZipEntry ze = new ZipEntry("zipEntry");
                mStream.PutNextEntry(ze);

                mStream.Write(bytesZip, 0, bytesZip.Length);
                mStream.Close();
                byte[] bytesUnzip = mMemory.ToArray();
                return bytesUnzip;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string UpZipBase64(string strSource)
        {
            bll.insert("str", strSource);
            try
            {
                //Base64解码
                byte[] bytesEncode = Convert.FromBase64String(strSource);
                //对字节数组BTYEB进行Zip解压，得到BYTEA
                bytesEncode = UnzipBytes(bytesEncode);
                //将字节数组BYTEA转换为字符串，得到明文STRA                
                string xml = Encoding.UTF8.GetString(bytesEncode);         
                return xml;
            }
            catch (Exception er)
            {
                return er.Message;
            }
        }

        public static byte[] UnzipBytes(byte[] bytesUnzip)
        {
            try
            {
                ZipInputStream mStream = new ZipInputStream(new MemoryStream(bytesUnzip));
                mStream.GetNextEntry();
                MemoryStream mMemory = new MemoryStream();
                Int32 mSize;
                byte[] mWriteData = new byte[4096];
                while (true)
                {
                    mSize = mStream.Read(mWriteData, 0, mWriteData.Length);
                    if (mSize > 0)
                    {
                        mMemory.Write(mWriteData, 0, mSize);
                    }
                    else
                    {
                        break;
                    }
                }
                mStream.Close();

                byte[] bytesZip = mMemory.ToArray();

                return bytesZip;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
