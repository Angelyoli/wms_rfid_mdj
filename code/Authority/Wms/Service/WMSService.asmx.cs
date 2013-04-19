using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

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
