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
using THOK.Authority.Bll.Interfaces;
using System.Transactions;
using System.Xml;
using System.IO;
using System.IO.Compression;
using System.Text;

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
        ServiceFactory factory = new ServiceFactory();
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
            DateTime timeNow = System.DateTime.Now;

            Guid billMasterID = Guid.NewGuid();

            if (!string.IsNullOrEmpty(xml))
            {
                XElement doc = null;
                try
                {
                    doc = XElement.Parse(xml);
                }
                catch (Exception ex)
                {
                    result = string.Format(returnMsg, "", "", "XML的数据格式不正确！", ex.Message, "", timeNow, "");
                }
                try
                {
                    #region var heads
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
                                       TruckPlateNo = zb1.Element("TRUCK_NO").Value
                                   };
                    var contractCodes = from nc1 in doc.Descendants("CONTRACT_CODE")
                                        select new
                                        {
                                            ContractCode = nc1.Value
                                        };
                    #endregion

                    var headList = heads.ToArray()[0];
                    if (headList.WsMethod == "PalletInfo")
                    {
                        WMSPalletInfo(xml);
                    }
                    else
                    {
                        #region TransactionScope
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
                                if (headList.WsMethod == "BillCreate")
                                {
                                    b = factory.GetService<IBillMasterService>().Add(bm, out strResult);
                                }
                                if (headList.WsMethod == "BillModify" || headList.WsMethod == "BillStart" || headList.WsMethod == "BillScan" || headList.WsMethod == "BillConfirm")
                                {
                                    b = factory.GetService<IBillMasterService>().Save(bm, out strResult);
                                }
                                if (headList.WsMethod == "BillDelete")
                                {
                                    b = true;
                                }
                                #region for (int j = 0; j < bmArray.BillDetail.Count(); j++)
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
                                    bd.BillMaster = bm;
                                    if (headList.WsMethod == "BillCreate")
                                    {
                                        b = factory.GetService<IBillDetailService>().Add(bd, out strResult);
                                    }
                                    if (headList.WsMethod == "BillModify")
                                    {
                                        b = factory.GetService<IBillDetailService>().Save(bd, out strResult);
                                    }
                                    if (headList.WsMethod == "BillDelete")
                                    {
                                        b = true;
                                    }
                                }
                                #endregion
                                if (b == false)
                                {
                                    break;
                                }
                                else
                                {
                                    #region for (int k = 0; k < contract.Count(); k++)
                                    for (int k = 0; k < contract.Count(); k++)
                                    {
                                        var cArray = contract.ToArray()[k];
                                        Contract con = new Contract();
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
                                        if (headList.WsMethod == "BillCreate")
                                        {
                                            b = factory.GetService<IContractService>().Add(con, out strResult);
                                        }
                                        if (headList.WsMethod == "BillModify")
                                        {
                                            b = factory.GetService<IContractService>().Save(con, out strResult);
                                        }
                                        if (headList.WsMethod == "BillDelete")
                                        {
                                            b = true;
                                        }
                                        #region (int l = 0; l < cArray.ContractDetail.Count(); l++)
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
                                            cd.Contract = con;
                                            if (headList.WsMethod == "BillCreate")
                                            {
                                                b = factory.GetService<IContractDetailService>().Add(cd, out strResult);
                                            }
                                            if (headList.WsMethod == "BillModify")
                                            {
                                                b = factory.GetService<IContractDetailService>().Save(cd, out strResult);
                                            }
                                            if (headList.WsMethod == "BillDelete")
                                            {
                                                b = true;
                                            }
                                        }
                                        #endregion
                                        if (b == false)
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            #region
                                            Navicert na = new Navicert();

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
                                                    if (headList.WsMethod == "BillCreate")
                                                    {
                                                        b = factory.GetService<INavicertService>().Add(na, out strResult);
                                                    }
                                                    if (headList.WsMethod == "BillModify")
                                                    {
                                                        b = factory.GetService<INavicertService>().Save(na, out strResult);
                                                    }
                                                    if (headList.WsMethod == "BillDelete")
                                                    {
                                                        b = factory.GetService<IBillMasterService>().Delete(con.ContractCode, bm.UUID, out strResult);
                                                    }
                                                    if (b == false)
                                                    {
                                                        break;
                                                    }
                                                }
                                                
                                            }
                                            #endregion
                                        }
                                    }
                                    #endregion
                                }
                            }
                            #endregion

                            if (b == true)
                            {
                                scope.Complete();
                                result = string.Format(returnMsg, headList.MsgId, headList.StateCode, headList.StateDesc, headList.WsMark, headList.WsMethod, headList.WsParam, headList.CurrTime, headList.CurrUser);
                            }
                        }
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    result = string.Format(returnMsg, "", "", "有几种可能性：1.XML标签不正确;2.MSDTC服务未开启;3.Hosts配置错误！", ex.Message, "", timeNow, "");
                }
            }
            else
            {
                result = string.Format(returnMsg, "", "", "XML参数是空的！", "", "", timeNow, "");
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
            string result = "";
            XElement doc = XElement.Parse(xml);
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
                                RFIDAntCode = (d.Element("RFIDAntCode") ?? null) == null ? null : d.Element("RFIDAntCode").Value,
                                scan_time = (d.Element("scan_time") ?? null) == null ? null : d.Element("scan_time").Value
                            };
            using (var scope = new TransactionScope())
            {
                for (int i = 0; i < queryData.Count(); i++)
                {
                    var data = queryData.ToArray()[i];
                    Pallet palletAdd = new Pallet();
                    string[] brandInfo = (data.brand_info).ToString().Split(';');
                    palletAdd.PalletID = data.pallet_id;
                    palletAdd.WmsUUID = "";//
                    palletAdd.UUID = data.bb_uuid;
                    palletAdd.TicketNo = data.bb_ticket_no;
                    palletAdd.OperateDate = Convert.ToDateTime(data.bb_oper_date);
                    palletAdd.OperateType = data.bb_type;
                    palletAdd.BarCodeType = data.barcode_type;
                    palletAdd.RfidAntCode = (data.RFIDAntCode).ToString();
                    palletAdd.PieceCigarCode = brandInfo[0];
                    palletAdd.BoxCigarCode = brandInfo[3];
                    palletAdd.CigaretteName = brandInfo[1];
                    palletAdd.Quantity = Convert.ToDecimal(brandInfo[2]);
                    palletAdd.ScanTime = Convert.ToDateTime(data.scan_time);
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
                return string.Format(returnMsg, head.msg_id.ToString(), "000", "发送成功", head.ws_mark.ToString(), head.ws_method.ToString(), head.ws_param.ToString(), head.curr_time.ToString(), head.curr_user.ToString());
            }
            else
            {
                return string.Format(returnMsg, head.msg_id, "001", result, head.ws_mark, head.ws_method, head.ws_param, head.curr_time, head.curr_user);
            }
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
            //base64解码
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(xml);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string decoded = new String(decoded_char);
            //解压缩
            byte[] compressBeforeByte = Convert.FromBase64String(decoded);
            byte[] buffer = new byte[0x1000];
            try
            {
                MemoryStream ms = new MemoryStream(compressBeforeByte);
                GZipStream zip = new GZipStream(ms, CompressionMode.Decompress, true);
                MemoryStream msreader = new MemoryStream();
                while (true)
                {
                    int reader = zip.Read(buffer, 0, buffer.Length);
                    if (reader <= 0)
                    {
                        break;
                    }
                    msreader.Write(buffer, 0, reader);
                }
                zip.Close();
                ms.Close();
                msreader.Position = 0;
                buffer = msreader.ToArray();
                msreader.Close();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            byte[] compressAfterByte = buffer;
            xml = Encoding.GetEncoding("UTF-8").GetString(compressAfterByte);
            return xml;
        }

        public string Zip(string xml)
        {
            //压缩
            string compressStr = "";
            byte[] compressBeforeByte = Encoding.GetEncoding("UTF-8").GetBytes(xml);
            try
            {
                MemoryStream ms = new MemoryStream();
                GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true);
                zip.Write(compressBeforeByte, 0, compressBeforeByte.Length);
                zip.Close();
                byte[] buffer = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(buffer, 0, buffer.Length);
                ms.Close();
                compressStr= Convert.ToBase64String(buffer);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            //base64编码
            byte[] encData_byte = new byte[compressStr.Length];
            encData_byte = System.Text.Encoding.UTF8.GetBytes(compressStr);
            xml= Convert.ToBase64String(encData_byte);
            return xml;
        }
    }
}
