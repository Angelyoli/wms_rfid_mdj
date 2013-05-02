using System;

using System.Collections;

using System.ComponentModel;

using System.Configuration.Install;

using System.Data;

using System.Data.SqlClient;

using System.IO;

using System.Reflection;

using System.Text.RegularExpressions;

using System.Windows.Forms;

using System.Text;

using Microsoft.Win32;



namespace install
{

    /// <summary>

    /// Installer 的摘要说明。

    /// </summary>

    [RunInstaller(true)]

    public class Installer : System.Configuration.Install.Installer
    {

        /// <summary>

        /// 必需的设计器变量。

        /// </summary>

        string conStr = "packet size=4096;integrated security=SSPI;" +

             "data source=\"(local)\";persist security info=False;" +

             "initial catalog=master;connect timeout=300";

        RijndaelCryptography rijndael = new RijndaelCryptography();

        private System.ComponentModel.Container components = null;

        public Installer()
        {

            // 该调用是设计器所必需的。

            InitializeComponent();



            // TODO: 在 InitializeComponent 调用后添加任何初始化

        }



        /// <summary> 

        /// 清理所有正在使用的资源。

        /// </summary>

        protected override void Dispose(bool disposing)
        {

            if (disposing)
            {

                if (components != null)
                {

                    components.Dispose();

                }

            }

            base.Dispose(disposing);

        }



        #region 组件设计器生成的代码

        /// <summary>

        /// 设计器支持所需的方法 - 不要使用代码编辑器修改

        /// 此方法的内容。

        /// </summary>

        private void InitializeComponent()
        {

            components = new System.ComponentModel.Container();

        }

        #endregion



        #region 重载自定义安装方法

        protected override void OnBeforeInstall(IDictionary savedState)
        {

            base.OnBeforeInstall(savedState);

        }

        public override void Install(IDictionary stateSaver)
        {

            base.Install(stateSaver);

            string databaseServer = Context.Parameters["server"].ToString();

            string userName = Context.Parameters["user"].ToString();

            string userPass = Context.Parameters["pwd"].ToString();

            string targetdir = this.Context.Parameters["targetdir"].ToString();



            conStr = GetLogin(databaseServer, userName, userPass, "master");

            SqlConnection sqlCon = new SqlConnection();

            try
            {                
                sqlCon.ConnectionString = conStr;

                sqlCon.Open();



                rijndael.GenKey();

                rijndael.Encrypt(conStr);



                stateSaver.Add("key", rijndael.Key);

                stateSaver.Add("IV", rijndael.IV);

                stateSaver.Add("conStr", rijndael.Encrypted);



                //ExecuteSql(sqlCon, "InstallDatabase.txt");

                //ExecuteSql(sqlCon, "InitializeData.txt");

                CreateDatabaseForAttach(sqlCon,targetdir);

                if (sqlCon.State != ConnectionState.Closed) sqlCon.Close();

            }

            catch (SqlException e)
            {

                MessageBox.Show("安装失败!\n数据库配置有误,请正确配置信息!\n" + e.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (sqlCon.State != ConnectionState.Closed) sqlCon.Close();

                this.Rollback(stateSaver);

            }



        }

        protected override void OnAfterInstall(IDictionary savedState)
        {

            base.OnAfterInstall(savedState);

        }



        public override void Rollback(IDictionary savedState)
        {

            base.Rollback(savedState);

        }

        public override void Uninstall(IDictionary savedState)
        {

            base.Uninstall(savedState);
            try
            {
                if (savedState.Contains("conStr"))
                {

                    string targetdir = this.Context.Parameters["targetdir"].ToString();

                    RijndaelCryptography rijndael = new RijndaelCryptography();

                    rijndael.Key = (byte[])savedState["key"];

                    rijndael.IV = (byte[])savedState["IV"];

                    conStr = rijndael.Decrypt((byte[])savedState["conStr"]);

                    SqlConnection sqlCon = new SqlConnection(conStr);

                    ExecuteDrop(sqlCon);

                }
            }
            catch (Exception) { }


        }

        #endregion



        #region 数据操作方法

        //从资源文件获取中数据执行脚本

        private static string GetScript(string name)
        {

            Assembly asm = Assembly.GetExecutingAssembly();

            Stream str = asm.GetManifestResourceStream(asm.GetName().Name + "." + name);

            StreamReader reader = new StreamReader(str, System.Text.Encoding.Default);

            System.Text.StringBuilder output = new System.Text.StringBuilder();

            string line = "";

            while ((line = reader.ReadLine()) != null)
            {

                output.Append(line + "\n");

            }

            return output.ToString();



        }

        //获取数据库登录连接字符串

        private static string GetLogin(string databaseServer, string userName, string userPass, string database)
        {

            return "server=" + databaseServer + ";database=" + database + ";User ID=" + userName + ";Password=" + userPass + ";connect timeout=300;";

        }

        //执行数据库脚本方法

        private static void CreateDatabaseForAttach(SqlConnection sqlCon, string targetdir)
        {         
            string strSql = @"USE [master]
                                CREATE DATABASE [Elinterface] ON 
                                ( FILENAME = N'" + targetdir + @"\data\Elinterface_Data.MDF' ),
                                ( FILENAME = N'" + targetdir + @"\data\Elinterface_Log.LDF')
                                FOR ATTACH";
            if (sqlCon.State != ConnectionState.Closed) sqlCon.Close();

            sqlCon.Open();

            SqlCommand cmd = sqlCon.CreateCommand();

            cmd.Connection = sqlCon;

            cmd.CommandText = strSql;

            cmd.CommandType = CommandType.Text;

            cmd.ExecuteNonQuery();

            sqlCon.Close();
        }

        private static void ExecuteSql(SqlConnection sqlCon, string sqlfile)
        {

            string[] SqlLine;

            Regex regex = new Regex("^GO", RegexOptions.IgnoreCase | RegexOptions.Multiline);



            string txtSQL = GetScript(sqlfile);

            SqlLine = regex.Split(txtSQL);



            if (sqlCon.State != ConnectionState.Closed) sqlCon.Close();

            sqlCon.Open();



            SqlCommand cmd = sqlCon.CreateCommand();

            cmd.Connection = sqlCon;



            foreach (string line in SqlLine)
            {

                if (line.Length > 0)
                {

                    cmd.CommandText = line;

                    cmd.CommandType = CommandType.Text;

                    try
                    {

                        cmd.ExecuteNonQuery();

                    }

                    catch (SqlException e)
                    {

                        //rollback
                        MessageBox.Show("安装失败!\n数据库配置有误,请正确配置信息!\n" + e.Message, "错误rollback", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ExecuteDrop(sqlCon);
                        break;

                    }

                }

            }

        }

        //删除数据库

        private static void ExecuteDrop(SqlConnection sqlCon)
        {

            if (sqlCon.State != ConnectionState.Closed) sqlCon.Close();

            sqlCon.Open();

            SqlCommand cmd = sqlCon.CreateCommand();

            cmd.Connection = sqlCon;

            cmd.CommandText = GetScript("DropDatabase.txt");

            cmd.CommandType = CommandType.Text;

            cmd.ExecuteNonQuery();

            sqlCon.Close();

        }

        #endregion

    }
}

