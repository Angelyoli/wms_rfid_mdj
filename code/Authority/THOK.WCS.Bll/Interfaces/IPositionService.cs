using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface IPositionService : IService<Position>
    {
        object GetDetails(int page, int rows, Position positions);

        bool Add(Position position);

        bool Save(Position position);

        bool Delete(int positionId);

        object GetPosition(int page, int rows, string queryString, string value);

        System.Data.DataTable GetPosition(int page, int rows , Position position);
    }
}
