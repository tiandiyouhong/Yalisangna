using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Runtime.InteropServices;
namespace 电源生产测试软件

{
    public partial class Form_Main : Form
    {
        
        //[DllImport("kernel32")]
        //public static extern bool WritePrivateProfileString(string lpApplicationName, string lpKeyName, string lpString, string lpFileName);
        //[DllImport("kernel32")]
        //public static extern uint GetPrivateProfileString(string lpApplicationName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);
        public static  string[] Device1 = new string[30];
        
        public static void Device_Mesg()
        {
            int i=0;
            
            string strPath = Application.StartupPath + @"\Device.ini";
            string strRead = GetStringFromINI("设备型号", i.ToString(), strPath);
         
            Device1[0] = strRead;
            while (strRead!="")
            {
                i++;
                strRead = GetStringFromINI("设备型号", i.ToString(), strPath);
                Device1[i] = strRead;
            }
            Device1[i] = "null_RES";
            
          //  MessageBox.Show(strRead);
        }

        public static string GetStringFromINI(string strApp, string strKey, string fileName)
        {
            string strTmp = string.Empty;
            StringBuilder sb = new StringBuilder("", 1024);

            if (GetPrivateProfileString(strApp, strKey, "", sb, 1024, fileName) >= 0)
            {
                return sb.ToString();
            }
            else
                return "ERROR";
        }
    }
}
