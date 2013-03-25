using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface ISRMService :IService<SRM>
    {
        object GetDetails(int page, int rows, string ID, string SRMName, string Description, string State);

        bool Add(SRM srm, out string strResult);

        bool Save(SRM srm, out string strResult);

        bool Delete(int srmId, out string strResult);

        object GetSRM(int page, int rows, string queryString, string value);

        System.Data.DataTable GetSRM(int page, int rows, string srmName, string state, string t);
    }
}
