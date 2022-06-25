using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace CHD.ActMsgV1
{
    public class WinForms
    {


       public static void ShowMsg ( ListBox lbx, string msgString)
       {
           MethodInvoker method = null;
           if (lbx.InvokeRequired)
           {
               if (method == null)
               {
                   method = delegate
                   {
                       lbx.Items.Insert(0, msgString);
                     //  item.set_EditValue(msgString);
                   };
               }
               lbx.Invoke(method);
           }
           else
           {
               lbx.Items.Insert(0, msgString);
           }
       }
    }
}
