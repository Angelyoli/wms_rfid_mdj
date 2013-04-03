using System;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.DbModel;
using System.Linq;

namespace THOK.Wms.Bll.Service
{
    public class SRMService : ServiceBase<SRM>, ISRMService
    {
        [Dependency]
        public ISRMRepository SRMRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public object GetDetails(int page, int rows,string SRMName,string State)
        {
            IQueryable<SRM> srmQuery = SRMRepository.GetQueryable();
            var srm = srmQuery.Where(s => s.SRMName.Contains(SRMName) && s.State.Contains(State))
                .OrderBy(s => s.ID).AsEnumerable()
                .Select(s => new
                {
                    s.ID,
                    s.SRMName,
                    s.Description,
                    State = s.State == "01" ? "可用" : "不可用",
                });
           
            int total = srm.Count();
            srm = srm.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = srm.ToArray() };
        }

        public bool Add(SRM srm, out string strResult)
        {
            strResult = string.Empty;
            bool result = false;
            var sr = new SRM();
            if (sr != null)
            {
                try
                {
                    sr.SRMName = srm.SRMName;
                    sr.Description = srm.Description;
                    sr.State = srm.State;

                    SRMRepository.Add(sr);
                    SRMRepository.SaveChanges();
                    result = true;
                }
                catch (Exception ex)
                {
                    strResult = "原因：" + ex.Message;
                }
            }
            else
            {
                strResult = "原因：找不到当前登陆用户！请重新登陆！";
            }
            return result;
        }

        public bool Save(SRM srm, out string strResult)
        {
            strResult = string.Empty;
            bool result = false;
            var sr = SRMRepository.GetQueryable().FirstOrDefault(s => s.ID == srm.ID);

            if (sr != null)
            {
                try
                {
                    sr.SRMName = srm.SRMName;
                    sr.Description = srm.Description;
                    sr.State = srm.State;

                    SRMRepository.SaveChanges();
                    result = true;
                }
                catch (Exception ex)
                {
                    strResult = "原因：" + ex.Message;
                }
            }
            else
            {
                strResult = "原因：未找到当前需要修改的数据！";
            }
            return result;
        }

        public bool Delete(int srmId, out string strResult)
        {
            strResult = string.Empty;
            bool result = false;
            var srm = SRMRepository.GetQueryable().FirstOrDefault(s => s.ID == srmId);
            if (srm != null)
            {
                try
                {
                    SRMRepository.Delete(srm);
                    SRMRepository.SaveChanges();
                    result = true;
                }
                catch (Exception)
                {
                    strResult = "原因：已在使用";
                }
            }
            else
            {
                strResult = "原因：未找到当前需要删除的数据！";
            }
            return result;
        }

        public object GetSRM(int page, int rows, string queryString, string value)
        {
            string id = "", srmName = "";

            if (queryString == "id")
            {
                id = value;
            }
            else
            {
                srmName = value;
            }
            IQueryable<SRM> srmQuery = SRMRepository.GetQueryable();
            int Id = Convert.ToInt32(id);
            var srm = srmQuery.Where(s => s.ID == Id && s.SRMName.Contains(srmName) && s.State == "01")
                .OrderBy(s => s.ID).AsEnumerable().
                Select(s => new
                {
                    s.ID,
                    s.SRMName,
                    s.Description,
                    State = s.State == "01" ? "可用" : "不可用"
                });
            int total = srm.Count();
            srm = srm.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = srm.ToArray() };
        }

        public System.Data.DataTable GetSRM(int page, int rows, string srmName, string state, string t)
        {
            IQueryable<SRM> srmQuery = SRMRepository.GetQueryable();
            var srm = srmQuery.Where(s => s.SRMName.Contains(srmName))
                .OrderBy(s => s.ID).AsEnumerable()
                .Select(s => new
                {
                    s.ID,
                    s.SRMName,
                    s.Description,
                    State = s.State == "01" ? "可用" : "不可用"
                });
            if (!state.Equals(""))
            {
                srm = srmQuery.Where(s => s.SRMName.Contains(srmName) && s.State.Contains(state))
                    .OrderBy(s => s.ID).AsEnumerable()
                    .Select(s => new
                    {
                        s.ID,
                        s.SRMName,
                        s.Description,
                        State = s.State == "01" ? "可用" : "不可用"
                    });
            }
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("堆垛机编码", typeof(string));
            dt.Columns.Add("堆垛机名称", typeof(string));
            dt.Columns.Add("描述", typeof(string));
            dt.Columns.Add("是否可用", typeof(string));
            foreach (var item in srm)
            {
                dt.Rows.Add
                    (
                        item.ID,
                        item.SRMName,
                        item.Description,
                        item.State
                    );
            }
            return dt;
        }
    }
}
