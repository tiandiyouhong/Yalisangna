using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        public static string dbPath = "D:\\test\\2002.db";

        //######################2020.02.24###
        //已测试
        public static void Create_Fault_Table()                                     //创建数据库文件
        {
            dbPath = 电源生产测试软件.Form_Main.BMS_Sn +"故障记录" +".db";//数据库名字为SN码
            if (!System.IO.File.Exists(dbPath))                                     //如果不存在该数据库文件，
            {
                SQLiteDBHelper.CreateDB(dbPath);                                    //创建数据库
            }
            SQLiteDBHelper db = new SQLiteDBHelper(dbPath);
            string sql = "CREATE TABLE Recorder" +                                 //创建数表
                "(ID integer NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE," +
                "单节欠压 integer," +
                "单节过放 integer," +
                "单节过压 integer," +
                "单节过充 integer," +
                "总电压故障 integer," +
                "电流故障 integer," +
                "温度故障1 integer," +
                "温度故障2 integer," +
                "温度故障3 integer," +
                "BMS状态 integer," +
                "电池总电压 integer," +
                "单节最高电压 integer," +
                "单节最低电压 integer," +
                "电芯温度1 integer," +
                "电芯温度2 integer," +
                "环境温度 integer," +
                "右R温度 integer," +
                "左R温度 integer," +              
                "电流 integer," +
                "状态机 integer," +
                "循环次数 integer," +
                "RES23 integer," +
                "RES24 integer," +
                "RES25 integer," +
                "RES26 integer," +
                "RES27 integer," +
                "RES28 integer," +
                "RES29 integer," +
                "RES30 integer," +
                "RES31 integer," +
                "RES32 integer)";
            db.ExecuteNonQuery(sql, null);
        }

        //######################2020.02.24###
        //已测试
        public static void InsertData_to_Fault_Table(Fault_Tables fault)            //向数据表中插入数据
        {
            dbPath =  电源生产测试软件.Form_Main.BMS_Sn + "故障记录" + ".db";//数据库名字为SN码
            string sql = "INSERT INTO Recorder(ID," +
                "单节欠压," +
                "单节过放," +
                "单节过压," +
                "单节过充," +
                "总电压故障," +
                "电流故障," +
                "温度故障1," +
                "温度故障2," +
                "温度故障3," +
                "BMS状态," +
                "电池总电压," +
                "单节最高电压," +
                "单节最低电压," +
                "电芯温度1," +
                "电芯温度2," +
                "环境温度," +
                "右R温度," +
                "左R温度," +
                "电流," +
                "状态机," +
                "循环次数," +
                "RES23," +
                "RES24," +
                "RES25," +
                "RES26," +
                "RES27," +
                "RES28," +
                "RES29," +
                "RES30," +
                "RES31," +
                "RES32)values(@A0,@A1,@A2,@A3,@A4,@A5,@A6,@A7,@A8,@A9,@A10,@A11,@A12,@A13,@A14,@A15,@A16,@A17,@A18,@A19,@A20,@A21,@A22,@A23,@A24,@A25,@A26,@A27,@A28,@A29,@A30,@A31)";//插入语句 名称+值
            SQLiteDBHelper db = new SQLiteDBHelper(dbPath);

            SQLiteParameter[] parameters = new SQLiteParameter[]{                          //变量插入
                       new SQLiteParameter("@A0",fault.ID),
                       new SQLiteParameter("@A1",fault.F1),
                       new SQLiteParameter("@A2",fault.F2),
                       new SQLiteParameter("@A3",fault.F3),
                       new SQLiteParameter("@A4",fault.F4),
                       new SQLiteParameter("@A5",fault.F5),
                       new SQLiteParameter("@A6",fault.F6),
                       new SQLiteParameter("@A7",fault.F7),
                       new SQLiteParameter("@A8",fault.F8),
                       new SQLiteParameter("@A9",fault.F9),
                       new SQLiteParameter("@A10",fault.F10),

                       new SQLiteParameter("@A11",fault.BAT_V_S),
                       new SQLiteParameter("@A12",fault.CEll_V_max_S),
                       new SQLiteParameter("@A13",fault.CEll_V_min_S),
                       new SQLiteParameter("@A14",fault.BAT_T1_S),
                       new SQLiteParameter("@A15",fault.BAT_T2_S),
                       new SQLiteParameter("@A16",fault.EV_T_S),
                       new SQLiteParameter("@A17",fault.Blance_R_T_S),
                       new SQLiteParameter("@A18",fault.Blance_L_T_S),
                       new SQLiteParameter("@A19",fault.Current_S),

                       new SQLiteParameter("@A20",fault.statement_S),
                       new SQLiteParameter("@A21",fault.soc_Num_S),
                       new SQLiteParameter("@A22",fault.RES23_S),
                       new SQLiteParameter("@A23",fault.RES24_S),
                       new SQLiteParameter("@A24",fault.RES25_S),
                       new SQLiteParameter("@A25",fault.RES26_S),
                       new SQLiteParameter("@A26",fault.RES27_S),
                       new SQLiteParameter("@A27",fault.RES28_S),
                       new SQLiteParameter("@A28",fault.RES29_S),
                       new SQLiteParameter("@A29",fault.RES30_S),
                       new SQLiteParameter("@A30",fault.RES31_S),
                       new SQLiteParameter("@A31",fault.RES32_S),                 
             };
            db.ExecuteNonQuery(sql, parameters);
        }
    

        public static void Create_Charge_Table()                                    //创建数据库文件
        {
            dbPath =  电源生产测试软件.Form_Main.BMS_Sn + "充电数据记录" + ".db";
            if (!System.IO.File.Exists(dbPath))                                     //如果不存在该数据库文件，
            {
                SQLiteDBHelper.CreateDB(dbPath);                                    //创建数据库
            }
            SQLiteDBHelper db = new SQLiteDBHelper(dbPath);
            string sql = "CREATE TABLE Recorder" +                                  //创建数表
                 "(ID integer NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE," +
                "充电次数 integer," +
                "充电持续时间 integer," +
                "充电最大电流 integer," +
                "充电前soc integer," +
                "充电前可用时间 integer," +
                "充电后soc integer," +
                "充电后可用时间 integer," +
                "充电前电池电压 integer," +
                "充电后电池电压 integer," +
                "前Vcell_Max integer," +
                "前Vcell_Max位置 integer," +
                "后Vcell_Max integer," +
                "后Vcell_Max位置 integer," +
                "前Vcell_Min integer," +
                "前Vcell_Min位置 integer," +
                "后Vcell_Min integer," +
                "后Vcell_Min位置 integer," +
                "最高温度 integer," +
                "最高温度位置 integer," +
                "最低温度 integer," +
                "最低温度位置 integer," +
                "高温持续时间 integer," +
                "充电标志位 integer," +
                "RES25 integer," +
                "RES26 integer," +
                "RES27 integer," +
                "RES28 integer," +
                "RES29 integer," +
                "RES30 integer," +
                "RES31 integer," +
                "RES32 integer)";
            db.ExecuteNonQuery(sql, null);
        }

        public static void InsertData_to_Charge_Table(Charge_Tables charge)            //向数据表中插入数据
        {
            dbPath =  电源生产测试软件.Form_Main.BMS_Sn + "充电数据记录" + ".db";
            string sql = "INSERT INTO Recorder(ID," +       
            "充电次数," +
            "充电持续时间," +
            "充电最大电流," +
            "充电前soc," +
            "充电前可用时间," +
             "充电后soc," +
            "充电后可用时间," +
            "充电前电池电压," +
            "充电后电池电压," +
            "前Vcell_Max," +
            "前Vcell_Max位置," +
            "后Vcell_Max," +
            "后Vcell_Max位置," +
            "前Vcell_Min," +
            "前Vcell_Min位置," +
            "后Vcell_Min," +
            "后Vcell_Min位置," +
            "最高温度," +
            "最高温度位置," +
            "最低温度," +
            "最低温度位置," +
            "高温持续时间," +
            "充电标志位," +
            "RES25," +
            "RES26," +
            "RES27," +
            "RES28," +
            "RES29," +
            "RES30," +
            "RES31," +
            "RES32)values(@A0,@A1,@A2,@A3,@A4,@A5,@A6,@A7,@A8,@A9,@A10,@A11,@A12,@A13,@A14,@A15,@A16,@A17,@A18,@A19,@A20,@A21,@A22,@A23,@A24,@A25,@A26,@A27,@A28,@A29,@A30,@A31)";//插入语句 名称+值
            SQLiteDBHelper db1 = new SQLiteDBHelper(dbPath);

            SQLiteParameter[] parameters = new SQLiteParameter[]{                          //变量插入
                       new SQLiteParameter("@A0",charge.ID),
                       new SQLiteParameter("@A1",charge.Charge_Num_S),
                       new SQLiteParameter("@A2",charge.Charge_Dur_Time_S),
                       new SQLiteParameter("@A3",charge.Current_Max_S),
                       new SQLiteParameter("@A4",charge.Per_Soc_S),
                       new SQLiteParameter("@A5",charge.Per_User_Time_S),
                       new SQLiteParameter("@A6",charge.Last_Soc_S),
                       new SQLiteParameter("@A7",charge.Last_User_Time_S),
                       new SQLiteParameter("@A8",charge.Per_Bat_Voltage_S),
                       new SQLiteParameter("@A9",charge.Last_Bat_Voltage_S),
                       new SQLiteParameter("@A10",charge.Per_Cell_Max_Voltage_S),

                       new SQLiteParameter("@A11",charge.Per_Cell_Min_Voltage_Num_S),
                       new SQLiteParameter("@A12",charge.Last_Cell_Max_Voltage_S),
                       new SQLiteParameter("@A13",charge.Last_Cell_Max_Voltage_Num_S),
                       new SQLiteParameter("@A14",charge.Per_Cell_Min_Voltage_S),
                       new SQLiteParameter("@A15",charge.Per_Cell_Min_Voltage_Num_S),
                       new SQLiteParameter("@A16",charge.Last_Cell_Min_Voltage_S),
                       new SQLiteParameter("@A17",charge.Last_Cell_Min_Voltage_Num_S),
                       new SQLiteParameter("@A18",charge.Cell_Max_Temp_S),
                       new SQLiteParameter("@A19",charge.Cell_Max_Temp_Point_S),

                       new SQLiteParameter("@A20",charge.Cell_Min_Temp_S),
                       new SQLiteParameter("@A21",charge.Cell_Min_Temp_Point_S),
                       new SQLiteParameter("@A22",charge.Hi_Temp_Dur_Time_S),
                       new SQLiteParameter("@A23",charge.Charge_ST_Flag_S),
                       new SQLiteParameter("@A24",charge.RES25_S),
                       new SQLiteParameter("@A25",charge.RES26_S),
                       new SQLiteParameter("@A26",charge.RES27_S),
                       new SQLiteParameter("@A27",charge.RES28_S),
                       new SQLiteParameter("@A28",charge.RES29_S),
                       new SQLiteParameter("@A29",charge.RES29_S),
                       new SQLiteParameter("@A30",charge.RES30_S),
                       new SQLiteParameter("@A31",charge.RES31_S),


             };
            db1.ExecuteNonQuery(sql, parameters);
        }
        //修改创建表的数据结构 2019/12/24
        public static void Create_Discharge_Table()                                 //创建数据库文件
        {
            dbPath =  电源生产测试软件.Form_Main.BMS_Sn + "放电数据记录" + ".db";
            if (!System.IO.File.Exists(dbPath))                 //如果不存在该数据库文件，
            {
                SQLiteDBHelper.CreateDB(dbPath);                 //创建数据库
            }
            SQLiteDBHelper db = new SQLiteDBHelper(dbPath);
            string sql = "CREATE TABLE Recorder" +              //创建数表
                "(ID integer NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE," +
                "放电次数 integer," +
                "放电持续时间 integer," +
                "放电最大电流 integer," +
                "放电前soc integer," +
                "放电前可用时间 integer," +
                "放电后soc integer," +
                "放电后可用时间 integer," +
                "放电前电池电压 integer," +
                "放电后电池电压 integer," +
                "前Vcell_Max integer," +
                "前Vcell_Max位置 integer," +
                "后Vcell_Max integer," +
                "后Vcell_Max位置 integer," +
                "前Vcell_Min integer," +
                "前Vcell_Min位置 integer," +
                "后Vcell_Min integer," +
                "后Vcell_Min位置 integer," +
                "最高温度 integer," +
                "最高温度位置 integer," +
                "最低温度 integer," +
                "最低温度位置 integer," +
                "高温持续时间 integer," +
                "放电标志位 integer," +
                "RES25 integer," +
                "RES26 integer," +
                "RES27 integer," +
                "RES28 integer," +
                "RES29 integer," +
                "RES30 integer," +
                "RES31 integer," +
                "RES32 integer)";
            db.ExecuteNonQuery(sql, null);
        }

        //######################2019.11.26###
        //已测试
        //修改创建表的数据结构 2019/12/24
        public static void InsertData_to_Discharge_Table(Charge_Tables discharge)            //向数据表中插入数据
        {
            dbPath = 电源生产测试软件.Form_Main.BMS_Sn + "放电数据记录" + ".db";
            string sql = "INSERT INTO Recorder(ID," +
             "放电次数," +
             "放电持续时间," +
             "放电最大电流," +
             "放电前soc," +
             "放电前可用时间," +
             "放电后soc," +
            "放电后可用时间," +
            "放电前电池电压," +
            "放电后电池电压," +
            "前Vcell_Max," +
            "前Vcell_Max位置," +
            "后Vcell_Max," +
            "后Vcell_Max位置," +
            "前Vcell_Min," +
            "前Vcell_Min位置," +
            "后Vcell_Min," +
            "后Vcell_Min位置," +
            "最高温度," +
            "最高温度位置," +
            "最低温度," +
            "最低温度位置," +
            "高温持续时间," +
            "放电标志位," +
            "RES25," +
            "RES26," +
            "RES27," +
            "RES28," +
            "RES29," +
            "RES30," +
            "RES31," +
            "RES32)values(@A0,@A1,@A2,@A3,@A4,@A5,@A6,@A7,@A8,@A9,@A10,@A11,@A12,@A13,@A14,@A15,@A16,@A17,@A18,@A19,@A20,@A21,@A22,@A23,@A24,@A25,@A26,@A27,@A28,@A29,@A30,@A31)";//插入语句 名称+值
            SQLiteDBHelper db = new SQLiteDBHelper(dbPath);

            SQLiteParameter[] parameters = new SQLiteParameter[]{                          //变量插入
                       new SQLiteParameter("@A0",discharge.ID),
                       new SQLiteParameter("@A1",discharge.Charge_Num_S),
                       new SQLiteParameter("@A2",discharge.Charge_Dur_Time_S),
                       new SQLiteParameter("@A3",discharge.Current_Max_S),
                       new SQLiteParameter("@A4",discharge.Per_Soc_S),
                       new SQLiteParameter("@A5",discharge.Per_User_Time_S),
                       new SQLiteParameter("@A6",discharge.Last_Soc_S),
                       new SQLiteParameter("@A7",discharge.Last_User_Time_S),
                       new SQLiteParameter("@A8",discharge.Per_Bat_Voltage_S),
                       new SQLiteParameter("@A9",discharge.Last_Bat_Voltage_S),
                       new SQLiteParameter("@A10",discharge.Per_Cell_Max_Voltage_S),

                       new SQLiteParameter("@A11",discharge.Per_Cell_Min_Voltage_Num_S),
                       new SQLiteParameter("@A12",discharge.Last_Cell_Max_Voltage_S),
                       new SQLiteParameter("@A13",discharge.Last_Cell_Min_Voltage_Num_S),
                       new SQLiteParameter("@A14",discharge.Per_Cell_Min_Voltage_S),
                       new SQLiteParameter("@A15",discharge.Per_Cell_Min_Voltage_Num_S),
                       new SQLiteParameter("@A16",discharge.Last_Cell_Min_Voltage_S),
                       new SQLiteParameter("@A17",discharge.Last_Cell_Min_Voltage_Num_S),
                       new SQLiteParameter("@A18",discharge.Cell_Max_Temp_S),
                       new SQLiteParameter("@A19",discharge.Cell_Max_Temp_Point_S),

                       new SQLiteParameter("@A20",discharge.Cell_Min_Temp_S),
                       new SQLiteParameter("@A21",discharge.Cell_Min_Temp_Point_S),
                       new SQLiteParameter("@A22",discharge.Hi_Temp_Dur_Time_S),
                       new SQLiteParameter("@A23",discharge.Charge_ST_Flag_S),
                       new SQLiteParameter("@A24",discharge.RES25_S),
                       new SQLiteParameter("@A25",discharge.RES26_S),
                       new SQLiteParameter("@A26",discharge.RES27_S),
                       new SQLiteParameter("@A27",discharge.RES28_S),
                       new SQLiteParameter("@A28",discharge.RES29_S),
                       new SQLiteParameter("@A29",discharge.RES29_S),
                       new SQLiteParameter("@A30",discharge.RES30_S),
                       new SQLiteParameter("@A31",discharge.RES31_S),
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
                                                                     //cmd.Parameters.Add(new SQLiteParameter("id", sn));
                int i = cmd.ExecuteNonQuery();
                return i == 1;
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
