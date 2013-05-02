using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using THOK.Wms.DbModel;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.Bll.Interfaces;

namespace THOK.Wms.Bll.Service
{
    public class ContractService : ServiceBase<Contract>, IContractService
    {
        [Dependency]
        public IContractRepository ContractRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public bool Add(Contract contract, out string strResult)
        {
            strResult = string.Empty;
            bool result = false;
            //var contracts = ContractRepository.GetQueryable().FirstOrDefault(c => c.ContractCode == contract.ContractCode);
            var con = new Contract();
            //if (contracts == null)
            //{
            if (con != null)
            {
                try
                {
                    con.ContractCode = contract.ContractCode;
                    con.MasterID = contract.MasterID;
                    con.SupplySideCode = contract.SupplySideCode;
                    con.DemandSideCode = contract.DemandSideCode;
                    con.ContractDate = contract.ContractDate;
                    con.StartDade = contract.StartDade;
                    con.EndDate = contract.EndDate;
                    con.SendPlaceCode = contract.SendPlaceCode;
                    con.SendAddress = contract.SendAddress;
                    con.ReceivePlaceCode = contract.ReceivePlaceCode;
                    con.ReceiveAddress = contract.ReceiveAddress;
                    con.SaleDate = contract.SaleDate;
                    con.State = contract.State;

                    ContractRepository.Add(con);
                    ContractRepository.SaveChanges();
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
            //}
            //else
            //{
            //    strResult = "原因：该编号已存在！";
            //}
            return result;
        }
        public bool Save(Contract contract, out string strResult)
        {
            strResult = string.Empty;
            bool result = false;
            var con = ContractRepository.GetQueryable().FirstOrDefault(c => c.ContractCode == contract.ContractCode);
            if (con != null)
            {
                try
                {
                    con.SupplySideCode = contract.SupplySideCode;
                    con.DemandSideCode = contract.DemandSideCode;
                    con.ContractDate = contract.ContractDate;
                    con.StartDade = contract.StartDade;
                    con.EndDate = contract.EndDate;
                    con.SendPlaceCode = contract.SendPlaceCode;
                    con.SendAddress = contract.SendAddress;
                    con.ReceivePlaceCode = contract.ReceivePlaceCode;
                    con.ReceiveAddress = contract.ReceiveAddress;
                    con.SaleDate = contract.SaleDate;
                    con.State = contract.State;

                    ContractRepository.SaveChanges();
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
