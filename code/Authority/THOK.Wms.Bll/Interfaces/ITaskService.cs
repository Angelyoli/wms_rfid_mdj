using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface ITaskService : IService<Task>
    {
        object GetDetails(int page, int rows, Task task);
        bool Add(Task task, out string strResult);
        bool CreateTask(Task task, out string strResult);
        bool Save(Task task, out string strResult);
        bool Delete(string taskID, out string strResult);
        bool InBillTask(string billNo, out string errInfo);
        bool OutBillTask(string billNo, out string errInfo);
        bool MoveBillTask(string billNo, out string errInfo);
    }
}
