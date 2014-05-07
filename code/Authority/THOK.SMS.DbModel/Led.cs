using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.SMS.DbModel
{
    public class Led
    {
        public Led()
        {
            this.Leds = new List<Led>();
            this.Channels = new List<Channel>();
        }

        public string LedCode { get; set; }
        public string SortingLineCode { get; set; }
        public string LedName { get; set; }
        public string LedType { get; set; }
        public string LedIp { get; set; }
        public int XAxes { get; set; }
        public int YAxes { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string LedGroupCode { get; set; }
        public int OrderNo { get; set; }
        public string Status { get; set; }

        public virtual Led GroupLed { get; set; }

        public virtual ICollection<Led> Leds { get; set; }
        public virtual ICollection<Channel> Channels { get; set; }
    }
}












