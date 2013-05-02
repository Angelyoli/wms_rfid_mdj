using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using THOK.Wms.DbModel;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.Dal.Interfaces;
using System.Collections;

namespace THOK.Wms.Bll.Service
{
    public class PalletService : ServiceBase<Pallet>, IPalletService
    {
        [Dependency]
        public IPalletRepository PalletRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public string Add(Pallet palletAdd)
        {
            string resultMsg = "";
            Pallet pallet = new Pallet();
            var pall = PalletRepository.GetQueryable().FirstOrDefault(p => p.PalletID == palletAdd.PalletID);
            if (pall != null)
            {
                try
                {
                    pall.WmsUUID = palletAdd.WmsUUID;
                    pall.UUID = palletAdd.UUID;
                    pall.TicketNo = palletAdd.TicketNo;
                    if (palletAdd.OperateDate>Convert.ToDateTime("0002-1-1"))
                    { 
                        pall.OperateDate = palletAdd.OperateDate; 
                    }
                    pall.OperateType = palletAdd.OperateType;
                    pall.BarCodeType = palletAdd.BarCodeType;
                    pall.RfidAntCode = palletAdd.RfidAntCode;
                    pall.PieceCigarCode = palletAdd.PieceCigarCode;
                    pall.BoxCigarCode = palletAdd.BoxCigarCode;
                    pall.CigaretteName = palletAdd.CigaretteName;
                    pall.Quantity = palletAdd.Quantity;
                    pall.ScanTime = palletAdd.ScanTime;

                    PalletRepository.SaveChanges();
                    return resultMsg = "";
                }
                catch (Exception e)
                {
                    resultMsg = "发送失败：" + e.Message;
                    return resultMsg;
                }
            }
            else
            {
                try
                {
                    pallet.PalletID = palletAdd.PalletID;
                    pallet.WmsUUID = palletAdd.WmsUUID;
                    pallet.UUID = palletAdd.UUID;
                    pallet.TicketNo = palletAdd.TicketNo;
                    if (palletAdd.OperateDate > Convert.ToDateTime("0002-1-1"))
                    {
                        pallet.OperateDate = palletAdd.OperateDate;
                    }
                    pallet.OperateType = palletAdd.OperateType;
                    pallet.BarCodeType = palletAdd.BarCodeType;
                    pallet.RfidAntCode = palletAdd.RfidAntCode;
                    pallet.PieceCigarCode = palletAdd.PieceCigarCode;
                    pallet.BoxCigarCode = palletAdd.BoxCigarCode;
                    pallet.CigaretteName = palletAdd.CigaretteName;
                    pallet.Quantity = palletAdd.Quantity;
                    pallet.ScanTime = palletAdd.ScanTime;

                    PalletRepository.Add(pallet);
                    PalletRepository.SaveChanges();
                    return resultMsg = "";
                }
                catch (Exception e)
                {
                    resultMsg = "发送失败：" + e.Message;
                    return resultMsg;
                }
            }
        }
    }
}
