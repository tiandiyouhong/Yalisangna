using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpiderModel;
using System.Data.SQLite;
using System.Data;
using 电源生产测试软件;
namespace SpiderModel
{
    public class PPDModel
    {
        public int PPId { get; set; }
        public string UserName { get; set; }
        public string TimeLimits { get; set; }
        public string Rate { get; set; }
        public string AllBalance { get; set; }
        public string RemainingBalance { get; set; }
        public string Plan { get; set; }
        public string LeftTime { get; set; }
        public string Recheck { get; set; }
        public int Finished { get; set; }
    }
}
namespace DBCenter
{
    public class PPDaiBui
    {
        public DataTable SearchPPDModel(int pageindex)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append(" select * from Recorder limit " + pageindex+",5000");
                       
            SQLiteCommand cmd = new SQLiteCommand(sb.ToString());
            return SQLiteHelper.ExecuteDataSet(cmd, 电源生产测试软件.Form_Main.Recorder_path).Tables[0];     
        }
        public int GetMaxPPId()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" select max(ID) from Recorder");
             
            SQLiteCommand cmd = new SQLiteCommand(sb.ToString());
            System.Data.DataSet ds = SQLiteHelper.ExecuteDataSet(cmd, Form_Main.Recorder_path);

            return Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
        }
        public DataTable GetPlanPPId(int paginex)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" select  ppid from ppdmodel where plan !='100%' and Finished=0 limit " + paginex + ",10");

            SQLiteCommand cmd = new SQLiteCommand(sb.ToString());
             DataSet ds = SQLiteHelper.ExecuteDataSet(cmd, Form_Main.Recorder_path);

            return ds.Tables[0];
        }
        public void InsertOrUpdate(PPDModel model)
        {

            if (CheckModel(model))
                InsertModel(model);
            else
                UpdateModel(model);
        }

        public void InsertModel(PPDModel model)
        {

            // 
            StringBuilder sb = new StringBuilder();
            sb.Append("insert into ppdmodel(PPId,UserName,TimeLimits,Rate,AllBalance,RemainingBalance,Plan)");
            sb.Append("values");
            sb.AppendFormat("('{0}','{1}','{2}','{3}','{4}','{5}','{6}')",
                model.PPId, model.UserName, model.TimeLimits, model.Rate, model.AllBalance, model.RemainingBalance, model.Plan);
            SQLiteCommand cmd = new SQLiteCommand(sb.ToString());
            SQLiteHelper.ExecuteNonQuery(cmd);

        }

        public void UpdateModel(PPDModel model)
        {


            StringBuilder sb = new StringBuilder();
            sb.Append("update   ppdmodel ");
            sb.Append(" set ");

            sb.AppendFormat("  UserName='{0}',", model.UserName);
            sb.AppendFormat("  TimeLimits='{0}',", model.TimeLimits);
            sb.AppendFormat("  Rate='{0}',", model.Rate);
            sb.AppendFormat("  AllBalance='{0}',", model.AllBalance);
            sb.AppendFormat("  RemainingBalance='{0}',", model.RemainingBalance);
            sb.AppendFormat("  Plan='{0}',", model.Plan);
           // sb.AppendFormat("  LeftTime='{0}',", model.LeftTime);
           // sb.AppendFormat("  Finished='{0}',", model.Finished);
           // sb.AppendFormat("  UpdateTime='{0}',", DateTime.Now);
           // sb.AppendFormat("  Recheck='{0}' ", model.Recheck);
          // sb.AppendFormat("  where ppid='{0}' ", model.PPId);

            SQLiteCommand cmd = new SQLiteCommand(sb.ToString());
            SQLiteHelper.ExecuteNonQuery(cmd);

        }


        /// <summary>
        /// 检查PPID是不是存在
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool CheckModel(PPDModel model)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" select count(1) from ppdmodel where PPId=" + model.PPId);



            SQLiteCommand cmd = new SQLiteCommand(sb.ToString());
            System.Data.DataSet ds = SQLiteHelper.ExecuteDataSet(cmd, Form_Main.Recorder_path);
            if (ds.Tables[0].Rows[0][0].ToString().Equals("0"))
                return true;
            else
                return false;

        }
    }
}
