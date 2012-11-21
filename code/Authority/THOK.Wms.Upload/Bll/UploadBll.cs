using System;
using System.Collections.Generic;
using System.Text;
using THOK.WMS.Upload.Dao;
using System.Data;
using THOK.Util;

namespace THOK.WMS.Upload.Bll
{
    public class UploadBll
    {
        #region �ϱ�����
        /// <summary>
        /// �ϱ�������Ϣ����
        /// </summary>
        /// <param name="brandSet"></param>
        public void InsertProduct(DataSet brandSet)
        {
            using (PersistentManager pm = new PersistentManager("ZYDB2Connection"))
            {
                UploadDao dao = new UploadDao();
                dao.SetPersistentManager(pm);
                dao.InsertProduct(brandSet);
            }
        }
        /// <summary>
        /// ��֯������
        /// </summary>
        /// <param name="companySet"></param>
        public void UploadOrganization(DataSet companySet)
        {
            using (PersistentManager pm = new PersistentManager("ZYDB2Connection"))
            {
                UploadDao dao = new UploadDao();
                dao.SetPersistentManager(pm);
                dao.InsertCompany(companySet);
            }
        }
        //��Ա��Ϣ��
        public void UploadEmployee(DataSet employee)
        {
            using (PersistentManager pm = new PersistentManager("ZYDB2Connection"))
            {
                UploadDao dao = new UploadDao();
                dao.SetPersistentManager(pm);
                dao.InsertEmployee(employee);
            }
        }
        /// <summary>
        /// �ϱ��ͻ���Ϣ����
        /// </summary>
        /// <param name="costomerSet"></param>
        public void InsertCustom(DataSet costomerSet)
        {
            using (PersistentManager pm = new PersistentManager("ZYDB2Connection"))
            {
                UploadDao dao = new UploadDao();
                dao.SetPersistentManager(pm);
                dao.InsertCustom(costomerSet);
            }
        }
        //�ִ����Ա�
        public void UploadCell(DataSet cell)
        {
            using (PersistentManager pm = new PersistentManager("ZYDB2Connection"))
            {
                UploadDao dao = new UploadDao();
                dao.SetPersistentManager(pm);
                dao.InsertCell(cell);
            }
        }
        //�ֿ����
        public void QueryStoreStock(DataSet storeStock)
        {
            using (PersistentManager pm = new PersistentManager("ZYDB2Connection"))
            {
                UploadDao dao = new UploadDao();
                dao.SetPersistentManager(pm);
                dao.InsertStoreStock(storeStock);
            }
        }
        //ҵ�����
        public void QueryBusiStock(DataSet busiStock)
        {
            using (PersistentManager pm = new PersistentManager("ZYDB2Connection"))
            {
                UploadDao dao = new UploadDao();
                dao.SetPersistentManager(pm);
                dao.InsertBustStock(busiStock);
            }
        }
        //�������
        public void InsertInMasterBill(DataSet busiStock)
        {
            using (PersistentManager pm = new PersistentManager("ZYDB2Connection"))
            {
                UploadDao dao = new UploadDao();
                dao.SetPersistentManager(pm);
                dao.InsertInMasterBill(busiStock);
            }
        }
        //���ϸ��
        public void InsertInDetailBill(DataSet busiStock)
        {
            using (PersistentManager pm = new PersistentManager("ZYDB2Connection"))
            {
                UploadDao dao = new UploadDao();
                dao.SetPersistentManager(pm);
                dao.InsertInDetailBill(busiStock);
            }
        }
        //���ҵ���
        public void InsertInBusiBill(DataSet busiStock)
        {
            using (PersistentManager pm = new PersistentManager("ZYDB2Connection"))
            {
                UploadDao dao = new UploadDao();
                dao.SetPersistentManager(pm);
                dao.InsertInBusiBill(busiStock);
            }
        }
        //��������
        public void InsertOutMasterBill(DataSet busiStock)
        {
            using (PersistentManager pm = new PersistentManager("ZYDB2Connection"))
            {
                UploadDao dao = new UploadDao();
                dao.SetPersistentManager(pm);
                dao.InsertOutMasterBill(busiStock);
            }
        }
        //���ⵥ��ϸ��
        public void InsertOutDetailBill(DataSet busiStock)
        {
            using (PersistentManager pm = new PersistentManager("ZYDB2Connection"))
            {
                UploadDao dao = new UploadDao();
                dao.SetPersistentManager(pm);
                dao.InsertOutDetailBill(busiStock);
            }
        }
        //����ҵ�񵥾ݱ�
        public void InsertOutBusiBill(DataSet busiStock)
        {
            using (PersistentManager pm = new PersistentManager("ZYDB2Connection"))
            {
                UploadDao dao = new UploadDao();
                dao.SetPersistentManager(pm);
                dao.InsertOutBusiBill(busiStock);
            }
        }

        // �ּ𶩵��ϱ�(����ϸ��)
        public void uploadSort(DataSet masterds, DataSet detailds)
        {
            using (PersistentManager pm = new PersistentManager("ZYDB2Connection"))
            {
                UploadDao dao = new UploadDao();
                dao.SetPersistentManager(pm);
                if (masterds.Tables["DWV_OUT_ORDER"].Rows.Count > 0)
                {
                    dao.UploadIordOrder(masterds);
                }
                if (detailds.Tables["DWV_OUT_ORDER_DETAIL"].Rows.Count > 0)
                {
                    dao.UploadIordOrderDetail(detailds);
                }
            }
        }

        //ͬ��״̬��
        public void InsertSynchro()
        {
            using (PersistentManager pm = new PersistentManager("ZYDB2Connection"))
            {
                UploadDao dao = new UploadDao();
                dao.SetPersistentManager(pm);
                dao.InsertSynchro();
            }
        }
        #endregion
    }
}
