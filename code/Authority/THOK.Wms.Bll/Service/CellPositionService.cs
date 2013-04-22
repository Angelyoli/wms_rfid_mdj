using System;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.DbModel;
using System.Linq;

namespace THOK.Wms.Bll.Service
{
    public class CellPositionService : ServiceBase<CellPosition>, ICellPositionService
    {
        [Dependency]
        public ICellPositionRepository CellPositionRepository { get; set; }
        [Dependency]
        public IPositionRepository PositionRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public object GetDetails(int page, int rows,string CellCode, string StockInPosition, string StockOutPosition)
        {
            IQueryable<CellPosition> cellPositionQuery = CellPositionRepository.GetQueryable();
            IQueryable<Position> positionQuery = PositionRepository.GetQueryable();
            var cellPosition = cellPositionQuery.Join(positionQuery,
                                         c => c.StockInPositionID,
                                         p1 => p1.ID,
                                         (c, p1) => new { c.ID, c.CellCode, c.StockInPositionID, c.StockOutPositionID, InName= p1.PositionName })
                                         .Join(positionQuery,
                                         c => c.StockOutPositionID,
                                         p2 => p2.ID,
                                         (c, p2) => new { c.ID, c.CellCode, c.StockInPositionID, c.StockOutPositionID, c.InName, OutName=p2.PositionName })
                                         .Where(p=>p.CellCode.Contains(CellCode) && p.InName.Contains(StockInPosition)&&p.OutName.Contains(StockOutPosition))
                                         .OrderBy(p => p.ID).AsEnumerable()
                                         .Select(p => new
                                         {
                                             p.ID,
                                             p.CellCode,
                                             p.StockInPositionID,
                                             p.InName,
                                             p.StockOutPositionID,
                                             p.OutName
                                         });
            
            int total = cellPosition.Count();
            cellPosition = cellPosition.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = cellPosition.ToArray() };
        }

        public bool Add(CellPosition cellPosition)
        {
            var cp = new CellPosition();
            cp.ID = cellPosition.ID;
            cp.CellCode = cellPosition.CellCode;
            cp.StockInPositionID = cellPosition.StockInPositionID;
            cp.StockOutPositionID = cellPosition.StockOutPositionID;
            CellPositionRepository.Add(cp);
            CellPositionRepository.SaveChanges();
            return true;
        }

        public bool Save(CellPosition CellPosition)
        {
           
            var c = CellPositionRepository.GetQueryable().FirstOrDefault(s => s.ID == CellPosition.ID);
            c.CellCode = CellPosition.CellCode;
            c.StockInPositionID = CellPosition.StockInPositionID;
            c.StockOutPositionID = CellPosition.StockOutPositionID;
            CellPositionRepository.SaveChanges();
            return true;
        }


        public bool Delete(int cellPositionId)
        {
             var cp = CellPositionRepository.GetQueryable().FirstOrDefault(s => s.ID == cellPositionId);
            if (cp != null)
            {
                CellPositionRepository.Delete(cp);
                CellPositionRepository.SaveChanges();
            }
            else
                return false;
            return true;
        }

        public System.Data.DataTable GetCellPosition(int page, int rows, string cellCode)
        {
            IQueryable<CellPosition> cellPositionQuery = CellPositionRepository.GetQueryable();
            IQueryable<Position> positionQuery = PositionRepository.GetQueryable();
            var cellPosition = cellPositionQuery.Join(positionQuery,
                                         c => c.StockInPositionID,
                                         p1 => p1.ID,
                                         (c, p1) => new { c.ID, c.CellCode, c.StockInPositionID, c.StockOutPositionID, InName = p1.PositionName })
                                         .Join(positionQuery,
                                         c => c.StockOutPositionID,
                                         p2 => p2.ID,
                                         (c, p2) => new { c.ID, c.CellCode, c.StockInPositionID, c.StockOutPositionID, c.InName, OutName = p2.PositionName })
                                         .Where(p => p.CellCode.Contains(cellCode))
                                         .OrderBy(p => p.ID).AsEnumerable()
                                         .Select(p => new
                                         {
                                             p.ID,
                                             p.CellCode,
                                             p.InName,
                                             p.OutName
                                         });
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("货位ID", typeof(string));
            dt.Columns.Add("货位代码", typeof(string));
            dt.Columns.Add("入库位置", typeof(string));
            dt.Columns.Add("出库位置", typeof(string));
            foreach (var item in cellPosition)
            {
                dt.Rows.Add
                    (
                        item.ID,
                        item.CellCode,
                        item.InName,
                        item.OutName
                    );
            }
            return dt;
        }
    }
}
