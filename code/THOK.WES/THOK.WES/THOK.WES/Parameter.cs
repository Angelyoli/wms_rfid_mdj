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
        [CategoryAttribute("[01] ��ҵ�����������ַ"), DescriptionAttribute("��ҵ�����������ַ"), Chinese("��ҵ�����������ַ")]
        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        int _comboBoxItems = 0;
        [Category("[02] �豸����"), Description("�豸"), TypeConverter(typeof(PropertyGridComboBoxItem)), Chinese("�豸����")]
        public int SelectItem
        {
            get { return _comboBoxItems; }
            set { _comboBoxItems = value; }
        }

        private string udpIP;
        [CategoryAttribute("[03] ���ӱ�ǩ����"), DescriptionAttribute("��ǩ�����ַ"), Chinese("��ַIP")]
        public string UdpIP
        {
            get { return udpIP; }
            set { udpIP = value; }
        }

        private string udpPort;
        [CategoryAttribute("[03] ���ӱ�ǩ����"), DescriptionAttribute("��ǩ����˿�"), Chinese("�˿�")]
        public string UdpPort
        {
            get { return udpPort; }
            set { udpPort = value; }
        }

        private string usedRFID;
        [CategoryAttribute("[04] RFID����"), DescriptionAttribute("ʹ��RFID"), Chinese("ʹ��RFID")]
        public string UsedRFID
        {
            get { return usedRFID; }
            set { usedRFID = value; }
        }

        private string rfidPort;
        [CategoryAttribute("[04] RFID����"), DescriptionAttribute("RFID�˿�"), Chinese("�˿�")]
        public string RfidPort
        {
            get { return rfidPort; }
            set { rfidPort = value; }
        }

        private string layersNumber;
        [CategoryAttribute("[05] ��ҵ��Χ"), DescriptionAttribute("�����ܲ�������"), Chinese("����")]
        public string LayersNumber
        {
            get { return layersNumber; }
            set { layersNumber = value; }
        }

        private string isMusicName;
        [CategoryAttribute("[06] ���ֲ���"), DescriptionAttribute("�Ƿ�����"), Chinese("�Ƿ���ʾ(0������ʾ��1����ʾ)")]
        public string IsMusicName
        {
            get { return isMusicName; }
            set { isMusicName = value; }
        } 

        private string musicName;
        [CategoryAttribute("[06] ���ֲ���"), DescriptionAttribute("�����ļ�"), Chinese("�ļ���ַ")]
        public string MusicName
        {
            get { return musicName; }
            set { musicName = value; }
        } 
    }

    public class wareHouse : BaseObject
    {
        private string productCode;
        [CategoryAttribute("��Ʒ��Ϣ"), DescriptionAttribute("������Ϣ"), Chinese("���̱���")]
        public string ProductCode
        {
            get { return productCode; }
            set { productCode = value; }
        }

        private string productName;
        [CategoryAttribute("��Ʒ��Ϣ"), DescriptionAttribute("������Ϣ"), Chinese("��������")]
        public string ProductName
        {
            get { return productName; }
            set { productName = value; }
        }

        private string quantity;
        [CategoryAttribute("��Ʒ��Ϣ"), DescriptionAttribute("������Ϣ"), Chinese("���̼���")]
        public string Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        private string quantityBar;
        [CategoryAttribute("��Ʒ��Ϣ"), DescriptionAttribute("������Ϣ"), Chinese("��������")]
        public string QuantityBar
        {
            get { return quantityBar; }
            set { quantityBar = value; }
        }

        private string inDate;
        [CategoryAttribute("��Ʒ��Ϣ"), DescriptionAttribute("������Ϣ"), Chinese("���ʱ��")]
        public string InDate
        {
            get { return inDate; }
            set { inDate = value; }
        }

        private string whCode;
        [CategoryAttribute("�ֿ���Ϣ"), DescriptionAttribute("�ֿ���Ϣ"), Chinese("�ֿ����")]
        public string WhCode
        {
            get { return whCode; }
            set { whCode = value; }
        }

        private string whName;
        [CategoryAttribute("�ֿ���Ϣ"), DescriptionAttribute("�ֿ���Ϣ"), Chinese("�ֿ�����")]
        public string WhName
        {
            get { return whName; }
            set { whName = value; }
        }

        private string shelfName;
        [CategoryAttribute("�ֿ���Ϣ"), DescriptionAttribute("������Ϣ"), Chinese("��������")]
        public string ShelfName
        {
            get { return shelfName; }
            set { shelfName = value; }
        }

        private string column;
        [CategoryAttribute("�ֿ���Ϣ"), DescriptionAttribute("������Ϣ"), Chinese("��")]
        public string Column
        {
            get { return column; }
            set { column = value; }
        }

        private string row;
        [CategoryAttribute("�ֿ���Ϣ"), DescriptionAttribute("������Ϣ"), Chinese("��")]
        public string Row
        {
            get { return row; }
            set { row = value; }
        }

        private string cellCode;
        [CategoryAttribute("�ֿ���Ϣ"), DescriptionAttribute("��λ��Ϣ"), Chinese("��λ����")]
        public string CellCode
        {
            get { return cellCode; }
            set { cellCode = value; }
        }

        private string cellName;
        [CategoryAttribute("�ֿ���Ϣ"), DescriptionAttribute("��λ��Ϣ"), Chinese("��λ����")]
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
            _hash.Add(0, "�泵");

            _hash.Add(1, "�泵����ӱ�ǩ");

            _hash.Add(2, "���ӱ�ǩ");
        }
    }   
}
