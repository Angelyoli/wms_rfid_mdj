using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Data.Linq;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using ELDB;
using System.IO.Log;
using System.IO;
using THOK.Zeng.ComfixtureHandle.el100;
using THOK.Zeng.ComfixtureHandle;
using DataRabbit.DBAccessing.Application;
using DataRabbit.DBAccessing.ORM;
using DataRabbit.DBAccessing.Relation;
using DataRabbit;
using System.Collections;

namespace LabelServer
{
    class TextStaticFun
    {
        public static void test123()
        {
            using (TransactionScope scope = ELDB2.DBFactory.NewTransactionScope(false))
            {
                IRelationAccesser r = scope.NewRelationAccesser();
                DataTable tb = r.DoQuery("select * from storages order by Address").Tables[0];

                // =================================
                System.Console.WriteLine(System.DateTime.Now.Second + "：" + System.DateTime.Now.Millisecond);
                ELDB2.HashSelectHandle h = new ELDB2.HashSelectHandle(tb);
                int j = 0;
                for (int k = 0; k < 1000; k++)
                {
                    foreach (DataRow row in tb.Rows)
                    {
                        Hashtable t = h.Select<string>("Address", row.Field<string>("Address").Trim());

                        for (int i = 0; i < t.Count; i++)
                        {
                            j++;
                        }
                    }
                }
                System.Console.WriteLine(j.ToString());
                System.Console.WriteLine(System.DateTime.Now.Second + "：" + System.DateTime.Now.Millisecond);
                // =================================
                scope.Commit();
            }
        }

        public static void test124()
        {
            using (TransactionScope scope = ELDB2.DBFactory.NewTransactionScope(false))
            {
                IRelationAccesser r = scope.NewRelationAccesser();
                DataTable tb = r.DoQuery("select * from storages order by Address").Tables[0];
                // ========================
                System.Console.WriteLine(System.DateTime.Now.Second + "：" + System.DateTime.Now.Millisecond);
                int j = 0;
                for (int i = 0; i < 1000; i++)
                {
                    foreach (DataRow row0 in tb.Rows)
                    {
                        DataRow[] rows = tb.Select(string.Format("Address = '{0}'", row0["Address"]));
                        foreach (DataRow row in rows)
                        {
                            j++;
                        }
                    }
                }
                System.Console.WriteLine(j.ToString());
                System.Console.WriteLine(System.DateTime.Now.Second + "：" + System.DateTime.Now.Millisecond);
                // =========================
                scope.Commit();
            }
        }
        public void Run0(bool isHash)
        {

            IList<ELDB2.Storages> list = null;
            IList<ELDB2.Storages> list1 = null;
            using (TransactionScope scope = ELDB2.DBFactory.NewTransactionScope(false))
            {
                System.Console.WriteLine(System.DateTime.Now.Second + "：" + System.DateTime.Now.Millisecond);
                ELDB2.HashSelectHandle hashTable = null;
                Hashtable  hashTable1 = null;

                IOrmAccesser<ELDB2.Storages> storages = ELDB2.DBFactory.NewOrmAccesser<ELDB2.Storages>(scope);
                Filter filter = new Filter(ELDB2.Storages._Sign, 0, ComparisonOperators.Equal);
                list1 = storages.GetMuch(filter);

                if (isHash)
                {
                    IRelationAccesser r = scope.NewRelationAccesser();
                    DataTable tb = r.DoQuery("select * from storages order by Address").Tables[0];
                    hashTable = new ELDB2.HashSelectHandle(tb);
                }
                else
                {
                    list = storages.GetAll();
                }


                for (int mm = 0; mm < 1; mm++)
                {
                    if (list1 != null)
                    {
                        foreach (ELDB2.Storages storage1 in list1)
                        {


                            if (isHash)
                            {
                                //hashTable1 = hashTable.Select(ELDB2.Storages._Address, storage1.Address);
                                list = hashTable.Select<ELDB2.Storages,string>(ELDB2.Storages._Address, storage1.Address);
                                IEnumerable<ELDB2.Storages> storages1 = from storage in list
                                                                        where storage.Address == storage1.Address
                                                                        orderby storage.Address
                                                                        select storage;
                                foreach (ELDB2.Storages item in storages1)
                                {
                                    item.Act = "10";
                                    hashTable.Update<ELDB2.Storages, string>(item);
                                }
                            }
                            else
                            {
                                IEnumerable<String> Address = from storage in list
                                                              where storage.Address == storage1.Address
                                                              orderby storage.Address
                                                              select storage.Address;
                                foreach (string item in Address)
                                {
                                    System.Console.WriteLine(item);
                                }
                            }
                        }
                    }

                }
                System.Console.WriteLine(System.DateTime.Now.Second + "：" + System.DateTime.Now.Millisecond);
                // =========================
                scope.Commit();
            }
        }
        public static void testhashclass()
        {
            ELDB2.HashSelectHandle hs = new ELDB2.HashSelectHandle();
            
            Hashtable ht = hs.Select("port", "12");

        }
        public void Run(bool isHash)
        {

            IList<ELDB2.Storages> list = null;
            IList<ELDB2.Storages> list1 = null;
            using (TransactionScope scope = ELDB2.DBFactory.NewTransactionScope(false))
            {
                System.Console.WriteLine(System.DateTime.Now.Second + "：" + System.DateTime.Now.Millisecond);
                ELDB2.HashSelectHandle hashTable = null;
                if (isHash)
                {
                    IRelationAccesser r = scope.NewRelationAccesser();
                    DataTable tb = r.DoQuery("select * from storages order by Address").Tables[0];
                    hashTable = new ELDB2.HashSelectHandle(tb);
                }


                for (int mm = 0; mm < 5; mm++)
                {
                    IOrmAccesser<ELDB2.Storages> storages = ELDB2.DBFactory.NewOrmAccesser<ELDB2.Storages>(scope);
                    Filter filter = new Filter(ELDB2.Storages._Sign, 0, ComparisonOperators.Equal);
                    list1 = storages.GetMuch(filter);
                    if (list1 != null)
                    {
                        foreach (ELDB2.Storages storage1 in list1)
                        {


                            if (isHash)
                            {
                                list = hashTable.Select<ELDB2.Storages, string>(ELDB2.Storages._Address, storage1.Address);
                            }
                            else
                            {
                                Filter filter1 = new Filter(ELDB2.Storages._Address, storage1.Address, ComparisonOperators.Equal);
                                list = storages.GetMuch(filter1);
                            }

                            string[] data = new string[5];
                            for (int i = 0; i < 5; i++)
                                data[i] = "";
                            foreach (ELDB2.Storages storage in list)
                            {
                                string s1, s2, s3, s4;
                                switch ("mode1")
                                {
                                    case "mode1":
                                        s1 = storage.StorageName.Trim() + " " + storage.Act.Trim() + " " + storage.Contents.Trim() + storage.ProductName.Trim() + "".PadRight(56, " "[0]);
                                        s1 = StrHandle.GetStringWith(s1, 56);

                                        if (storage.Row == "3")
                                            data[0] = s1;
                                        if (storage.Row == "2")
                                            data[1] = s1;
                                        if (storage.Row == "1")
                                            data[2] = s1;
                                        break;
                                    case "mode2":
                                        s1 = "储位：" + storage.StorageName.Trim();
                                        s1 = StrHandle.GetStringWith(s1, 24);

                                        s2 = "操作：" + storage.Act.Trim();
                                        s2 = StrHandle.GetStringWith(s2, 24);

                                        s3 = "品牌：" + storage.ProductName.Trim();
                                        s3 = StrHandle.GetStringWith(s3, 24);

                                        s4 = "数量：" + storage.Contents.Trim();
                                        s4 = StrHandle.GetStringWith(s4, 20);

                                        data[0] = s1;
                                        data[0] = data[0] + s2;
                                        data[0] = data[0] + s3;
                                        data[0] = data[0] + s4;
                                        break;
                                    default:
                                        break;
                                }
                                storage.Sign = 0;
                                //storages.Update(storage);
                                IRelationAccesser ra = scope.NewRelationAccesser();

                                //ra.DoCommand("Update [Sy_ShowInfo] set [HardwareReadState] = 1 where ReadState =1 and [StorageID] = '" + storage.StorageID + "'");
                            }
                            //this.elOpertor.SendData(int.Parse(storage1.Address.Substring(storage1.Address.Length - 2, 2)), data);
                        }
                    }

                }
                System.Console.WriteLine(System.DateTime.Now.Second + "：" + System.DateTime.Now.Millisecond);
                // =========================
                scope.Commit();
            }
        }
        public void Run1()
        {
            IList<ELDB2.Storages> list = null;
            using (TransactionScope scope = ELDB2.DBFactory.NewTransactionScope(false))
            {
                IOrmAccesser<ELDB2.Storages> storages = ELDB2.DBFactory.NewOrmAccesser<ELDB2.Storages>(scope);
                Filter filter = new Filter(ELDB2.Storages._Sign, 1, ComparisonOperators.Equal);
                ELDB2.Storages storage1 = storages.GetOne(filter);
                if (storage1 != null)
                {
                    Filter filter1 = new Filter(ELDB2.Storages._Address, storage1.Address, ComparisonOperators.Equal);
                    list = storages.GetMuch(filter1);

                    string[] data = new string[5];
                    for (int i = 0; i < 5; i++)
                        data[i] = "";
                    foreach (ELDB2.Storages storage in list)
                    {
                        string s1, s2, s3, s4;
                        switch ("mode1")
                        {
                            case "mode1":
                                s1 = storage.StorageName.Trim() + " " + storage.Act.Trim() + " " + storage.Contents.Trim() + storage.ProductName.Trim() + "".PadRight(56, " "[0]);
                                s1 = StrHandle.GetStringWith(s1, 56);

                                if (storage.Row == "3")
                                    data[0] = s1;
                                if (storage.Row == "2")
                                    data[1] = s1;
                                if (storage.Row == "1")
                                    data[2] = s1;
                                break;
                            case "mode2":
                                s1 = "储位：" + storage.StorageName.Trim();
                                s1 = StrHandle.GetStringWith(s1, 24);

                                s2 = "操作：" + storage.Act.Trim();
                                s2 = StrHandle.GetStringWith(s2, 24);

                                s3 = "品牌：" + storage.ProductName.Trim();
                                s3 = StrHandle.GetStringWith(s3, 24);

                                s4 = "数量：" + storage.Contents.Trim();
                                s4 = StrHandle.GetStringWith(s4, 20);

                                data[0] = s1;
                                data[0] = data[0] + s2;
                                data[0] = data[0] + s3;
                                data[0] = data[0] + s4;
                                break;
                            default:
                                break;
                        }
                        storage.Sign = 2;
                        storages.Update(storage);
                        IRelationAccesser ra = scope.NewRelationAccesser();

                        ra.DoCommand("Update [Sy_ShowInfo] set [HardwareReadState] = 1 where ReadState =1 and [StorageID] = '" + storage.StorageID + "'");
                    }
                    //this.elOpertor.SendData(int.Parse(storage1.Address.Substring(storage1.Address.Length - 2, 2)), data);
                }
                scope.Commit();
            }
        }
        public static void getdata()
        {
            IList<ELDB2.Sy_ShowInfo> list = null;
            IList<ELDB2.Storages> list1 = null;
            using (TransactionScope scope = ELDB2.DBFactory.NewTransactionScope(false))
            {
                IOrmAccesser<ELDB2.Sy_ShowInfo> ShowInfo = ELDB2.DBFactory.NewOrmAccesser<ELDB2.Sy_ShowInfo>(scope);
                Filter filter = new Filter(ELDB2.Sy_ShowInfo._ReadState, 0, ComparisonOperators.Equal);
                list = ShowInfo.GetMuch(filter);
                foreach (ELDB2.Sy_ShowInfo showinfo in list)
                {
                    IOrmAccesser<ELDB2.Storages> storages = ELDB2.DBFactory.NewOrmAccesser<ELDB2.Storages>(scope);
                    Filter filter1 = new Filter(ELDB2.Storages._StorageID, showinfo.StorageID, ComparisonOperators.Equal);

                    foreach (ELDB2.Storages storage in list1)
                    {
                        if (storage.Sign == 0)
                        {
                            switch (showinfo.OperateType)
                            {
                                case 1:
                                    storage.Act = "入库";
                                    break;
                                case 2:
                                    storage.Act = "出库";
                                    break;
                                case 3:
                                    storage.Act = "库存";
                                    break;
                                case 4:
                                    storage.Act = "移入";
                                    break;
                                case 5:
                                    storage.Act = "移出";
                                    break;
                                case 6:
                                    storage.Act = "移库";
                                    break;
                                default:
                                    break;
                            }

                            storage.ProductName = showinfo.TobaccoName;
                            storage.Contents = (showinfo.OperatePiece > 0 ? showinfo.OperatePiece.ToString() + "件" : "") + (showinfo.OperateItem > 0 ? showinfo.OperateItem + "条" : "");
                            storage.Sign = 1;
                            showinfo.ReadState = 1;
                            storages.Update(storage);
                            ShowInfo.Update(showinfo);
                        }
                    }
                }
                scope.Commit();
            }
        }
    }
}
