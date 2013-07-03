using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Common.Inspur
{
    public class WebService
    {
        string returnMsg = @"<?xml version='1.0' encoding='UTF-8'?>
                            <DATASET>
	                            <HEAD>
                                    <STATE_CODE>000</STATE_CODE>
		                            <STATE_DESC>接口交互成功! 生成月计划单据：【CQ00148】</STATE_DESC>
		                            <USER>SUPERADMIN</USER>
		                            <TIME>2013-05-20 09:32:00</TIME>
	                            </HEAD>
                            </DATASET>";

        #region 入库
        public void WarehouseInBillProgressFeedback(string STORE_IN_NUM, string ITEM_CODE, string QTY_COMPLETE)
        {
            string xml = @"<?xml version='1.0' encoding='UTF-8'?>
                                <DATASET>
	                                <HEAD>
		                                <PARAM></PARAM>
		                                <USER>0532</USER>
                                        <TIME>2013-05-20 09:32:00</TIME>
	                                </HEAD>
	                                <DATA>
		                                <OUT_INV_NUM>{0}</OUT_INV_NUM>
                                        <WHSE_ID>O0002146</WHSE_ID>
		                                <OWNER_ID>O0004210</OWNER_ID>
		                                <PLAN_DATE>20130520</PLAN_DATE>
		                                <DATA_DETAIL>
			                                <ITEM_ID>{1}</ITEM_ID>
                                            <BATCH_ID>1234567890</BATCH_ID>
                                            <QTY>{2}</QTY>
		                                </DATA_DETAIL>
	                                </DATA>
                                </DATASET>";
            string result = string.Format(xml, STORE_IN_NUM, ITEM_CODE, QTY_COMPLETE);
            //LwmWarehouseWorkService lwws = new LwmWarehouseWorkService();
            //lwws.lwmStroeInProgFeedback(result);
        }
        public void WarehouseInBillFinish(string STORE_IN_NUM)
        {
            string xml = @"<?xml version='1.0' encoding='UTF-8'?>
                                <DATASET>
	                                <HEAD>
		                                <PARAM></PARAM>
		                                <USER>0532</USER>
                                        <TIME>2013-05-20 09:32:00</TIME>
	                                </HEAD>
	                                <DATA>
		                                <OUT_INV_NUM>{0}</OUT_INV_NUM>
                                        <WHSE_ID>O0002146</WHSE_ID>
		                                <OWNER_ID>O0004210</OWNER_ID>
		                                <PLAN_DATE>20130520</PLAN_DATE>
		                                <DATA_DETAIL>
			                                <ITEM_ID>{1}</ITEM_ID>
                                            <BATCH_ID>1234567890</BATCH_ID>
                                            <QTY>{2}</QTY>
		                                </DATA_DETAIL>
	                                </DATA>
                                </DATASET>";
            string result = string.Format(xml, STORE_IN_NUM);
            //LwmWarehouseWorkService lwws = new LwmWarehouseWorkService();
            //lwws.lwmStroeInProgFeedback(result);
        } 
        #endregion

        #region 出库
        public void WarehouseOutBillProgressFeedback(string STORE_OUT_NUM, string ITEM_CODE, string QTY_COMPLETE)
        {
            string xml = @"<?xml version='1.0' encoding='UTF-8'?>
                                <DATASET>
	                                <HEAD>
		                                <PARAM></PARAM>
		                                <USER>0532</USER>
                                        <TIME>2013-05-20 09:32:00</TIME>
	                                </HEAD>
	                                <DATA>
		                                <OUT_INV_NUM>{0}</OUT_INV_NUM>
                                        <WHSE_ID>O0002146</WHSE_ID>
		                                <OWNER_ID>O0004210</OWNER_ID>
		                                <PLAN_DATE>20130520</PLAN_DATE>
		                                <DATA_DETAIL>
			                                <ITEM_ID>{1}</ITEM_ID>
                                            <BATCH_ID>1234567890</BATCH_ID>
                                            <QTY>{2}</QTY>
		                                </DATA_DETAIL>
	                                </DATA>
                                </DATASET>";
            string result = string.Format(xml, STORE_OUT_NUM, ITEM_CODE, QTY_COMPLETE);
            //LwmWarehouseWorkService lwws = new LwmWarehouseWorkService();
            //lwws.lwmStroeInProgFeedback(result);
        }
        public void WarehouseOutBillFinish(string STORE_OUT_NUM)
        {
            string xml = @"<?xml version='1.0' encoding='UTF-8'?>
                                <DATASET>
	                                <HEAD>
		                                <PARAM></PARAM>
		                                <USER>0532</USER>
                                        <TIME>2013-05-20 09:32:00</TIME>
	                                </HEAD>
	                                <DATA>
		                                <OUT_INV_NUM>{0}</OUT_INV_NUM>
                                        <WHSE_ID>O0002146</WHSE_ID>
		                                <OWNER_ID>O0004210</OWNER_ID>
		                                <PLAN_DATE>20130520</PLAN_DATE>
		                                <DATA_DETAIL>
			                                <ITEM_ID>{1}</ITEM_ID>
                                            <BATCH_ID>1234567890</BATCH_ID>
                                            <QTY>{2}</QTY>
		                                </DATA_DETAIL>
	                                </DATA>
                                </DATASET>";
            string result = string.Format(xml, STORE_OUT_NUM);
            //LwmWarehouseWorkService lwws = new LwmWarehouseWorkService();
            //lwws.lwmStroeInProgFeedback(result);
        } 
        #endregion

    }
}