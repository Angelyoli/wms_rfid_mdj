using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;

namespace THOK.Wms.Bll.Service
{
    public class NavicertService : ServiceBase<Navicert>, INavicertService
    {
        [Dependency]
        public INavicertRepository NavicertRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public bool Add(Navicert navicert, out string strResult)
        {
            strResult = string.Empty;
            bool result = false;
            var n = new Navicert();
            if (n != null)
            {
                try
                {
                    n.ID = navicert.ID;
                    n.MasterID = navicert.MasterID;
                    n.NavicertCode = navicert.NavicertCode;
                    n.NavicertDate = navicert.NavicertDate;
                    n.TruckPlateNo = navicert.TruckPlateNo;
                    n.ContractCode = navicert.ContractCode;

                    NavicertRepository.Add(n);
                    NavicertRepository.SaveChanges();
                    result = true;
                }
                catch (Exception ex)
                {
                    strResult = "原因：" + ex.Message;
                }
            }
            else
            {
                strResult = "原因：找不到当前登陆用户！请重新登陆！";
            }
            return result;
        }
        public bool Save(Navicert navicert, out string strResult)
        {
            strResult = string.Empty;
            bool result = false;
            var n = NavicertRepository.GetQueryable().FirstOrDefault(c => c.ContractCode == navicert.Contract.ContractCode);
            if (n != null)
            {
                try
                {
                    n.NavicertCode = navicert.NavicertCode;
                    n.NavicertDate = navicert.NavicertDate;
                    n.TruckPlateNo = navicert.TruckPlateNo;

                    NavicertRepository.SaveChanges();
                    result = true;
                }
                catch (Exception ex)
                {
                    strResult = "原因：" + ex.Message;
                }
            }
            return result;
        }
    }
}
