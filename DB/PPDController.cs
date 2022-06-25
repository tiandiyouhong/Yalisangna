using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using CHD.Framework.Extender;
using SpiderModel;
using DBCenter;
using System.Windows.Forms;
using CHD.ActMsgV1;
using System.Data;

namespace SpiderCenter
{
    public class PPDController
    {
        PPDaiBui bui = null;
        public PPDController(){
            if (bui == null)
                bui = new PPDaiBui();

        }

        public DataTable SearchPPDModel(int pageindex)
        {
            if (bui == null)
                bui = new PPDaiBui();
            return bui.SearchPPDModel(pageindex);
        }
        public DataTable GetPlanPPId(int paginex)
        {
            if (bui == null)
                bui = new PPDaiBui();
            return bui.GetPlanPPId(paginex);

        }
        public int GetMaxPPId()
        {
            if (bui == null)
                bui = new PPDaiBui();
            return bui.GetMaxPPId();

        }
        public void StartRun(ListBox lbx,int PPId)
        {
            if (PPId == -1)
            {
                WinForms.ShowMsg(lbx, "任务完成");
                return;

            }

            PPDModel model = SpiderModel(PPId);

            try
            {
                if (model.PPId != 0)
                {
                    if (bui == null)
                        bui = new PPDaiBui();
                    bui.InsertOrUpdate(model);
                    WinForms.ShowMsg(lbx, model.PPId+" 采集成功入库");

                }
                else
                {
                   WinForms.ShowMsg(lbx, " 采集异常");

                }
            }
            catch (Exception ex)
            {

               // WinForms.ShowMsg(lbx, ex.ToString());

            }
           


        }

        private PPDModel SpiderModel(int PPId)
        {
            PPDModel model=null;
            //try
            //{
            //    model = new PPDModel();
            //    string url = "http://invest.ppdai.com/loan/info?id=" + PPId;//"http://invest.ppdai.com/loan/info?id=5547806";

            //string content = url.GetHtmlSource();

         
            //model.UserName = content.RegexMatch("<a href=\"http://www.ppdai.com/user/(?<a>.*?)\"", "a");
            //model.TimeLimits = content.RegexMatch("<dt>期限：</dt>(?<a>.*?)</dl>", "a");


            //model.Rate = content.RegexMatch("<dt>年利率：</dt>(?<a>.*?)</dl>", "a");

            //model.AllBalance = content.RegexMatch("<dt>借款金额：</dt>(?<a>.*?)</dl>", "a");
            //model.RemainingBalance = content.RegexMatch("<span id=\"listRestMoney\">(?<a>.*?)</span>", "a");

            //model.Plan = content.RegexMatch("<span id=\"process\" style=\"width: (?<a>.*?);\"></span>", "a");
            //// model.Recheck = content.RegexMatch("<a href=\"http://www.ppdai.com/user/(?<a>.*?)\"", "a");
            ////<span class="countdown_row countdown_amount">18天 09:57:04</span>
            ////<span class="countdown_row countdown_amount" id="leftTime">2015/11/20</span>
            //string lefttime = content.RegexMatch("<span class=\"countdown_row countdown_amount\".*?>(?<a>.*?)</span>", "a");
            //if (lefttime.RegexMatch(":", "") != null)
            //    model.Finished = 0;
            //else
            //    model.Finished = 1;
            //model.LeftTime = lefttime;
            //model.TimeLimits = model.TimeLimits.RegexReplace(new string[] { "\r", "\n", "<.*?>", "\\s" });
            //model.Rate = model.Rate.RegexReplace(new string[] { "\r", "\n", "<.*?>", "\\s" });
            //model.AllBalance = model.AllBalance.RegexReplace(new string[] { "\r", "\n", "<.*?>", "\\s", "&yen;" });
            //model.RemainingBalance = model.RemainingBalance.RegexReplace(new string[] { "\r", "\n", "<.*?>", "\\s", "&yen;", "&#165;" });

            //model.TimeLimits = model.TimeLimits.RegexReplace(new string[] { "\r", "\n", "<.*?>", "\\s" });
            //// model.UserName = content.RegexMatch("<a href=\"http://www.ppdai.com/user/(?<a>.*?)\"", "a");
           
              
            //    model.PPId = PPId;
            //   // new PPDaiBui().SaveModel(model);
            //}
            //catch (Exception ex)
            //{
            //    model.PPId = 0;

            //}
            return model;

        }
    }
}
