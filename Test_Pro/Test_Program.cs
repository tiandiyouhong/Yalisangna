using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.IO.Ports;
using System.Net;
using System.Collections;
using System.Diagnostics;
using System.Xml;
using System.Runtime.InteropServices;
using My_Sqlite_test;
namespace 电源生产测试软件
{
    partial class Form_Main
    {
        public void Read_elec_load()
        {
            flag_btnclick = "读取电压";////
            ArrayForDc_elec_load();
            SendData_ShowData_Elec_load();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
            Time_Delay_MS(1500);

            ArrayForDc_elec_load();
            SendData_ShowData_Elec_load();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
            Time_Delay_MS(1500);
        }

        public UInt16 Convert_to_Uint16(string STR1)
        {
            byte[] a = StringToByte32(STR1);
            UInt32 DC_set_32_temp;
            UInt16 Uint16_back;
            Uint16_back = 0;
            if (a.Length == 1)
            {
                Back_Date32 = a[0];
            }
            else if (a.Length == 2)
            {
                Back_Date32 = a[0];
                Back_Date32 = ((Back_Date32 << 8) + a[1]);
            }
            else if (a.Length == 3)
            {
                Back_Date32 = a[0];
                Back_Date32 = ((Back_Date32 << 8) + a[1]);
                Back_Date32 = ((Back_Date32 << 8) + a[2]);
            }
            else if (a.Length == 4)
            {
                Back_Date32 = a[0];
                Back_Date32 = ((Back_Date32 << 8) + a[1]);
                Back_Date32 = ((Back_Date32 << 8) + a[2]);
                Back_Date32 = ((Back_Date32 << 8) + a[3]);
            }
            ge = Back_Date32 & 0xF;
            shi = 10 * ((Back_Date32 >> 4) & 0xF);
            bai = 100 * ((Back_Date32 >> 8) & 0xF);
            qian = 1000 * ((Back_Date32 >> 12) & 0xF);
            wan = 10000 * ((Back_Date32 >> 16) & 0xF);
            shiwan = 100000 * ((Back_Date32 >> 20) & 0xF);
            baiwan = 1000000 * ((Back_Date32 >> 24) & 0xF);
            qianwan = 10000000 * ((Back_Date32 >> 28) & 0xF);
            DC_set_32_temp = (ge + shi + bai + qian + wan + shiwan + baiwan + qianwan) / 100;
            Uint16_back = (UInt16)(DC_set_32_temp);
            return Uint16_back;
        }

        public UInt16 Convert_to_setUI(string STR1)
        {
            byte[] a = StringToByte32(STR1);
            UInt32 DC_set_32_temp;
            UInt16 Uint16_back;
            Uint16_back = 0;
            if (a.Length == 1)
            {
                Back_Date32 = a[0];
            }
            else if (a.Length == 2)
            {
                Back_Date32 = a[0];
                Back_Date32 = ((Back_Date32 << 8) + a[1]);
            }
            else if (a.Length == 3)
            {
                Back_Date32 = a[0];
                Back_Date32 = ((Back_Date32 << 8) + a[1]);
                Back_Date32 = ((Back_Date32 << 8) + a[2]);
            }
            else if (a.Length == 4)
            {
                Back_Date32 = a[0];
                Back_Date32 = ((Back_Date32 << 8) + a[1]);
                Back_Date32 = ((Back_Date32 << 8) + a[2]);
                Back_Date32 = ((Back_Date32 << 8) + a[3]);
            }
          
            Uint16_back = (UInt16)((Back_Date32 >> 8)&0xFFFF);
            return Uint16_back;
        }
    }

}
