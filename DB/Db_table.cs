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

public class Fault_Tables
{
    private int id;
    private int Fault_word1;
    private int Fault_word2;
    private int Fault_word3;
    private int Fault_word4;
    private int Fault_word5;
    private int Fault_word6;
    private int Fault_word7;
    private int Fault_word8;
    private int Fault_word9;
    private int Fault_word10;
    private int BAT_V;
    private int CEll_V_max;
    private int CEll_V_min;
    private int BAT_T1;
    private int BAT_T2;
    private int EV_T;
    private int Blance_R_T;
    private int Blance_L_T;
    private int Current;
    private int statement;
    private int soc_Num;
    private int RES23;
    private int RES24;
    private int RES25;
    private int RES26;
    private int RES27;
    private int RES28;
    private int RES29;
    private int RES30;
    private int RES31;
    private int RES32;
    public int ID
    {
        get { return id; }
        set { id = value; }
    }
    public int F1
    {
        get { return Fault_word1; }
        set { Fault_word1 = value; }
    }
    public int F2
    {
        get { return Fault_word2; }
        set { Fault_word2 = value; }
    }
    public int F3
    {
        get { return Fault_word3; }
        set { Fault_word3 = value; }
    }
    public int F4
    {
        get { return Fault_word4; }
        set { Fault_word4 = value; }
    }
    public int F5
    {
        get { return Fault_word5; }
        set { Fault_word5 = value; }
    }
    public int F6
    {
        get { return Fault_word6; }
        set { Fault_word6 = value; }
    }
    public int F7
    {
        get { return Fault_word7; }
        set { Fault_word7 = value; }
    }
    public int F8
    {
        get { return Fault_word8; }
        set { Fault_word8 = value; }
    }
    public int F9
    {
        get { return Fault_word9; }
        set { Fault_word9 = value; }
    }
    public int F10
    {
        get { return Fault_word10; }
        set { Fault_word10 = value; }
    }
    public int BAT_V_S
    {
        get { return BAT_V; }
        set { BAT_V = value; }
    }
    public int CEll_V_max_S
    {
        get { return CEll_V_max; }
        set { CEll_V_max = value; }
    }
    public int CEll_V_min_S
    {
        get { return CEll_V_min; }
        set { CEll_V_min = value; }
    }
    public int BAT_T1_S
    {
        get { return BAT_T1; }
        set { BAT_T1 = value; }
    }
    public int BAT_T2_S
    {
        get { return BAT_T2; }
        set { BAT_T2 = value; }
    }
     public int EV_T_S
    {
        get { return EV_T; }
        set { EV_T = value; }
    }
     public int Blance_R_T_S
    {
        get { return Blance_R_T; }
        set { Blance_R_T = value; }
    }
     public int Blance_L_T_S
    {
        get { return Blance_L_T; }
        set { Blance_L_T = value; }
    }
    public int Current_S
    {
        get { return Current; }
        set { Current = value; }
    }
    public int statement_S
    {
        get { return statement; }
        set { statement = value; }
    }
          public int soc_Num_S
    {
        get { return soc_Num; }
        set { soc_Num = value; }
    }
          public int RES23_S
    {
        get { return RES23; }
        set { RES23 = value; }
    }
          public int RES24_S
    {
        get { return RES24; }
        set { RES24 = value; }
    }
          public int RES25_S
    {
        get { return RES25; }
        set { RES25 = value; }
    }
          public int RES26_S
    {
        get { return RES26; }
        set { RES26 = value; }
    }
          public int RES27_S
    {
        get { return RES27; }
        set { RES27 = value; }
    }
          public int RES28_S
    {
        get { return RES28; }
        set { RES28 = value; }
    }
          public int RES29_S
    {
        get { return RES29; }
        set { RES29 = value; }
    }
          public int RES30_S
    {
        get { return RES30; }
        set { RES30 = value; }
    }
          public int RES31_S
    {
        get { return RES31; }
        set { RES31 = value; }
    }
          public int RES32_S
    {
        get { return RES32; }
        set { RES32 = value; }
    }

}



public class Charge_Tables
{
    private int id;
    private int Charge_Num;
    private int Charge_Dur_Time;
    private int Current_Max;
    private int Per_Soc;
    private int Per_User_Time;
    private int Last_Soc;
    private int Last_User_Time;
    private int Per_Bat_Voltage;
    private int Last_Bat_Voltage;
    private int Per_Cell_Max_Voltage;
    private int Per_Cell_Max_Voltage_Num;
    private int Last_Cell_Max_Voltage;
    private int Last_Cell_Max_Voltage_Num;
    private int Per_Cell_Min_Voltage;
    private int Per_Cell_Min_Voltage_Num;
    private int Last_Cell_Min_Voltage;
    private int Last_Cell_Min_Voltage_Num;
    private int Cell_Max_Temp;
    private int Cell_Max_Temp_Point;
    private int Cell_Min_Temp;
    private int Cell_Min_Temp_Point;
    private int Hi_Temp_Dur_Time;
    private int Charge_ST_Flag;
    private int RES25;
    private int RES26;
    private int RES27;
    private int RES28;
    private int RES29;
    private int RES30;
    private int RES31;
    private int RES32;
    public int ID
    {
        get { return id; }
        set { id = value; }
    }
    public int Charge_Num_S
    {
        get { return Charge_Num; }
        set { Charge_Num = value; }
    }
    public int Charge_Dur_Time_S
    {
        get { return Charge_Dur_Time; }
        set { Charge_Dur_Time = value; }
    }
    public int Current_Max_S
    {
        get { return Current_Max; }
        set { Current_Max = value; }
    }
    public int Per_Soc_S
    {
        get { return Per_Soc; }
        set { Per_Soc = value; }
    }
    public int Per_User_Time_S
    {
        get { return Per_User_Time; }
        set { Per_User_Time = value; }
    }
    public int Last_Soc_S
    {
        get { return Last_Soc; }
        set { Last_Soc = value; }
    }
    public int Last_User_Time_S
    {
        get { return Last_User_Time; }
        set { Last_User_Time = value; }
    }
    public int Per_Bat_Voltage_S
    {
        get { return Per_Bat_Voltage; }
        set { Per_Bat_Voltage = value; }
    }
    public int Last_Bat_Voltage_S
    {
        get { return Last_Bat_Voltage; }
        set { Last_Bat_Voltage = value; }
    }
    public int Per_Cell_Max_Voltage_S
    {
        get { return Per_Cell_Max_Voltage; }
        set { Per_Cell_Max_Voltage = value; }
    }
    public int Per_Cell_Max_Voltage_Num_S
    {
        get { return Per_Cell_Max_Voltage_Num; }
        set { Per_Cell_Max_Voltage_Num = value; }
    }
    public int Last_Cell_Max_Voltage_S
    {
        get { return Last_Cell_Max_Voltage; }
        set { Last_Cell_Max_Voltage = value; }
    }
    public int Last_Cell_Max_Voltage_Num_S
    {
        get { return Last_Cell_Max_Voltage_Num; }
        set { Last_Cell_Max_Voltage_Num = value; }
    }
    public int Per_Cell_Min_Voltage_S
    {
        get { return Per_Cell_Min_Voltage; }
        set { Per_Cell_Min_Voltage = value; }
    }
    public int Per_Cell_Min_Voltage_Num_S
    {
        get { return Per_Cell_Min_Voltage_Num; }
        set { Per_Cell_Min_Voltage_Num = value; }
    }
    public int Last_Cell_Min_Voltage_S
    {
        get { return Last_Cell_Min_Voltage; }
        set { Last_Cell_Min_Voltage = value; }
    }
    public int Last_Cell_Min_Voltage_Num_S
    {
        get { return Last_Cell_Min_Voltage_Num; }
        set { Last_Cell_Min_Voltage_Num = value; }
    }
    public int Cell_Max_Temp_S
    {
        get { return Cell_Max_Temp; }
        set { Cell_Max_Temp = value; }
    }
    public int Cell_Max_Temp_Point_S
    {
        get { return Cell_Max_Temp_Point; }
        set { Cell_Max_Temp_Point = value; }
    }
    public int Cell_Min_Temp_S
    {
        get { return Cell_Min_Temp; }
        set { Cell_Min_Temp = value; }
    }
    public int Cell_Min_Temp_Point_S
    {
        get { return Cell_Min_Temp_Point; }
        set { Cell_Min_Temp_Point = value; }
    }
    public int Hi_Temp_Dur_Time_S
    {
        get { return Hi_Temp_Dur_Time; }
        set { Hi_Temp_Dur_Time = value; }
    }
    public int Charge_ST_Flag_S
    {
        get { return Charge_ST_Flag; }
        set { Charge_ST_Flag = value; }
    }
    public int RES25_S
    {
        get { return RES25; }
        set { RES25 = value; }
    }
    public int RES26_S
    {
        get { return RES26; }
        set { RES26 = value; }
    }
    public int RES27_S
    {
        get { return RES27; }
        set { RES27 = value; }
    }
    public int RES28_S
    {
        get { return RES28; }
        set { RES28 = value; }
    }
    public int RES29_S
    {
        get { return RES29; }
        set { RES29 = value; }
    }
    public int RES30_S
    {
        get { return RES30; }
        set { RES30 = value; }
    }
    public int RES31_S
    {
        get { return RES31; }
        set { RES31 = value; }
    }
    public int RES32_S
    {
        get { return RES32; }
        set { RES32 = value; }
    }
}
namespace SQLiteQueryBrowser
{
    /// <summary>  
    /// 说明：这是一个针对System.Data.SQLite的数据库常规操作封装的通用类。  
    /// </summary>  
    public class SQLiteDBHelper
    {
        private string connectionString = string.Empty;
        /// <summary>  
        /// 构造函数  
        /// </summary>  
        /// <param name="dbPath">SQLite数据库文件路径</param>  
        public SQLiteDBHelper(string dbPath)
        {
            this.connectionString = "Data Source=" + dbPath;
        }
        /// <summary>  
        /// 创建SQLite数据库文件  
        /// </summary>  
        /// <param name="dbPath">要创建的SQLite数据库文件路径</param>  
        public static void CreateDB(string dbPath)
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + dbPath))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "CREATE TABLE Demo(id integer NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE)";
                    command.ExecuteNonQuery();
                    command.CommandText = "DROP TABLE Demo";
                    command.ExecuteNonQuery();
                }
            }
        }
        /// <summary>  
        /// 对SQLite数据库执行增删改操作，返回受影响的行数。  
        /// </summary>  
        /// <param name="sql">要执行的增删改的SQL语句</param>  
        /// <param name="parameters">执行增删改语句所需要的参数，参数必须以它们在SQL语句中的顺序为准</param>  
        /// <returns></returns>  
        public int ExecuteNonQuery(string sql, SQLiteParameter[] parameters)
        {
            int affectedRows = 0;
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    using (SQLiteCommand command = new SQLiteCommand(connection))
                    {
                        command.CommandText = sql;
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }
                        try {
                            affectedRows = command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                           // Response.Write(ex.Message);
                        }

                    }
                    transaction.Commit();
                }
            }
            return affectedRows;
        }
        /// <summary>  
        /// 执行一个查询语句，返回一个关联的SQLiteDataReader实例  
        /// </summary>  
        /// <param name="sql">要执行的查询语句</param>  
        /// <param name="parameters">执行SQL查询语句所需要的参数，参数必须以它们在SQL语句中的顺序为准</param>  
        /// <returns></returns>  
        public SQLiteDataReader ExecuteReader(string sql, SQLiteParameter[] parameters)
        {
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            SQLiteCommand command = new SQLiteCommand(sql, connection);
            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }
            connection.Open();
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }
        /// <summary>  
        /// 执行一个查询语句，返回一个包含查询结果的DataTable  
        /// </summary>  
        /// <param name="sql">要执行的查询语句</param>  
        /// <param name="parameters">执行SQL查询语句所需要的参数，参数必须以它们在SQL语句中的顺序为准</param>  
        /// <returns></returns>  
        public DataTable ExecuteDataTable(string sql, SQLiteParameter[] parameters)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    DataTable data = new DataTable();
                    adapter.Fill(data);
                    return data;
                }
            }
        }
        /// <summary>  
        /// 执行一个查询语句，返回查询结果的第一行第一列  
        /// </summary>  
        /// <param name="sql">要执行的查询语句</param>  
        /// <param name="parameters">执行SQL查询语句所需要的参数，参数必须以它们在SQL语句中的顺序为准</param>  
        /// <returns></returns>  
        public Object ExecuteScalar(string sql, SQLiteParameter[] parameters)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    DataTable data = new DataTable();
                    adapter.Fill(data);
                    return data;
                }
            }
        }
        /// <summary>  
        /// 查询数据库中的所有数据类型信息  
        /// </summary>  
        /// <returns></returns>  
        public DataTable GetSchema()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                DataTable data = connection.GetSchema("TABLES");
                connection.Close();
                //foreach (DataColumn column in data.Columns)  
                //{  
                //  Console.WriteLine(column.ColumnName);  
                //}  
                return data;
            }
        }
    }
}

