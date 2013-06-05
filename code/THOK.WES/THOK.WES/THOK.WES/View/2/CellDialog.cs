using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Reflection;
using THOK.ParamUtil;

namespace THOK.WES.View
{
    public partial class CellDialog : Form
    {
        private wareHouse ware = new wareHouse();

        public CellDialog(Dictionary<string, Dictionary<string, object>> properties)
        {
            InitializeComponent();

            DictionaryPropertyGridAdapter adapter = new DictionaryPropertyGridAdapter();
            foreach (string key in properties.Keys)
            {
                adapter.Add(key, properties[key], true);
            }
            pgCell.SelectedObject = adapter;            
        }

        public CellDialog(wareHouse wh)
        {
            InitializeComponent();
            pgCell.SelectedObject = wh;
        }
    }
}