using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;
using THOK.Wms.Bll.Interfaces;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using System.Data;
using THOK.WMS.Upload.Bll;

namespace THOK.Wms.Bll.Service
{
    public class PositionService : ServiceBase<Position>, IPositionService
    {
        [Dependency]
        public IPositionRepository PositionRepository { get; set; }

        UploadBll Upload = new UploadBll();

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

       

        public object GetDetails(int page, int rows, string ID, string PositionName, string PositionType, string SRMName, string RegionID, string State)
        {
            IQueryable<Position> companyQuery = PositionRepository.GetQueryable();
            var position = companyQuery.Where(p => p.PositionName.Contains(PositionName)
                && p.PositionType.Contains(PositionType))
                 .OrderByDescending(p => p.ID).AsEnumerable()
                .Select(p => new
                {
                    p.ID,
                    p.PositionName,
                   
                    p.RegionID,
                    p.SRMName,
                    //PositionType = p.PositionType == "1" ? "正常位置" : p.PositionType == "2" ? "大品种出库位" : "小",
                    p.PositionType,
                    p.TravelPos,
                    p.LiftPos,
                    p.Extension,
                    p.Description,
                    p.HasGoods,
                    p.AbleStockOut,
                    p.AbleStockInPallet,
                    p.TagAddress,
                    p.CurrentTaskID,
                    p.CurrentOperateQuantity,
                    State = p.State == "1" ? "可用" : "不可用",
                    //UpdateTime = p.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss")
                });
            if (!State.Equals(""))
            {
                position = companyQuery.Where(p => p.PositionName.Contains(PositionName)
                    && p.PositionType.Contains(PositionType)
                    && p.State.Contains(State))
                  .OrderByDescending(p => p.ID).AsEnumerable()
                .Select(p => new
                {
                    p.ID,
                    p.PositionName,
                    p.RegionID,
                    p.SRMName,
                    //PositionType = p.PositionType == "1" ? "正常位置" : p.PositionType == "2" ? "大品种出库位" : "小",
                    p.PositionType,
                    p.TravelPos,
                    p.LiftPos,
                    p.Extension,
                    p.Description,
                    p.HasGoods,
                    p.AbleStockOut,
                    p.AbleStockInPallet,
                    p.TagAddress,
                    p.CurrentTaskID,
                    p.CurrentOperateQuantity,
                    State = p.State == "1" ? "可用" : "不可用",
                });
            }
            int total = position.Count();
            position = position.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = position.ToArray() };
        }


        public bool Add(Position position, out string strResult)
        {
            strResult = string.Empty;
            bool result = false;
            var post = new Position();
            var parent = PositionRepository.GetQueryable().FirstOrDefault(p => p.ID == position.ID);

            var posExist = PositionRepository.GetQueryable().FirstOrDefault(p => p.ID == position.ID);
            if (posExist == null)
            {
                if (post != null)
                {
                    try
                    {
                        post.ID = position.ID;
                        //post.CompanyCode = company.CompanyCode;
                        post.PositionName = position.PositionName;
                        post.PositionType = position.PositionType;
                        post.RegionID = position.RegionID;
                        post.SRMName = position.SRMName;
                        post.TravelPos = position.TravelPos;
                        post.LiftPos = position.LiftPos;
                        post.Extension = position.Extension;
                        post.Description = position.Description;
                        post.HasGoods = position.HasGoods;
                        post.AbleStockOut = position.AbleStockOut;
                        post.AbleStockInPallet = position.AbleStockInPallet;
                        post.TagAddress = position.TagAddress;
                        post.CurrentOperateQuantity = position.CurrentOperateQuantity;
                        post.CurrentTaskID = position.CurrentTaskID;
                        post.State = position.State;

                        PositionRepository.Add(post);
                        PositionRepository.SaveChanges();
                        
                        //组织机构上报
                        //DataSet ds = this.Insert(comp);
                        //Upload.UploadOrganization (ds);
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
            }
            else
            {
                strResult = "原因：该编号已存在！";
            }
            return result;
        }


        public bool Save(Position position, out string strResult)
        {
            strResult = string.Empty;
            bool result = false;
            var post = PositionRepository.GetQueryable().FirstOrDefault(p => p.ID == position.ID);
            var par = PositionRepository.GetQueryable().FirstOrDefault(p => p.ID == position.ID);

            if (post != null)
            {
                try
                {
                    post.ID = position.ID;
                    //post.CompanyCode = company.CompanyCode;
                    post.PositionName = position.PositionName;
                    post.PositionType = position.PositionType;
                    post.RegionID = position.RegionID;
                    post.SRMName = position.SRMName;
                    post.TravelPos = position.TravelPos;
                    post.LiftPos = position.LiftPos;
                    post.Extension = position.Extension;
                    post.Description = position.Description;
                    post.HasGoods = position.HasGoods;
                    post.AbleStockOut = position.AbleStockOut;
                    post.AbleStockInPallet = position.AbleStockInPallet;
                    post.TagAddress = position.TagAddress;
                    post.CurrentOperateQuantity = position.CurrentOperateQuantity;
                    post.CurrentTaskID = position.CurrentTaskID;
                    post.State = position.State;

                    //PositionRepository.Add(post);
                    PositionRepository.SaveChanges();
                    //组织机构上报
                    //DataSet ds = this.Insert(comp);
                    //Upload.UploadOrganization (ds);
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


        public bool Delete(int positionId, out string strResult)
        {
            strResult = string.Empty;
            bool result = false;
            //Guid cid = new Guid(positionId);
            var pos = PositionRepository.GetQueryable().FirstOrDefault(p => p.ID == positionId);
            if (pos != null)
            {
                try
                {
                    //Del(PositionRepository, pos.PositionName);
                    PositionRepository.Delete(pos);
                    PositionRepository.SaveChanges();
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


        public object GetPosition(int page, int rows, string queryString, string value)
        {
            string id = "", PositionName = "";

            if (queryString == "id")
            {
                id = value;
            }
            else
            {
                PositionName = value;
            }
            IQueryable<Position> companyQuery = PositionRepository.GetQueryable();
            var position = companyQuery.Where(p => p.PositionName.Contains(PositionName) && p.State == "1")
                .OrderBy(p => p.ID).AsEnumerable()
                .Select(p => new
                {
                    p.ID,
                    p.PositionName,
                    p.RegionID,
                    p.SRMName,
                    //PositionType = p.PositionType == "1" ? "正常位置" : p.PositionType == "2" ? "大品种出库位" : "小",
                    p.PositionType,
                    p.TravelPos,
                    p.LiftPos,
                    p.Extension,
                    p.Description,
                    p.HasGoods,
                    p.AbleStockOut,
                    p.AbleStockInPallet,
                    p.TagAddress,
                    p.CurrentTaskID,
                    p.CurrentOperateQuantity,
                    State = p.State == "1" ? "可用" : "不可用",
                });
            int total = position.Count();
            position = position.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = position.ToArray() };
        }


        public System.Data.DataTable GetPosition(int page, int rows, string id, string positioNname, string positionType, string sRMName, string regionID, string state)
        {
            IQueryable<Position> companyQuery = PositionRepository.GetQueryable();
            var position = companyQuery.Where(p => p.PositionName.Contains(positioNname)
                && p.PositionType.Contains(positionType))
                .OrderByDescending(p => p.ID).AsEnumerable()
                .Select(p => new
                {
                    p.ID,
                    p.PositionName,
                    p.RegionID,
                    p.SRMName,
                    //PositionType = p.PositionType == "1" ? "正常位置" : p.PositionType == "2" ? "大品种出库位" : "小",
                    p.PositionType,
                    p.TravelPos,
                    p.LiftPos,
                    p.Extension,
                    p.Description,
                    p.HasGoods,
                    p.AbleStockOut,
                    p.AbleStockInPallet,
                    p.TagAddress,
                    p.CurrentTaskID,
                    p.CurrentOperateQuantity,
                    State = p.State == "1" ? "可用" : "不可用",
                });
            if (!state.Equals(""))
            {
                position = companyQuery.Where(p => p.PositionName.Contains(positioNname)
                    && p.PositionType.Contains(positionType)
                    && p.State.Contains(state))
                .OrderByDescending(p => p.ID).AsEnumerable()
                .Select(p => new
                {
                    p.ID,
                    p.PositionName,
                    p.RegionID,
                    p.SRMName,
                    //PositionType = p.PositionType == "1" ? "正常位置" : p.PositionType == "2" ? "大品种出库位" : "小",
                    p.PositionType,
                    p.TravelPos,
                    p.LiftPos,
                    p.Extension,
                    p.Description,
                    p.HasGoods,
                    p.AbleStockOut,
                    p.AbleStockInPallet,
                    p.TagAddress,
                    p.CurrentTaskID,
                    p.CurrentOperateQuantity,
                    State = p.State == "1" ? "可用" : "不可用",
                });
            }
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("位置ID", typeof(int));
            dt.Columns.Add("位置名称", typeof(string));
            dt.Columns.Add("位置类型", typeof(string));
            dt.Columns.Add("区域ID", typeof(int));
            dt.Columns.Add("堆垛机名称", typeof(string));
            dt.Columns.Add("行走位置", typeof(int));
            dt.Columns.Add("升降位置", typeof(int));
            dt.Columns.Add("货叉伸位", typeof(string));
            dt.Columns.Add("描述", typeof(string));
            dt.Columns.Add("是否有货物", typeof(bool));
            dt.Columns.Add("可否出库", typeof(bool));
            dt.Columns.Add("可否叠空托盘", typeof(bool));

            dt.Columns.Add("电子标签地址", typeof(string));
            dt.Columns.Add("当前任务ID", typeof(int));
            dt.Columns.Add("当前操作数量", typeof(int));
            dt.Columns.Add("状态", typeof(string));
           
            foreach (var item in position)
            {
                dt.Rows.Add
                    (
                    item.ID,
                    item.PositionName,
                    item.RegionID,
                    item.SRMName,
                    //PositionType = p.PositionType == "1" ? "正常位置" : p.PositionType == "2" ? "大品种出库位" : "小",
                    item.PositionType,
                    item.TravelPos,
                    item.LiftPos,
                    item.Extension,
                    item.Description,
                    item.HasGoods,
                    item.AbleStockOut,
                    item.AbleStockInPallet,
                    item.TagAddress,
                    item.CurrentTaskID,
                    item.CurrentOperateQuantity,
                    item.State
                    );

            }
            return dt;
        }
    }
}
