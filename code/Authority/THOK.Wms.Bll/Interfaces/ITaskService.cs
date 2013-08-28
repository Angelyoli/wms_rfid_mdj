using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface ITaskService : IService<Task>
    {
        object GetDetails(int page, int rows);
        bool InBillTask(string billNo, out string errInfo);
        bool OutBillTask(string billNo, out string errInfo);
        bool MoveBillTask(string billNo, out string errInfo);
    }
}
