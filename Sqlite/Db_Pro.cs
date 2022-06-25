using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using SQLiteQueryBrowser;

namespace My_Sqlite_test
{
    
    public class Sqlite_Pro
    {
        
        public static string dbPath = "D:\\test\\2019.db";

        //######################2019.11.26###
        //已测试
        //修改创建表的数据结构 2019/12/24
        public static void CreateTable()                        //创建数据库文件
        {
            if (!System.IO.File.Exists(dbPath))                 //如果不存在该数据库文件，
            {
               SQLiteDBHelper.CreateDB(dbPath);                 //创建数据库
            }
            SQLiteDBHelper db = new SQLiteDBHelper(dbPath);
            string sql = "CREATE TABLE Recorder" +              //创建数表
                "(ID integer NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE," +
                "SN string," +
                "S_VER string," +
                "H_VER string," +
                "PASS string," +
                "ERR_CODE char," +
                "DATE string," +
                "RES string)";
            db.ExecuteNonQuery(sql, null);
        }

        //######################2019.11.26###
        //已测试
        //修改创建表的数据结构 2019/12/24
        public static void InsertData(Product_Record Record1)                         //向数据表中插入数据
        {
            string sql = "INSERT INTO Recorder(ID,SN,S_VER,H_VER,PASS,ERR_CODE,DATE,RES)values(@ID_S,@SN_S,@S_VER_S,@H_VER_S,@PASS_S,@ERR_CODE_S,@DATE_S,@RES_S)";//插入语句 名称+值
            SQLiteDBHelper db = new SQLiteDBHelper(dbPath);
  
            SQLiteParameter[] parameters = new SQLiteParameter[]{                          //变量插入
                     new SQLiteParameter("@ID_S",Record1.ID),
                     new SQLiteParameter("@SN_S",Record1.SN),
                     new SQLiteParameter("@S_VER_S",Record1.S_VER),
                     new SQLiteParameter("@H_VER_S",Record1.H_VER),
                     new SQLiteParameter("@PASS_S",Record1.PASS),
                     new SQLiteParameter("@ERR_CODE_S",Record1.ERR_CODE),
                     new SQLiteParameter("@DATE_S",Record1.DATE),
                     new SQLiteParameter("@RES_S",Record1.RES),
                                                                    };
                    db.ExecuteNonQuery(sql, parameters);
        }

        //######################2019.11.26###
        //已测试
        public static void ShowData()       //查询数据
        {
            //查询从50条起的20条记录  
            string sql = "select * from Recorder order by id desc limit 50 offset 20";
            SQLiteDBHelper db = new SQLiteDBHelper(dbPath);
            using (SQLiteDataReader reader = db.ExecuteReader(sql, null))
            {
                while (reader.Read())
                {
                    Console.WriteLine("ID:{0},SN{1},DATA{2}", reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
                }
            }
        }
        public static bool Delete_Recorder(int sn)
        {
            try
            {
                SQLiteConnection conn = new SQLiteConnection("Data Source = D:\\test\\123.sqlite;");
                conn.Open();
                SQLiteCommand cmd = conn.CreateCommand();
                cmd.CommandText = "delete from Recorder where id=sn";//INSERT INTO Recorder(id, SN, DATE, PASS)values(102, 33, 2222, 2222)";// "delete from Recorder where id=@sn;";
              //      cmd.Parameters.Add(new SQLiteParameter("id", sn));
                 int i = cmd.ExecuteNonQuery();
                    return i ==1;
                //}
            }
            catch (ArgumentException ae)
            {
                MessageBox.Show(ae.Message + " \n\n" + ae.Source + "\n\n" + ae.StackTrace);
                return false;
            }
            catch (Exception ex)
            {
                //Do　any　logging　operation　here　if　necessary  
                MessageBox.Show(ex.Message);
                return false;
            }
        }
    }
    
}
