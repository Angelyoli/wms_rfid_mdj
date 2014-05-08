using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.SMS.DbModel;

namespace THOK.SMS.Bll.Interfaces
{
    public interface ISortOrderAllotDetailService : IService<SortOrderAllotDetail>
    {
        object GetDetails(int page, int rows, SortOrderAllotDetail SortOrderAllotDetail);
    }
}
