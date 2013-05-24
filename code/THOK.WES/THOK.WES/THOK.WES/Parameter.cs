using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Collections;  
using THOK.ParamUtil;


namespace THOK.WES
{
    public class Parameter: BaseObject
    {
        private string url;
        [CategoryAttribute("[01] 作业任务服务器地址"), DescriptionAttribute("作业任务服务器地址"), Chinese("作业任务服务器地址")]
        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        int _comboBoxItems = 0;
        [Category("[02] 设备类型"), Description("设备"), TypeConverter(typeof(PropertyGridComboBoxItem)), Chinese("设备类型")]
        public int SelectItem
        {
            get { return _comboBoxItems; }
            set { _comboBoxItems = value; }
        }

        private string udpIP;
        [CategoryAttribute("[03] 电子标签参数"), DescriptionAttribute("标签服务地址"), Chinese("地址IP")]
        public string UdpIP
        {
            get { return udpIP; }
            set { udpIP = value; }
        }

        private string udpPort;
        [CategoryAttribute("[03] 电子标签参数"), DescriptionAttribute("标签服务端口"), Chinese("端口")]
        public string UdpPort
        {
            get { return udpPort; }
            set { udpPort = value; }
        }

        private string usedRFID;
        [CategoryAttribute("[04] RFID参数"), DescriptionAttribute("使用RFID"), Chinese("使用RFID")]
        public string UsedRFID
        {
            get { return usedRFID; }
            set { usedRFID = value; }
        }

        private string rfidPort;
        [CategoryAttribute("[04] RFID参数"), DescriptionAttribute("RFID端口"), Chinese("端口")]
        public string RfidPort
        {
            get { return rfidPort; }
            set { rfidPort = value; }
        }

        private string layersNumber;
        [CategoryAttribute("[05] 作业范围"), DescriptionAttribute("货架能操作层数"), Chinese("层数")]
        public string LayersNumber
        {
            get { return layersNumber; }
            set { layersNumber = value; }
        }

        private string isMusicName;
        [CategoryAttribute("[06] 音乐参数"), DescriptionAttribute("是否启用"), Chinese("是否提示(0：不提示，1：提示)")]
        public string IsMusicName
        {
            get { return isMusicName; }
            set { isMusicName = value; }
        } 

        private string musicName;
        [CategoryAttribute("[06] 音乐参数"), DescriptionAttribute("音乐文件"), Chinese("文件地址")]
        public string MusicName
        {
            get { return musicName; }
            set { musicName = value; }
        } 
    }

    public class wareHouse : BaseObject
    {
        private string productCode;
        [CategoryAttribute("产品信息"), DescriptionAttribute("卷烟信息"), Chinese("卷烟编码")]
        public string ProductCode
        {
            get { return productCode; }
            set { productCode = value; }
        }

        private string productName;
        [CategoryAttribute("产品信息"), DescriptionAttribute("卷烟信息"), Chinese("卷烟名称")]
        public string ProductName
        {
            get { return productName; }
            set { productName = value; }
        }

        private string quantity;
        [CategoryAttribute("产品信息"), DescriptionAttribute("卷烟信息"), Chinese("卷烟件数")]
        public string Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        private string quantityBar;
        [CategoryAttribute("产品信息"), DescriptionAttribute("卷烟信息"), Chinese("卷烟条数")]
        public string QuantityBar
        {
            get { return quantityBar; }
            set { quantityBar = value; }
        }

        private string inDate;
        [CategoryAttribute("产品信息"), DescriptionAttribute("卷烟信息"), Chinese("入库时间")]
        public string InDate
        {
            get { return inDate; }
            set { inDate = value; }
        }

        private string whCode;
        [CategoryAttribute("仓库信息"), DescriptionAttribute("仓库信息"), Chinese("仓库编码")]
        public string WhCode
        {
            get { return whCode; }
            set { whCode = value; }
        }

        private string whName;
        [CategoryAttribute("仓库信息"), DescriptionAttribute("仓库信息"), Chinese("仓库名称")]
        public string WhName
        {
            get { return whName; }
            set { whName = value; }
        }

        private string shelfName;
        [CategoryAttribute("仓库信息"), DescriptionAttribute("货架信息"), Chinese("货架名称")]
        public string ShelfName
        {
            get { return shelfName; }
            set { shelfName = value; }
        }

        private string column;
        [CategoryAttribute("仓库信息"), DescriptionAttribute("货架信息"), Chinese("列")]
        public string Column
        {
            get { return column; }
            set { column = value; }
        }

        private string row;
        [CategoryAttribute("仓库信息"), DescriptionAttribute("货架信息"), Chinese("层")]
        public string Row
        {
            get { return row; }
            set { row = value; }
        }

        private string cellCode;
        [CategoryAttribute("仓库信息"), DescriptionAttribute("货位信息"), Chinese("货位编码")]
        public string CellCode
        {
            get { return cellCode; }
            set { cellCode = value; }
        }

        private string cellName;
        [CategoryAttribute("仓库信息"), DescriptionAttribute("货位信息"), Chinese("货位名称")]
        public string CellName
        {
            get { return cellName; }
            set { cellName = value; }
        }
    }


    public abstract class ComboBoxItemTypeConvert : TypeConverter
    {
        public Hashtable _hash = null;
        public ComboBoxItemTypeConvert()
        {
            _hash = new Hashtable();
            GetConvertHash();
        }

        public abstract void GetConvertHash();
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            int[] ids = new int[_hash.Values.Count];
            int i = 0;
            foreach (DictionaryEntry myDE in _hash)
            {
                ids[i++] = (int)(myDE.Key);
            }
            return new StandardValuesCollection(ids);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object v)
        {
            if (v is string)
            {
                foreach (DictionaryEntry myDE in _hash)
                {
                    if (myDE.Value.Equals((v.ToString())))
                        return myDE.Key;
                }
            }
            return base.ConvertFrom(context, culture, v);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture,object v, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                foreach (DictionaryEntry myDE in _hash)
                {
                    if (myDE.Key.Equals(v))
                        return myDE.Value.ToString();
                }
                return "";
            }
            return base.ConvertTo(context, culture, v, destinationType);
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }

    public class PropertyGridComboBoxItem : ComboBoxItemTypeConvert
    {
        public override void GetConvertHash()
        {
            _hash.Add(0, "叉车");

            _hash.Add(1, "叉车与电子标签");

            _hash.Add(2, "电子标签");
        }
    }   
}
