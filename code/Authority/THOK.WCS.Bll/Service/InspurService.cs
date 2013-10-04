using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.WCS.Bll.Models;

namespace THOK.WCS.Bll.Service
{
    public class InspurService
    {
        #region 入库和出库 进度反馈
        public string BillProgressFeedback(Inspur inspur, string inOrOut)
        {
            string result = null;

            string xmlBegin = "<?xml version='1.0' encoding='UTF-8'?>"
                            + "<dataset> ";

            string xmlHead = "  <head>"
                           + "      <param>{0}</param>"
                           + "      <user>{1}</user>"
                           + "      <time>{2}</time>"
                           + "  </head>";

            string xmlData = "  <data>"
                            + "      <store_" + inOrOut + "_num>{3}</store_" + inOrOut + "_num>"
                            + "      <data_detail>"
                            + "          <item_code>{4}</item_code>"
                            + "          <qty_complete>{5}</qty_complete>"
                            + "      </data_detail>"
                            + "  </data>";

            string xmlEnd = "</dataset>";
            try
            {
                string xmlFeedback = xmlBegin + xmlHead + xmlData + xmlEnd;
                result = string.Format(xmlFeedback, inspur.Param, inspur.User, inspur.Time, inspur.BillNo, inspur.ProductCode, inspur.RealQuantity);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }
        #endregion

        #region 入库和出库 完成
        public string BillFinished(Inspur inspur, string inOrOut)
        {
            string result = null;
            string xmlBegin = "<?xml version='1.0' encoding='UTF-8'?>"
                            + "<dataset> ";

            string xmlHead = "  <head>"
                           + "      <param>{0}</param>"
                           + "      <user>{1}</user>"
                           + "      <time>{2}</time>"
                           + "  </head>";
            string xmlData = "  <data>"
                            + "      <store_" + inOrOut + "_num>{3}</store_" + inOrOut + "_num>"
                            + "  </data>";
            string xmlEnd = "</dataset>";

            try
            {
                string xmlFinished = xmlBegin + xmlHead + xmlData + xmlEnd;
                result = string.Format(xmlFinished, inspur.Param, inspur.User, inspur.Time, inspur.BillNo);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }
        #endregion
    }
}
