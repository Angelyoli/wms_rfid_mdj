using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.Download.Interfaces;

namespace THOK.Wms.Bll.Service
{
    class DeliverLineService : ServiceBase<DeliverLine>, IDeliverLineService
    {

        [Dependency]
        public IDeliverLineRepository DeliverLineRepository { get; set; }

        [Dependency]
        public IDeliverLineDownService DeliverLineDownService { get; set; }


        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public bool DownDeliverLine(out string errorInfo)
        {
            errorInfo = string.Empty;
            bool result = false;
            try
            {
                var deliverLineCodes = DeliverLineRepository.GetQueryable().Where(d => d.DeliverLineCode == d.DeliverLineCode).Select(s => new { s.DeliverLineCode }).ToArray();
                string deliverStrs = "";
                for (int i = 0; i < deliverLineCodes.Length; i++)
                {
                    deliverStrs += deliverLineCodes[i].DeliverLineCode + ",";
                }
                DeliverLine[] deliverLines = DeliverLineDownService.GetDeliverLine(deliverStrs);
                foreach (var item in deliverLines)
                {
                    var deliverLine = new DeliverLine();
                    deliverLine.DeliverLineCode = item.DeliverLineCode;
                    deliverLine.DeliverLineName = item.DeliverLineName;
                    deliverLine.DeliverOrder = item.DeliverOrder;
                    deliverLine.Description = item.Description;
                    deliverLine.DistCode = item.DistCode;
                    deliverLine.CustomCode = item.CustomCode;
                    deliverLine.IsActive = item.IsActive;
                    deliverLine.UpdateTime = item.UpdateTime;
                    DeliverLineRepository.Add(deliverLine);
                }
                DeliverLineRepository.SaveChanges();
                result = true;
            }
            catch (Exception e)
            {
                errorInfo = "出错，原因:" + e.Message;
            }
            return result;
        }

    }
}
