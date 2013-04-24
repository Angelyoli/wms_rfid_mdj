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
            #region 参数
            xml = @"<?xml version='1.0' encoding='GBK'?>
                    <dataset>
                        <head>
                            <msg_id>消息ID</msg_id>
                            <state_code>状态代码（000表示成功，其他表示失败）</state_code>
                            <state_desc>状态描述</state_desc>
	                        <ws_mark>WMSBillService</ws_mark>
	                        <ws_method>BillCreate</ws_method>
	                        <ws_param></ws_param>
                            <curr_time></curr_time>
	                        <curr_user></curr_user>
                        </head>
                        <datalist>
	                        <data>
                                <bb_type>业务类型</bb_type>
　　　　　                      <bb_uuid>单据UUID，唯一即可</bb_uuid>
	                            <bb_input_date>2013-04-24 16:04:50</bb_input_date>
	                            <bb_input_person>单据录入人</bb_input_person>
	                            <bb_oper_date>2013-04-24</bb_oper_date>
	                            <bb_cig_type>单据卷烟类型</bb_cig_type>
	                            <bb_commerce_code>单据所属单位编号</bb_commerce_code>
                                <bb_flow_code>来源组织机构编号</bb_flow_code>
	                            <bb_flow_type>来源组织机构类型</bb_flow_type>
                                <BB_STATE>单据状态</BB_STATE>
                                <list>
	                                <data_1>
		                                <bd_pcig_code>标准件烟卷烟代码</bd_pcig_code>
		                                <bd_bcig_code>标准条烟卷烟代码</bd_bcig_code>
		                                <bd_bill_all_num1>应出/入货总量（万支）</bd_bill_all_num1>
		                                <bd_bill_fixed_all_num1>缺失/补足总量（万支）</bd_bill_fixed_all_num1>
　　　　　　　                          <BD_BILL_ALL_SCAN_NUM1>实际扫描总万支数</BD_BILL_ALL_SCAN_NUM1>
	                                </data_1>
	                                <data_1>
	                                </data_1>
                                </list>
                                <CONTRACT_LIST>
                                    <CONTRACT_BASE_1>
                                        <CONTRACT_NO>合同编号</CONTRACT_NO>
                                        <A_NO>供方编号（合同乙方）</A_NO>
                                        <B_NO>需方编号（合同甲方）</B_NO>
                                        <CONTRACT_DATE>合同日期</CONTRACT_DATE>
                                        <START_DATE>发货时间开始</START_DATE>
                                        <END_DATE>发货时间截止</END_DATE>
                                        <SEND_CODE>发货地点（编号）</SEND_CODE>
                                        <SEND_ADD>发货地点（地址）</SEND_ADD>
                                        <RECEIVE_CODE>到货地点（编号）</RECEIVE_CODE>
                                        <RECEIVE_ADD>到货地点（地址）</RECEIVE_ADD>
                                        <SALE_DATE>交易年度</SALE_DATE>
                                        <USE_STATE>合同使用状态：1-主合同；2-关联合同</USE_STATE>
                                        <CONTRACT_DETAIL_1>
                                            <CONTRACT_NO>合同编号</CONTRACT_NO>
                                            <BRAND_CODE>件烟牌号规格代码</BRAND_CODE>
                                            <NUM>数量（万支</NUM>
                                            <PRICE>单价（元）</PRICE>
                                            <SUM>金额（元）</SUM>
                                            <SUM_TAX>含税总金额（元）</SUM_TAX>
                                        </CONTRACT_DETAIL_1>
　　　　　　　　                        <CONTRACT_DETAIL_1>
                                        </CONTRACT_DETAIL_1>
                                    </CONTRACT_BASE_1>
                                <CONTRACT_BASE_1>
                                </CONTRACT_BASE_1>
                            </CONTRACT_LIST>
                            <ZYZ_LIST>
                                <ZYZ_BASE_1>
                                    <PERM_ID>微机编号</PERM_ID>
                                    <PERM_DATE>制证日期</PERM_DATE>
                                    <TRUCK_NO>车牌号</TRUCK_NO>
                                    <ZYZ_CONTRACT_1>
                                        <CONTRACT_CODE>准运证关联的合同号1</CONTRACT_CODE>
                                        <CONTRACT_CODE>准运证关联的合同号2</CONTRACT_CODE>
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
            XElement doc = XElement.Parse(xml);

            #region BillMaster
            var billMaster = from data in doc.Descendants("data")                 //定位到节点 
                             //.Where(w => w.Element("to").Value.Contains('@'))//若要筛选就用上这个语句 
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
            foreach (var bm in billMaster)
            {
                BillMaster entityBM = new BillMaster();
                entityBM.BillType = bm.BillType;
                entityBM.UUID = bm.UUID;
                entityBM.BillDate = Convert.ToDateTime(bm.BillDate);
                entityBM.MakerName = bm.MakerName;
                entityBM.OperateDate = Convert.ToDateTime(bm.OperateDate);
                entityBM.CigaretteType = bm.CigaretteType;
                entityBM.BillCompanyCode = bm.BillCompanyCode;
                entityBM.SupplierCode = bm.SupplierCode;
                entityBM.SupplierType = bm.SupplierType;
                entityBM.State = bm.State;

                b = factory.GetService<IBillMasterService>().Add(entityBM, out strResult);

                foreach (var bd in bm.BillDetail)
                {
                    BillDetail entityBD = new BillDetail();
                    entityBD.PieceCigarCode = bd.PieceCigarCode;
                    entityBD.BoxCigarCode = bd.BoxCigarCode;
                    entityBD.BillQuantity = Convert.ToInt32(bd.BillQuantity);
                    entityBD.FixedQuantity = Convert.ToInt32(bd.FixedQuantity);
                    entityBD.RealQuantity = Convert.ToInt32(bd.RealQuantity);
                }
            }
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
