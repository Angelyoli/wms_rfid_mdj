using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataRabbit.DBAccessing;
using DataRabbit.DBAccessing.Application;
using DataRabbit.HashOrm;

namespace THOK.Application.LabelServer
{
    public partial class Set : Form
    {
        public Set()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IList<Storages> ModifiyStorages = new List<Storages>();
            IHashOrmAccesser storagesAccesser = DBFactory.NewHashOrmAccesser();
            Storages storage;
            int j = 1;
            for (int i = Convert.ToInt32(textBox2.Text); i <= Convert.ToInt32(textBox4.Text); i++)
            {
                storage = new Storages();// ZA-A-01-01
                storage.StorageID = textBox1.Text.ToString() + "-1-" + j.ToString().PadLeft(1,"0"[0]);
                storage.StorageName = textBox1.Text.ToString() + "-1-" + j.ToString().PadLeft(1, "0"[0]);
                storage.Address = textBox3.Text.ToString() + i.ToString().PadLeft(3, "0"[0]);
                storage.Port = textBox3.Text.ToString();
                storage.Row = "1";
                ModifiyStorages.Add(storage);

                storage = new Storages();// ZA-A-01-01
                storage.StorageID = textBox1.Text.ToString() + "-2-" + j.ToString().PadLeft(1, "0"[0]);
                storage.StorageName = textBox1.Text.ToString() + "-2-" + j.ToString().PadLeft(1, "0"[0]);
                storage.Address = textBox3.Text.ToString() + i.ToString().PadLeft(3, "0"[0]);
                storage.Port = textBox3.Text.ToString();
                storage.Row = "2";
                ModifiyStorages.Add(storage);

                storage = new Storages();// ZA-A-01-01
                storage.StorageID = textBox1.Text.ToString() + "-3-" + j.ToString().PadLeft(1, "0"[0]);
                storage.StorageName = textBox1.Text.ToString() + "-3-" + j.ToString().PadLeft(1, "0"[0]);
                storage.Address = textBox3.Text.ToString() + i.ToString().PadLeft(3, "0"[0]);
                storage.Port = textBox3.Text.ToString();
                storage.Row = "3";
                ModifiyStorages.Add(storage);
                j++;
            }
            storagesAccesser.Insert<Storages>(ModifiyStorages, false);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            IHashOrmAccesser storagesAccesser = DBFactory.NewHashOrmAccesser();
            storagesAccesser.ExecuteNonQuery("delete  from storages ");
        }
    }
}
