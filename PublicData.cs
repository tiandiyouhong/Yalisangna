using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;


namespace 电源生产测试软件
{

    partial class Form_Main
    {
        string flag_btnclick = "遥信";                 //定义点击的按键标志位
        string flag_btnclick_pwr = "关机";                 //定义点击的按键标志位
        static int[] CRCHI = new int[]  {
            0x00,0xc1,0x81,0x40,0x01,0xc0,0x80,0x41,0x01,0xc0,0x80,0x41,0x00,0xc1,0x81,
            0x40,0x01,0xc0,0x80,0x41,0x00,0xc1,0x81,0x40,0x00,0xc1,0x81,0x40,0x01,0xc0,
            0x80,0x41,0x01,0xc0,0x80,0x41,0x00,0xc1,0x81,0x40,0x00,0xc1,0x81,0x40,0x01,
            0xc0,0x80,0x41,0x00,0xc1,0x81,0x40,0x01,0xc0,0x80,0x41,0x01,0xc0,0x80,0x41,
            0x00,0xc1,0x81,0x40,0x01,0xc0,0x80,0x41,0x00,0xc1,0x81,0x40,0x00,0xc1,0x81,
            0x40,0x01,0xc0,0x80,0x41,0x00,0xc1,0x81,0x40,0x01,0xc0,0x80,0x41,0x01,0xc0,
            0x80,0x41,0x00,0xc1,0x81,0x40,0x00,0xc1,0x81,0x40,0x01,0xc0,0x80,0x41,0x01,
            0xc0,0x80,0x41,0x00,0xc1,0x81,0x40,0x01,0xc0,0x80,0x41,0x00,0xc1,0x81,0x40,
            0x00,0xc1,0x81,0x40,0x01,0xc0,0x80,0x41,0x01,0xc0,0x80,0x41,0x00,0xc1,0x81,
            0x40,0x00,0xc1,0x81,0x40,0x01,0xc0,0x80,0x41,0x00,0xc1,0x81,0x40,0x01,0xc0,
            0x80,0x41,0x01,0xc0,0x80,0x41,0x00,0xc1,0x81,0x40,0x00,0xc1,0x81,0x40,0x01,
            0xc0,0x80,0x41,0x01,0xc0,0x80,0x41,0x00,0xc1,0x81,0x40,0x01,0xc0,0x80,0x41,
            0x00,0xc1,0x81,0x40,0x00,0xc1,0x81,0x40,0x01,0xc0,0x80,0x41,0x00,0xc1,0x81,
            0x40,0x01,0xc0,0x80,0x41,0x01,0xc0,0x80,0x41,0x00,0xc1,0x81,0x40,0x01,0xc0,
            0x80,0x41,0x00,0xc1,0x81,0x40,0x00,0xc1,0x81,0x40,0x01,0xc0,0x80,0x41,0x01,
            0xc0,0x80,0x41,0x00,0xc1,0x81,0x40,0x00,0xc1,0x81,0x40,0x01,0xc0,0x80,0x41,
            0x00,0xc1,0x81,0x40,0x01,0xc0,0x80,0x41,0x01,0xc0,0x80,0x41,0x00,0xc1,0x81,
            0x40
        };

        static int[] CECLO = new int[]{
            0x00,0xc0,0xc1,0x01,0xc3,0x03,0x02,0xc2,0xc6,0x06,0x07,0xc7,0x05,0xc5,0xc4,
            0x04,0xcc,0x0c,0x0d,0xcd,0x0f,0xcf,0xce,0x0e,0x0a,0xca,0xcb,0x0b,0xc9,0x09,
            0x08,0xc8,0xd8,0x18,0x19,0xd9,0x1b,0xdb,0xda,0x1a,0x1e,0xde,0xdf,0x1f,0xdd,
            0x1d,0x1c,0xdc,0x14,0xd4,0xd5,0x15,0xd7,0x17,0x16,0xd6,0xd2,0x12,0x13,0xd3,
            0x11,0xd1,0xd0,0x10,0xf0,0x30,0x31,0xf1,0x33,0xf3,0xf2,0x32,0x36,0xf6,0xf7,
            0x37,0xf5,0x35,0x34,0xf4,0x3c,0xfc,0xfd,0x3d,0xff,0x3f,0x3e,0xfe,0xfa,0x3a,
            0x3b,0xfb,0x39,0xf9,0xf8,0x38,0x28,0xe8,0xe9,0x29,0xeb,0x2b,0x2a,0xea,0xee,
            0x2e,0x2f,0xef,0x2d,0xed,0xec,0x2c,0xe4,0x24,0x25,0xe5,0x27,0xe7,0xe6,0x26,
            0x22,0xe2,0xe3,0x23,0xe1,0x21,0x20,0xe0,0xa0,0x60,0x61,0xa1,0x63,0xa3,0xa2,
            0x62,0x66,0xa6,0xa7,0x67,0xa5,0x65,0x64,0xa4,0x6c,0xac,0xad,0x6d,0xaf,0x6f,
            0x6e,0xae,0xaa,0x6a,0x6b,0xab,0x69,0xa9,0xa8,0x68,0x78,0xb8,0xb9,0x79,0xbb,
            0x7b,0x7a,0xba,0xbe,0x7e,0x7f,0xbf,0x7d,0xbd,0xbc,0x7c,0xb4,0x74,0x75,0xb5,
            0x77,0xb7,0xb6,0x76,0x72,0xb2,0xb3,0x73,0xb1,0x71,0x70,0xb0,0x50,0x90,0x91,
            0x51,0x93,0x53,0x52,0x92,0x96,0x56,0x57,0x97,0x55,0x95,0x94,0x54,0x9c,0x5c,
            0x5d,0x9d,0x5f,0x9f,0x9e,0x5e,0x5a,0x9a,0x9b,0x5b,0x99,0x59,0x58,0x98,0x88,
            0x48,0x49,0x89,0x4b,0x8b,0x8a,0x4a,0x4e,0x8e,0x8f,0x4f,0x8d,0x4d,0x4c,0x8c,
            0x44,0x84,0x85,0x45,0x87,0x47,0x46,0x86,0x82,0x42,0x43,0x83,0x41,0x81,0x80,
            0x40
        };
        public static void CRC16_Check(ArrayList array)//CRC16校验，高字节在前
        {
            byte[] temdata = new byte[array.Count];
            for (int m = 0; m < temdata.Length; m++)
            {
                temdata[m] = Convert.ToByte(array[m]);
            }
            int xda, xdapoly;
            int i, j, xdabit;
            xda = 0xFFFF;
            xdapoly = 0xA001;
            for (i = 0; i < temdata.Length; i++)
            {
                xda ^= temdata[i];
                for (j = 0; j < 8; j++)
                {
                    xdabit = (int)(xda & 0x01);
                    xda >>= 1;
                    if (xdabit == 1)
                        xda ^= xdapoly;
                }
            }
            array.Add((byte)(xda & 0xFF));
            array.Add((byte)(xda >> 8));
        }

            /// <summary>
            /// 累加校验和
            /// </summary>
            /// <param name="memorySpage">需要校验的数据</param>
            /// <returns>返回校验和结果</returns>
            public static void ADD8_Add(ArrayList memorySpage)
            {
                byte[] temdata = new byte[memorySpage.Count];


              byte sum = 0;
                for (int i = 0; i < memorySpage.Count; i++)
                {
                    temdata[i] = Convert.ToByte(memorySpage[i]);
                    sum += temdata[i];
                }
   
                memorySpage.Add(Convert.ToByte(sum));
       
            }

            /// <summary>
            /// 累加校验和
            /// </summary>
            /// <param name="memorySpage">需要校验的数据</param>
            /// <returns>返回校验是否正确</returns>
            //public static bool ADD8_Check(byte[] memorySpage)
            //{
            //    List<byte> list = new List<byte>();
            //    list.AddRange(memorySpage);
            //    byte a = list[list.Count - 1];
            //    list.RemoveAt(list.Count - 1);
            //    int sum = 0;
            //    for (int i = 0; i < list.Count; i++)
            //    {
            //        sum += list[i];
            //    }
            //    sum = sum & 0xff;
            //    var str = sum.ToString("X");
            //    if (str.ToHex()[0] == a)
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}
 

        public static void Delay_Ms(int milliSecond)//mS级延时函数（不死机）
        {
            int start = Environment.TickCount;
            while (Math.Abs(Environment.TickCount - start) < milliSecond)
            {
                Application.DoEvents();
            }
        }
        public byte[] F_rev_data_checksum(byte[] data_temp, int len)
        {
            int HI = 0xff, LO = 0xff;
            int i = 0;
            byte[] temp_data = new byte[2];
            for (int j = 0; j < len; j++)
            {
                i = HI ^ data_temp[j];
                HI = LO ^ CRCHI[i];
                LO = CECLO[i];
            }
            temp_data[0] = (byte)HI;
            temp_data[1] = (byte)LO;
            return temp_data;
        }
        public byte[] CRC_Data_Check(byte[] data_temp, int len)
        {
            int HI = 0xff, LO = 0xff;
            int i = 0, j = 0;
            byte[] temp_data = new byte[2];
            for (j = 0; j < len; j++)
            {
                i = HI ^ data_temp[j];
                HI = LO ^ CRCHI[i];
                LO = CECLO[i];
            }
            if ((HI != data_temp[j] || LO != data_temp[j + 1]) && (LO != data_temp[j] || HI != data_temp[j + 1]))
            {
                temp_data[0] = 0xFF;
            }
            else
            {
                temp_data[0] = 0;
            }
            return temp_data;
        }
        public static void CRC16_put(byte[] A1)//CRC16校验，高字节在前
        {
            byte[] temdata = new byte[A1.Length - 2];
            for (int m = 0; m < temdata.Length; m++)
            {
                temdata[m] = A1[m];
            }
            int xda, xdapoly;
            int i, j, xdabit;
            xda = 0xFFFF;
            xdapoly = 0xA001;
            for (i = 0; i < temdata.Length; i++)
            {
                xda ^= temdata[i];
                for (j = 0; j < 8; j++)
                {
                    xdabit = (int)(xda & 0x01);
                    xda >>= 1;
                    if (xdabit == 1)
                        xda ^= xdapoly;
                }
            }
            A1[i] = ((byte)(xda >> 8));
            A1[i + 1] = ((byte)(xda & 0xFF));
        }

        ArrayList Array_BMS1 = new ArrayList();    //定义逆变器动态数组
        ArrayList Array_Load = new ArrayList();    //定义逆变器动态数组
        ArrayList Array_CaiJiQi = new ArrayList();     //定义透传动态数组
        ArrayList Array_ZhuZhan = new ArrayList();     //定义376报文动态数组
        ArrayList Array_YC = new ArrayList();     //定义376报文动态数组
        ArrayList Array_BMS = new ArrayList();     //定义376报文动态数组
                                                   // ArrayList Array_YC = new ArrayList();     //定义376报文动态数组
        int[] YC_Data = new int[64];             //定义返回数据数组
        int[] YC_Set = new int[64];             //定义返回数据数组
        char[] Ver = new char[32];
        byte[] BMS_Ver_A1_Read = new byte[64];
        byte[] Com_Rc_Data = new byte[1024];
        char[] Dc_Pwr_Vol = new char[6];
        char[] Dc_Pwr_Cur = new char[6];
        char[] Dc_Pwr_P = new char[6];
        public static UInt16 Com_Rc_Data_Lenth;
        public UInt16 Recorder_len;
        public UInt16 Recorder_num;
        public UInt16 Flag_Com;

        public UInt16 Com_OverTime_ctc;
        public static byte[] BMS_Ver_A1_Write = new byte[64];

        public UInt16 U_set;
        public UInt16 U_set_pwr;
        public UInt16 I_set;
        public UInt16 U_Adj_set1;
        public UInt16 U_Adj_set2;
        public UInt16 I_Adj_set1;
        public UInt16 I_Adj_set2;
        public UInt16 U_Samp1;
        public UInt16 U_Samp2;
        public UInt16 U_Samp_adjust;
        public UInt16 U_Samp_min=5750;//58.00-0.50
        public UInt16 U_Samp_max=5850;//58.00+0.50

        public UInt16 I_Samp_adjust;
        public UInt16 I_Samp_min = 1950;//20.0 -0.50
        public UInt16 I_Samp_max = 2050;//


        public UInt16 I_Samp_drop;
        public UInt16 I_Samp_drop_min = 900;//20.0 -0.50
        public UInt16 I_Samp_drop_max = 1100;//

        public UInt16 U_Samp_short;
        public UInt16 U_Samp_short_min = 200;//20.0 -0.50
        public UInt16 U_Samp_short_max = 5000;//

        public UInt16 Adjust_Flag=0;
        public UInt16 Drop_Flag = 0;
        public UInt16 I_Samp1;
        public UInt16 I_Samp2;
        public UInt16 YX_CTL;
        UInt16 Back_Date ;
        UInt32 Back_Date32;
        UInt32 shi;
        UInt32 bai;
        UInt32 qian;
        UInt32 wan;
        UInt32 ge;
        UInt32 shiwan;
        UInt32 baiwan;
        UInt32 qianwan;
        UInt32 yi;
        UInt32 shiyi;
        UInt32 DC_set_32;
        UInt32 U_Power;

        byte[] Data_Rec = new byte[1024];             //定义返回数据数组  
        byte[] Data_Rec_buffer = new byte[120];
        byte[] Recorder = new byte[65];
        int uuID = 0;                                  //定义376报文消息ID
        int flag_error = 0;                            //定义错误信息标志位

        int ReciviedRight;                            //定义接收数据正确标志位
        int flag_ClearDataShow = 1;                    //定义清空数据标志位
        public static string BMS_Sn = "0000000000000000";
        public static string Recorder_path = "0000000000000000故障记录";
        Fault_Tables bms_fault = new Fault_Tables();
        Charge_Tables bms_charge = new Charge_Tables();
        Charge_Tables bms_discharge = new Charge_Tables();
        /// <summary>
        /// 给逆变器动态数组赋值函数
        /// </summary>
        public void ArrayForBMS()
        {
            Array_BMS1.Clear();//清空逆变器动态数组
            switch (flag_btnclick)
            {
                case "遥信":
                    Array_BMS1.Add(Convert.ToByte(0xA2));
                    Array_BMS1.Add(Convert.ToByte(0x02));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0));
                    Array_BMS1.Add(Convert.ToByte(32));
                    break;
                case "遥测":
                    Array_BMS1.Add(Convert.ToByte(0xA2));
                    Array_BMS1.Add(Convert.ToByte(0x03));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0));//((int.Parse(textBox_YCNum.Text)) & 0xff00) >> 8));
                    Array_BMS1.Add(Convert.ToByte(6));//(int.Parse(textBox_YCNum.Text)) & 0x00ff));
                    break;
                case "控制遥测":
                    Array_BMS1.Add(Convert.ToByte(0xCF));
                    Array_BMS1.Add(Convert.ToByte(0x03));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0));//((int.Parse(textBox_YCNum.Text)) & 0xff00) >> 8));
                    Array_BMS1.Add(Convert.ToByte(12));//(int.Parse(textBox_YCNum.Text)) & 0x00ff));
                    break;
                case "读版本号":
                    Array_BMS1.Add(Convert.ToByte(0x01));
                    Array_BMS1.Add(Convert.ToByte(0x09));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x20));

                    break;
                case "自动模式":
                    Array_BMS1.Add(Convert.ToByte(0xA2));
                    Array_BMS1.Add(Convert.ToByte(0x11));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x08));
                    Array_BMS1.Add(Convert.ToByte(0xAA));
                    Array_BMS1.Add(Convert.ToByte(0xA1));
                    Array_BMS1.Add(Convert.ToByte(0xA2));
                    Array_BMS1.Add(Convert.ToByte(0xA3));
                    Array_BMS1.Add(Convert.ToByte(0xA4));
                    Array_BMS1.Add(Convert.ToByte(0xA5));
                    Array_BMS1.Add(Convert.ToByte(0xA6));
                    Array_BMS1.Add(Convert.ToByte(0xA7));
                    break;
                case "维护模式":
                    Array_BMS1.Add(Convert.ToByte(0xA2));
                    Array_BMS1.Add(Convert.ToByte(0x11));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x08));
                    Array_BMS1.Add(Convert.ToByte(0xFF));
                    Array_BMS1.Add(Convert.ToByte(0xF1));
                    Array_BMS1.Add(Convert.ToByte(0xF2));
                    Array_BMS1.Add(Convert.ToByte(0xF3));
                    Array_BMS1.Add(Convert.ToByte(0xF4));
                    Array_BMS1.Add(Convert.ToByte(0xF5));
                    Array_BMS1.Add(Convert.ToByte(0xF6));
                    Array_BMS1.Add(Convert.ToByte(0xF7));

                    break;
                case "进入校准模式":
                    Array_BMS1.Add(Convert.ToByte(0xA2));
                    Array_BMS1.Add(Convert.ToByte(0x11));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x08));
                    Array_BMS1.Add(Convert.ToByte(0xEE));
                    Array_BMS1.Add(Convert.ToByte(0xE1));
                    Array_BMS1.Add(Convert.ToByte(0xE2));
                    Array_BMS1.Add(Convert.ToByte(0xE3));
                    Array_BMS1.Add(Convert.ToByte(0xE4));
                    Array_BMS1.Add(Convert.ToByte(0xE5));
                    Array_BMS1.Add(Convert.ToByte(0xE6));
                    Array_BMS1.Add(Convert.ToByte(0xE7));
                 
                    break;
                case "退出校准模式":
                    Array_BMS1.Add(Convert.ToByte(0xA2));
                    Array_BMS1.Add(Convert.ToByte(0x11));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x08));
                    Array_BMS1.Add(Convert.ToByte(0xEE));
                    Array_BMS1.Add(Convert.ToByte(0xE7));
                    Array_BMS1.Add(Convert.ToByte(0xE6));
                    Array_BMS1.Add(Convert.ToByte(0xE5));
                    Array_BMS1.Add(Convert.ToByte(0xE4));
                    Array_BMS1.Add(Convert.ToByte(0xE3));
                    Array_BMS1.Add(Convert.ToByte(0xE2));
                    Array_BMS1.Add(Convert.ToByte(0xE1));
                    break;
                case "恢复默认参数":
                    Array_BMS1.Add(Convert.ToByte(0xA2));
                    Array_BMS1.Add(Convert.ToByte(0x5F));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x05));
                    Array_BMS1.Add(Convert.ToByte(0x5F));
                    Array_BMS1.Add(Convert.ToByte(0xC1));
                    Array_BMS1.Add(Convert.ToByte(0x12));
                    Array_BMS1.Add(Convert.ToByte(0x34));
                    Array_BMS1.Add(Convert.ToByte(0x56));
                    break;
                case "恢复控制默认参数":
                    Array_BMS1.Add(Convert.ToByte(0xCF));
                    Array_BMS1.Add(Convert.ToByte(0x5F));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x05));
                    Array_BMS1.Add(Convert.ToByte(0x5F));
                    Array_BMS1.Add(Convert.ToByte(0xC1));
                    Array_BMS1.Add(Convert.ToByte(0x12));
                    Array_BMS1.Add(Convert.ToByte(0x34));
                    Array_BMS1.Add(Convert.ToByte(0x56));
                    break;
                case "第一次给定电压":
                    Array_BMS1.Add(Convert.ToByte(0xA2));
                    Array_BMS1.Add(Convert.ToByte(0x51));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x05));
                    Array_BMS1.Add(Convert.ToByte(0xDD));
                    Array_BMS1.Add(Convert.ToByte(0xC1));
                    Array_BMS1.Add(Convert.ToByte(0xB1));
                    Array_BMS1.Add(Convert.ToByte(U_Adj_set1 >> 8));
                    Array_BMS1.Add(Convert.ToByte(U_Adj_set1 & 0xFF));
                    break;
                case "第一次实际电压":
                    Array_BMS1.Add(Convert.ToByte(0xA2));
                    Array_BMS1.Add(Convert.ToByte(0x51));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x05));
                    Array_BMS1.Add(Convert.ToByte(0xEE));
                    Array_BMS1.Add(Convert.ToByte(0xC1));
                    Array_BMS1.Add(Convert.ToByte(0xB1));
                    Array_BMS1.Add(Convert.ToByte(U_Samp1 >> 8));
                    Array_BMS1.Add(Convert.ToByte(U_Samp1 & 0xFF));
                    break;
                case "第二次给定电压":
                    Array_BMS1.Add(Convert.ToByte(0xA2));
                    Array_BMS1.Add(Convert.ToByte(0x51));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x05));
                    Array_BMS1.Add(Convert.ToByte(0xDD));
                    Array_BMS1.Add(Convert.ToByte(0xC1));
                    Array_BMS1.Add(Convert.ToByte(0xB2));
                    Array_BMS1.Add(Convert.ToByte(U_Adj_set2>>8));
                    Array_BMS1.Add(Convert.ToByte(U_Adj_set2 & 0xFF));
                    break;
                case "第二次实际电压":
                    Array_BMS1.Add(Convert.ToByte(0xA2));
                    Array_BMS1.Add(Convert.ToByte(0x51));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x05));
                    Array_BMS1.Add(Convert.ToByte(0xEE));
                    Array_BMS1.Add(Convert.ToByte(0xC1));
                    Array_BMS1.Add(Convert.ToByte(0xB2));
                    Array_BMS1.Add(Convert.ToByte(U_Samp2 >> 8));
                    Array_BMS1.Add(Convert.ToByte(U_Samp2 & 0xFF));
                    break;

                case "第一次给定电流":
                    Array_BMS1.Add(Convert.ToByte(0xA2));
                    Array_BMS1.Add(Convert.ToByte(0x52));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x05));
                    Array_BMS1.Add(Convert.ToByte(0xDD));
                    Array_BMS1.Add(Convert.ToByte(0xC1));
                    Array_BMS1.Add(Convert.ToByte(0xB1));
                    Array_BMS1.Add(Convert.ToByte(I_Adj_set1 >> 8));
                    Array_BMS1.Add(Convert.ToByte(I_Adj_set1 & 0xFF));
                    break;
                case "第一次实际电流":
                    Array_BMS1.Add(Convert.ToByte(0xA2));
                    Array_BMS1.Add(Convert.ToByte(0x52));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x05));
                    Array_BMS1.Add(Convert.ToByte(0xEE));
                    Array_BMS1.Add(Convert.ToByte(0xC1));
                    Array_BMS1.Add(Convert.ToByte(0xB1));
                    Array_BMS1.Add(Convert.ToByte(I_Samp1 >> 8));
                    Array_BMS1.Add(Convert.ToByte(I_Samp1 & 0xFF));
                    break;
                case "第二次给定电流":
                    Array_BMS1.Add(Convert.ToByte(0xA2));
                    Array_BMS1.Add(Convert.ToByte(0x52));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x05));
                    Array_BMS1.Add(Convert.ToByte(0xDD));
                    Array_BMS1.Add(Convert.ToByte(0xC1));
                    Array_BMS1.Add(Convert.ToByte(0xB2));
                    Array_BMS1.Add(Convert.ToByte(I_Adj_set2 >> 8));
                    Array_BMS1.Add(Convert.ToByte(I_Adj_set2 & 0xFF));
                    break;
                case "第二次实际电流":
                    Array_BMS1.Add(Convert.ToByte(0xA2));
                    Array_BMS1.Add(Convert.ToByte(0x52));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x05));
                    Array_BMS1.Add(Convert.ToByte(0xEE));
                    Array_BMS1.Add(Convert.ToByte(0xC1));
                    Array_BMS1.Add(Convert.ToByte(0xB2));
                    Array_BMS1.Add(Convert.ToByte(I_Samp2 >> 8));
                    Array_BMS1.Add(Convert.ToByte(I_Samp2 & 0xFF));
                    break;
                case "输出给定":
                    Array_BMS1.Add(Convert.ToByte(0xA2));
                    Array_BMS1.Add(Convert.ToByte(0x13));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x06));
                    Array_BMS1.Add(Convert.ToByte(0xDD));
                    Array_BMS1.Add(Convert.ToByte(0xC1));
                    Array_BMS1.Add(Convert.ToByte(U_set >> 8));
                    Array_BMS1.Add(Convert.ToByte(U_set&0xFF));
                    Array_BMS1.Add(Convert.ToByte(I_set >> 8));
                    Array_BMS1.Add(Convert.ToByte(I_set&0xFF));
                    break;
                case "控制第一次给电压值":
                    Array_BMS1.Add(Convert.ToByte(0xCF));
                    Array_BMS1.Add(Convert.ToByte(0x54));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x05));
                    Array_BMS1.Add(Convert.ToByte(0xEE));
                    Array_BMS1.Add(Convert.ToByte(0xCF));
                    Array_BMS1.Add(Convert.ToByte(0xB1));
                    Array_BMS1.Add(Convert.ToByte(U_Samp1 >> 8));
                    Array_BMS1.Add(Convert.ToByte(U_Samp1 & 0xFF));
                 
                    break;
                case "控制第二次给电压值":
                    Array_BMS1.Add(Convert.ToByte(0xCF));
                    Array_BMS1.Add(Convert.ToByte(0x54));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x05));
                    Array_BMS1.Add(Convert.ToByte(0xEE));
                    Array_BMS1.Add(Convert.ToByte(0xCF));
                    Array_BMS1.Add(Convert.ToByte(0xB2));
                    Array_BMS1.Add(Convert.ToByte(U_Samp1 >> 8));
                    Array_BMS1.Add(Convert.ToByte(U_Samp1 & 0xFF));
                    break;
                case "写配置信息A1":
                    Array_BMS1.Add(Convert.ToByte(0x01));
                    Array_BMS1.Add(Convert.ToByte(0xB1));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x20));
                    CRC16_put(BMS_Ver_A1_Write);
                    for (int i = 0; i < 64; i++)
                    {
                        Array_BMS1.Add(Convert.ToByte(BMS_Ver_A1_Write[i]));
                    }
                    break;
                case "读配置信息A1":
                    Array_BMS1.Add(Convert.ToByte(0x01));
                    Array_BMS1.Add(Convert.ToByte(0xA1));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x20));
                    break;
                case "读取故障记录长度":
                    Array_BMS1.Add(Convert.ToByte(0x01));
                    Array_BMS1.Add(Convert.ToByte(0xA3));
                    Array_BMS1.Add(Convert.ToByte(0x11));
                    Array_BMS1.Add(Convert.ToByte(0x11));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    break;
                case "读充电记录长度":
                    Array_BMS1.Add(Convert.ToByte(0x01));
                    Array_BMS1.Add(Convert.ToByte(0xA3));
                    Array_BMS1.Add(Convert.ToByte(0x22));
                    Array_BMS1.Add(Convert.ToByte(0x22));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    break;
                case "读放电记录长度":
                    Array_BMS1.Add(Convert.ToByte(0x01));
                    Array_BMS1.Add(Convert.ToByte(0xA3));
                    Array_BMS1.Add(Convert.ToByte(0x33));
                    Array_BMS1.Add(Convert.ToByte(0x33));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    Array_BMS1.Add(Convert.ToByte(0x00));
                    break;
                case "读取故障记录数据":

                    Array_BMS1.Add(Convert.ToByte(0x01));
                    Array_BMS1.Add(Convert.ToByte(0xA4));
                    Array_BMS1.Add(Convert.ToByte(0x11));
                    Array_BMS1.Add(Convert.ToByte(0x11));
                    Array_BMS1.Add(Convert.ToByte(Recorder_num >> 8));
                    Array_BMS1.Add(Convert.ToByte(Recorder_num & 0xff));
                    break;
                case "读充电记录数据":
                    Array_BMS1.Add(Convert.ToByte(0x01));
                    Array_BMS1.Add(Convert.ToByte(0xA4));
                    Array_BMS1.Add(Convert.ToByte(0x22));
                    Array_BMS1.Add(Convert.ToByte(0x22));
                    Array_BMS1.Add(Convert.ToByte(Recorder_num >> 8));
                    Array_BMS1.Add(Convert.ToByte(Recorder_num & 0xff));
                    break;
                case "读放电记录数据":
                    Array_BMS1.Add(Convert.ToByte(0x01));
                    Array_BMS1.Add(Convert.ToByte(0xA4));
                    Array_BMS1.Add(Convert.ToByte(0x33));
                    Array_BMS1.Add(Convert.ToByte(0x33));
                    Array_BMS1.Add(Convert.ToByte(Recorder_num >> 8));
                    Array_BMS1.Add(Convert.ToByte(Recorder_num & 0xff));

                    break;
                default: MessageBox.Show("本软件无此功能！", "提示！");
                    break;
            }

            CRC16_Check(Array_BMS1);
        }

        public void ArrayForDc_elec_load()
        {
            Array_Load.Clear();//清空逆变器动态数组
            switch (flag_btnclick)
            {               
                case "程控模式":
                    Array_Load.Add(Convert.ToByte(0xAA));//1
                    Array_Load.Add(Convert.ToByte(0x00));//2
                    Array_Load.Add(Convert.ToByte(0x20));//3
                    Array_Load.Add(Convert.ToByte(0x01));//4**
                    Array_Load.Add(Convert.ToByte(0));   //5
                    Array_Load.Add(Convert.ToByte(0));   //6
                    Array_Load.Add(Convert.ToByte(0));   //7
                    Array_Load.Add(Convert.ToByte(0));   //8
                    Array_Load.Add(Convert.ToByte(0));   //9
                    Array_Load.Add(Convert.ToByte(0));   //10
                    Array_Load.Add(Convert.ToByte(0));   //11
                    Array_Load.Add(Convert.ToByte(0));   //12
                    Array_Load.Add(Convert.ToByte(0));   //13
                    Array_Load.Add(Convert.ToByte(0));   //14
                    Array_Load.Add(Convert.ToByte(0));   //15
                    Array_Load.Add(Convert.ToByte(0));   //16
                    Array_Load.Add(Convert.ToByte(0));   //17
                    Array_Load.Add(Convert.ToByte(0));   //18
                    Array_Load.Add(Convert.ToByte(0));   //19
                    Array_Load.Add(Convert.ToByte(0));   //20
                    Array_Load.Add(Convert.ToByte(0));   //21
                    Array_Load.Add(Convert.ToByte(0));   //22
                    Array_Load.Add(Convert.ToByte(0));   //23
                    Array_Load.Add(Convert.ToByte(0));   //24
                    Array_Load.Add(Convert.ToByte(0));   //25
                
                break;              
                case "手动模式":
                    Array_Load.Add(Convert.ToByte(0xAA));//1
                    Array_Load.Add(Convert.ToByte(0x00));//2
                    Array_Load.Add(Convert.ToByte(0x20));//3
                    Array_Load.Add(Convert.ToByte(0x00));//4**
                    Array_Load.Add(Convert.ToByte(0));   //5
                    Array_Load.Add(Convert.ToByte(0));   //6
                    Array_Load.Add(Convert.ToByte(0));   //7
                    Array_Load.Add(Convert.ToByte(0));   //8
                    Array_Load.Add(Convert.ToByte(0));   //9
                    Array_Load.Add(Convert.ToByte(0));   //10
                    Array_Load.Add(Convert.ToByte(0));   //11
                    Array_Load.Add(Convert.ToByte(0));   //12
                    Array_Load.Add(Convert.ToByte(0));   //13
                    Array_Load.Add(Convert.ToByte(0));   //14
                    Array_Load.Add(Convert.ToByte(0));   //15
                    Array_Load.Add(Convert.ToByte(0));   //16
                    Array_Load.Add(Convert.ToByte(0));   //17
                    Array_Load.Add(Convert.ToByte(0));   //18
                    Array_Load.Add(Convert.ToByte(0));   //19
                    Array_Load.Add(Convert.ToByte(0));   //20
                    Array_Load.Add(Convert.ToByte(0));   //21
                    Array_Load.Add(Convert.ToByte(0));   //22
                    Array_Load.Add(Convert.ToByte(0));   //23
                    Array_Load.Add(Convert.ToByte(0));   //24
                    Array_Load.Add(Convert.ToByte(0));   //25
                    break;
                case "开机":
                    Array_Load.Add(Convert.ToByte(0xAA));//1
                    Array_Load.Add(Convert.ToByte(0x00));//2
                    Array_Load.Add(Convert.ToByte(0x21));//3
                    Array_Load.Add(Convert.ToByte(0x01));//4
                    Array_Load.Add(Convert.ToByte(0));   //5
                    Array_Load.Add(Convert.ToByte(0));   //6
                    Array_Load.Add(Convert.ToByte(0));   //7
                    Array_Load.Add(Convert.ToByte(0));   //8
                    Array_Load.Add(Convert.ToByte(0));   //9
                    Array_Load.Add(Convert.ToByte(0));   //10
                    Array_Load.Add(Convert.ToByte(0));   //11
                    Array_Load.Add(Convert.ToByte(0));   //12
                    Array_Load.Add(Convert.ToByte(0));   //13
                    Array_Load.Add(Convert.ToByte(0));   //14
                    Array_Load.Add(Convert.ToByte(0));   //15
                    Array_Load.Add(Convert.ToByte(0));   //16
                    Array_Load.Add(Convert.ToByte(0));   //17
                    Array_Load.Add(Convert.ToByte(0));   //18
                    Array_Load.Add(Convert.ToByte(0));   //19
                    Array_Load.Add(Convert.ToByte(0));   //20
                    Array_Load.Add(Convert.ToByte(0));   //21
                    Array_Load.Add(Convert.ToByte(0));   //22
                    Array_Load.Add(Convert.ToByte(0));   //23
                    Array_Load.Add(Convert.ToByte(0));   //24
                    Array_Load.Add(Convert.ToByte(0));   //25
                    break;
                case "关机":
                    Array_Load.Add(Convert.ToByte(0xAA));//1
                    Array_Load.Add(Convert.ToByte(0x00));//2
                    Array_Load.Add(Convert.ToByte(0x21));//3
                    Array_Load.Add(Convert.ToByte(0x00));//4**
                    Array_Load.Add(Convert.ToByte(0));   //5
                    Array_Load.Add(Convert.ToByte(0));   //6
                    Array_Load.Add(Convert.ToByte(0));   //7
                    Array_Load.Add(Convert.ToByte(0));   //8
                    Array_Load.Add(Convert.ToByte(0));   //9
                    Array_Load.Add(Convert.ToByte(0));   //10
                    Array_Load.Add(Convert.ToByte(0));   //11
                    Array_Load.Add(Convert.ToByte(0));   //12
                    Array_Load.Add(Convert.ToByte(0));   //13
                    Array_Load.Add(Convert.ToByte(0));   //14
                    Array_Load.Add(Convert.ToByte(0));   //15
                    Array_Load.Add(Convert.ToByte(0));   //16
                    Array_Load.Add(Convert.ToByte(0));   //17
                    Array_Load.Add(Convert.ToByte(0));   //18
                    Array_Load.Add(Convert.ToByte(0));   //19
                    Array_Load.Add(Convert.ToByte(0));   //20
                    Array_Load.Add(Convert.ToByte(0));   //21
                    Array_Load.Add(Convert.ToByte(0));   //22
                    Array_Load.Add(Convert.ToByte(0));   //23
                    Array_Load.Add(Convert.ToByte(0));   //24
                    Array_Load.Add(Convert.ToByte(0));   //25
                    break;
                case "读取电压":
                    Array_Load.Add(Convert.ToByte(0xAA));//1
                    Array_Load.Add(Convert.ToByte(0x00));//2
                    Array_Load.Add(Convert.ToByte(0x5F));//3
                    Array_Load.Add(Convert.ToByte(0x00));//4**
                    Array_Load.Add(Convert.ToByte(0));   //5
                    Array_Load.Add(Convert.ToByte(0));   //6
                    Array_Load.Add(Convert.ToByte(0));   //7
                    Array_Load.Add(Convert.ToByte(0));   //8
                    Array_Load.Add(Convert.ToByte(0));   //9
                    Array_Load.Add(Convert.ToByte(0));   //10
                    Array_Load.Add(Convert.ToByte(0));   //11
                    Array_Load.Add(Convert.ToByte(0));   //12
                    Array_Load.Add(Convert.ToByte(0));   //13
                    Array_Load.Add(Convert.ToByte(0));   //14
                    Array_Load.Add(Convert.ToByte(0));   //15
                    Array_Load.Add(Convert.ToByte(0));   //16
                    Array_Load.Add(Convert.ToByte(0));   //17
                    Array_Load.Add(Convert.ToByte(0));   //18
                    Array_Load.Add(Convert.ToByte(0));   //19
                    Array_Load.Add(Convert.ToByte(0));   //20
                    Array_Load.Add(Convert.ToByte(0));   //21
                    Array_Load.Add(Convert.ToByte(0));   //22
                    Array_Load.Add(Convert.ToByte(0));   //23
                    Array_Load.Add(Convert.ToByte(0));   //24
                    Array_Load.Add(Convert.ToByte(0));   //25
                    break;
                case "CC":
                    Array_Load.Add(Convert.ToByte(0xAA));//1
                    Array_Load.Add(Convert.ToByte(0x00));//2
                    Array_Load.Add(Convert.ToByte(0x28));//3
                    Array_Load.Add(Convert.ToByte(0x00));//4**
                    Array_Load.Add(Convert.ToByte(0));   //5
                    Array_Load.Add(Convert.ToByte(0));   //6
                    Array_Load.Add(Convert.ToByte(0));   //7
                    Array_Load.Add(Convert.ToByte(0));   //8
                    Array_Load.Add(Convert.ToByte(0));   //9
                    Array_Load.Add(Convert.ToByte(0));   //10
                    Array_Load.Add(Convert.ToByte(0));   //11
                    Array_Load.Add(Convert.ToByte(0));   //12
                    Array_Load.Add(Convert.ToByte(0));   //13
                    Array_Load.Add(Convert.ToByte(0));   //14
                    Array_Load.Add(Convert.ToByte(0));   //15
                    Array_Load.Add(Convert.ToByte(0));   //16
                    Array_Load.Add(Convert.ToByte(0));   //17
                    Array_Load.Add(Convert.ToByte(0));   //18
                    Array_Load.Add(Convert.ToByte(0));   //19
                    Array_Load.Add(Convert.ToByte(0));   //20
                    Array_Load.Add(Convert.ToByte(0));   //21
                    Array_Load.Add(Convert.ToByte(0));   //22
                    Array_Load.Add(Convert.ToByte(0));   //23
                    Array_Load.Add(Convert.ToByte(0));   //24
                    Array_Load.Add(Convert.ToByte(0));   //25
                    break;
                case "CV":
                    Array_Load.Add(Convert.ToByte(0xAA));//1
                    Array_Load.Add(Convert.ToByte(0x00));//2
                    Array_Load.Add(Convert.ToByte(0x28));//3
                    Array_Load.Add(Convert.ToByte(0x01));//4**
                    Array_Load.Add(Convert.ToByte(0));   //5
                    Array_Load.Add(Convert.ToByte(0));   //6
                    Array_Load.Add(Convert.ToByte(0));   //7
                    Array_Load.Add(Convert.ToByte(0));   //8
                    Array_Load.Add(Convert.ToByte(0));   //9
                    Array_Load.Add(Convert.ToByte(0));   //10
                    Array_Load.Add(Convert.ToByte(0));   //11
                    Array_Load.Add(Convert.ToByte(0));   //12
                    Array_Load.Add(Convert.ToByte(0));   //13
                    Array_Load.Add(Convert.ToByte(0));   //14
                    Array_Load.Add(Convert.ToByte(0));   //15
                    Array_Load.Add(Convert.ToByte(0));   //16
                    Array_Load.Add(Convert.ToByte(0));   //17
                    Array_Load.Add(Convert.ToByte(0));   //18
                    Array_Load.Add(Convert.ToByte(0));   //19
                    Array_Load.Add(Convert.ToByte(0));   //20
                    Array_Load.Add(Convert.ToByte(0));   //21
                    Array_Load.Add(Convert.ToByte(0));   //22
                    Array_Load.Add(Convert.ToByte(0));   //23
                    Array_Load.Add(Convert.ToByte(0));   //24
                    Array_Load.Add(Convert.ToByte(0));   //25
                    break;
                case "CW":
                    Array_Load.Add(Convert.ToByte(0xAA));//1
                    Array_Load.Add(Convert.ToByte(0x00));//2
                    Array_Load.Add(Convert.ToByte(0x28));//3
                    Array_Load.Add(Convert.ToByte(0x02));//4**
                    Array_Load.Add(Convert.ToByte(0));   //5
                    Array_Load.Add(Convert.ToByte(0));   //6
                    Array_Load.Add(Convert.ToByte(0));   //7
                    Array_Load.Add(Convert.ToByte(0));   //8
                    Array_Load.Add(Convert.ToByte(0));   //9
                    Array_Load.Add(Convert.ToByte(0));   //10
                    Array_Load.Add(Convert.ToByte(0));   //11
                    Array_Load.Add(Convert.ToByte(0));   //12
                    Array_Load.Add(Convert.ToByte(0));   //13
                    Array_Load.Add(Convert.ToByte(0));   //14
                    Array_Load.Add(Convert.ToByte(0));   //15
                    Array_Load.Add(Convert.ToByte(0));   //16
                    Array_Load.Add(Convert.ToByte(0));   //17
                    Array_Load.Add(Convert.ToByte(0));   //18
                    Array_Load.Add(Convert.ToByte(0));   //19
                    Array_Load.Add(Convert.ToByte(0));   //20
                    Array_Load.Add(Convert.ToByte(0));   //21
                    Array_Load.Add(Convert.ToByte(0));   //22
                    Array_Load.Add(Convert.ToByte(0));   //23
                    Array_Load.Add(Convert.ToByte(0));   //24
                    Array_Load.Add(Convert.ToByte(0));   //25
                    break;
                case "CR":
                    Array_Load.Add(Convert.ToByte(0xAA));//1
                    Array_Load.Add(Convert.ToByte(0x00));//2
                    Array_Load.Add(Convert.ToByte(0x28));//3
                    Array_Load.Add(Convert.ToByte(0x03));//4**
                    Array_Load.Add(Convert.ToByte(0));   //5
                    Array_Load.Add(Convert.ToByte(0));   //6
                    Array_Load.Add(Convert.ToByte(0));   //7
                    Array_Load.Add(Convert.ToByte(0));   //8
                    Array_Load.Add(Convert.ToByte(0));   //9
                    Array_Load.Add(Convert.ToByte(0));   //10
                    Array_Load.Add(Convert.ToByte(0));   //11
                    Array_Load.Add(Convert.ToByte(0));   //12
                    Array_Load.Add(Convert.ToByte(0));   //13
                    Array_Load.Add(Convert.ToByte(0));   //14
                    Array_Load.Add(Convert.ToByte(0));   //15
                    Array_Load.Add(Convert.ToByte(0));   //16
                    Array_Load.Add(Convert.ToByte(0));   //17
                    Array_Load.Add(Convert.ToByte(0));   //18
                    Array_Load.Add(Convert.ToByte(0));   //19
                    Array_Load.Add(Convert.ToByte(0));   //20
                    Array_Load.Add(Convert.ToByte(0));   //21
                    Array_Load.Add(Convert.ToByte(0));   //22
                    Array_Load.Add(Convert.ToByte(0));   //23
                    Array_Load.Add(Convert.ToByte(0));   //24
                    Array_Load.Add(Convert.ToByte(0));   //25
                    break;
                case "读CC值":
                    Array_Load.Add(Convert.ToByte(0xAA));//1
                    Array_Load.Add(Convert.ToByte(0x00));//2
                    Array_Load.Add(Convert.ToByte(0x2B));//3
                    Array_Load.Add(Convert.ToByte(0x00));//4**
                    Array_Load.Add(Convert.ToByte(0));   //5
                    Array_Load.Add(Convert.ToByte(0));   //6
                    Array_Load.Add(Convert.ToByte(0));   //7
                    Array_Load.Add(Convert.ToByte(0));   //8
                    Array_Load.Add(Convert.ToByte(0));   //9
                    Array_Load.Add(Convert.ToByte(0));   //10
                    Array_Load.Add(Convert.ToByte(0));   //11
                    Array_Load.Add(Convert.ToByte(0));   //12
                    Array_Load.Add(Convert.ToByte(0));   //13
                    Array_Load.Add(Convert.ToByte(0));   //14
                    Array_Load.Add(Convert.ToByte(0));   //15
                    Array_Load.Add(Convert.ToByte(0));   //16
                    Array_Load.Add(Convert.ToByte(0));   //17
                    Array_Load.Add(Convert.ToByte(0));   //18
                    Array_Load.Add(Convert.ToByte(0));   //19
                    Array_Load.Add(Convert.ToByte(0));   //20
                    Array_Load.Add(Convert.ToByte(0));   //21
                    Array_Load.Add(Convert.ToByte(0));   //22
                    Array_Load.Add(Convert.ToByte(0));   //23
                    Array_Load.Add(Convert.ToByte(0));   //24
                    Array_Load.Add(Convert.ToByte(0));   //25
                    break;
                case "读CV值":
                    Array_Load.Add(Convert.ToByte(0xAA));//1
                    Array_Load.Add(Convert.ToByte(0x00));//2
                    Array_Load.Add(Convert.ToByte(0x2D));//3
                    Array_Load.Add(Convert.ToByte(0x01));//4**
                    Array_Load.Add(Convert.ToByte(0));   //5
                    Array_Load.Add(Convert.ToByte(0));   //6
                    Array_Load.Add(Convert.ToByte(0));   //7
                    Array_Load.Add(Convert.ToByte(0));   //8
                    Array_Load.Add(Convert.ToByte(0));   //9
                    Array_Load.Add(Convert.ToByte(0));   //10
                    Array_Load.Add(Convert.ToByte(0));   //11
                    Array_Load.Add(Convert.ToByte(0));   //12
                    Array_Load.Add(Convert.ToByte(0));   //13
                    Array_Load.Add(Convert.ToByte(0));   //14
                    Array_Load.Add(Convert.ToByte(0));   //15
                    Array_Load.Add(Convert.ToByte(0));   //16
                    Array_Load.Add(Convert.ToByte(0));   //17
                    Array_Load.Add(Convert.ToByte(0));   //18
                    Array_Load.Add(Convert.ToByte(0));   //19
                    Array_Load.Add(Convert.ToByte(0));   //20
                    Array_Load.Add(Convert.ToByte(0));   //21
                    Array_Load.Add(Convert.ToByte(0));   //22
                    Array_Load.Add(Convert.ToByte(0));   //23
                    Array_Load.Add(Convert.ToByte(0));   //24
                    Array_Load.Add(Convert.ToByte(0));   //25
                    break;
                case "读CW值":
                    Array_Load.Add(Convert.ToByte(0xAA));//1
                    Array_Load.Add(Convert.ToByte(0x00));//2
                    Array_Load.Add(Convert.ToByte(0x2F));//3
                    Array_Load.Add(Convert.ToByte(0x02));//4**
                    Array_Load.Add(Convert.ToByte(0));   //5
                    Array_Load.Add(Convert.ToByte(0));   //6
                    Array_Load.Add(Convert.ToByte(0));   //7
                    Array_Load.Add(Convert.ToByte(0));   //8
                    Array_Load.Add(Convert.ToByte(0));   //9
                    Array_Load.Add(Convert.ToByte(0));   //10
                    Array_Load.Add(Convert.ToByte(0));   //11
                    Array_Load.Add(Convert.ToByte(0));   //12
                    Array_Load.Add(Convert.ToByte(0));   //13
                    Array_Load.Add(Convert.ToByte(0));   //14
                    Array_Load.Add(Convert.ToByte(0));   //15
                    Array_Load.Add(Convert.ToByte(0));   //16
                    Array_Load.Add(Convert.ToByte(0));   //17
                    Array_Load.Add(Convert.ToByte(0));   //18
                    Array_Load.Add(Convert.ToByte(0));   //19
                    Array_Load.Add(Convert.ToByte(0));   //20
                    Array_Load.Add(Convert.ToByte(0));   //21
                    Array_Load.Add(Convert.ToByte(0));   //22
                    Array_Load.Add(Convert.ToByte(0));   //23
                    Array_Load.Add(Convert.ToByte(0));   //24
                    Array_Load.Add(Convert.ToByte(0));   //25
                    break;
                case "读CR值":
                    Array_Load.Add(Convert.ToByte(0xAA));//1
                    Array_Load.Add(Convert.ToByte(0x00));//2
                    Array_Load.Add(Convert.ToByte(0x28));//3
                    Array_Load.Add(Convert.ToByte(0x31));//4**
                    Array_Load.Add(Convert.ToByte(0));   //5
                    Array_Load.Add(Convert.ToByte(0));   //6
                    Array_Load.Add(Convert.ToByte(0));   //7
                    Array_Load.Add(Convert.ToByte(0));   //8
                    Array_Load.Add(Convert.ToByte(0));   //9
                    Array_Load.Add(Convert.ToByte(0));   //10
                    Array_Load.Add(Convert.ToByte(0));   //11
                    Array_Load.Add(Convert.ToByte(0));   //12
                    Array_Load.Add(Convert.ToByte(0));   //13
                    Array_Load.Add(Convert.ToByte(0));   //14
                    Array_Load.Add(Convert.ToByte(0));   //15
                    Array_Load.Add(Convert.ToByte(0));   //16
                    Array_Load.Add(Convert.ToByte(0));   //17
                    Array_Load.Add(Convert.ToByte(0));   //18
                    Array_Load.Add(Convert.ToByte(0));   //19
                    Array_Load.Add(Convert.ToByte(0));   //20
                    Array_Load.Add(Convert.ToByte(0));   //21
                    Array_Load.Add(Convert.ToByte(0));   //22
                    Array_Load.Add(Convert.ToByte(0));   //23
                    Array_Load.Add(Convert.ToByte(0));   //24
                    Array_Load.Add(Convert.ToByte(0));   //25
                    break;
                case "设CC值":
                    Array_Load.Add(Convert.ToByte(0xAA));//1
                    Array_Load.Add(Convert.ToByte(0x00));//2
                    Array_Load.Add(Convert.ToByte(0x2A));//3
                    Array_Load.Add(Convert.ToByte(DC_set_32&0xFF));//4**
                    Array_Load.Add(Convert.ToByte((DC_set_32>>8) & 0xFF));   //5
                    Array_Load.Add(Convert.ToByte((DC_set_32 >>16) & 0xFF));    //6
                    Array_Load.Add(Convert.ToByte((DC_set_32 >>24) & 0xFF));    //7
                    Array_Load.Add(Convert.ToByte(0));   //8
                    Array_Load.Add(Convert.ToByte(0));   //9
                    Array_Load.Add(Convert.ToByte(0));   //10
                    Array_Load.Add(Convert.ToByte(0));   //11
                    Array_Load.Add(Convert.ToByte(0));   //12
                    Array_Load.Add(Convert.ToByte(0));   //13
                    Array_Load.Add(Convert.ToByte(0));   //14
                    Array_Load.Add(Convert.ToByte(0));   //15
                    Array_Load.Add(Convert.ToByte(0));   //16
                    Array_Load.Add(Convert.ToByte(0));   //17
                    Array_Load.Add(Convert.ToByte(0));   //18
                    Array_Load.Add(Convert.ToByte(0));   //19
                    Array_Load.Add(Convert.ToByte(0));   //20
                    Array_Load.Add(Convert.ToByte(0));   //21
                    Array_Load.Add(Convert.ToByte(0));   //22
                    Array_Load.Add(Convert.ToByte(0));   //23
                    Array_Load.Add(Convert.ToByte(0));   //24
                    Array_Load.Add(Convert.ToByte(0));   //25
                    break;
                case "设CV值":
                    Array_Load.Add(Convert.ToByte(0xAA));//1
                    Array_Load.Add(Convert.ToByte(0x00));//2
                    Array_Load.Add(Convert.ToByte(0x2C));//3
                    Array_Load.Add(Convert.ToByte(DC_set_32 & 0xFF));//4**
                    Array_Load.Add(Convert.ToByte((DC_set_32 >> 8) & 0xFF));   //5
                    Array_Load.Add(Convert.ToByte((DC_set_32 >> 16) & 0xFF));    //6
                    Array_Load.Add(Convert.ToByte((DC_set_32 >> 24) & 0xFF));    //7
                    Array_Load.Add(Convert.ToByte(0));   //8
                    Array_Load.Add(Convert.ToByte(0));   //9
                    Array_Load.Add(Convert.ToByte(0));   //10
                    Array_Load.Add(Convert.ToByte(0));   //11
                    Array_Load.Add(Convert.ToByte(0));   //12
                    Array_Load.Add(Convert.ToByte(0));   //13
                    Array_Load.Add(Convert.ToByte(0));   //14
                    Array_Load.Add(Convert.ToByte(0));   //15
                    Array_Load.Add(Convert.ToByte(0));   //16
                    Array_Load.Add(Convert.ToByte(0));   //17
                    Array_Load.Add(Convert.ToByte(0));   //18
                    Array_Load.Add(Convert.ToByte(0));   //19
                    Array_Load.Add(Convert.ToByte(0));   //20
                    Array_Load.Add(Convert.ToByte(0));   //21
                    Array_Load.Add(Convert.ToByte(0));   //22
                    Array_Load.Add(Convert.ToByte(0));   //23
                    Array_Load.Add(Convert.ToByte(0));   //24
                    Array_Load.Add(Convert.ToByte(0));   //25
                    break;
                case "设CW值":
                    Array_Load.Add(Convert.ToByte(0xAA));//1
                    Array_Load.Add(Convert.ToByte(0x00));//2
                    Array_Load.Add(Convert.ToByte(0x2E));//3
                    Array_Load.Add(Convert.ToByte(DC_set_32 & 0xFF));//4**
                    Array_Load.Add(Convert.ToByte((DC_set_32 >> 8) & 0xFF));   //5
                    Array_Load.Add(Convert.ToByte((DC_set_32 >> 16) & 0xFF));    //6
                    Array_Load.Add(Convert.ToByte((DC_set_32 >> 24) & 0xFF));    //7
                    Array_Load.Add(Convert.ToByte(0));   //8
                    Array_Load.Add(Convert.ToByte(0));   //9
                    Array_Load.Add(Convert.ToByte(0));   //10
                    Array_Load.Add(Convert.ToByte(0));   //11
                    Array_Load.Add(Convert.ToByte(0));   //12
                    Array_Load.Add(Convert.ToByte(0));   //13
                    Array_Load.Add(Convert.ToByte(0));   //14
                    Array_Load.Add(Convert.ToByte(0));   //15
                    Array_Load.Add(Convert.ToByte(0));   //16
                    Array_Load.Add(Convert.ToByte(0));   //17
                    Array_Load.Add(Convert.ToByte(0));   //18
                    Array_Load.Add(Convert.ToByte(0));   //19
                    Array_Load.Add(Convert.ToByte(0));   //20
                    Array_Load.Add(Convert.ToByte(0));   //21
                    Array_Load.Add(Convert.ToByte(0));   //22
                    Array_Load.Add(Convert.ToByte(0));   //23
                    Array_Load.Add(Convert.ToByte(0));   //24
                    Array_Load.Add(Convert.ToByte(0));   //25
                    break;
                case "设CR值":
                    Array_Load.Add(Convert.ToByte(0xAA));//1
                    Array_Load.Add(Convert.ToByte(0x00));//2
                    Array_Load.Add(Convert.ToByte(0x30));//3
                    Array_Load.Add(Convert.ToByte(DC_set_32 & 0xFF));//4**
                    Array_Load.Add(Convert.ToByte((DC_set_32>>8) & 0xFF));   //5
                    Array_Load.Add(Convert.ToByte((DC_set_32>>16)& 0xFF));    //6
                    Array_Load.Add(Convert.ToByte((DC_set_32>>24)& 0xFF));    //7
                    Array_Load.Add(Convert.ToByte(0));   //8
                    Array_Load.Add(Convert.ToByte(0));   //9
                    Array_Load.Add(Convert.ToByte(0));   //10
                    Array_Load.Add(Convert.ToByte(0));   //11
                    Array_Load.Add(Convert.ToByte(0));   //12
                    Array_Load.Add(Convert.ToByte(0));   //13
                    Array_Load.Add(Convert.ToByte(0));   //14
                    Array_Load.Add(Convert.ToByte(0));   //15
                    Array_Load.Add(Convert.ToByte(0));   //16
                    Array_Load.Add(Convert.ToByte(0));   //17
                    Array_Load.Add(Convert.ToByte(0));   //18
                    Array_Load.Add(Convert.ToByte(0));   //19
                    Array_Load.Add(Convert.ToByte(0));   //20
                    Array_Load.Add(Convert.ToByte(0));   //21
                    Array_Load.Add(Convert.ToByte(0));   //22
                    Array_Load.Add(Convert.ToByte(0));   //23
                    Array_Load.Add(Convert.ToByte(0));   //24
                    Array_Load.Add(Convert.ToByte(0));   //25
                    break;

                default:
                    MessageBox.Show("本软件无此功能！", "提示！");
                    break;
                   
            }


            ADD8_Add(Array_Load);

        }
        public void ArrayForDc_elec_pwr()
        {
            Array_Load.Clear();//清空逆变器动态数组
            switch (flag_btnclick_pwr)
            {
                case "程控模式":
                    Array_Load.Add(Convert.ToByte('S'));//1
                    Array_Load.Add(Convert.ToByte('Y'));//2
                    Array_Load.Add(Convert.ToByte('S'));//3
                    Array_Load.Add(Convert.ToByte('T'));//4**
                    Array_Load.Add(Convert.ToByte(':'));   //5
                    Array_Load.Add(Convert.ToByte('R'));   //6
                    Array_Load.Add(Convert.ToByte('E'));   //7
                    Array_Load.Add(Convert.ToByte('M'));   //8
                    break;
                case "手动模式":
                    Array_Load.Add(Convert.ToByte('S'));//1
                    Array_Load.Add(Convert.ToByte('Y'));//2
                    Array_Load.Add(Convert.ToByte('S'));//3
                    Array_Load.Add(Convert.ToByte('T'));//4**
                    Array_Load.Add(Convert.ToByte(':'));   //5
                    Array_Load.Add(Convert.ToByte('L'));   //6
                    Array_Load.Add(Convert.ToByte('O'));   //7
                    Array_Load.Add(Convert.ToByte('C'));   //8
                    break;
                case "开机":
                    Array_Load.Add(Convert.ToByte('O'));//1
                    Array_Load.Add(Convert.ToByte('U'));//2
                    Array_Load.Add(Convert.ToByte('T'));//3
                    Array_Load.Add(Convert.ToByte('P'));//4
                    Array_Load.Add(Convert.ToByte(' '));//5
                    Array_Load.Add(Convert.ToByte('1'));//6

                    break;
                case "关机":
                    Array_Load.Add(Convert.ToByte('O'));//1
                    Array_Load.Add(Convert.ToByte('U'));//2
                    Array_Load.Add(Convert.ToByte('T'));//3
                    Array_Load.Add(Convert.ToByte('P'));//4
                    Array_Load.Add(Convert.ToByte(' '));//5
                    Array_Load.Add(Convert.ToByte('0'));//6
                    break;
                case "电压给定":
                    Array_Load.Add(Convert.ToByte('V'));//1
                    Array_Load.Add(Convert.ToByte('O'));//2
                    Array_Load.Add(Convert.ToByte('L'));//3
                    Array_Load.Add(Convert.ToByte('T'));//4
                    Array_Load.Add(Convert.ToByte(' '));//5
                    Array_Load.Add(Convert.ToByte('0'));//6
                    break;
                case "读取电流":
                    Array_Load.Add(Convert.ToByte('F'));//1
                    Array_Load.Add(Convert.ToByte('E'));//2
                    Array_Load.Add(Convert.ToByte('T'));//3
                    Array_Load.Add(Convert.ToByte('C'));//4
                    Array_Load.Add(Convert.ToByte(':'));//5
                    Array_Load.Add(Convert.ToByte('C'));//6
                    Array_Load.Add(Convert.ToByte('U'));//7
                    Array_Load.Add(Convert.ToByte('R'));//8
                    Array_Load.Add(Convert.ToByte('R'));//9
                    Array_Load.Add(Convert.ToByte('?'));//10
                    break;
                case "读取电压":
                    Array_Load.Add(Convert.ToByte('F'));//1
                    Array_Load.Add(Convert.ToByte('E'));//2
                    Array_Load.Add(Convert.ToByte('T'));//3
                    Array_Load.Add(Convert.ToByte('C'));//4
                    Array_Load.Add(Convert.ToByte(':'));//5
                    Array_Load.Add(Convert.ToByte('V'));//6
                    Array_Load.Add(Convert.ToByte('O'));//7
                    Array_Load.Add(Convert.ToByte('L'));//8
                    Array_Load.Add(Convert.ToByte('T'));//9
                    Array_Load.Add(Convert.ToByte('?'));//10
                    break;
                case "读取功率":
                    Array_Load.Add(Convert.ToByte('F'));//1
                    Array_Load.Add(Convert.ToByte('E'));//2
                    Array_Load.Add(Convert.ToByte('T'));//3
                    Array_Load.Add(Convert.ToByte('C'));//4
                    Array_Load.Add(Convert.ToByte(':'));//5
                    Array_Load.Add(Convert.ToByte('P'));//6
                    Array_Load.Add(Convert.ToByte('O'));//7
                    Array_Load.Add(Convert.ToByte('W'));//8
                    Array_Load.Add(Convert.ToByte('?'));//10
                    break;

                default:
                    MessageBox.Show("本软件无此功能！", "提示！");
                    break;

            }

            Array_Load.Add(Convert.ToByte(0x0D));   //25
            Array_Load.Add(Convert.ToByte(0x0A));   //25
          //  ADD8_Add(Array_Load);

        }
        /// <summary>
        ///给采集器动态数组赋值函数
        /// </summary>
        public void ArrayForCaiJiQi()
        {
            Array_CaiJiQi.Clear();//清空采集器动态数组
            byte[] TouChuan = new byte[Array_BMS1.Count + 42];//定义透传数组

            TouChuan[0] = 0x68;                                                              //给第一个0x68赋值
            TouChuan[1] = (byte)((((TouChuan.Length - 8) << 2) ^ 0x02) & 0x00ff);            //给第一个L低字节赋值
            TouChuan[2] = (byte)(((((TouChuan.Length - 8) << 2) ^ 0x02) & 0xff00) >> 8);     //给第一个L高字节赋值
            TouChuan[3] = (byte)((((TouChuan.Length - 8) << 2) ^ 0x02) & 0x00ff);            //给第二个L低字节赋值
            TouChuan[4] = (byte)(((((TouChuan.Length - 8) << 2) ^ 0x02) & 0xff00) >> 8);     //给第二个L高字节赋值

            TouChuan[5] = 0x68;                                                              //给第二个0x68赋值
            TouChuan[6] = 0x5b;                                                              //给控制域C赋值(不明确待定)

            // TouChuan[10] = Convert.ToByte(((Convert.ToInt32(textBox_CJQQRID.Text)) & 0xff000000) >> 24);//采集器确认ID设置：A的前4个字节
            //TouChuan[9] = Convert.ToByte(((Convert.ToInt32(textBox_CJQQRID.Text)) & 0x00ff0000) >> 16);
            //TouChuan[8] = Convert.ToByte(((Convert.ToInt32(textBox_CJQQRID.Text)) & 0x0000ff00) >> 8);
            //TouChuan[7] = Convert.ToByte((Convert.ToInt32(textBox_CJQQRID.Text)) & 0x000000ff);
            //ouChuan[11] = 0x02;                                                             //A的第五个字节赋值（主站地址）
            //      TouChuan[12] = 0x10;                                                             //AFN赋值
            //    TouChuan[13] = 0x70;                                                             //SEQ赋值


            for (int i = 0; i < Array_BMS1.Count; i++)                                //将逆变器动态数组放进采集器动态数组
            {
                TouChuan[i + 24] = (byte)Array_BMS1[i];
            }

            TouChuan[24 + Array_BMS1.Count] = 0x1f;
            TouChuan[25 + Array_BMS1.Count] = 0x20;
            TouChuan[26 + Array_BMS1.Count] = 0x21;
            TouChuan[27 + Array_BMS1.Count] = 0x22;
            TouChuan[28 + Array_BMS1.Count] = 0x23;
            TouChuan[29 + Array_BMS1.Count] = 0x24;
            TouChuan[30 + Array_BMS1.Count] = 0x00;
            TouChuan[31 + Array_BMS1.Count] = 0x00;
            TouChuan[32 + Array_BMS1.Count] = 0x00;
            TouChuan[33 + Array_BMS1.Count] = 0x00;
            TouChuan[34 + Array_BMS1.Count] = 0x00;
            TouChuan[35 + Array_BMS1.Count] = 0x00;
            TouChuan[36 + Array_BMS1.Count] = 0x00;
            TouChuan[37 + Array_BMS1.Count] = 0x00;
            TouChuan[38 + Array_BMS1.Count] = 0x00;
            TouChuan[39 + Array_BMS1.Count] = 0x00;

            int M = 0;
            for (int j = 6; j < TouChuan.Length - 2; j++)//给累透传加和字节（CS）赋值
            {
                M = M + TouChuan[j];
            }
            TouChuan[TouChuan.Length - 2] = Convert.ToByte(M & 0x00ff);
            TouChuan[TouChuan.Length - 1] = 0x16;

            for (int i = 0; i < TouChuan.Length; i++)
            {
                Array_CaiJiQi.Add(TouChuan[i]);
            }

        }
        /// <summary>
        /// 给主站动态数组赋值函数
        /// </summary>
        public void ArrayForZhuZhan()
        {
            Array_ZhuZhan.Clear();
            /// 消息头赋值             
            Array_ZhuZhan.Add(Convert.ToByte(0x01));//msg_type赋值，固定位1
            Array_ZhuZhan.Add(Convert.ToByte(0x03));
            Array_ZhuZhan.Add(Convert.ToByte(0x00));
            Array_ZhuZhan.Add(Convert.ToByte(0x00));
          //  Array_ZhuZhan.Add(Convert.ToByte((textBox_YCNum.Text)) >> 8);//data_len赋值
           // Array_ZhuZhan.Add(Convert.ToByte((textBox_YCNum.Text)));//data_len赋值
            for (int i = 0; i < 8; i++)//Data_frm
            {
                Array_ZhuZhan.Add(Array_CaiJiQi[i]);
            }
        }
        public void ArrayForYC()
        {
            Array_YC.Clear();
            /// 消息头赋值             
            Array_YC.Add(Convert.ToByte(0x01));//msg_type赋值，固定位1
            Array_YC.Add(Convert.ToByte(0x03));
            Array_YC.Add(Convert.ToByte(0x00));
            Array_YC.Add(Convert.ToByte(0x00));
        //    Array_YC.Add(Convert.ToByte((textBox_YCNum.Text)) >> 8);//data_len赋值
        //    Array_YC.Add(Convert.ToByte((textBox_YCNum.Text)));//data_len赋值

        }
        public void ArrayForSet()
        {
            Array_YC.Clear();
            switch (flag_btnclick)
            {

                case "写配置信息A1":
                    Array_BMS.Add(Convert.ToByte(0x01));
                    Array_BMS.Add(Convert.ToByte(0xB1));
                    Array_BMS.Add(Convert.ToByte(0x00));
                    Array_BMS.Add(Convert.ToByte(0x20));
                    CRC16_put(BMS_Ver_A1_Write);
                    for (int i = 0; i < 64; i++)
                    {
                        Array_BMS.Add(Convert.ToByte(BMS_Ver_A1_Write[i]));
                    }
                    break;
                case "读配置信息A1":
                    Array_BMS.Add(Convert.ToByte(0x01));
                    Array_BMS.Add(Convert.ToByte(0xA1));
                    Array_BMS.Add(Convert.ToByte(0x00));
                    Array_BMS.Add(Convert.ToByte(0x00));
                    Array_BMS.Add(Convert.ToByte(0x00));
                    Array_BMS.Add(Convert.ToByte(0x20));
                    break;
                default: break;
            }

        }
        /// <summary>
        /// 将发送接收的数据显示在richTextBox_data_show
        /// </summary>
        /// <param name="data"></param>
        /// <param name="length"></param>
        public void SendData_show(byte[] data, int length)//将发送数据显示在报文区
        {
            if (textBox_fasong.Text != "")
                textBox_fasong.AppendText("\n");
            DateTime datetime = DateTime.Now;
            string send_Data = Byte_To_String(data, length);
            textBox_fasong.SelectionStart = textBox_fasong.Text.Length;
            textBox_fasong.ForeColor = System.Drawing.Color.Red;
            textBox_fasong.AppendText("[" + datetime.ToLongTimeString() + "]发送:" + send_Data);
            textBox_fasong.ScrollToCaret();
        }

        public void RecData_show(byte[] data, int length)//将接收到的数据显示在报文区
        {
            if (textBox_fasong.Text != "")
                textBox_fasong.AppendText("\n");
            DateTime datetime = DateTime.Now;
            string send_Data = Byte_To_String(data, length);
            textBox_fasong.SelectionStart = textBox_fasong.Text.Length;
            textBox_fasong.ForeColor = System.Drawing.Color.Blue;
            textBox_fasong.AppendText("[" + datetime.ToLongTimeString() + "]接收:" + "◆" + send_Data);
            textBox_fasong.ScrollToCaret();
        }

        public string Byte_To_String(byte[] temp_data, int length)
        {
            string temp_string = "";
            for (int i = (length - 1); i >= 0; i--)
            {
                switch (temp_data[i] & 0x0f)
                {
                    case 0x00:
                        temp_string = temp_string.Insert(0, "0");
                        break;
                    case 0x01:
                        temp_string = temp_string.Insert(0, "1");
                        break;
                    case 0x02:
                        temp_string = temp_string.Insert(0, "2");
                        break;
                    case 0x03:
                        temp_string = temp_string.Insert(0, "3");
                        break;
                    case 0x04:
                        temp_string = temp_string.Insert(0, "4");
                        break;
                    case 0x05:
                        temp_string = temp_string.Insert(0, "5");
                        break;
                    case 0x06:
                        temp_string = temp_string.Insert(0, "6");
                        break;
                    case 0x07:
                        temp_string = temp_string.Insert(0, "7");
                        break;
                    case 0x08:
                        temp_string = temp_string.Insert(0, "8");
                        break;
                    case 0x09:
                        temp_string = temp_string.Insert(0, "9");
                        break;
                    case (byte)0x0a:
                        temp_string = temp_string.Insert(0, "A");
                        break;
                    case (byte)0x0b:
                        temp_string = temp_string.Insert(0, "B");
                        break;
                    case (byte)0x0c:
                        temp_string = temp_string.Insert(0, "C");
                        break;
                    case (byte)0x0d:
                        temp_string = temp_string.Insert(0, "D");
                        break;
                    case (byte)0x0e:
                        temp_string = temp_string.Insert(0, "E");
                        break;
                    case (byte)0x0f:
                        temp_string = temp_string.Insert(0, "F");
                        break;
                    default: break;
                }
                switch (temp_data[i] >> 4 & 0xf)
                {
                    case 0x00:
                        temp_string = temp_string.Insert(0, " 0");
                        break;
                    case 0x01:
                        temp_string = temp_string.Insert(0, " 1");
                        break;
                    case 0x02:
                        temp_string = temp_string.Insert(0, " 2");
                        break;
                    case 0x03:
                        temp_string = temp_string.Insert(0, " 3");
                        break;
                    case 0x04:
                        temp_string = temp_string.Insert(0, " 4");
                        break;
                    case 0x05:
                        temp_string = temp_string.Insert(0, " 5");
                        break;
                    case 0x06:
                        temp_string = temp_string.Insert(0, " 6");
                        break;
                    case 0x07:
                        temp_string = temp_string.Insert(0, " 7");
                        break;
                    case 0x08:
                        temp_string = temp_string.Insert(0, " 8");
                        break;
                    case 0x09:
                        temp_string = temp_string.Insert(0, " 9");
                        break;
                    case (byte)0x0a:
                        temp_string = temp_string.Insert(0, " A");
                        break;
                    case (byte)0x0b:
                        temp_string = temp_string.Insert(0, " B");
                        break;
                    case (byte)0x0c:
                        temp_string = temp_string.Insert(0, " C");
                        break;
                    case (byte)0x0d:
                        temp_string = temp_string.Insert(0, " D");
                        break;
                    case (byte)0x0e:
                        temp_string = temp_string.Insert(0, " E");
                        break;
                    case (byte)0x0f:
                        temp_string = temp_string.Insert(0, " F");
                        break;
                    default: break;
                }
            }
            return temp_string;
        }

        public void String_show(string STR)//将字符串显示在报文区黑色字体
        {
            /*  if (richTextBox_data_show.Text != "")
                  richTextBox_data_show.AppendText("\n");
              DateTime datetime = DateTime.Now;
              //string send_Data = Byte_To_String(data, length);
              richTextBox_data_show.SelectionStart = richTextBox_data_show.Text.Length;
              richTextBox_data_show.SelectionColor = System.Drawing.Color.Black;
              richTextBox_data_show.AppendText("[" + datetime.ToLongTimeString() + "]" + STR);
              richTextBox_data_show.ScrollToCaret();*/
        }
        /// <summary>
        /// 发送数据并显示
        /// </summary>
        class Fruit
        {
            public string 名字1 { get; set; }
            public string 状态1 { get; set; }
            public string 名字2 { get; set; }
            public string 状态2 { get; set; }
            public string 名字3 { get; set; }
            public string 状态3 { get; set; }
            public string 名字4 { get; set; }
            public string 状态4 { get; set; }
            public string 名字5 { get; set; }
            public string 状态5 { get; set; }
            public string 名字6 { get; set; }
            public string 状态6 { get; set; }
            public string 名字7 { get; set; }
            public string 状态7 { get; set; }
            public string 名字8 { get; set; }
            public string 状态8 { get; set; }
        }

        public void SendData_ShowData()//发送函数
        {
            byte[] SendData = new byte[Array_BMS1.Count];
            for (int i = 0; i < SendData.Length; i++)
            {
                SendData[i] = (byte)Array_BMS1[i];
            }
            try //发送clientsocket数组给服务器
            {
                serialPort_7112.Write(SendData, 0, SendData.Length);
                SendData_show(SendData, SendData.Length);
                flag_error = 0;
            }
            catch (Exception exp)
            {
                flag_error = 102;
                MessageBox.Show(exp.Message);
            }
            //           finally
            //           { SendData_show(SendData, SendData.Length); }
        }

        public void SendData_ShowData_Elec_load()//发送函数
        {
            byte[] SendData = new byte[Array_Load.Count];
            for (int i = 0; i < SendData.Length; i++)
            {
                SendData[i] = (byte)Array_Load[i];
            }
            try //发送clientsocket数组给服务器
            {
                负载串口.Write(SendData, 0, SendData.Length);
                SendData_show(SendData, SendData.Length);
                flag_error = 0;
            }
            catch (Exception exp)
            {
                flag_error = 102;
                MessageBox.Show(exp.Message);
            }
            //           finally
            //           { SendData_show(SendData, SendData.Length); }
        }

        public int RecData_ShowData()//接收判断函数
        {
            //  int delay = Convert.ToInt32(textBox_OverTime.Text) + 5;//获取超时时间            
            clientSocket.ReceiveTimeout = 100;                     //超时时间为100ms          
            int successRECBtyes = 0;                               //初始化接收字节
            int reciviedRight = 1;                                 //初始化接收到数据标志位
            for (int a = 0; a < 10; a++)
            {
                Array.Clear(Data_Rec, 0, 1024);                    //清空接收数组
                if (clientSocket.Poll(100, SelectMode.SelectRead))
                {
                    try
                    {
                        successRECBtyes = clientSocket.Receive(Data_Rec);
                        if (successRECBtyes != 0)
                        {
                            byte[] RecByte = new byte[successRECBtyes];
                            for (int j = 0; j < RecByte.Length; j++)
                            {
                                RecByte[j] = Data_Rec[j];//两个数组一个用于显示\一个用于计算
                            }
                            RecData_show(RecByte, RecByte.Length);              //显示接收到的数据,
                            switch (flag_btnclick)
                            {
                                case "费控":
                                    if (Data_Rec[0] == 0x02)
                                        reciviedRight = 4;
                                    else if ((Data_Rec[0] == 0x04) && (Data_Rec[4] == (byte)Array_ZhuZhan[4]))
                                        reciviedRight = 0;
                                    else
                                        reciviedRight = 2;
                                    break;
                                case "遥信":
                                    if (Data_Rec[0] == 0x02)
                                        reciviedRight = 4;
                                    else if ((Data_Rec[0] == 0x04) && (Data_Rec[4] == (byte)Array_ZhuZhan[4]) && (Data_Rec[39] == (byte)Array_ZhuZhan[83]) && (Data_Rec[40] == (byte)Array_ZhuZhan[84]) && (Data_Rec[41] == 16))
                                        reciviedRight = 0;
                                    else
                                        reciviedRight = 2;
                                    break;
                                case "遥测":
                                    if (Data_Rec[0] == 0x02)
                                        reciviedRight = 4;
                                    else if ((Data_Rec[0] == 0x04) && (Data_Rec[4] == (byte)Array_ZhuZhan[4]) && (Data_Rec[39] == (byte)Array_ZhuZhan[83]) && (Data_Rec[40] == (byte)Array_ZhuZhan[84]) && (Data_Rec[41] == 86))
                                        reciviedRight = 0;
                                    else
                                        reciviedRight = 2;
                                    break;
                                case "召SOE"://需要改动
                                    if (Data_Rec[0] == 0x02)
                                        reciviedRight = 4;
                                    else if ((Data_Rec[0] == 0x04) && (Data_Rec[4] == (byte)Array_ZhuZhan[4]) && (Data_Rec[39] == (byte)Array_ZhuZhan[83]) && (Data_Rec[40] == (byte)Array_ZhuZhan[84]))
                                        reciviedRight = 0;
                                    else
                                        reciviedRight = 2;
                                    break;
                                case "读取设置参数":
                                    if (Data_Rec[0] == 0x02)
                                        reciviedRight = 4;
                                    else if ((Data_Rec[0] == 0x04) && (Data_Rec[4] == (byte)Array_ZhuZhan[4]) && (Data_Rec[39] == (byte)Array_ZhuZhan[83]) && (Data_Rec[40] == (byte)Array_ZhuZhan[84]) && (Data_Rec[41] == 160))
                                        reciviedRight = 0;
                                    else
                                        reciviedRight = 2;
                                    break;
                                case "遥控":
                                    if (Data_Rec[0] == 0x02)
                                        reciviedRight = 4;
                                    else if ((Data_Rec[0] == 0x04) && (Data_Rec[4] == (byte)Array_ZhuZhan[4]) && (Data_Rec[39] == (byte)Array_ZhuZhan[83]) && (Data_Rec[40] == (byte)Array_ZhuZhan[84]))
                                    {
                                        if ((Data_Rec[41] == (byte)Array_BMS1[2]) && (Data_Rec[42] == (byte)Array_BMS1[3]) && (Data_Rec[43] == (byte)Array_BMS1[4]) && (Data_Rec[44] == (byte)Array_BMS1[5]))
                                            reciviedRight = 0;
                                        else if ((Data_Rec[41] == (byte)Array_BMS1[2]) && (Data_Rec[42] == (byte)Array_BMS1[3]) && (Data_Rec[43] == 0xff) && (Data_Rec[44] == 0xff))
                                            reciviedRight = 3;
                                        else
                                            reciviedRight = 2;
                                    }
                                    else
                                        reciviedRight = 2;
                                    break;
                                case "遥调":
                                    if (Data_Rec[0] == 0x02)
                                        reciviedRight = 4;
                                    else if ((Data_Rec[0] == 0x04) && (Data_Rec[4] == (byte)Array_ZhuZhan[4]) && (Data_Rec[39] == (byte)Array_ZhuZhan[83]) && (Data_Rec[40] == (byte)Array_ZhuZhan[84]))
                                    {
                                        if ((Data_Rec[41] == (byte)Array_BMS1[2]) && (Data_Rec[42] == (byte)Array_BMS1[3]) && (Data_Rec[43] == (byte)Array_BMS1[4]) && (Data_Rec[44] == (byte)Array_BMS1[5]))
                                            reciviedRight = 0;
                                        else if ((Data_Rec[41] == (byte)Array_BMS1[2]) && (Data_Rec[42] == (byte)Array_BMS1[3]) && (Data_Rec[43] == 0xff) && (Data_Rec[44] == 0xff))
                                            reciviedRight = 3;
                                        else
                                            reciviedRight = 2;
                                    }
                                    else
                                        reciviedRight = 2;
                                    break;
                                case "读版本号":
                                    if (Data_Rec[0] == 0x02)
                                        reciviedRight = 4;
                                    else if ((Data_Rec[0] == 0x04) && (Data_Rec[4] == (byte)Array_ZhuZhan[4]) && (Data_Rec[39] == (byte)Array_ZhuZhan[83]) && (Data_Rec[40] == (byte)Array_ZhuZhan[84]) && (Data_Rec[43] == (byte)Array_ZhuZhan[87]) && (Data_Rec[44] == (byte)Array_ZhuZhan[88]))
                                        reciviedRight = 0;
                                    else
                                        reciviedRight = 2;
                                    break;
                                case "设置总电量":
                                    if (Data_Rec[0] == 0x02)
                                        reciviedRight = 4;
                                    else if ((Data_Rec[0] == 0x04) && (Data_Rec[4] == (byte)Array_ZhuZhan[4]) && (Data_Rec[39] == (byte)Array_ZhuZhan[83]) && (Data_Rec[40] == (byte)Array_ZhuZhan[84]))
                                    {
                                        if ((Data_Rec[42] == (byte)Array_BMS1[3]) && (Data_Rec[43] == (byte)Array_BMS1[4]) && (Data_Rec[44] == (byte)Array_BMS1[5]) && (Data_Rec[45] == (byte)Array_BMS1[6]))
                                            reciviedRight = 0;
                                        else if ((Data_Rec[42] == 0xff) && (Data_Rec[43] == 0xff) && (Data_Rec[44] == 0xff) && (Data_Rec[45] == 0xff))
                                            reciviedRight = 3;
                                        else
                                            reciviedRight = 2;
                                    }
                                    else
                                        reciviedRight = 2;
                                    break;
                                case "读设备时钟":
                                    if (Data_Rec[0] == 0x02)
                                        reciviedRight = 4;
                                    else if ((Data_Rec[0] == 0x04) && (Data_Rec[4] == (byte)Array_ZhuZhan[4]) && (Data_Rec[39] == (byte)Array_ZhuZhan[83]) && (Data_Rec[40] == (byte)Array_ZhuZhan[84]))
                                        reciviedRight = 0;
                                    else
                                        reciviedRight = 2;
                                    break;
                                case "读总电量":
                                    if (Data_Rec[0] == 0x02)
                                        reciviedRight = 4;
                                    else if ((Data_Rec[0] == 0x04) && (Data_Rec[4] == (byte)Array_ZhuZhan[4]) && (Data_Rec[39] == (byte)Array_ZhuZhan[83]) && (Data_Rec[40] == (byte)Array_ZhuZhan[84]) && (Data_Rec[41] == 4))
                                        reciviedRight = 0;
                                    else
                                        reciviedRight = 2;
                                    break;
                                case "召日发电量":
                                    if (Data_Rec[0] == 0x02)
                                        reciviedRight = 4;
                                    else if ((Data_Rec[0] == 0x04) && (Data_Rec[4] == (byte)Array_ZhuZhan[4]) && (Data_Rec[39] == (byte)Array_ZhuZhan[83]) && (Data_Rec[40] == (byte)Array_ZhuZhan[84]) && (Data_Rec[41] == 0x05))
                                    {
                                        if ((Data_Rec[42] == (byte)Array_ZhuZhan[85]) && (Data_Rec[43] == (byte)Array_ZhuZhan[86]) && (Data_Rec[44] == (byte)Array_ZhuZhan[87]))
                                            reciviedRight = 0;
                                        else if ((Data_Rec[42] == 0xff) && (Data_Rec[43] == 0xff) && (Data_Rec[44] == 0xff))
                                            reciviedRight = 3;
                                        else
                                            reciviedRight = 2;
                                    }
                                    else
                                        reciviedRight = 2;
                                    break;
                                case "清除数据信息":
                                    if (Data_Rec[0] == 0x02)
                                        reciviedRight = 4;
                                    else if ((Data_Rec[0] == 0x04) && (Data_Rec[4] == (byte)Array_ZhuZhan[4]) && (Data_Rec[39] == (byte)Array_ZhuZhan[83]) && (Data_Rec[40] == (byte)Array_ZhuZhan[84]))
                                    {
                                        if ((Data_Rec[41] == 0x00) && (Data_Rec[42] == 0x00) && (Data_Rec[43] == 0x54) && (Data_Rec[44] == 0x45))
                                            reciviedRight = 0;
                                        else if ((Data_Rec[41] == 0xff) && (Data_Rec[42] == 0xff) && (Data_Rec[43] == 0xff) && (Data_Rec[44] == 0xff))
                                            reciviedRight = 3;
                                        else
                                            reciviedRight = 2;
                                    }
                                    else
                                        reciviedRight = 2;
                                    break;
                                case "清除配置信息":
                                    if (Data_Rec[0] == 0x02)
                                        reciviedRight = 4;
                                    else if ((Data_Rec[0] == 0x04) && (Data_Rec[4] == (byte)Array_ZhuZhan[4]) && (Data_Rec[39] == (byte)Array_ZhuZhan[83]) && (Data_Rec[40] == (byte)Array_ZhuZhan[84]))
                                    {
                                        if ((Data_Rec[41] == 0x00) && (Data_Rec[42] == 0x00) && (Data_Rec[43] == 0x51) && (Data_Rec[44] == 0x15))
                                            reciviedRight = 0;
                                        else if ((Data_Rec[41] == 0xff) && (Data_Rec[42] == 0xff) && (Data_Rec[43] == 0xff) && (Data_Rec[44] == 0xff))
                                            reciviedRight = 3;
                                        else
                                            reciviedRight = 2;
                                    }
                                    else
                                        reciviedRight = 2;
                                    break;
                            }
                        }
                        else
                        {
                            reciviedRight = 1;
                        }
                    }
                    catch (Exception exp)
                    {
                        flag_error = 103;
                        MessageBox.Show(exp.Message + "（主站拒绝访问！）");
                        return reciviedRight;
                    }
                }
                if ((reciviedRight == 0) || (reciviedRight == 3) || (reciviedRight == 4))
                    break;
                Delay_Ms(1000);
            }
            return reciviedRight;
        }
        public int RecData_ShowData_New()//接收判断函数
        {
            int successRECBtyes = 0;                               //初始化接收字节
            int reciviedRight = 1;                                 //初始化接收到数据标志位
            int recivice_Len = 1;
            Array.Clear(Data_Rec, 0, 1024);                       //清空接收数组
            byte[] CRC_Result = new byte[2];
            RecData_show(Com_Rc_Data, Com_Rc_Data_Lenth);
            recivice_Len = Com_Rc_Data[2];
            switch (flag_btnclick)
            {
                case "遥信":
                    if (Com_Rc_Data[1] == 0x02 && Com_Rc_Data_Lenth == 25)
                    {
                        CRC_Result = CRC_Data_Check(Com_Rc_Data, recivice_Len + 3);
                        if (CRC_Result[0] == 0 && CRC_Result[1] == 0)
                        {
                            reciviedRight = 4;
                            for (int i = 0; i < recivice_Len; i++)
                            {
                                Data_Rec[i] = Com_Rc_Data[i + 3];
                            }
                            Message_show();
                        }
                        else
                        {
                            reciviedRight = 3;
                        }
                    }
                    else
                    {
                        reciviedRight = 2;
                    }
                    break;
                case "遥测":
                    if (Com_Rc_Data[1] == 0x03 && Com_Rc_Data_Lenth == 14)
                    {

                        CRC_Result = CRC_Data_Check(Com_Rc_Data, 12);
                        if (CRC_Result[0] == 0 && CRC_Result[1] == 0)
                        {
                            UInt16 tmp;
                            tmp = Com_Rc_Data[4];
                            tmp = (UInt16)(tmp << 8);
                            tmp += Com_Rc_Data[5];
                           //交流电压.Text = tmp.ToString();
                            tmp = Com_Rc_Data[6];
                            tmp = (UInt16)(tmp << 8);
                            tmp += Com_Rc_Data[7];
                            U_Power = tmp;
                            输出电压.Text = (tmp/100).ToString()+"."+(tmp %100).ToString();
             
                            tmp = Com_Rc_Data[8];
                            tmp = (UInt16)(tmp << 8);
                            tmp += Com_Rc_Data[9];
                            输出电流.Text = (tmp / 1000).ToString() + "." + (tmp % 1000).ToString(); 
                            //textBox14.Text = Com_Rc_Data[6].ToString() + Com_Rc_Data[7].ToString(); 
                            //textBox15.Text = Com_Rc_Data[8].ToString() + Com_Rc_Data[9].ToString(); 
                            reciviedRight = 4;
                        }
                        else
                        {
                            reciviedRight = 3;
                        }
                    }
                    else
                    {
                        reciviedRight = 2;
                        Message_show();
                    }

                    break;
                case "控制遥测":
                    if (Com_Rc_Data[1] == 0x03 && Com_Rc_Data_Lenth == 32)
                    {

                        CRC_Result = CRC_Data_Check(Com_Rc_Data, 30);
                        if (CRC_Result[0] == 0 && CRC_Result[1] == 0)
                        {
         //                   UInt16 tmp;
         //                   int tmp1;
         //                   ///////////////////////////////////////////////直流电压///////////////////////////////////
         //                   tmp = Com_Rc_Data[6];
         //                   tmp = (UInt16)(tmp << 8);
         //                   tmp += Com_Rc_Data[7];
         //                   textBox16.Text = (tmp / 100).ToString() + "." + (tmp % 100).ToString();

         //                   tmp = Com_Rc_Data[14]; 
         //                   tmp = (UInt16)(tmp << 8);
         //                   tmp += Com_Rc_Data[15];
         //                   textBox17.Text = (tmp / 100).ToString() + "." + (tmp % 100).ToString();

         //                   tmp = Com_Rc_Data[8];
         //                   tmp = (UInt16)(tmp << 8);
         //                   tmp += Com_Rc_Data[9];
         //                   textBox18.Text = (tmp / 100).ToString() + "." + (tmp % 100).ToString();

         //                   tmp = Com_Rc_Data[16];
         //                   tmp = (UInt16)(tmp << 8);
         //                   tmp += Com_Rc_Data[17];
         //                   textBox19.Text = (tmp / 100).ToString() + "." + (tmp % 100).ToString();

         //                   tmp = Com_Rc_Data[10];
         //                   tmp = (UInt16)(tmp << 8);
         //                   tmp += Com_Rc_Data[11];
         //                   textBox20.Text = (tmp / 100).ToString() + "." + (tmp % 100).ToString();

         //                   tmp = Com_Rc_Data[18];
         //                   tmp = (UInt16)(tmp << 8);
         //                   tmp += Com_Rc_Data[19];
         //                   textBox21.Text = (tmp / 100).ToString() + "." + (tmp % 100).ToString();

         //                   tmp = Com_Rc_Data[12];
         //                   tmp = (UInt16)(tmp << 8);
         //                   tmp += Com_Rc_Data[13];
         //                   textBox22.Text = (tmp / 100).ToString() + "." + (tmp % 100).ToString();

         //                   tmp = Com_Rc_Data[20];
         //                   tmp = (UInt16)(tmp << 8);
         //                   tmp += Com_Rc_Data[21];
         //                   textBox23.Text = (tmp / 100).ToString() + "." + (tmp % 100).ToString();

         //     ///////////////////////////////////////////////温度///////////////////////////////////
         //                   tmp1 = Com_Rc_Data[22];
         //                   tmp1 = (tmp1 << 8);
         //                   tmp1 += Com_Rc_Data[23];
         //                   textBox24.Text = (tmp1).ToString();

         //                   tmp1 = Com_Rc_Data[24];
         //                   tmp1 = (tmp1 << 8);
         //                   tmp1 += Com_Rc_Data[25];
         //                   textBox25.Text = (tmp1).ToString();

         //                   tmp = Com_Rc_Data[24];
         //                   tmp = (UInt16)(tmp << 8);
         //                   tmp += Com_Rc_Data[25];
         //                   textBox26.Text = (tmp).ToString();

         /////////////////////////////////////////////////交流电压///////////////////////////////////
         //                   tmp = Com_Rc_Data[4];
         //                   tmp = (UInt16)(tmp << 8);
         //                   tmp += Com_Rc_Data[5];
         //                   textBox28.Text = (tmp).ToString();
                         

         //                   tmp = Com_Rc_Data[28];
         //                   tmp = (UInt16)(tmp << 8);
         //                   tmp += Com_Rc_Data[29];
         //                   textBox29.Text = (tmp).ToString();
         //                   YX_CTL = tmp;
                            reciviedRight = 4;
                        }
                        else
                        {
                            reciviedRight = 3;
                        }
                    }
                    else
                    {
                        reciviedRight = 2;
                        Message_show();
                    }

                    break;
                case "读取设置参数":
                    if (Data_Rec[0] == 0x02)
                        reciviedRight = 4;
                    else if ((Data_Rec[0] == 0x04) && (Data_Rec[4] == (byte)Array_ZhuZhan[4]) && (Data_Rec[39] == (byte)Array_ZhuZhan[83]) && (Data_Rec[40] == (byte)Array_ZhuZhan[84]) && (Data_Rec[41] == 160))
                        reciviedRight = 0;
                    else
                        reciviedRight = 2;
                    break;

                case "遥调":
                    if (Data_Rec[0] == 0x02)
                        reciviedRight = 4;
                    else if ((Data_Rec[0] == 0x04) && (Data_Rec[4] == (byte)Array_ZhuZhan[4]) && (Data_Rec[39] == (byte)Array_ZhuZhan[83]) && (Data_Rec[40] == (byte)Array_ZhuZhan[84]))
                    {
                        if ((Data_Rec[41] == (byte)Array_BMS1[2]) && (Data_Rec[42] == (byte)Array_BMS1[3]) && (Data_Rec[43] == (byte)Array_BMS1[4]) && (Data_Rec[44] == (byte)Array_BMS1[5]))
                            reciviedRight = 0;
                        else if ((Data_Rec[41] == (byte)Array_BMS1[2]) && (Data_Rec[42] == (byte)Array_BMS1[3]) && (Data_Rec[43] == 0xff) && (Data_Rec[44] == 0xff))
                            reciviedRight = 3;
                        else
                            reciviedRight = 2;
                    }
                    else
                        reciviedRight = 2;
                    break;
                case "读配置信息A1":
                    if (Com_Rc_Data[1] == 0xA1 && Com_Rc_Data_Lenth == 70)
                    {
                        CRC_Result = CRC_Data_Check(Com_Rc_Data, 68);
                        if (CRC_Result[0] == 0 && CRC_Result[1] == 0)
                        {
                            for (int i = 0; i < 64; i++)
                            {
                                BMS_Ver_A1_Read[i] = Com_Rc_Data[i + 4];
                            }
                            CRC_Result = CRC_Data_Check(BMS_Ver_A1_Read, 62);
                            if (CRC_Result[0] == 0 && CRC_Result[1] == 0)
                            {

                            }
                            else
                            {
                                for (int i = 0; i < 64; i++)
                                {
                                    BMS_Ver_A1_Read[i] = 0;
                                }
                            }
                            Message_show();
                            reciviedRight = 4;
                        }
                        else
                        {
                            reciviedRight = 3;
                        }
                    }
                    else
                    {
                        reciviedRight = 2;
                    }
                    break;
                case "读版本号":
                    if (Com_Rc_Data[1] == 0x09 && Com_Rc_Data_Lenth == 37)
                    {

                        CRC_Result = CRC_Data_Check(Com_Rc_Data, recivice_Len + 3);
                        if (CRC_Result[0] == 0 && CRC_Result[1] == 0)
                        {
                            for (int i = 0; i < recivice_Len; i++)
                            {
                                Data_Rec[i] = Com_Rc_Data[i + 3];
                                Data_Rec_buffer[2 * i] = Com_Rc_Data[i + 3];
                            }
                            for (int i = 0; i < recivice_Len; i++)
                            {
                                Ver[i] = BitConverter.ToChar(Data_Rec_buffer, 2 * i);
                            }


                            reciviedRight = 4;
                        }
                        else
                        {
                            reciviedRight = 3;
                        }
                    }
                    else
                    {
                        reciviedRight = 2;
                    }
                    break;

                case "读取故障记录长度":
                    if (Com_Rc_Data[1] == 0xA3 && Com_Rc_Data_Lenth == 8)
                    {

                        CRC_Result = CRC_Data_Check(Com_Rc_Data, 6);
                        if (CRC_Result[0] == 0 && CRC_Result[1] == 0)
                        {
                            Recorder_len = Com_Rc_Data[4];
                            Recorder_len *= 256;
                            Recorder_len += Com_Rc_Data[5];
                            reciviedRight = 4;
                        }
                        else
                        {
                            reciviedRight = 3;
                        }
                    }
                    else
                    {
                        reciviedRight = 2;
                    }
                    break;
                case "读放电记录长度":
                    if (Com_Rc_Data[1] == 0xA3 && Com_Rc_Data_Lenth == 8)
                    {

                        CRC_Result = CRC_Data_Check(Com_Rc_Data, 6);
                        if (CRC_Result[0] == 0 && CRC_Result[1] == 0)
                        {
                            Recorder_len = Com_Rc_Data[4];
                            Recorder_len *= 256;
                            Recorder_len += Com_Rc_Data[5];
                            reciviedRight = 4;
                        }
                        else
                        {
                            reciviedRight = 3;
                        }
                    }
                    else
                    {
                        reciviedRight = 2;
                    }
                    break;
                case "读充电记录长度":
                    if (Com_Rc_Data[1] == 0xA3 && Com_Rc_Data_Lenth == 8)
                    {

                        CRC_Result = CRC_Data_Check(Com_Rc_Data, 6);
                        if (CRC_Result[0] == 0 && CRC_Result[1] == 0)
                        {
                            Recorder_len = Com_Rc_Data[4];
                            Recorder_len *= 256;
                            Recorder_len += Com_Rc_Data[5];
                            reciviedRight = 4;
                        }
                        else
                        {
                            reciviedRight = 3;
                        }
                    }
                    else
                    {
                        reciviedRight = 2;
                    }
                    break;
                case "读取故障记录数据":
                    if (Com_Rc_Data[1] == 0xA4 && Com_Rc_Data_Lenth == 72)
                    {
                        CRC_Result = CRC_Data_Check(Com_Rc_Data, 70);
                        if (CRC_Result[0] == 0 && CRC_Result[1] == 0)
                        {
                            My_Sqlite_test.Sqlite_Pro.Create_Fault_Table();
                            for (int j = 1; j < 65; j++)
                            {
                                Recorder[j] = Com_Rc_Data[j + 5];
                            }
                            Byte_to_Bmsfault(Recorder);
                            My_Sqlite_test.Sqlite_Pro.InsertData_to_Fault_Table(bms_fault);
                            reciviedRight = 4;
                        }
                        else
                        {
                            reciviedRight = 3;
                        }
                    }
                    else
                    {
                        reciviedRight = 2;
                    }
                    break;
                case "读充电记录数据":
                    if (Com_Rc_Data[1] == 0xA4 && Com_Rc_Data_Lenth == 72)
                    {
                        CRC_Result = CRC_Data_Check(Com_Rc_Data, 70);
                        if (CRC_Result[0] == 0 && CRC_Result[1] == 0)
                        {
                            My_Sqlite_test.Sqlite_Pro.Create_Charge_Table();
                            for (int j = 1; j < 65; j++)
                            {
                                Recorder[j] = Com_Rc_Data[j + 5];
                            }
                            Byte_to_BmsCHdata(Recorder);
                            My_Sqlite_test.Sqlite_Pro.InsertData_to_Charge_Table(bms_charge);
                            reciviedRight = 4;
                        }
                        else
                        {
                            reciviedRight = 3;
                        }
                    }
                    else
                    {
                        reciviedRight = 2;
                    }
                    break;
                case "读放电记录数据":
                    if (Com_Rc_Data[1] == 0xA4 && Com_Rc_Data_Lenth == 72)
                    {
                        CRC_Result = CRC_Data_Check(Com_Rc_Data, 70);
                        if (CRC_Result[0] == 0 && CRC_Result[1] == 0)
                        {
                            My_Sqlite_test.Sqlite_Pro.Create_Discharge_Table();
                            for (int j = 1; j < 65; j++)
                            {
                                Recorder[j] = Com_Rc_Data[j + 5];
                            }
                            Byte_to_BmsDisCHdata(Recorder);
                            My_Sqlite_test.Sqlite_Pro.InsertData_to_Discharge_Table(bms_discharge);
                            reciviedRight = 4;
                        }
                        else
                        {
                            reciviedRight = 3;
                        }
                    }
                    else
                    {
                        reciviedRight = 2;
                    }
                    break;
                case "程控模式":
                    if (Com_Rc_Data[0] == 0xAA && Com_Rc_Data[2] == 0x12 && Com_Rc_Data[3] == 0x80)
                    { 
                        reciviedRight = 4;
                    }
                    else 
                    {
                        reciviedRight = 2;
                    }
                    break;
                case "手动模式":
                    if (Com_Rc_Data[0] == 0xAA && Com_Rc_Data[2] == 0x12 && Com_Rc_Data[3] == 0x80)
                    {
                        reciviedRight = 4;
                    }
                    else
                    {
                        reciviedRight = 2;
                    }
                    break;
                case "开机":
                    if (Com_Rc_Data[0] == 0xAA && Com_Rc_Data[2] == 0x12 && Com_Rc_Data[3] == 0x80)
                    {
                        reciviedRight = 4;
                    }
                    else
                    {
                        reciviedRight = 2;
                    }
                    break;
                case "关机":
                    if (Com_Rc_Data[0] == 0xAA && Com_Rc_Data[2] == 0x12 && Com_Rc_Data[3] == 0x80)
                    {
                        reciviedRight = 4;
                    }
                    else
                    {
                        reciviedRight = 2;
                    }
                    break;
                case "读取电流":
                    if (Com_Rc_Data_Lenth==6)
                    {
                        reciviedRight = 4;
                        for (int i = 0; i < 6; i++)
                        {
                            Dc_Pwr_Cur[i] = (char)Com_Rc_Data[i];
                        }
                    }
                    else
                    {
                        reciviedRight = 2;
                    }
                    break;
                case "读取电压":
                    if (Com_Rc_Data_Lenth == 6)
                    {
                        reciviedRight = 4;
                        for (int i = 0; i < 6; i++)
                        {
                            Dc_Pwr_Vol[i] = (char)Com_Rc_Data[i];
                        }
                    }
                    else
                    {
                        reciviedRight = 2;
                    }
                    break;
                case "读取功率":
                    if (Com_Rc_Data_Lenth == 6)
                    {
                        reciviedRight = 4;
                        for (int i = 0; i < 6; i++)
                        {
                            Dc_Pwr_P[i] = (char)Com_Rc_Data[i];
                        }
                    }
                    else
                    {
                        reciviedRight = 2;
                    }
                    break;
                case "读CC值":
                    if (Com_Rc_Data[0] == 0xAA && Com_Rc_Data[2] == 0x2B)
                    {
                        UInt32 temp = 0;
                        temp = Com_Rc_Data[6];
                        temp = temp << 8;
                        temp += Com_Rc_Data[5];
                        temp = temp << 8;
                        temp += Com_Rc_Data[4];
                        temp = temp << 8;
                        temp += Com_Rc_Data[3];

                        //CC读.Text = (temp / 1000).ToString() + "." + (temp % 1000).ToString();
                     
                    }
                    else
                    {
                        reciviedRight = 2;
                    }
                    break;

                case "读CV值":
                    if (Com_Rc_Data[0] == 0xAA && Com_Rc_Data[2] == 0x2D)
                    {
                        UInt32 temp = 0;
                        temp = Com_Rc_Data[6];
                        temp = temp << 8;
                        temp += Com_Rc_Data[5];
                        temp = temp << 8;
                        temp += Com_Rc_Data[4];
                        temp = temp << 8;
                        temp += Com_Rc_Data[3];

                        //CV读.Text = (temp / 1000).ToString() + "." + (temp % 1000).ToString();

                    }
                    else
                    {
                        reciviedRight = 2;
                    }
                    break;
                 case "读CR值":
                    if (Com_Rc_Data[0] == 0xAA && Com_Rc_Data[2] == 0x31)
                    {
                        UInt32 temp = 0;
                        temp = Com_Rc_Data[6];
                        temp = temp << 8;
                        temp += Com_Rc_Data[5];
                        temp = temp << 8;
                        temp += Com_Rc_Data[4];
                        temp = temp << 8;
                        temp += Com_Rc_Data[3];

                        //CR读.Text = (temp / 1000).ToString() + "." + (temp % 1000).ToString();

                    }
                    else
                    {
                        reciviedRight = 2;
                    }
                    break;
                case "读CW值":
                    if (Com_Rc_Data[0] == 0xAA && Com_Rc_Data[2] == 0x2F)
                    {
                        UInt32 temp = 0;
                        temp = Com_Rc_Data[6];
                        temp = temp << 8;
                        temp += Com_Rc_Data[5];
                        temp = temp << 8;
                        temp += Com_Rc_Data[4];
                        temp = temp << 8;
                        temp += Com_Rc_Data[3];

                        //CW读.Text = (temp / 1000).ToString() + "." + (temp % 1000).ToString();

                    }
                    else
                    {
                        reciviedRight = 2;
                    }
                    break;
                case "设CC值":
                    if (Com_Rc_Data[0] == 0xAA && Com_Rc_Data[2] == 0x2A)
                    {
                        reciviedRight = 4;

                    }
                    else
                    {
                        reciviedRight = 2;
                    }
                    break;

                case "设CV值":
                    if (Com_Rc_Data[0] == 0xAA && Com_Rc_Data[2] == 0x2C)
                    {
                        reciviedRight = 4;
                    }
                    else
                    {
                        reciviedRight = 2;
                    }
                    break;
                case "设CR值":
                    if (Com_Rc_Data[0] == 0xAA && Com_Rc_Data[2] == 0x30)
                    {
                        reciviedRight = 4;
                    }
                    else
                    {
                        reciviedRight = 2;
                    }
                    break;
                case "设CW值":
                    if (Com_Rc_Data[0] == 0xAA && Com_Rc_Data[2] == 0x2E)
                    {
                        reciviedRight = 4;
                    }
                    else
                    {
                        reciviedRight = 2;
                    }
                    break;

            }
            Com_Rc_Data_Lenth = 0;
            Array.Clear(Com_Rc_Data, 0, 1024);
            return 1;
        }
        /// <summary>
        /// 解析数据并显示在界面
        /// </summary>
        public void Message_show()//数据解析函数
        {
           // this.Dispose();
            switch (flag_btnclick)
            {
                case "遥信":
                    //dataGridView_YX.DataSource = new List<Fruit>() {
                    //new Fruit(){名字1 = "BMS运行状态" ,  状态1  = ((double)GetYC(0,Data_Rec[0])).ToString (),   名字2 = "C6低压保护",  状态2 = GetBit(Data_Rec[3],5).ToString (),   名字3 = "C12高压告警",  状态3 = GetBit(Data_Rec[6],3).ToString (),      名字4 = "BATC过流保护",  状态4 = GetBit(Data_Rec[11],1).ToString (),    名字5 = "RR_T_H保护",       状态5  = GetBit(Data_Rec[17],3).ToString ()},
                    //new Fruit(){名字1 = "C1低压告警" ,   状态1  = GetBit(Data_Rec[1],0).ToString (),            名字2 = "C7低压保护",  状态2 = GetBit(Data_Rec[3],6).ToString (),   名字3 = "C1高压保护",   状态3 = GetBit(Data_Rec[7],0).ToString (),      名字4 = "BATD过流告警",  状态4 = GetBit(Data_Rec[11],2).ToString (),    名字5 = "E_T_H告警" ,       状态5  = GetBit(Data_Rec[18],0).ToString ()},
                    //new Fruit(){名字1 = "C2低压告警" ,   状态1  = GetBit(Data_Rec[1],1).ToString (),            名字2 = "C8低压保护",  状态2 = GetBit(Data_Rec[3],7).ToString (),   名字3 = "C2高压保护",   状态3 = GetBit(Data_Rec[7],1).ToString (),      名字4 = "BATD过流保护",  状态4 = GetBit(Data_Rec[11],3).ToString (),    名字5 = "E_T_H保护" ,       状态5  = GetBit(Data_Rec[18],1).ToString ()},
                    //new Fruit(){名字1 = "C3低压告警" ,   状态1  = GetBit(Data_Rec[1],2).ToString (),            名字2 = "C9低压保护",  状态2 = GetBit(Data_Rec[4],0).ToString (),   名字3 = "C3高压保护",   状态3 = GetBit(Data_Rec[7],2).ToString (),      名字4 = "BATC_T1_H告警", 状态4 = GetBit(Data_Rec[13],0).ToString (),    名字5 = "E_T_L告警" ,       状态5  = GetBit(Data_Rec[18],2).ToString ()},
                    //new Fruit(){名字1 = "C4低压告警" ,   状态1  = GetBit(Data_Rec[1],3).ToString (),            名字2 = "C10低压保护", 状态2 = GetBit(Data_Rec[4],1).ToString (),   名字3 = "C4高压保护",   状态3 = GetBit(Data_Rec[7],3).ToString (),      名字4 = "BATC_T1_H保护", 状态4 = GetBit(Data_Rec[13],1).ToString (),    名字5 = "E_T_L保护" ,       状态5  = GetBit(Data_Rec[18],3).ToString ()},
                    //new Fruit(){名字1 = "C5低压告警" ,   状态1  = GetBit(Data_Rec[1],4).ToString (),            名字2 = "C11低压保护", 状态2 = GetBit(Data_Rec[4],2).ToString (),   名字3 = "C5高压保护",   状态3 = GetBit(Data_Rec[7],4).ToString (),      名字4 = "BATC_T1_L告警", 状态4 = GetBit(Data_Rec[13],2).ToString (),    名字5 = "BATD_T2_H告警" ,   状态5  = GetBit(Data_Rec[16],0).ToString ()},
                    //new Fruit(){名字1 = "C6低压告警" ,   状态1  = GetBit(Data_Rec[1],5).ToString (),            名字2 = "C12低压保护", 状态2 = GetBit(Data_Rec[4],3).ToString (),   名字3 = "C6高压保护",   状态3 = GetBit(Data_Rec[7],5).ToString (),      名字4 = "BATC_T1_L保护", 状态4 = GetBit(Data_Rec[13],3).ToString (),    名字5 = "BATD_T2_H保护" ,   状态5  = GetBit(Data_Rec[16],1).ToString ()},
                    //new Fruit(){名字1 = "C7低压告警" ,   状态1  = GetBit(Data_Rec[1],6).ToString (),            名字2 = "C1高压告警",  状态2 = GetBit(Data_Rec[5],0).ToString (),   名字3 = "C7高压保护",   状态3 = GetBit(Data_Rec[7],6).ToString (),      名字4 = "BATC_T2_H告警", 状态4 = GetBit(Data_Rec[15],0).ToString (),    名字5 = "BATD_T2_L告警" ,   状态5  = GetBit(Data_Rec[16],2).ToString ()},
                    //new Fruit(){名字1 = "C8低压告警" ,   状态1  = GetBit(Data_Rec[1],7).ToString (),            名字2 = "C2高压告警",  状态2 = GetBit(Data_Rec[5],1).ToString () ,  名字3 = "C8高压保护",   状态3 = GetBit(Data_Rec[7],7).ToString (),      名字4 = "BATC_T2_H保护", 状态4 = GetBit(Data_Rec[15],1).ToString (),    名字5 = "BATD_T2_L保护" ,   状态5  = GetBit(Data_Rec[16],3).ToString ()},
                    //new Fruit(){名字1 = "C9低压告警" ,   状态1  = GetBit(Data_Rec[2],0).ToString (),            名字2 = "C3高压告警",  状态2 = GetBit(Data_Rec[5],2).ToString () ,  名字3 = "C9高压保护",   状态3 = GetBit(Data_Rec[8],0).ToString (),      名字4 = "BATC_T2_L告警", 状态4 = GetBit(Data_Rec[15],2).ToString (),    名字5 = "通讯故障",         状态5 = GetBit(Data_Rec[19],0).ToString ()},
                    //new Fruit(){名字1 = "C10低压告警" ,  状态1  = GetBit(Data_Rec[2],1).ToString (),            名字2 = "C4高压告警",  状态2 = GetBit(Data_Rec[5],3).ToString () ,  名字3 = "C10高压保护",  状态3 = GetBit(Data_Rec[8],1).ToString (),      名字4 = "BATC_T2_L保护", 状态4 = GetBit(Data_Rec[15],3).ToString (),    名字5 = "I2C故障",          状态5 = GetBit(Data_Rec[19],1).ToString ()},
                    //new Fruit(){名字1 = "C11低压告警" ,  状态1  = GetBit(Data_Rec[2],2).ToString (),            名字2 = "C5高压告警",  状态2 = GetBit(Data_Rec[5],4).ToString () ,  名字3 = "C11高压保护",  状态3 = GetBit(Data_Rec[8],2).ToString (),      名字4 = "BATD_T1_H告警", 状态4 = GetBit(Data_Rec[14],0).ToString () ,   名字5 = "MOS管故障",        状态5 = GetBit(Data_Rec[19],2).ToString ()},
                    //new Fruit(){名字1 = "C12低压告警" ,  状态1  = GetBit(Data_Rec[2],3).ToString (),            名字2 = "C6高压告警",  状态2 = GetBit(Data_Rec[5],5).ToString () ,  名字3 = "C12高压保护",  状态3 = GetBit(Data_Rec[8],3).ToString (),      名字4 = "BATD_T1_H保护", 状态4 = GetBit(Data_Rec[14],1).ToString (),    名字5 = "电池管理芯片故障", 状态5 = GetBit(Data_Rec[19],3).ToString ()},
                    //new Fruit(){名字1 = "C1低压保护" ,   状态1  = GetBit(Data_Rec[3],0).ToString (),            名字2 = "C7高压告警",  状态2 = GetBit(Data_Rec[5],6).ToString (),   名字3 = "BAT低压告警",  状态3 = GetBit(Data_Rec[9],0).ToString (),      名字4 = "BATD_T1_L告警", 状态4 = GetBit(Data_Rec[14],2).ToString (),    名字5 = "存储器故障",         状态5 = GetBit(Data_Rec[19],4).ToString ()},
                    //new Fruit(){名字1 = "C2低压保护" ,   状态1  = GetBit(Data_Rec[3],1).ToString (),            名字2 = "C8高压告警",  状态2 = GetBit(Data_Rec[5],7).ToString (),   名字3 = "BAT低压保护",  状态3 = GetBit(Data_Rec[9],1).ToString (),      名字4 = "BATD_T1_L保护", 状态4 = GetBit(Data_Rec[14],3).ToString (),    名字5 = "采样故障",         状态5 = GetBit(Data_Rec[19],5).ToString ()},
                    //new Fruit(){名字1 = "C3低压保护" ,   状态1  = GetBit(Data_Rec[3],2).ToString (),            名字2 = "C9高压告警",  状态2 = GetBit(Data_Rec[6],0).ToString (),   名字3 = "BAT高压告警",  状态3 = GetBit(Data_Rec[9],2).ToString (),      名字4 = "LR_T_H告警",    状态4  = GetBit(Data_Rec[17],0).ToString (),   名字5 = "时钟故障",     状态5 = GetBit(Data_Rec[19],6).ToString ()},
                    //new Fruit(){名字1 = "C4低压保护",    状态1  = GetBit(Data_Rec[3],3).ToString (),            名字2 = "C10高压告警", 状态2 = GetBit(Data_Rec[6],1).ToString (),   名字3 = "BAT高压保护",  状态3 = GetBit(Data_Rec[9],3).ToString (),      名字4 = "LR_T_H保护" ,   状态4  = GetBit(Data_Rec[17],1).ToString (),   名字5 = "充电允许标志",     状态5 = GetBit(Data_Rec[19],7).ToString ()},
                    //new Fruit(){名字1 = "C5低压保护",    状态1  = GetBit(Data_Rec[3],4).ToString (),            名字2 = "C11高压告警", 状态2 = GetBit(Data_Rec[6],2).ToString (),   名字3 = "BATC过流告警", 状态3 = GetBit(Data_Rec[11],0).ToString(),      名字4 = "RR_T_H保护"  ,  状态4  = GetBit(Data_Rec[17],2).ToString (),   名字5 = "充电结束标志",     状态5 = GetBit(Data_Rec[20],0).ToString ()}};
                    break;
                case "遥测":
                    /////////////C1--C12/////////////////////////////////
                    YC_Data[0] = GetYC(Data_Rec[0], Data_Rec[1]);
                    YC_Data[1] = (Int16)GetYC(Data_Rec[2], Data_Rec[3]);
                    YC_Data[2] = (Int16)GetYC(Data_Rec[4], Data_Rec[5]);
                    YC_Data[3] = (Int16)GetYC(Data_Rec[6], Data_Rec[7]);
                    YC_Data[4] = (Int16)GetYC(Data_Rec[8], Data_Rec[9]);
                    YC_Data[5] = (Int16)GetYC(Data_Rec[10], Data_Rec[11]);
                    YC_Data[6] = (Int16)GetYC(Data_Rec[12], Data_Rec[13]);
                    YC_Data[7] = GetYC(Data_Rec[14], Data_Rec[15]);
                    YC_Data[8] = GetYC(Data_Rec[16], Data_Rec[17]);
                    YC_Data[9] = GetYC(Data_Rec[18], Data_Rec[19]);
                    YC_Data[10] = GetYC(Data_Rec[20], Data_Rec[21]);
                    YC_Data[11] = GetYC(Data_Rec[22], Data_Rec[23]);
                    /////////////////////
                    YC_Data[12] = GetYC(Data_Rec[24], Data_Rec[25]);
                    YC_Data[13] = GetYC(Data_Rec[26], Data_Rec[27]);
                    YC_Data[14] = GetYC(Data_Rec[28], Data_Rec[29]);
                    YC_Data[15] = GetYC(Data_Rec[30], Data_Rec[31]);
                    YC_Data[16] = GetYC(Data_Rec[32], Data_Rec[33]);
                    YC_Data[17] = GetYC(Data_Rec[34], Data_Rec[35]);
                    YC_Data[18] = GetYC(Data_Rec[36], Data_Rec[37]);
                    YC_Data[19] = GetYC(Data_Rec[38], Data_Rec[39]);

                    YC_Data[20] = GetYC(Data_Rec[40], Data_Rec[41]);
                    YC_Data[21] = GetYC(Data_Rec[42], Data_Rec[43]);
                    YC_Data[22] = GetYC(Data_Rec[44], Data_Rec[45]);
                    YC_Data[23] = GetYC(Data_Rec[46], Data_Rec[47]);
                    YC_Data[24] = GetYC(Data_Rec[48], Data_Rec[49]);
                    YC_Data[25] = GetYC(Data_Rec[50], Data_Rec[51]);
                    YC_Data[26] = GetYC(Data_Rec[52], Data_Rec[53]);
                    YC_Data[27] = GetYC(Data_Rec[54], Data_Rec[55]);
                    YC_Data[28] = GetYC(Data_Rec[56], Data_Rec[57]);

                  
                    break;
                case "读配置信息A1":
                    //textBox14.Text = ByteTostring(BMS_Ver_A1_Read[0]);//.ToString();
                    //textBox15.Text = ByteTostring(BMS_Ver_A1_Read[4]) + ByteTostring(BMS_Ver_A1_Read[3]) + ByteTostring(BMS_Ver_A1_Read[2]) + ByteTostring(BMS_Ver_A1_Read[1]);
                    //textBox16.Text = ByteTostring(BMS_Ver_A1_Read[7]) + ByteTostring(BMS_Ver_A1_Read[6]) + ByteTostring(BMS_Ver_A1_Read[5]);
                    //textBox17.Text = ByteTostring(BMS_Ver_A1_Read[9]) + ByteTostring(BMS_Ver_A1_Read[8]);
                    //textBox18.Text = ByteTostring(BMS_Ver_A1_Read[11]) + ByteTostring(BMS_Ver_A1_Read[10]);
                    //textBox19.Text = ByteTostring(BMS_Ver_A1_Read[13]) + ByteTostring(BMS_Ver_A1_Read[12]);
                    //textBox20.Text = ByteTostring(BMS_Ver_A1_Read[15]) + ByteTostring(BMS_Ver_A1_Read[14]);
                    //textBox21.Text = ByteTostring(BMS_Ver_A1_Read[16]);
                    //textBox22.Text = ByteTostring(BMS_Ver_A1_Read[20]) + ByteTostring(BMS_Ver_A1_Read[19]) + ByteTostring(BMS_Ver_A1_Read[18]) + ByteTostring(BMS_Ver_A1_Read[17]);
                    //textBox23.Text = ByteTostring(BMS_Ver_A1_Read[23]) + ByteTostring(BMS_Ver_A1_Read[22]) + ByteTostring(BMS_Ver_A1_Read[21]);
                    //textBox24.Text = ByteTostring(BMS_Ver_A1_Read[24]);
                    //textBox25.Text = ByteTostring(BMS_Ver_A1_Read[28]) + ByteTostring(BMS_Ver_A1_Read[27]) + ByteTostring(BMS_Ver_A1_Read[26]) + ByteTostring(BMS_Ver_A1_Read[25]);
                    //textBox26.Text = ByteTostring(BMS_Ver_A1_Read[31]) + ByteTostring(BMS_Ver_A1_Read[30]) + ByteTostring(BMS_Ver_A1_Read[29]);
                    //textBox27.Text = ByteTostring(BMS_Ver_A1_Read[39]) + ByteTostring(BMS_Ver_A1_Read[38]) + ByteTostring(BMS_Ver_A1_Read[37]) + ByteTostring(BMS_Ver_A1_Read[36]) + ByteTostring(BMS_Ver_A1_Read[35]) + ByteTostring(BMS_Ver_A1_Read[34]) + ByteTostring(BMS_Ver_A1_Read[33]) + ByteTostring(BMS_Ver_A1_Read[32]);
                    //if (textBox17.Text == "0101")
                    //{
                    //    Para_Disy.Text = "6.5AH-格式电芯-4.2V";
                    //}
                    //else if (textBox17.Text == "0102")
                    //{
                    //    Para_Disy.Text = "16AH-格式电芯-4.2V";
                    //}
                    //else if (textBox17.Text == "0201")
                    //{
                    //    Para_Disy.Text = "6.5AH-瑟福电芯-4.2V";
                    //}
                    //else if (textBox17.Text == "0202")
                    //{
                    //    Para_Disy.Text = "16AH-瑟福电芯-4.2V";
                    //}
                    //else if (textBox17.Text == "0251")
                    //{
                    //    Para_Disy.Text = "20AH-瑟福电芯-4.35V";
                    //}
                    //else
                    //{
                    //    Para_Disy.Text = "规格未知";
                    //}

                    break;
                case "广播对时":
                    break;
                case "遥控":
                    break;
                case "读总电量":
                    double totleEnergy = ((double)((Data_Rec[42] << 24) + (Data_Rec[43] << 16) + (Data_Rec[44] << 8) + Data_Rec[45])) / 100;
                  //  textBox_Totle.Text = "总发电量为：" + totleEnergy.ToString("F2") + "KWh";
                    break;
                case "召SOE":
                    Show_SOEMessage();
                    break;
                case "设置总电量":
                    break;
                case "读设备时钟":
                    //textBox_ReadTime.Text =  ((Data_Rec[48] << 8) + Data_Rec[49]).ToString() + "/" + Data_Rec[47].ToString() + "/" + Data_Rec[46].ToString() + "/" + Data_Rec[45].ToString() + ":" + Data_Rec[44].ToString() + ":" + Data_Rec[43].ToString() + ":" + ((Data_Rec[41] << 8) + Data_Rec[42]).ToString();               
                    break;
                case "召日发电量":
                    double dayEnergy = ((double)((Data_Rec[45] << 8) + Data_Rec[46])) / 100;
                    //textBox_Day.Text = "日发电量为：" + dayEnergy.ToString("F2") + "KWh";
                    break;
                case "清除数据信息":
                    break;
                case "清除配置信息":
                    break;
                case "遥调":
                    break;
  
                case "读取设置参数":
                    //dataGridView_SettedData.DataSource = new List<Fruit>() {
                    //new Fruit(){名字1 = "标准/特殊保护模式" , 状态1  = GetYC(Data_Rec[42],Data_Rec[43]).ToString (), 名字2 = "预留4",          状态2 = GetYC(Data_Rec[62],Data_Rec[63]).ToString (),               名字3 = "频率保护最小值",       状态3 = ((double)GetYC(Data_Rec[82],Data_Rec[83])/100).ToString ("F2"),   名字4 = "母线电压校准系数1",     状态4 = GetYC(Data_Rec[102],Data_Rec[103]).ToString (),名字5 = "DSP交流B电流校准系数1" , 状态5  = GetYC(Data_Rec[122],Data_Rec[123]).ToString (), 名字6 = "ARM交流B电压校准系数1", 状态6 = GetYC(Data_Rec[142],Data_Rec[143]).ToString (), 名字7 = "预留3", 状态7 = GetYC(Data_Rec[162],Data_Rec[163]).ToString (), 名字8 = "预留13", 状态8 = GetYC(Data_Rec[182],Data_Rec[183]).ToString ()},
                    //new Fruit(){名字1 = "限功率开关" ,        状态1  = GetYC(Data_Rec[44],Data_Rec[45]).ToString (), 名字2 = "预留5",          状态2 = GetYC(Data_Rec[64],Data_Rec[65]).ToString (),               名字3 = "漏电流增加限值",       状态3 = GetYC(Data_Rec[84],Data_Rec[85]).ToString (),                 名字4 = "母线电压校准系数2",     状态4 = GetYC(Data_Rec[104],Data_Rec[105]).ToString (),名字5 = "DSP交流B电流校准系数2" , 状态5  = GetYC(Data_Rec[124],Data_Rec[125]).ToString (), 名字6 = "ARM交流B电压校准系数2", 状态6 = GetYC(Data_Rec[144],Data_Rec[145]).ToString (), 名字7 = "预留4", 状态7 = GetYC(Data_Rec[164],Data_Rec[165]).ToString (), 名字8 = "预留14", 状态8 = GetYC(Data_Rec[184],Data_Rec[185]).ToString ()},
                    //new Fruit(){名字1 = "逆变器启停" ,        状态1  = GetYC(Data_Rec[46],Data_Rec[47]).ToString (), 名字2 = "预留6",          状态2 = GetYC(Data_Rec[66],Data_Rec[67]).ToString (),               名字3 = "直流1侧电压校准系数1", 状态3 = GetYC(Data_Rec[86],Data_Rec[87]).ToString (),                 名字4 = "母线电流校准系数1",     状态4 = GetYC(Data_Rec[106],Data_Rec[107]).ToString (),名字5 = "DSP交流C电压校准系数1" , 状态5  = GetYC(Data_Rec[126],Data_Rec[127]).ToString (), 名字6 = "ARM交流B电流校准系数1", 状态6 = GetYC(Data_Rec[146],Data_Rec[147]).ToString (), 名字7 = "预留5", 状态7 = GetYC(Data_Rec[166],Data_Rec[167]).ToString (), 名字8 = "预留15", 状态8 = GetYC(Data_Rec[186],Data_Rec[187]).ToString ()},
                    //new Fruit(){名字1 = "MPPT开关" ,          状态1  = GetYC(Data_Rec[48],Data_Rec[49]).ToString (), 名字2 = "预留7",          状态2 = GetYC(Data_Rec[68],Data_Rec[69]).ToString (),               名字3 = "直流1侧电压校准系数2", 状态3 = GetYC(Data_Rec[88],Data_Rec[89]).ToString (),                 名字4 = "母线电流校准系数2",     状态4 = GetYC(Data_Rec[108],Data_Rec[109]).ToString (),名字5 = "DSP交流C电压校准系数2" , 状态5  = GetYC(Data_Rec[128],Data_Rec[129]).ToString (), 名字6 = "ARM交流B电流校准系数2", 状态6 = GetYC(Data_Rec[148],Data_Rec[149]).ToString (), 名字7 = "预留6", 状态7 = GetYC(Data_Rec[168],Data_Rec[169]).ToString (), 名字8 = "预留16", 状态8 = GetYC(Data_Rec[188],Data_Rec[189]).ToString ()},
                    //new Fruit(){名字1 = "孤岛开关" ,          状态1  = GetYC(Data_Rec[50],Data_Rec[51]).ToString (), 名字2 = "预留8",          状态2 = GetYC(Data_Rec[70],Data_Rec[71]).ToString (),               名字3 = "直流1侧电流校准系数1", 状态3 = GetYC(Data_Rec[90],Data_Rec[91]).ToString (),                 名字4 = "DSP交流A电压校准系数1", 状态4 = GetYC(Data_Rec[110],Data_Rec[111]).ToString (),名字5 = "DSP交流C电流校准系数1" , 状态5  = GetYC(Data_Rec[130],Data_Rec[131]).ToString (), 名字6 = "ARM交流C电压校准系数1", 状态6 = GetYC(Data_Rec[150],Data_Rec[151]).ToString (), 名字7 = "预留7", 状态7 = GetYC(Data_Rec[170],Data_Rec[171]).ToString (), 名字8 = "预留17", 状态8 = GetYC(Data_Rec[190],Data_Rec[191]).ToString ()},
                    //new Fruit(){名字1 = "绝缘阻抗检测开关" ,  状态1  = GetYC(Data_Rec[52],Data_Rec[53]).ToString (), 名字2 = "限功率输出",     状态2 = GetYC(Data_Rec[72],Data_Rec[73]).ToString (),               名字3 = "直流1侧电流校准系数2", 状态3 = GetYC(Data_Rec[92],Data_Rec[93]).ToString (),                 名字4 = "DSP交流A电压校准系数2", 状态4 = GetYC(Data_Rec[112],Data_Rec[113]).ToString (),名字5 = "DSP交流C电流校准系数2" , 状态5  = GetYC(Data_Rec[132],Data_Rec[133]).ToString (), 名字6 = "ARM交流C电压校准系数2", 状态6 = GetYC(Data_Rec[152],Data_Rec[153]).ToString (), 名字7 = "预留8", 状态7 = GetYC(Data_Rec[172],Data_Rec[173]).ToString (), 名字8 = "预留18", 状态8 = GetYC(Data_Rec[192],Data_Rec[193]).ToString ()},
                    //new Fruit(){名字1 = "计量板开关" ,        状态1  = GetYC(Data_Rec[54],Data_Rec[55]).ToString (), 名字2 = "功率因数",       状态2 = GetYC(Data_Rec[74],Data_Rec[75]).ToString (),               名字3 = "直流2侧电压校准系数1", 状态3 = GetYC(Data_Rec[94],Data_Rec[95]).ToString (),                 名字4 = "DSP交流A电流校准系数1", 状态4 = GetYC(Data_Rec[114],Data_Rec[115]).ToString (),名字5 = "ARM交流A电压校准系数1" , 状态5  = GetYC(Data_Rec[134],Data_Rec[135]).ToString (), 名字6 = "ARM交流C电压校准系数1", 状态6 = GetYC(Data_Rec[154],Data_Rec[155]).ToString (), 名字7 = "预留9", 状态7 = GetYC(Data_Rec[174],Data_Rec[175]).ToString (), 名字8 = "预留19", 状态8 = GetYC(Data_Rec[194],Data_Rec[195]).ToString ()},
                    //new Fruit(){名字1 = "预留1" ,             状态1  = GetYC(Data_Rec[56],Data_Rec[57]).ToString (), 名字2 = "电压保护最大值", 状态2 = ((double)GetYC(Data_Rec[76],Data_Rec[77])/10).ToString ("F1"),  名字3 = "直流2侧电压校准系数2", 状态3 = GetYC(Data_Rec[96],Data_Rec[97]).ToString (),                 名字4 = "DSP交流A电流校准系数2", 状态4 = GetYC(Data_Rec[116],Data_Rec[117]).ToString (),名字5 = "ARM交流A电压校准系数2" , 状态5  = GetYC(Data_Rec[136],Data_Rec[137]).ToString (), 名字6 = "ARM交流C电流校准系数2", 状态6 = GetYC(Data_Rec[156],Data_Rec[157]).ToString (), 名字7 = "预留10", 状态7 = GetYC(Data_Rec[176],Data_Rec[177]).ToString (), 名字8 = "预留20", 状态8 = GetYC(Data_Rec[196],Data_Rec[197]).ToString ()},
                    //new Fruit(){名字1 = "预留2" ,             状态1  = GetYC(Data_Rec[58],Data_Rec[59]).ToString (), 名字2 = "电压保护最小值", 状态2 = ((double)GetYC(Data_Rec[78],Data_Rec[79])/10).ToString ("F1"),  名字3 = "直流2侧电流校准系数1", 状态3 = GetYC(Data_Rec[98],Data_Rec[99]).ToString (),                 名字4 = "DSP交流B电压校准系数1", 状态4 = GetYC(Data_Rec[118],Data_Rec[119]).ToString (),名字5 = "ARM交流A电流校准系数1" , 状态5  = GetYC(Data_Rec[138],Data_Rec[139]).ToString (), 名字6 = "预留1",                 状态6 = GetYC(Data_Rec[158],Data_Rec[159]).ToString (), 名字7 = "预留11", 状态7 = GetYC(Data_Rec[178],Data_Rec[189]).ToString (), 名字8 = "预留21", 状态8 = GetYC(Data_Rec[198],Data_Rec[199]).ToString ()},
                    //new Fruit(){名字1 = "预留3" ,             状态1  = GetYC(Data_Rec[60],Data_Rec[61]).ToString (), 名字2 = "频率保护最大值", 状态2 = ((double)GetYC(Data_Rec[80],Data_Rec[81])/100).ToString ("F2"), 名字3 = "直流2侧电流校准系数2", 状态3 = GetYC(Data_Rec[100],Data_Rec[101]).ToString (),               名字4 = "DSP交流B电压校准系数2", 状态4 = GetYC(Data_Rec[120],Data_Rec[121]).ToString (),名字5 = "ARM交流A电流校准系数2" , 状态5  = GetYC(Data_Rec[140],Data_Rec[141]).ToString (), 名字6 = "预留2",                 状态6 = GetYC(Data_Rec[160],Data_Rec[161]).ToString (), 名字7 = "预留12", 状态7 = GetYC(Data_Rec[180],Data_Rec[181]).ToString (), 名字8 = "预留22", 状态8 = GetYC(Data_Rec[200],Data_Rec[201]).ToString ()}};
                    break;
                default: break;
            }
          //  this.Close();
        }

        public static int GetBit(int Resource, int Mask)//获取整数的某一位
        {
            return Resource >> Mask & 1;
        }

        public static int GetYC(int Byte1, int Byte2)//根据两个字节获取遥测值
        {
            int yaoce = (Byte1 << 8) + Byte2;
            return yaoce;
        }

        public string[] dgv_string_data = new string[7];//定义存放SOE信息的string数组

        public void Show_SOEMessage()//将SOE数据显示在窗体
        {
            int num = Data_Rec[42];
            for (int a = 0; a < num; a++)
            {
                dgv_string_data[0] = Convert.ToString(Data_Rec[50 + 8 * a]);
                if (Data_Rec[49 + 8 * a] == 0x80)
                {
                    dgv_string_data[1] = Convert.ToString(1);
                }
                else if (Data_Rec[49 + 8 * a] == 0x00)
                {
                    dgv_string_data[1] = Convert.ToString(0);
                }
                else
                {
                    MessageBox.Show("第" + (a + 1).ToString() + "个SOE的遥信状态错误，不为1或0", "警告！");
                    break;
                }
                dgv_string_data[2] = Convert.ToString(Data_Rec[48 + 8 * a]);
                dgv_string_data[3] = Convert.ToString(Data_Rec[47 + 8 * a]);
                dgv_string_data[4] = Convert.ToString(Data_Rec[46 + 8 * a]);
                dgv_string_data[5] = Convert.ToString(Data_Rec[45 + 8 * a]);
                int ms = ((Data_Rec[43 + 8 * a] & 0x00ff) << 8) + Data_Rec[44 + 8 * a];
                dgv_string_data[6] = Convert.ToString(ms);
                DataGridViewRowCollection rows = this.dataGridView_SOE.Rows;
                rows.Add(dgv_string_data);
            }

        }

        public void Error_Check()//纠错函数
        {

            switch (flag_btnclick)
            {
                case "遥信":
                    if ((textBox_YXStartAddress.Text == "") || (textBox_YXNum.Text == "") || (int.Parse(textBox_YXStartAddress.Text) != 0))
                    {
                        flag_error = 1;
                        MessageBox.Show("遥信参数设置错误，请检查", "提示！");
                    }
                    else
                        flag_error = 0;
                    break;
                case "遥测":
                    //if ((textBox_YCStartAddress.Text == "") || (textBox_YCNum.Text == "") || (int.Parse(textBox_YCStartAddress.Text) != 0))
                    //{
                    //    flag_error = 2;
                    //    MessageBox.Show("遥测参数设置错误，请检查", "提示！");
                    //}
                    //else
                    //    flag_error = 0;
                    break;
                case "召SOE":
                    if ((textBox_SOEStartAddress.Text == "") || (textBox_SOENum.Text == "") || (int.Parse(textBox_SOEStartAddress.Text) > 80) || (int.Parse(textBox_SOENum.Text) > 80) || (((int.Parse(textBox_SOENum.Text)) - (int.Parse(textBox_SOEStartAddress.Text))) > 9) || (((int.Parse(textBox_SOEStartAddress.Text)) > (int.Parse(textBox_SOENum.Text)))))
                    {
                        flag_error = 3;
                        MessageBox.Show("读SOE参数设置错误，请检查", "提示！");
                    }
                    else
                        flag_error = 0;
                    break;
                case "读取设置参数":
                    //if ((textBox_SettedDataStartAddress.Text == "") || (textBox_SettedDataNum.Text == "") || (int.Parse(textBox_SettedDataStartAddress.Text) != 0) || (int.Parse(textBox_SettedDataNum.Text) != 80))
                    //{
                    //    flag_error = 4;
                    //    MessageBox.Show("读取设置参数设置错误，请检查", "提示！");
                    //}
                    //else
                    //    flag_error = 0;
                    break;
                    /*case "遥控":
                        if ((combB_YKName.Text == "") || (combB_YKSelect.Text == ""))
                        {
                            flag_error = 5;
                            MessageBox.Show("遥控参数设置错误，请检查", "提示！");
                        }
                        else
                            flag_error = 0;
                        break;
                    case "遥调":
                        if ((combB_YTName.Text == "") || (textBox_YTData.Text == ""))
                        {
                            flag_error = 6;
                            MessageBox.Show("遥调参数设置错误，请检查", "提示！");
                        }
                        else
                            flag_error = 0;
                        break;
                    case "读版本号":
                        if (combB_VersionName.Text == "") 
                        {
                            flag_error = 7;
                            MessageBox.Show("读版本号参数设置错误，请检查", "提示！");
                        }
                        else
                            flag_error = 0;
                        break;
                    case "设置总电量":
                        if (textBox_TotleEnergy.Text == "")
                        {
                            flag_error = 8;
                            MessageBox.Show("设置总电量参数设置错误，请检查", "提示！");
                        }
                        else
                            flag_error = 0;
                        break;*/
            }
        }

        public void Result_Show()//最终结果显示函数
        {
            switch (flag_btnclick)
            {
                case "费控":
                    if (ReciviedRight == 0)
                        MessageBox.Show("【费控专用】返回数据正确，请查看报文！", "恭喜");
                    if (ReciviedRight == 1)
                        MessageBox.Show("【费控专用】返回数据错误，未返回数据！", "遗憾！");
                    if (ReciviedRight == 2)
                        MessageBox.Show("【费控专用】返回数据错误，请查看报文！", "遗憾！");
                    if (ReciviedRight == 4)
                        MessageBox.Show("【费控专用】返回数据错误，超时或放入发送队伍失败！", "遗憾！");
                    break;
                case "遥信":
                    if (ReciviedRight == 0)
                        MessageBox.Show("遥信召测成功！", "恭喜");
                    if (ReciviedRight == 1)
                        MessageBox.Show("遥信召测失败，未返回数据！", "遗憾！");
                    if (ReciviedRight == 2)
                        MessageBox.Show("遥信召测失败，返回数据错误！", "遗憾！");
                    if (ReciviedRight == 4)
                        MessageBox.Show("遥信召测失败，超时或放入发送队伍失败！", "遗憾！");
                    break;
                case "遥测":
                    if (ReciviedRight == 0)
                        MessageBox.Show("遥测召测成功！", "恭喜");
                    if (ReciviedRight == 1)
                        MessageBox.Show("遥测召测失败，未返回数据！", "遗憾！");
                    if (ReciviedRight == 2)
                        MessageBox.Show("遥测召测失败，返回数据错误！", "遗憾！");
                    if (ReciviedRight == 4)
                        MessageBox.Show("遥测召测失败，超时或放入发送队伍失败！", "遗憾！");
                    break;
                case "召SOE":
                    if (ReciviedRight == 0)
                        MessageBox.Show("SOE召测成功！条数为：" + Data_Rec[42].ToString(), "恭喜");
                    if (ReciviedRight == 1)
                        MessageBox.Show("SOE召测失败，未返回数据！", "遗憾！");
                    if (ReciviedRight == 2)
                        MessageBox.Show("SOE召测失败，返回数据错误！", "遗憾！");
                    if (ReciviedRight == 4)
                        MessageBox.Show("SOE召测失败，超时或放入发送队伍失败！", "遗憾！");
                    break;
                case "读取设置参数":
                    if (ReciviedRight == 0)
                        MessageBox.Show("读取设置参数成功！", "恭喜");
                    if (ReciviedRight == 1)
                        MessageBox.Show("读取设置参数失败，未返回数据！", "遗憾！");
                    if (ReciviedRight == 2)
                        MessageBox.Show("读取设置参数失败，返回数据错误！", "遗憾！");
                    if (ReciviedRight == 4)
                        MessageBox.Show("读取设置参数失败，超时或放入发送队伍失败！", "遗憾！");
                    break;
                case "遥控":
                    if (ReciviedRight == 0)
                        MessageBox.Show("遥控成功！", "恭喜");
                    if (ReciviedRight == 1)
                        MessageBox.Show("遥控失败，未返回数据！", "遗憾！");
                    if (ReciviedRight == 2)
                        MessageBox.Show("遥控失败，返回数据错误！", "遗憾！");
                    if (ReciviedRight == 3)
                        MessageBox.Show("遥控失败，逆变器拒控！", "遗憾！");
                    if (ReciviedRight == 4)
                        MessageBox.Show("遥控失败，超时或放入发送队伍失败！", "遗憾！");
                    break;
                case "遥调":
                    if (ReciviedRight == 0)
                        MessageBox.Show("遥调成功！", "恭喜");
                    if (ReciviedRight == 1)
                        MessageBox.Show("遥调失败，未返回数据！", "遗憾！");
                    if (ReciviedRight == 2)
                        MessageBox.Show("遥调失败，返回数据错误！", "遗憾！");
                    if (ReciviedRight == 3)
                        MessageBox.Show("遥调失败，逆变器拒调！", "遗憾！");
                    if (ReciviedRight == 4)
                        MessageBox.Show("遥调失败，超时或放入发送队伍失败！", "遗憾！");
                    break;
                case "读版本号":
                    if (ReciviedRight == 0)
                        MessageBox.Show("读版本号成功！", "恭喜");
                    if (ReciviedRight == 1)
                        MessageBox.Show("读版本号失败，未返回数据！", "遗憾！");
                    if (ReciviedRight == 2)
                        MessageBox.Show("读版本号失败，返回数据错误！", "遗憾！");
                    if (ReciviedRight == 4)
                        MessageBox.Show("读版本号失败，超时或放入发送队伍失败！", "遗憾！");
                    break;
                case "设置总电量":
                    if (ReciviedRight == 0)
                        MessageBox.Show("设置总电量成功！", "恭喜");
                    if (ReciviedRight == 1)
                        MessageBox.Show("设置总电量失败，未返回数据！", "遗憾！");
                    if (ReciviedRight == 2)
                        MessageBox.Show("设置总电量失败，返回数据错误！", "遗憾！");
                    if (ReciviedRight == 3)
                        MessageBox.Show("设置总电量失败，逆变器拒设！", "遗憾！");
                    if (ReciviedRight == 4)
                        MessageBox.Show("设置总电量失败，超时或放入发送队伍失败！", "遗憾！");
                    break;
                case "读设备时钟":
                    if (ReciviedRight == 0)
                        MessageBox.Show("读设备时钟成功！", "恭喜");
                    if (ReciviedRight == 1)
                        MessageBox.Show("读设备时钟失败，未返回数据！", "遗憾！");
                    if (ReciviedRight == 2)
                        MessageBox.Show("读设备时钟失败，返回数据错误！", "遗憾！");
                    if (ReciviedRight == 4)
                        MessageBox.Show("读设备时钟失败，超时或放入发送队伍失败！", "遗憾！");
                    break;
                case "读总电量":
                    if (ReciviedRight == 0)
                        MessageBox.Show("读总电量成功！", "恭喜");
                    if (ReciviedRight == 1)
                        MessageBox.Show("读总电量失败，未返回数据！", "遗憾！");
                    if (ReciviedRight == 2)
                        MessageBox.Show("读总电量失败，返回数据错误！", "遗憾！");
                    if (ReciviedRight == 4)
                        MessageBox.Show("读总电量失败，超时或放入发送队伍失败！", "遗憾！");
                    break;
                case "召日发电量":
                    if (ReciviedRight == 0)
                        MessageBox.Show("召日发电量成功！", "恭喜");
                    if (ReciviedRight == 1)
                        MessageBox.Show("召日发电量失败，未返回数据！", "遗憾！");
                    if (ReciviedRight == 2)
                        MessageBox.Show("召日发电量失败，返回数据错误！", "遗憾！");
                    if (ReciviedRight == 3)
                        MessageBox.Show("召日发电量失败，无该天的日发电量！", "遗憾！");
                    if (ReciviedRight == 4)
                        MessageBox.Show("召日发电量失败，超时或放入发送队伍失败！", "遗憾！");
                    break;
                case "清除数据信息":
                    if (ReciviedRight == 0)
                        MessageBox.Show("清除数据信息成功！", "恭喜");
                    if (ReciviedRight == 1)
                        MessageBox.Show("清除数据信息失败，未返回数据！", "遗憾！");
                    if (ReciviedRight == 2)
                        MessageBox.Show("清除数据信息失败，返回数据错误！", "遗憾！");
                    if (ReciviedRight == 3)
                        MessageBox.Show("清除数据信息失败，逆变器拒清！", "遗憾！");
                    if (ReciviedRight == 4)
                        MessageBox.Show("清除数据信息失败，超时或放入发送队伍失败！", "遗憾！");
                    break;
                case "清除配置信息":
                    if (ReciviedRight == 0)
                        MessageBox.Show("清除配置信息成功！", "恭喜");
                    if (ReciviedRight == 1)
                        MessageBox.Show("清除配置信息失败，未返回数据！", "遗憾！");
                    if (ReciviedRight == 2)
                        MessageBox.Show("清除配置信息失败，返回数据错误！", "遗憾！");
                    if (ReciviedRight == 3)
                        MessageBox.Show("清除配置信息失败，逆变器拒清！", "遗憾！");
                    if (ReciviedRight == 4)
                        MessageBox.Show("清除配置信息失败，超时或放入发送队伍失败！", "遗憾！");
                    break;
            }
        }

        public void AllButton_false()//使所有按键不可用
        {
            //btn_FeiKong.Enabled = false;
            // textBox_FeiKong.Enabled = false;
            btn_YX.Enabled = false;
            //btn_YC.Enabled = false;
           // btn_SOE.Enabled = false;
            //btn_ReadSettedData.Enabled = false;
            //btn_CheckTime.Enabled = false;
            //     btn_YK.Enabled = false;
            //     btn_YT.Enabled = false;
            //     btn_ReadVersion.Enabled = false;
            //     btn_SetTotalEnergy.Enabled = false;
            //     btn_ReadTime.Enabled = false;
            //     btn_ReadTotalEnergy.Enabled = false;
            //     btn_ReadDayEnergy.Enabled = false;
            //     btn_ClearData.Enabled = false;
            //     btn_ClearConfig.Enabled = false;
            //btn_FeiKong.Enabled = false;
            //     combB_YKName.Enabled = false;
            //     combB_YKSelect.Enabled = false;
            //    combB_YTName.Enabled = false;
            //    textBox_YTData.Enabled = false;
            //   combB_VersionName.Enabled = false;
            //textBox_TotleEnergy.Enabled = false;
            //     dateTimePicker.Enabled = false;
            textBox_SOEStartAddress.Enabled = false;
            textBox_SOENum.Enabled = false;
            //textBox_FeiKong.Enabled = false;
        }

        public void AllButton_true()//使所有按键可用
        {
            //btn_FeiKong.Enabled = true;
            // textBox_FeiKong.Enabled = true;
            btn_YX.Enabled = true;
            //btn_YC.Enabled = true;
            btn_SOE.Enabled = true;
            //btn_ReadSettedData.Enabled = true;
            //btn_CheckTime.Enabled = true;
            // btn_YK.Enabled = true;
            //  btn_YT.Enabled = true;
            //  btn_ReadVersion.Enabled = true;
            //  btn_SetTotalEnergy.Enabled = true;
            //  btn_ReadTime.Enabled = true;
            //  btn_ReadTotalEnergy.Enabled = true;
            //  btn_ReadDayEnergy.Enabled = true;
            //   btn_ClearData.Enabled = true;
            //   btn_ClearConfig.Enabled = true;
            //btn_FeiKong.Enabled = true;
            //   combB_YKName.Enabled = true;
            //   combB_YKSelect.Enabled = true;
            //  combB_YTName.Enabled = true;
            //textBox_YTData.Enabled = true;
            //  combB_VersionName.Enabled = true;
            //textBox_TotleEnergy.Enabled = true;
            //  dateTimePicker.Enabled = true;
            textBox_SOEStartAddress.Enabled = true;
            textBox_SOENum.Enabled = true;
            //textBox_FeiKong.Enabled = true;
        }

        public byte[] StringToByte(string STR1)//字符串转数组
        {
            String mind = "";
            String STR = "";
            int pont = 0;
            if (STR1.Length > 5)
            {
                MessageBox.Show("数据越界", "提示！");
            }
            pont=STR1.IndexOf('.');
            if (pont == 0)
            {
                MessageBox.Show("输入格式错误", "提示！");
            }
            else if (pont == 1)
            {
                STR = STR1.Remove(1, 1);
                if (STR.Length == 1)
                {
                    mind = "0" + STR + "00";
                }
                else if (STR.Length == 2)
                {
                    mind = "0" + STR + "0";
                }
                else if (STR.Length == 3)
                {
                    mind = "0" + STR;
                }
                else 
                {
                    MessageBox.Show("数据越界", "提示！");
                }
            }
            else if (pont == 2)
            {
                STR = STR1.Remove(2, 1);
                if (STR.Length == 2)
                {
                    mind = STR + "00";
                }
                else if (STR.Length == 3)
                {
                    mind = STR + "0";
                }
                else if (STR.Length == 4)
                {
                    mind = STR;
                }
            }
            else
            {
                STR = STR1;
                if (STR.Length == 1)
                {
                    mind = "0" + STR + "00";
                }
                else if (STR.Length == 2)
                {
                    mind = STR + "00";
                }
                else 
                {
                    MessageBox.Show("数据越界", "提示！");
                }

            }
           
            byte[] BITE = new byte[mind.Length / 2];
            int High = 0, Low = 0;
            for (int i = 0; i < BITE.Length; i++)
            {
                switch (mind.Substring(2 * i, 1))
                {
                    case "0": High = 0; break;
                    case "1": High = 1; break;
                    case "2": High = 2; break;
                    case "3": High = 3; break;
                    case "4": High = 4; break;
                    case "5": High = 5; break;
                    case "6": High = 6; break;
                    case "7": High = 7; break;
                    case "8": High = 8; break;
                    case "9": High = 9; break;
                    case "a": High = 10; break;
                    case "b": High = 11; break;
                    case "c": High = 12; break;
                    case "d": High = 13; break;
                    case "e": High = 14; break;
                    case "f": High = 15; break;
                    case "A": High = 10; break;
                    case "B": High = 11; break;
                    case "C": High = 12; break;
                    case "D": High = 13; break;
                    case "E": High = 14; break;
                    case "F": High = 15; break;
                }
                switch (mind.Substring(2 * i + 1, 1))
                {
                    case "0": Low = 0; break;
                    case "1": Low = 1; break;
                    case "2": Low = 2; break;
                    case "3": Low = 3; break;
                    case "4": Low = 4; break;
                    case "5": Low = 5; break;
                    case "6": Low = 6; break;
                    case "7": Low = 7; break;
                    case "8": Low = 8; break;
                    case "9": Low = 9; break;
                    case "a": Low = 10; break;
                    case "b": Low = 11; break;
                    case "c": Low = 12; break;
                    case "d": Low = 13; break;
                    case "e": Low = 14; break;
                    case "f": Low = 15; break;
                    case "A": Low = 10; break;
                    case "B": Low = 11; break;
                    case "C": Low = 12; break;
                    case "D": Low = 13; break;
                    case "E": Low = 14; break;
                    case "F": Low = 15; break;
                }
                BITE[i] = Convert.ToByte(High * 16 + Low);
            }
            return BITE;
        }

        public byte[] StringToByte32(string STR1)//字符串转数组
        {
            String mind = "";
            String STR = "";
            int pont = 0;
            if (STR1.Length >9)
            {
                MessageBox.Show("数据越界", "提示！");
            }
            pont = STR1.IndexOf('.');
            if (pont == 0)
            {
                MessageBox.Show("输入格式错误", "提示！");
            }
            else if (pont == 1)
            {
                STR = STR1.Remove(1, 1);
                if (STR.Length == 1)
                {
                    mind = "000" + STR + "0000";
                }
                else if (STR.Length == 2)
                {
                    mind = "000" + STR + "000";
                }
                else if (STR.Length == 3)
                {
                    mind = "000" + STR+"00" ;
                }
                else if (STR.Length == 4)
                {
                    mind = "000" + STR + "0";
                }
                else if (STR.Length == 5)
                {
                    mind = "000" + STR;
                }
                else
                {
                    MessageBox.Show("数据越界", "提示！");
                }
            }
            else if (pont == 2)
            {
                STR = STR1.Remove(2, 1);
                if (STR.Length == 2)
                {
                    mind = "00"+STR + "0000";
                }
                else if (STR.Length == 3)
                {
                    mind = "00" + STR + "000";
                }
                else if (STR.Length == 4)
                {
                    mind = "00" + STR + "00";
                }
                else if (STR.Length == 5)
                {
                    mind = "00" + STR + "0";
                }
                else if (STR.Length == 6)
                {
                    mind = "00" + STR ;
                }
            }
            else if (pont == 3)
            {
                STR = STR1.Remove(3, 1);
                if (STR.Length == 3)
                {
                    mind = "0" + STR + "0000";
                }
                else if (STR.Length == 4)
                {
                    mind = "0" + STR + "000";
                }
                else if (STR.Length == 5)
                {
                    mind = "0" + STR + "00";
                }
                else if (STR.Length == 6)
                {
                    mind = "0" + STR + "0";
                }
                else if (STR.Length == 7)
                {
                    mind = "0" + STR;
                }
            }
            else if (pont == 4)
            {
                STR = STR1.Remove(4, 1);
                if (STR.Length == 4)
                {
                    mind =  STR + "0000";
                }
                else if (STR.Length == 5)
                {
                    mind = STR + "000";
                }
                else if (STR.Length == 6)
                {
                    mind =  STR + "00";
                }
                else if (STR.Length == 7)
                {
                    mind = STR + "0";
                }
                else if (STR.Length == 8)
                {
                    mind = STR;
                }
            }          
            else
            {
               MessageBox.Show("数据越界", "提示！");
            }

            byte[] BITE = new byte[(mind.Length) / 2];
            int High = 0, Low = 0;
            for (int i = 0; i < BITE.Length; i++)
            {
                switch (mind.Substring(2 * i, 1))
                {
                    case "0": High = 0; break;
                    case "1": High = 1; break;
                    case "2": High = 2; break;
                    case "3": High = 3; break;
                    case "4": High = 4; break;
                    case "5": High = 5; break;
                    case "6": High = 6; break;
                    case "7": High = 7; break;
                    case "8": High = 8; break;
                    case "9": High = 9; break;
                    case "a": High = 10; break;
                    case "b": High = 11; break;
                    case "c": High = 12; break;
                    case "d": High = 13; break;
                    case "e": High = 14; break;
                    case "f": High = 15; break;
                    case "A": High = 10; break;
                    case "B": High = 11; break;
                    case "C": High = 12; break;
                    case "D": High = 13; break;
                    case "E": High = 14; break;
                    case "F": High = 15; break;
                }
                switch (mind.Substring(2 * i + 1, 1))
                {
                    case "0": Low = 0; break;
                    case "1": Low = 1; break;
                    case "2": Low = 2; break;
                    case "3": Low = 3; break;
                    case "4": Low = 4; break;
                    case "5": Low = 5; break;
                    case "6": Low = 6; break;
                    case "7": Low = 7; break;
                    case "8": Low = 8; break;
                    case "9": Low = 9; break;
                    case "a": Low = 10; break;
                    case "b": Low = 11; break;
                    case "c": Low = 12; break;
                    case "d": Low = 13; break;
                    case "e": Low = 14; break;
                    case "f": Low = 15; break;
                    case "A": Low = 10; break;
                    case "B": Low = 11; break;
                    case "C": Low = 12; break;
                    case "D": Low = 13; break;
                    case "E": Low = 14; break;
                    case "F": Low = 15; break;
                }
                BITE[i] = Convert.ToByte(High * 16 + Low);
            }
            return BITE;
        }
        public byte[] StringToTen(string STR)//字符串转数组
        {
            byte[] BITE = new byte[STR.Length];
            int High = 0, Low = 0;
            for (int i = 0; i < BITE.Length; i++)
            {
                switch (STR.Substring(i, 1))
                {
                    case "0": High = 0; break;
                    case "1": High = 1; break;
                    case "2": High = 2; break;
                    case "3": High = 3; break;
                    case "4": High = 4; break;
                    case "5": High = 5; break;
                    case "6": High = 6; break;
                    case "7": High = 7; break;
                    case "8": High = 8; break;
                    case "9": High = 9; break;
                }

                BITE[i] = Convert.ToByte(High);
            }
            return BITE;
        }
        public static char[] StringToTen_char(string STR)//字符串转数组
        {
            char[] BITE = new char[STR.Length];
            int High = 0, Low = 0;
            for (int i = 0; i < BITE.Length; i++)
            {
                switch (STR.Substring(i, 1))
                {
                    case "0": High = 0; break;
                    case "1": High = 1; break;
                    case "2": High = 2; break;
                    case "3": High = 3; break;
                    case "4": High = 4; break;
                    case "5": High = 5; break;
                    case "6": High = 6; break;
                    case "7": High = 7; break;
                    case "8": High = 8; break;
                    case "9": High = 9; break;
                }

                BITE[i] = Convert.ToChar(High);
            }
            return BITE;
        }

        public static char[] String_to_char(string STR)//字符串转数组
        {
            char[] BITE = new char[STR.Length];
            char High = '0';
            for (int i = 0; i < BITE.Length; i++)
            {
                switch (STR.Substring(i, 1))
                {
                    case "0": High = '0'; break;
                    case "1": High = '1'; break;
                    case "2": High = '2'; break;
                    case "3": High = '3'; break;
                    case "4": High = '4'; break;
                    case "5": High = '5'; break;
                    case "6": High = '6'; break;
                    case "7": High = '7'; break;
                    case "8": High = '8'; break;
                    case "9": High = '9'; break;
                    case ".": High = '.'; break;
                }

                BITE[i] = High;
            }
            return BITE;
        }

        public void Byte_to_Bmsfault(Byte[] a)//字符串转数组
        {
            UInt16 FF = 256;
            bms_fault.ID = Recorder_num;
            bms_fault.F1 = a[4] * FF + a[3];
            bms_fault.F2 = a[6] * FF + a[5];
            bms_fault.F3 = a[8] * FF + a[7];
            bms_fault.F4 = a[10] * FF + a[9];
            bms_fault.F5 = a[12] * FF + a[11];
            bms_fault.F6 = a[14] * FF + a[13];
            bms_fault.F7 = a[16] * FF + a[15];
            bms_fault.F8 = a[18] * FF + a[17];
            bms_fault.F9 = a[20] * FF + a[19];
            bms_fault.F10 = a[22] * FF + a[21];
            bms_fault.BAT_V_S = a[24] * FF + a[23];
            bms_fault.CEll_V_max_S = a[26] * FF + a[25];
            bms_fault.CEll_V_min_S = a[28] * FF + a[27];
            bms_fault.BAT_T1_S = a[30] * FF + a[29];
            bms_fault.BAT_T2_S = a[32] * FF + a[31];
            bms_fault.EV_T_S = a[34] * FF + a[33];
            bms_fault.Blance_R_T_S = a[36] * FF + a[35];
            bms_fault.Blance_L_T_S = a[38] * FF + a[37];
            bms_fault.Current_S = a[40] * FF + a[39];
            bms_fault.statement_S = a[42] * FF + a[41];
            bms_fault.soc_Num_S = a[44] * FF + a[43];
            bms_fault.RES23_S = a[46] * FF + a[45];
            bms_fault.RES24_S = a[48] * FF + a[47];
            bms_fault.RES25_S = a[50] * FF + a[49];
            bms_fault.RES26_S = a[52] * FF + a[51];
            bms_fault.RES27_S = a[54] * FF + a[53];
            bms_fault.RES28_S = a[56] * FF + a[55];
            bms_fault.RES29_S = a[58] * FF + a[57];
            bms_fault.RES30_S = a[60] * FF + a[59];
            bms_fault.RES31_S = a[62] * FF + a[61];
            bms_fault.RES32_S = a[64] * FF + a[63];

        }

        public void Byte_to_BmsCHdata(Byte[] a)//字符串转数组
        {
            UInt16 FF = 256;
            bms_charge.ID = Recorder_num;
            bms_charge.Charge_Num_S = a[4] * FF + a[3];
            bms_charge.Charge_Dur_Time_S = a[6] * FF + a[5];
            bms_charge.Current_Max_S = a[8] * FF + a[7];
            bms_charge.Per_Soc_S = a[10] * FF + a[9];
            bms_charge.Per_User_Time_S = a[12] * FF + a[11];
            bms_charge.Last_Soc_S = a[14] * FF + a[13];
            bms_charge.Last_User_Time_S = a[16] * FF + a[15];
            bms_charge.Per_Bat_Voltage_S = a[18] * FF + a[17];
            bms_charge.Last_Bat_Voltage_S = a[20] * FF + a[19];
            bms_charge.Per_Cell_Max_Voltage_S = a[22] * FF + a[21];
            bms_charge.Per_Cell_Max_Voltage_Num_S = a[24] * FF + a[23];
            bms_charge.Last_Cell_Max_Voltage_S = a[26] * FF + a[25];
            bms_charge.Last_Cell_Min_Voltage_Num_S = a[28] * FF + a[27];
            bms_charge.Per_Cell_Min_Voltage_S = a[30] * FF + a[29];
            bms_charge.Per_Cell_Min_Voltage_Num_S = a[32] * FF + a[31];
            bms_charge.Last_Cell_Min_Voltage_S = a[34] * FF + a[33];
            bms_charge.Last_Cell_Min_Voltage_Num_S = a[36] * FF + a[35];
            bms_charge.Cell_Max_Temp_S = a[38] * FF + a[37];
            bms_charge.Cell_Max_Temp_Point_S = a[40] * FF + a[39];
            bms_charge.Cell_Min_Temp_S = a[42] * FF + a[41];
            bms_charge.Cell_Min_Temp_Point_S = a[44] * FF + a[43];
            bms_charge.Hi_Temp_Dur_Time_S = a[46] * FF + a[45];
            bms_charge.Charge_ST_Flag_S = a[48] * FF + a[47];
            bms_charge.RES25_S = a[50] * FF + a[49];
            bms_charge.RES26_S = a[52] * FF + a[51];
            bms_charge.RES27_S = a[54] * FF + a[53];
            bms_charge.RES28_S = a[56] * FF + a[55];
            bms_charge.RES29_S = a[58] * FF + a[57];
            bms_charge.RES30_S = a[60] * FF + a[59];
            bms_charge.RES31_S = a[62] * FF + a[61];
            bms_charge.RES32_S = a[64] * FF + a[63];
        }
        public void Byte_to_BmsDisCHdata(Byte[] a)//字符串转数组
        {
            UInt16 FF = 256;
            bms_discharge.ID = Recorder_num;
            bms_discharge.Charge_Num_S = a[4] * FF + a[3];
            bms_discharge.Charge_Dur_Time_S = a[6] * FF + a[5];
            bms_discharge.Current_Max_S = a[8] * FF + a[7];
            bms_discharge.Per_Soc_S = a[10] * FF + a[9];
            bms_discharge.Per_User_Time_S = a[12] * FF + a[11];
            bms_discharge.Last_Soc_S = a[14] * FF + a[13];
            bms_discharge.Last_User_Time_S = a[16] * FF + a[15];
            bms_discharge.Per_Bat_Voltage_S = a[18] * FF + a[17];
            bms_discharge.Last_Bat_Voltage_S = a[20] * FF + a[19];
            bms_discharge.Per_Cell_Max_Voltage_S = a[22] * FF + a[21];
            bms_discharge.Per_Cell_Max_Voltage_Num_S = a[24] * FF + a[23];
            bms_discharge.Last_Cell_Max_Voltage_S = a[26] * FF + a[25];
            bms_discharge.Last_Cell_Min_Voltage_Num_S = a[28] * FF + a[27];
            bms_discharge.Per_Cell_Min_Voltage_S = a[30] * FF + a[29];
            bms_discharge.Per_Cell_Min_Voltage_Num_S = a[32] * FF + a[31];
            bms_discharge.Last_Cell_Min_Voltage_S = a[34] * FF + a[33];
            bms_discharge.Last_Cell_Min_Voltage_Num_S = a[36] * FF + a[35];
            bms_discharge.Cell_Max_Temp_S = a[38] * FF + a[37];
            bms_discharge.Cell_Max_Temp_Point_S = a[40] * FF + a[39];
            bms_discharge.Cell_Min_Temp_S = a[42] * FF + a[41];
            bms_discharge.Cell_Min_Temp_Point_S = a[44] * FF + a[43];
            bms_discharge.Hi_Temp_Dur_Time_S = a[46] * FF + a[45];
            bms_discharge.Charge_ST_Flag_S = a[48] * FF + a[47];
            bms_discharge.RES25_S = a[50] * FF + a[49];
            bms_discharge.RES26_S = a[52] * FF + a[51];
            bms_discharge.RES27_S = a[54] * FF + a[53];
            bms_discharge.RES28_S = a[56] * FF + a[55];
            bms_discharge.RES29_S = a[58] * FF + a[57];
            bms_discharge.RES30_S = a[60] * FF + a[59];
            bms_discharge.RES31_S = a[62] * FF + a[61];
            bms_discharge.RES32_S = a[64] * FF + a[63];
        }
        public static string ByteTostring(byte STR)//字符串转数组
        {
            string R = "";
            if (STR > 9)
            {
                R = STR.ToString();

            }
            else
            {
                R = "0" + STR.ToString();
            }
            return R;

        }

        /// <summary>
        /// 配置文件读写函数定义
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="filepath"></param>
        /// <returns></returns>
        #region "声明变量"
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filepath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder reval, int size, string filePath);
        private string strFilePath = System.Windows.Forms.Application.StartupPath + "\\FileConfig.ini";
        private string strSec = "";
        #endregion
        private string ContentValue(string Section, string key)
        {
            StringBuilder temp = new StringBuilder(1024);
            GetPrivateProfileString(Section, key, "", temp, 1024, strFilePath);
            return temp.ToString();
        }
        public  void Com_Over_Time_Pro()
        {
                Flag_Com = 0;
                Com_OverTime_ctc = 0;
            Time_Delay_MS(150);
            while (Flag_Com == 0)
            {
                Time_Delay_MS(5);
                Com_OverTime_ctc++;
                if (Com_OverTime_ctc == 1500)
                {
                    SendData_ShowData();
                }
                else if (Com_OverTime_ctc == 2000)
                {
                    SendData_ShowData();
                }
                else if (Com_OverTime_ctc == 3000)
                {
                    SendData_ShowData();
                }
                else if (Com_OverTime_ctc > 4000)
                {
                    Com_OverTime_ctc = 0;
                    break;
                }
            }
        }

    }
    //下面讲一个打开窗体定时执行按钮的东西

   

   // private void button1_Click(object sender, EventArgs e) { }

}
