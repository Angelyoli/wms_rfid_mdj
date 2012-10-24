﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface ISortOrderService : IService<SortOrder>
    {
        object GetDetails(int page, int rows, string OrderID, string orderDate,string productCode);
        object GetDetails(string orderDate);

        bool DownSortOrder(string beginDate, string endDate, out string errorInfo);
    }
}
