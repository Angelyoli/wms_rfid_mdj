using System;
using THOK.SMS.DbModel;
using THOK.SMS.Dal.Interfaces;
using THOK.SMS.Bll.Interfaces;
using System.Linq;
using THOK.Wms.SignalR.Common;
using Microsoft.Practices.Unity;
using THOK.Common.Entity;

namespace THOK.SMS.Bll.Service
{
    public class LedService : ServiceBase<Led>, ILedService
    {
        [Dependency]
        public ILedRepository LedRepository { get; set; }

        [Dependency]
        public IChannelRepository ChannelRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public object GetDetails(int page, int rows, string Status, string LedName, string LedGroupCode, string LedType, string LedCode)
        {
            IQueryable<Led> ledquery = LedRepository.GetQueryable();
            IQueryable<Channel> channelquery = ChannelRepository.GetQueryable();


            var led = ledquery.OrderBy(c => c.LedCode==LedCode).Select(a => new
            {
                a.Height,
                a.LedCode,
                a.LedGroupCode,
                a.LedIp,
                a.LedName,
                a.LedType,
                a.OrderNo,
                a.SortingLineCode,
                a.Status,
                a.Width,
                a.XAxes,
                a.YAxes

            });
            int ss = led.Count();
            if (LedCode != null && LedCode != string.Empty)
            {
                led = led.Where(a => a.LedGroupCode == LedCode);

            }
            if (LedType != null && LedType != string.Empty)
            {
                led = led.Where(a => a.LedType == LedType);
            }
            if (LedName != null && LedName != string.Empty)
            {
                led = led.Where(a => a.LedName.Contains( LedName));

            }
            if (LedGroupCode != null && LedGroupCode != string.Empty)
            {
                led = led.Where(a => a.LedGroupCode == LedGroupCode);

            }
            if (Status != null && Status != string.Empty)
            {
                led = led.Where(a => a.Status == Status);

            }
            var channel = led.OrderByDescending(a => a.LedCode).ToArray()
                .Select(a => new
                {

                    a.Height,
                    a.LedCode,
                    a.LedGroupCode,
                    a.LedIp,
                    a.LedName,
                    LedType = a.LedType == "1" ? "整屏" : "分屏",
                    SortingLineCode = a.SortingLineCode == "01" ? "分拣线A" : "分拣线B",
                    Status = a.Status == "1" ? "可用" : "不可用",
                    OrderNo = a.OrderNo,
                    a.Width,
                    a.XAxes,
                    a.YAxes


                });


            int total = channel.Count();
            var batchsRow = channel.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = channel.ToArray() };
        }


        public bool Add(Led ledInfo, out string strResult)
        {

            string groupCode;
            if (ledInfo.LedGroupCode == "" && ledInfo.LedGroupCode == string.Empty)
            {

                groupCode = null;
            }
            else
            {

                groupCode = ledInfo.LedGroupCode;
            }

            strResult = string.Empty;
            bool result = false;
            var al = LedRepository.GetQueryable();
            if (al != null)
            {

                Led leds = new Led();
                try
                {
                    leds.LedCode = ledInfo.LedCode;                
                    leds.Height = ledInfo.Height;
                    leds.LedGroupCode = groupCode;
                    leds.LedIp ="127.0.0.1"; //ledInfo.LedIp;   结构限制 注意及时修改
                    leds.LedName = ledInfo.LedName;
                    leds.LedType = ledInfo.LedType;
                    leds.OrderNo = ledInfo.OrderNo;
                    leds.Status = ledInfo.Status;
                    leds.Width = ledInfo.Width;
                    leds.XAxes = ledInfo.XAxes;
                    leds.YAxes = ledInfo.YAxes;
                    leds.SortingLineCode =ledInfo.SortingLineCode; 

                    LedRepository.Add(leds);
                    LedRepository.SaveChanges();

                    result = true;
                }
                catch (Exception e)
                {
                    strResult = "原因:" + e.Message;
                }
            }
            else
            {
                strResult = "原因:数据已存在";
            }
            return result;
        }

        public bool Save(Led ledInfo, out string strResult)
        {

            string groupCode;
            if (ledInfo.LedGroupCode == "" && ledInfo.LedGroupCode == string.Empty)
            {

                groupCode = null;
            }
            else
            {

                groupCode = ledInfo.LedGroupCode;
            }


            strResult = string.Empty;
            bool result = false;
            var leds = LedRepository.GetQueryable().FirstOrDefault(a => a.LedCode == ledInfo.LedCode);
            if (leds != null)
            {
                leds.LedCode = ledInfo.LedCode;
                leds.Height = ledInfo.Height;
                leds.LedGroupCode = groupCode;
                leds.LedIp = "127.0.0.1"; //ledInfo.LedIp;   结构限制 注意及时修改
                leds.LedName = ledInfo.LedName;
                leds.LedType = ledInfo.LedType=="整屏"?"1":"2";
                leds.OrderNo = ledInfo.OrderNo;
                leds.Status = ledInfo.Status;// == "可用" ? "1" : "0";
                leds.Width = ledInfo.Width;
                leds.XAxes = ledInfo.XAxes;
                leds.YAxes = ledInfo.YAxes;
                leds.SortingLineCode =ledInfo.SortingLineCode;  

                LedRepository.SaveChanges();
                result = true;
            }
            else
            {

                strResult = "原因:找不到相应数据";
            }
            return result;
        }


        public bool Delete(string LedCode, out string strResult)
        {

            strResult = string.Empty;
            bool result = false;
            var ledInfo = LedRepository.GetQueryable().FirstOrDefault(a => a.LedCode.Contains(LedCode));
         
            if (ledInfo != null)
            {


               
              
                LedRepository.Delete(ledInfo);
                LedRepository.SaveChanges();
                result = true;
            }
            else
            {
                strResult = "原因:没有找到相应数据";

            }
            return result;
        }



        public object GetLedGroupCode(int page, int rows, string QueryString, string Value)
        {
            string LedName = "";
            string LedIp = "";
            string LedType = "1";
            if (QueryString == "LedName")
            {
                LedName = Value;
            }
            else
            {
                LedIp = Value;
            }
            IQueryable<Led> HelpContentQuery = LedRepository.GetQueryable();
            var LedContent = HelpContentQuery.Where(c => c.LedName.Contains(LedName) && c.LedIp.Contains(LedIp));

            //if (!LedIp.Equals(string.Empty))
            //{
            //    LedContent = LedContent.Where(p => p.LedIp == LedIp);
            //}

            if (!LedType.Equals(string.Empty))
            {
                LedContent = LedContent.Where(p => p.LedType == LedType);
            }

            var leds = LedContent.OrderByDescending(a => a.LedCode).AsEnumerable().Select(a => new
            {

                a.LedCode,
                a.LedName,
                a.LedIp,
                LedType = a.LedType == "1" ? "整屏" : "分屏",
                Status = a.Status == "1" ? "可用" : "不可用"
            });


            int total = LedContent.Count();
            leds = leds.Skip((page - 1) * rows).Take(rows);


            return new { total, rows = leds.ToArray() };
        }

    }
}
