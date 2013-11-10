using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.WCS.DbModel;
using THOK.WCS.Bll.Interfaces;
using Microsoft.Practices.Unity;
using THOK.WCS.Dal.Interfaces;
using System.Data;

namespace THOK.WCS.Bll.Service
{
    public class PositionService : ServiceBase<Position>, IPositionService
    {
        [Dependency]
        public IPositionRepository PositionRepository { get; set; }
        [Dependency]
        public IRegionRepository RegionRepository { get; set; }


        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

       
        public object GetDetails(int page, int rows,Position positions)
        {
            IQueryable<Position> positionQuery = PositionRepository.GetQueryable();
            IQueryable<Region> regionQuery = RegionRepository.GetQueryable();

            var positionDetail = positionQuery.Join(regionQuery,
                        p => p.RegionID,
                        r => r.ID,
                        (p, r) => new
                        {
                            p.ID,
                            p.PositionName,
                            p.RegionID,
                            r.RegionName,
                            p.SRMName,
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
                            p.ChannelCode,
                            p.State,
                            p.HasGetRequest,
                            p.HasPutRequest
                        })
                .Where(p => p.PositionName.Contains(positions.PositionName) 
                    && p.SRMName.Contains(positions.SRMName)
                    && p.State.Contains(positions.State)
                    && p.PositionType.Contains(positions.PositionType))
                .OrderBy(ul => ul.ID);
            int total = positionDetail.Count();
            var positionDetails = positionDetail.Skip((page - 1) * rows).Take(rows);
            var position_Detail = positionDetails.ToArray().Select(p => new
                {
                    p.ID,
                    p.PositionName,
                    PositionType = p.PositionType == "01" ? "正常位置" : (p.PositionType == "02" ? "大品种出库位" : (p.PositionType == "03" ? "小品种出库位" : (p.PositionType == "04" ? "异形烟出库位" : (p.PositionType == "05" ? "空托盘出库位" : (p.PositionType == "06" ? "出入库位" : "异常"))))),
                    p.RegionID,
                    p.RegionName,
                    p.SRMName,
                    p.TravelPos,
                    p.LiftPos,
                    Extension = p.Extension == 0 ? "单右伸" : (p.Extension == 4 ? "双右伸" : (p.Extension == 8 ? "单左伸" : "双左伸")),
                    p.Description,
                    HasGoods = p.HasGoods == true ? "是" : "否",
                    AbleStockOut = p.AbleStockOut == true ? "是" : "否",
                    AbleStockInPallet = p.AbleStockInPallet == true ? "是" : "否",
                    p.TagAddress,
                    p.CurrentTaskID,
                    p.CurrentOperateQuantity,
                    p.ChannelCode,
                    State = p.State == "01" ? "可用" : "不可用",
                    HasGetRequest = p.HasGetRequest == true ? "是" : "否",
                    HasPutRequest = p.HasPutRequest == true ? "是" : "否"
                });

            return new { total, rows = position_Detail.ToArray() };
        }

        
        public bool Add(Position position)
        {
            var post = new Position();
            try
            {
                var region = RegionRepository.GetQueryable().FirstOrDefault(r => r.ID == position.RegionID);
                post.ID = position.ID;
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
                post.ChannelCode = position.ChannelCode;
                post.State = position.State;
                post.HasGetRequest = position.HasGetRequest;
                post.HasPutRequest = position.HasPutRequest;

                PositionRepository.Add(post);
                PositionRepository.SaveChanges();
            }
            catch (Exception e) { }
    
            return true;
        }


        public bool Save(Position position)
        {
            var post = PositionRepository.GetQueryable().FirstOrDefault(p => p.ID == position.ID);

                    post.ID = position.ID;
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
                    post.ChannelCode = position.ChannelCode;
                    post.State = position.State;
                    post.HasGetRequest = position.HasGetRequest;
                    post.HasPutRequest = position.HasPutRequest;

                    PositionRepository.SaveChanges();

            return true;
        }


        public bool Delete(int positionId)
        {
            var pos = PositionRepository.GetQueryable().FirstOrDefault(p => p.ID == positionId);
            if (pos != null)
            {
                PositionRepository.Delete(pos);
                PositionRepository.SaveChanges();
            }
            else
                return false;
            return true;
        }


        public object GetPosition(int page, int rows, string queryString, string value)
        {
            string positionName = "", positionType = "";

            string type1 = "正常位置";
            string type2 = "大品种出库位";
            string type3 = "小品种出库位";
            string type4 = "异形烟出库位";
            string type5 = "空托盘出库位";

            if (queryString == "PositionName")
            {
                positionName = value;
            }
            else
            {
                if (type1.Contains(value)) positionType = "01";
                else if (type2.Contains(value)) positionType = "02";
                else if (type3.Contains(value)) positionType = "03";
                else if (type4.Contains(value)) positionType = "04";
                else if (type5.Contains(value)) positionType = "05";
                else positionType = "异常";
            }
            IQueryable<Position> companyQuery = PositionRepository.GetQueryable();
            var position = companyQuery.Where(p => p.State.Contains("01")
                && p.PositionName.Contains(positionName) && p.PositionType.Contains(positionType))
                .OrderBy(p => p.ID).AsEnumerable()
                .Select(p => new
                {
                    p.ID,
                    p.PositionName,
                    PositionType = p.PositionType == "01" ? type1 : (p.PositionType == "02" ? type2 : (p.PositionType == "03" ? type3 : (p.PositionType == "04" ? type4 : type5))),
                    State = p.State == "01" ? "可用" : "不可用"
                });
            int total = position.Count();
            position = position.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = position.ToArray() };
        }

        public System.Data.DataTable GetPosition(int page, int rows, Position position)
        {
            IQueryable<Position> positionQuery = PositionRepository.GetQueryable();
            IQueryable<Region> regionQuery = RegionRepository.GetQueryable();

            var positionDetail = positionQuery.Join(regionQuery,
                    p => p.RegionID,
                    r => r.ID,
                    (p, r) => new
                    {
                        p.ID,
                        p.PositionName,
                        p.RegionID,
                        r.RegionName,
                        p.SRMName,
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
                        p.ChannelCode,
                        p.State,
                        p.HasGetRequest,
                        p.HasPutRequest
                    })
            .Where(p => p.PositionName.Contains(position.PositionName)
                && p.SRMName.Contains(position.SRMName)
                && p.State.Contains(position.State)
                && p.PositionType.Contains(position.PositionType))
            .OrderBy(ul => ul.ID);
            var position_Detail = positionDetail.ToArray().Select(p => new
            {
                p.ID,
                p.PositionName,
                PositionType = p.PositionType == "01" ? "正常位置" : (p.PositionType == "02" ? "大品种出库位" : (p.PositionType == "03" ? "小品种出库位" : (p.PositionType == "04" ? "异形烟出库位" : "空托盘出库位"))),
                p.RegionID,
                p.RegionName,
                p.SRMName,
                p.TravelPos,
                p.LiftPos,
                HasGoods = p.HasGoods == true ? "是" : "否",
                AbleStockOut = p.AbleStockOut == true ? "是" : "否",
                AbleStockInPallet = p.AbleStockInPallet == true ? "是" : "否",
                p.TagAddress,
                p.CurrentTaskID,
                p.CurrentOperateQuantity,
                p.ChannelCode,
                Extension = p.Extension == 0 ? "单右伸" : (p.Extension == 4 ? "双右伸" : (p.Extension == 8 ? "单左伸" : "双左伸")),
                State = p.State == "01" ? "可用" : "不可用",
                HasGetRequest = p.HasGetRequest == true ? "是" : "否",
                HasPutRequest = p.HasPutRequest == true ? "是" : "否",
                p.Description
            });

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("位置ID", typeof(string));
            dt.Columns.Add("位置名称", typeof(string));
            dt.Columns.Add("位置类型", typeof(string));
            dt.Columns.Add("区域ID", typeof(string));
            dt.Columns.Add("堆垛机名称", typeof(string));
            dt.Columns.Add("行走位置", typeof(string));
            dt.Columns.Add("升降位置", typeof(string));
            dt.Columns.Add("是否有货物", typeof(string));
            dt.Columns.Add("可否出库", typeof(string));
            dt.Columns.Add("可否叠空托盘", typeof(string));
            dt.Columns.Add("电子标签地址", typeof(string));
            dt.Columns.Add("当前任务ID", typeof(string));
            dt.Columns.Add("当前操作数量", typeof(string));
            dt.Columns.Add("补货烟道代码", typeof(string));
            dt.Columns.Add("货叉伸位", typeof(string));
            dt.Columns.Add("状态", typeof(string));
            dt.Columns.Add("取货请求", typeof(string));
            dt.Columns.Add("放货请求", typeof(string));
            dt.Columns.Add("描述", typeof(string));

            foreach (var item in position_Detail)
            {
                dt.Rows.Add
                    (
                    item.ID,
                    item.PositionName,
                    item.RegionName,
                    item.SRMName,
                    item.PositionType,
                    item.TravelPos,
                    item.LiftPos,
                    item.HasGoods,
                    item.AbleStockOut,
                    item.AbleStockInPallet,
                    item.TagAddress,
                    item.CurrentTaskID,
                    item.CurrentOperateQuantity,
                    item.ChannelCode,
                    item.Extension,
                    item.State,
                    item.HasGetRequest,
                    item.HasPutRequest,
                    item.Description
                );

            }
            return dt;
        }
    }
}
