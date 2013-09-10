using System;
using Microsoft.Practices.Unity;
using THOK.WCS.Bll.Interfaces;
using THOK.WCS.Dal.Interfaces;
using THOK.Wms.Dal.Interfaces;
using THOK.WCS.DbModel;
using THOK.Wms.DbModel;
using System.Linq;

namespace THOK.WCS.Bll.Service
{
    public class CellPositionService : ServiceBase<CellPosition>, ICellPositionService
    {
        [Dependency]
        public ICellPositionRepository CellPositionRepository { get; set; }
        [Dependency]
        public IPositionRepository PositionRepository { get; set; }

        [Dependency]
        public ICellRepository CellRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }


        public object GetDetails(int page, int rows, string CellCode, string CellName, string StockInPosition, string StockOutPosition)
        {
            IQueryable<CellPosition> cellPositionQuery = CellPositionRepository.GetQueryable();
            IQueryable<Position> positionQuery = PositionRepository.GetQueryable();
            IQueryable<Cell> cellQuery = CellRepository.GetQueryable();

            var cellPosition = cellPositionQuery.Join(positionQuery,
                                         c => c.StockInPositionID,
                                         p1 => p1.ID,
                                         (c, p1) => new { c.ID, c.CellCode, c.StockInPositionID, c.StockOutPositionID, InName = p1.PositionName })
                                         .Join(positionQuery,
                                         c => c.StockOutPositionID,
                                         p2 => p2.ID,
                                         (c, p2) => new { c.ID, c.CellCode, c.StockInPositionID, c.StockOutPositionID, c.InName, OutName = p2.PositionName })
                                         .Join(cellQuery,
                                         c => c.CellCode,
                                         cq => cq.CellCode,
                                         (c, cq) => new { c.ID, c.CellCode, c.StockInPositionID, c.StockOutPositionID,CellName=cq.CellName,c.InName ,c.OutName }
                                         )
                                         .Where(p => p.CellCode.Contains(CellCode) && p.CellName.Contains(CellName)&&p.InName.Contains(StockInPosition) && p.OutName.Contains(StockOutPosition))
                                         .OrderBy(p => p.ID).AsEnumerable()
                                         .Select(p => new
                                         {
                                             p.ID,
                                             p.CellCode,
                                             p.StockInPositionID,
                                             p.InName,
                                             p.StockOutPositionID,
                                             p.OutName,
                                             p.CellName
                                         });

            int total = cellPosition.Count();
            cellPosition = cellPosition.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = cellPosition.ToArray() };
        }

        public bool Add(CellPosition cellPosition)
        {
            var cp = new CellPosition();
           // cp.ID = cellPosition.ID;
            //cp.CellCode = cellPosition.CellCode;
            cp.CellCode = cellPosition.CellCode;
            cp.StockInPositionID = cellPosition.StockInPositionID;
            cp.StockOutPositionID = cellPosition.StockOutPositionID;
            CellPositionRepository.Add(cp);
            CellPositionRepository.SaveChanges();
            return true;
        }

        public bool Save(CellPosition cellPosition)
        {
              IQueryable<Cell> cellQuery = CellRepository.GetQueryable();
              IQueryable<Position> positionQuery = PositionRepository.GetQueryable();
              var cell = cellQuery.FirstOrDefault(p => p.CellCode == cellPosition.CellCode);
              var c = CellPositionRepository.GetQueryable().FirstOrDefault(s => s.ID == cellPosition.ID);
              //var position = positionQuery.FirstOrDefault(po => po.ID == cellPosition.ID);
              if (c != null)
              {
                  c.ID = cellPosition.ID;
                  c.CellCode = cellPosition.CellCode;
                  c.StockInPositionID = cellPosition.StockInPositionID;
                  c.StockOutPositionID = cellPosition.StockOutPositionID;
                  CellPositionRepository.SaveChanges();
              }
               
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

        public System.Data.DataTable GetCellPosition(int page, int rows, CellPosition cp)
        {
            IQueryable<CellPosition> cellPositionQuery = CellPositionRepository.GetQueryable();
            IQueryable<Position> positionQuery = PositionRepository.GetQueryable();
            IQueryable<Cell> cellQuery = CellRepository.GetQueryable();

            var cellPosition = cellPositionQuery.Join(positionQuery,
                                         c => c.StockInPositionID,
                                         p1 => p1.ID,
                                         (c, p1) => new { c.ID, c.CellCode, c.StockInPositionID, c.StockOutPositionID, InName = p1.PositionName })
                                         .Join(positionQuery,
                                         c => c.StockOutPositionID,
                                         p2 => p2.ID,
                                         (c, p2) => new { c.ID, c.CellCode, c.StockInPositionID, c.StockOutPositionID, c.InName, OutName = p2.PositionName })
                                          .Join(cellQuery,
                                         c => c.CellCode,
                                         cq => cq.CellCode,
                                         (c, cq) => new { c.ID, c.CellCode, c.StockInPositionID, c.StockOutPositionID, CellName = cq.CellName, c.InName, c.OutName }
                                         )
                                         .Where(p => p.CellCode.Contains(cp.CellCode))
                                         .OrderBy(p => p.ID).AsEnumerable()
                                         .Select(p => new
                                         {
                                             p.ID,
                                             p.CellCode,
                                             p.InName,
                                             p.OutName,
                                             p.CellName
                                         });
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("货位ID", typeof(string));
            dt.Columns.Add("货位代码", typeof(string));
            dt.Columns.Add("货位名称",typeof(string));
            dt.Columns.Add("入库位置", typeof(string));
            dt.Columns.Add("出库位置", typeof(string));
            foreach (var item in cellPosition)
            {
                dt.Rows.Add
                    (
                        item.ID,
                        item.CellCode,
                        item.CellName,
                        item.InName,
                        item.OutName
                    );
            }
            return dt;
        }






        
    }
}
