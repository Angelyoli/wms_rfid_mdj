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

        [WebMethod]
        public string WMSBillService(string xml)
        {
            return returnMsg;
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
            string result="";
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
                    palletAdd.BarCodeType =data.barcode_type;
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
    }
}
