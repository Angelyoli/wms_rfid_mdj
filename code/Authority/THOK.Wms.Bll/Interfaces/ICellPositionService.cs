using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface ICellPositionService : IService<CellPosition>
    {
        //object GetDetails(int page, int rows, string CellCode, string StockInPosition, string StockOutPosition);

        bool Add(CellPosition cellPosition);

        //bool Save(CellPosition cellPosition);

        bool Delete(int cellPositionId);

        //System.Data.DataTable GetCellPosition(int page, int rows, string cellCode);

        //bool Save(CellPosition cellPosition, out string strResult);

        System.Data.DataTable GetCellPosition(int page, int rows, CellPosition cp);



        object GetDetails(int page, int rows, string CellCode, string CellName, string StockInPosition, string StockOutPosition);

        bool Save(CellPosition cellPosition);
    }
}
