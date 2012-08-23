using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.Download.Interfaces;
using THOK.Wms.DbModel;

namespace THOK.Wms.Download.Service
{
    public class DeliverLineDownService : IDeliverLineDownService
    {
        public DeliverLine[] GetDeliverLine()
        {
            using (SortingDbContext sortdb = new SortingDbContext())
            {
                return sortdb.Database.SqlQuery<DeliverLine>(@"
                    SELECT [DELIVER_LINE_CODE] AS DeliverLineCode
                            ,[LINE_TYPE] AS CustomCode
                            ,[DELIVER_LINE_NAME] AS DeliverLineName
                            ,[DIST_STA_CODE] AS DistCode
                            ,[DELIVER_LINE_ORDER] AS DeliverOrder
                            ,'' AS Description
                            ,[ISACTIVE] AS IsActive
                            ,getdate() AS UpdateTime
                        FROM [V_WMS_DELIVER_LINE]
                    "
                    )
                    .ToArray();
            }
        }
    }
}
