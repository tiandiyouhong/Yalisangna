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
    public partial class Form_Main : Form
    {
        Socket clientSocket = null;//客户端套接字 
                                   // public static string[] Device1 = new string[30];

        public Form_Main()
        {
            InitializeComponent();
        }

        private void Form_Main_Load(object sender, EventArgs e)//登录界面
        {
            tabPage1.Parent = null;
            //tabPage2.Parent = null;
            //    tabPage3.Parent = null;
            tabPage4.Parent = null;
            //    tabPage5.Parent = null;
            //tabPage6.Parent = null;
            this.Left = (Screen.PrimaryScreen.Bounds.Width - this.Width) / 2;//将主界面显示在屏幕中央
            this.Top = (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2;

            //dataGridView_YX.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            //dataGridView_YX.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            Back_Date = 3900;
            YC_Set[1] = 3700;
            YC_Set[2] = 50;
            YC_Set[3] = 4680;
            YC_Set[4] = 4500;
            YC_Set[5] = 680;
            YC_Set[6] = 1000;
            YC_Set[7] = 200;
            YC_Set[8] = 400;
            U_set = 0x5200;
            I_set = 0x300;
            U_Adj_set1 = 0x4800;
            U_Adj_set2 = 0x5600;
            I_Adj_set1 = 0x800;
            I_Adj_set2 = 0x1800;
            if (File.Exists(strFilePath))
            {
                strSec = Path.GetFileNameWithoutExtension(strFilePath);
                //textB_COM.Text = ContentValue(strSec, "COM");
                //textB_IP.Text = ContentValue(strSec, "IP");

                textBox_SOENum.Text = ContentValue(strSec, "SOE终止条数");
            }
            else
            {
                MessageBox.Show("INI文件不存在", "提示！");
            }
        }

        private void Form_Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                strSec = Path.GetFileNameWithoutExtension(strFilePath);
                //           WritePrivateProfileString(strSec, "COM", //textB_COM.Text.Trim(), strFilePath);
                //          WritePrivateProfileString(strSec, "IP", textB_IP.Text.Trim(), strFilePath);

                WritePrivateProfileString(strSec, "SOE起始条数", textBox_SOEStartAddress.Text.Trim(), strFilePath);
                WritePrivateProfileString(strSec, "SOE终止条数", textBox_SOENum.Text.Trim(), strFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "提示！");
            }
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)//树图显示各个操作界面
        {
            BMS_Sn = ByteTostring(BMS_Ver_A1_Read[39]) + ByteTostring(BMS_Ver_A1_Read[38]) + ByteTostring(BMS_Ver_A1_Read[37]) + ByteTostring(BMS_Ver_A1_Read[36]) + ByteTostring(BMS_Ver_A1_Read[35]) + ByteTostring(BMS_Ver_A1_Read[34]) + ByteTostring(BMS_Ver_A1_Read[33]) + ByteTostring(BMS_Ver_A1_Read[32]);
            if (e.Node.Name == "节点1")
            {
                tabPage1.Parent = this.tabControl;
                //tabPage2.Parent = null;
                tabPage4.Parent = null;

                //tabPage6.Parent = null;
                flag_ClearDataShow = 1;
            }
            if (e.Node.Name == "节点2")
            {
                tabPage1.Parent = null;
                //tabPage2.Parent = this.tabControl;
                tabPage4.Parent = null;

                //tabPage6.Parent = null;
                flag_ClearDataShow = 2;
            }
            if (e.Node.Name == "节点3")
            {
                tabPage1.Parent = null;
                //tabPage2.Parent = null;
                // tabPage3.Parent = this.tabControl;
                tabPage4.Parent = null;
                // tabPage5.Parent = null;
                //tabPage6.Parent = null;
                flag_ClearDataShow = 3;
            }
            if (e.Node.Name == "节点4")
            {
                tabPage1.Parent = null;
                //tabPage2.Parent = null;
                // tabPage3.Parent = null;
                tabPage4.Parent = this.tabControl;
                //  tabPage5.Parent = null;
                //tabPage6.Parent = null;
                flag_ClearDataShow = 4;
            }
            if (e.Node.Name == "节点5")
            {
                tabPage1.Parent = null;
                //tabPage2.Parent = null;
                //  tabPage3.Parent = null;
                tabPage4.Parent = null;
                // tabPage5.Parent = this.tabControl;
                //tabPage6.Parent = null;
                flag_ClearDataShow = 5;
            }
            if (e.Node.Name == "节点6")
            {
                tabPage1.Parent = null;
                ////tabPage2.Parent = null;
                //  tabPage3.Parent = null;
                tabPage4.Parent = null;
                //  tabPage5.Parent = null;
                //tabPage6.Parent = this.tabControl;
                flag_ClearDataShow = 5;
            }
        }

        /// <summary>
        /// 公共参数设置区:输入10进制数字，前面和中间不能有空格
        /// </summary>      


        private void btn_Help_Click(object sender, EventArgs e)//帮助按键
        {
            string V_file_name = System.Windows.Forms.Application.StartupPath + @"\软件使用说明.pdf";
            try
            {
                System.Diagnostics.Process.Start(V_file_name);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }
        /// <summary>
        /// 功能操作区
        /// </summary>      
        private void btn_YX_Click(object sender, EventArgs e)//采集遥信并显示按键
        {
            AllButton_false();
            flag_btnclick = "遥信";
            /***************************纠错，保证参数文本不为空************************/
            Error_Check();
            if ((flag_error == 1) || (flag_error == 2) || (flag_error == 3) || (flag_error == 4) || (flag_error == 5) || (flag_error == 6) || (flag_error == 7) || (flag_error == 8) || (flag_error == 9))//flag_error可能会出现123456789错误
            {
                AllButton_true();
                return;
            }

            if (ReciviedRight == 0)                                               //如果接收数据正确，显示在桌面
                Message_show();
            while (flag_btnclick == "遥信")
            {
                ArrayForBMS();

                /*************************发送数据***************************************/
                SendData_ShowData();
                Com_Over_Time_Pro();
                /***************************接收数据并判断是否合格*********************************/
                RecData_ShowData_New();
                Time_Delay_MS(150);
                AllButton_true();
            }
            AllButton_true();
        }

        private void btn_YC_Click(object sender, EventArgs e)//采集 并显示按键
        {
            AllButton_false();
            flag_btnclick = "遥测";
            /***************************纠错，保证参数文本不为空************************/
            Error_Check();
            if ((flag_error == 1) || (flag_error == 2) || (flag_error == 3) || (flag_error == 4) || (flag_error == 5) || (flag_error == 6) || (flag_error == 7) || (flag_error == 8) || (flag_error == 9))//flag_error可能会出现123456789错误
            {
                AllButton_true();
                return;
            }
            /***************************组帧********************************************/
            while (flag_btnclick == "遥测")
            {
                //Time_Delay_MS(150);
                ArrayForBMS();

                /*************************发送数据***************************************/
                SendData_ShowData();
                Time_Delay_MS(500);
                /***************************接收数据并判断是否合格*********************************/
                RecData_ShowData_New();
                AllButton_true();
            }

            AllButton_true();
        }

        private void btn_SOE_Click(object sender, EventArgs e)//采集SOE并显示按键
        {
            AllButton_false();
            flag_btnclick = "召SOE";
            /***************************纠错，保证参数文本不为空************************/
            Error_Check();
            if ((flag_error == 1) || (flag_error == 2) || (flag_error == 3) || (flag_error == 4) || (flag_error == 5) || (flag_error == 6) || (flag_error == 7) || (flag_error == 8) || (flag_error == 9))//flag_error可能会出现123456789错误
            {
                AllButton_true();
                return;
            }
            /***************************组帧********************************************/
            ArrayForBMS();
            if (flag_error == 101)                                                   //flag_error可能会出现101错误
            {
                MessageBox.Show("标准模式或特殊模式不能设为关闭，请更正！", "提示！");//此错误只需要在遥控按键中添加
                AllButton_true();
                return;
            }
            ArrayForCaiJiQi();
            ArrayForZhuZhan();
            /*************************发送数据***************************************/
            SendData_ShowData();
            if (flag_error == 102)                                                  //flag_error可能会出现102错误（发送数据过程中检测到通讯中断）
            {
                AllButton_true();
                return;
            }
            /***************************接收数据并判断是否合格*********************************/
            ReciviedRight = RecData_ShowData();                                     //接收数据并判断是否合格
            if (flag_error == 103)                                                 //flag_error可能会出现103错误（接收数据过程中检测到通讯中断）
            {
                AllButton_true();
                return;
            }
            /**************************显示数据，显示操作结果*******************************/
            Result_Show();                                                        //弹出结果提示框，正确与否都回弹出
            if (ReciviedRight == 0)                                               //如果接收数据正确，显示在桌面
                Message_show();

            AllButton_true();
        }

        private void btn_ReadSettedData_Click(object sender, EventArgs e)//采集设置参数并显示按键
        {
            AllButton_false();
            flag_btnclick = "读取设置参数";
            /***************************纠错，保证参数文本不为空************************/
            Error_Check();
            if ((flag_error == 1) || (flag_error == 2) || (flag_error == 3) || (flag_error == 4) || (flag_error == 5) || (flag_error == 6) || (flag_error == 7) || (flag_error == 8) || (flag_error == 9))//flag_error可能会出现123456789错误
            {
                AllButton_true();
                return;
            }
            /***************************组帧********************************************/
            ArrayForBMS();
            if (flag_error == 101)                                                   //flag_error可能会出现101错误
            {
                MessageBox.Show("标准模式或特殊模式不能设为关闭，请更正！", "提示！");//此错误只需要在遥控按键中添加
                AllButton_true();
                return;
            }
            ArrayForCaiJiQi();
            ArrayForZhuZhan();
            /*************************发送数据***************************************/
            SendData_ShowData();
            if (flag_error == 102)                                                  //flag_error可能会出现102错误（发送数据过程中检测到通讯中断）
            {
                AllButton_true();
                return;
            }
            /***************************接收数据并判断是否合格*********************************/
            ReciviedRight = RecData_ShowData();                                     //接收数据并判断是否合格
            if (flag_error == 103)                                                 //flag_error可能会出现103错误（接收数据过程中检测到通讯中断）
            {
                AllButton_true();
                return;
            }
            /**************************显示数据，显示操作结果*******************************/
            Result_Show();                                                        //弹出结果提示框，正确与否都回弹出
            if (ReciviedRight == 0)                                               //如果接收数据正确，显示在桌面
                Message_show();

            AllButton_true();
        }



        private void btn_YK_Click(object sender, EventArgs e)//遥控按键
        {
            AllButton_false();
            flag_btnclick = "遥控";
            /***************************纠错，保证参数文本不为空************************/
            Error_Check();
            if ((flag_error == 1) || (flag_error == 2) || (flag_error == 3) || (flag_error == 4) || (flag_error == 5) || (flag_error == 6) || (flag_error == 7) || (flag_error == 8) || (flag_error == 9))//flag_error可能会出现123456789错误
            {
                AllButton_true();
                return;
            }
            /***************************组帧********************************************/
            ArrayForBMS();
            if (flag_error == 101)                                                   //flag_error可能会出现101错误
            {
                MessageBox.Show("标准模式或特殊模式不能设为关闭，请更正！", "提示！");//此错误只需要在遥控按键中添加
                AllButton_true();
                return;
            }
            ArrayForCaiJiQi();
            ArrayForZhuZhan();
            /*************************发送数据***************************************/
            SendData_ShowData();
            if (flag_error == 102)                                                  //flag_error可能会出现102错误（发送数据过程中检测到通讯中断）
            {
                AllButton_true();
                return;
            }
            /***************************接收数据并判断是否合格*********************************/
            ReciviedRight = RecData_ShowData();                                     //接收数据并判断是否合格
            if (flag_error == 103)                                                 //flag_error可能会出现103错误（接收数据过程中检测到通讯中断）
            {
                AllButton_true();
                return;
            }
            /**************************显示数据，显示操作结果*******************************/
            Result_Show();                                                        //弹出结果提示框，正确与否都回弹出
            if (ReciviedRight == 0)                                               //如果接收数据正确，显示在桌面
                Message_show();

            AllButton_true();
        }

        private void btn_YT_Click(object sender, EventArgs e)//遥调按键
        {
            AllButton_false();
            flag_btnclick = "遥调";
            /***************************纠错，保证参数文本不为空************************/
            Error_Check();
            if ((flag_error == 1) || (flag_error == 2) || (flag_error == 3) || (flag_error == 4) || (flag_error == 5) || (flag_error == 6) || (flag_error == 7) || (flag_error == 8) || (flag_error == 9))//flag_error可能会出现123456789错误
            {
                AllButton_true();
                return;
            }
            /***************************组帧********************************************/
            ArrayForBMS();
            if (flag_error == 101)                                                   //flag_error可能会出现101错误
            {
                MessageBox.Show("标准模式或特殊模式不能设为关闭，请更正！", "提示！");//此错误只需要在遥控按键中添加
                AllButton_true();
                return;
            }
            // ArrayForCaiJiQi();
            // ArrayForZhuZhan();
            /*************************发送数据***************************************/
            SendData_ShowData();
            if (flag_error == 102)                                                  //flag_error可能会出现102错误（发送数据过程中检测到通讯中断）
            {
                AllButton_true();
                return;
            }
            /***************************接收数据并判断是否合格*********************************/
            // ReciviedRight = RecData_ShowData();                                     //接收数据并判断是否合格
            if (flag_error == 103)                                                 //flag_error可能会出现103错误（接收数据过程中检测到通讯中断）
            {
                AllButton_true();
                return;
            }
            /**************************显示数据，显示操作结果*******************************/
            Result_Show();                                                        //弹出结果提示框，正确与否都回弹出
            if (ReciviedRight == 0)                                               //如果接收数据正确，显示在桌面
                Message_show();

            AllButton_true();
        }

        private void btn_ReadVersion_Click(object sender, EventArgs e)//读版本号按键
        {
            AllButton_false();
            flag_btnclick = "读版本号";
            /***************************纠错，保证参数文本不为空************************/
            Error_Check();
            if ((flag_error == 1) || (flag_error == 2) || (flag_error == 3) || (flag_error == 4) || (flag_error == 5) || (flag_error == 6) || (flag_error == 7) || (flag_error == 8) || (flag_error == 9))//flag_error可能会出现123456789错误
            {
                AllButton_true();
                return;
            }
            /***************************组帧********************************************/
            ArrayForBMS();
            if (flag_error == 101)                                                   //flag_error可能会出现101错误
            {
                MessageBox.Show("标准模式或特殊模式不能设为关闭，请更正！", "提示！");//此错误只需要在遥控按键中添加
                AllButton_true();
                return;
            }
            ArrayForCaiJiQi();
            ArrayForZhuZhan();
            /*************************发送数据***************************************/
            SendData_ShowData();
            if (flag_error == 102)                                                  //flag_error可能会出现102错误（发送数据过程中检测到通讯中断）
            {
                AllButton_true();
                return;
            }
            /***************************接收数据并判断是否合格*********************************/
            ReciviedRight = RecData_ShowData();                                     //接收数据并判断是否合格
            if (flag_error == 103)                                                 //flag_error可能会出现103错误（接收数据过程中检测到通讯中断）
            {
                AllButton_true();
                return;
            }
            /**************************显示数据，显示操作结果*******************************/
            Result_Show();                                                        //弹出结果提示框，正确与否都回弹出
            if (ReciviedRight == 0)                                               //如果接收数据正确，显示在桌面
                Message_show();

            AllButton_true();
        }

        private void btn_SetTotalEnergy_Click(object sender, EventArgs e)//设置总发电量按键
        {
            AllButton_false();
            flag_btnclick = "设置总电量";
            /***************************纠错，保证参数文本不为空************************/
            Error_Check();
            if ((flag_error == 1) || (flag_error == 2) || (flag_error == 3) || (flag_error == 4) || (flag_error == 5) || (flag_error == 6) || (flag_error == 7) || (flag_error == 8) || (flag_error == 9))//flag_error可能会出现123456789错误
            {
                AllButton_true();
                return;
            }
            /***************************组帧********************************************/
            ArrayForBMS();
            if (flag_error == 101)                                                   //flag_error可能会出现101错误
            {
                MessageBox.Show("标准模式或特殊模式不能设为关闭，请更正！", "提示！");//此错误只需要在遥控按键中添加
                AllButton_true();
                return;
            }
            ArrayForCaiJiQi();
            ArrayForZhuZhan();
            /*************************发送数据***************************************/
            SendData_ShowData();
            if (flag_error == 102)                                                  //flag_error可能会出现102错误（发送数据过程中检测到通讯中断）
            {
                AllButton_true();
                return;
            }
            /***************************接收数据并判断是否合格*********************************/
            ReciviedRight = RecData_ShowData();                                     //接收数据并判断是否合格
            if (flag_error == 103)                                                 //flag_error可能会出现103错误（接收数据过程中检测到通讯中断）
            {
                AllButton_true();
                return;
            }
            /**************************显示数据，显示操作结果*******************************/
            Result_Show();                                                        //弹出结果提示框，正确与否都回弹出
            if (ReciviedRight == 0)                                               //如果接收数据正确，显示在桌面
                Message_show();

            AllButton_true();
        }

        private void btn_ReadTime_Click(object sender, EventArgs e)//读取设备时钟按键
        {
            AllButton_false();
            flag_btnclick = "读设备时钟";
            /***************************纠错，保证参数文本不为空************************/
            Error_Check();
            if ((flag_error == 1) || (flag_error == 2) || (flag_error == 3) || (flag_error == 4) || (flag_error == 5) || (flag_error == 6) || (flag_error == 7) || (flag_error == 8) || (flag_error == 9))//flag_error可能会出现123456789错误
            {
                AllButton_true();
                return;
            }
            /***************************组帧********************************************/
            ArrayForBMS();
            if (flag_error == 101)                                                   //flag_error可能会出现101错误
            {
                MessageBox.Show("标准模式或特殊模式不能设为关闭，请更正！", "提示！");//此错误只需要在遥控按键中添加
                AllButton_true();
                return;
            }
            ArrayForCaiJiQi();
            ArrayForZhuZhan();
            /*************************发送数据***************************************/
            SendData_ShowData();
            if (flag_error == 102)                                                  //flag_error可能会出现102错误（发送数据过程中检测到通讯中断）
            {
                AllButton_true();
                return;
            }
            /***************************接收数据并判断是否合格*********************************/
            ReciviedRight = RecData_ShowData();                                     //接收数据并判断是否合格
            if (flag_error == 103)                                                 //flag_error可能会出现103错误（接收数据过程中检测到通讯中断）
            {
                AllButton_true();
                return;
            }
            /**************************显示数据，显示操作结果*******************************/
            Result_Show();                                                        //弹出结果提示框，正确与否都回弹出
            if (ReciviedRight == 0)                                               //如果接收数据正确，显示在桌面
                Message_show();

            AllButton_true();
        }

        private void btn_ReadTotalEnergy_Click(object sender, EventArgs e)//读取总发电量按键
        {
            AllButton_false();
            flag_btnclick = "读总电量";
            /***************************纠错，保证参数文本不为空************************/
            Error_Check();
            if ((flag_error == 1) || (flag_error == 2) || (flag_error == 3) || (flag_error == 4) || (flag_error == 5) || (flag_error == 6) || (flag_error == 7) || (flag_error == 8) || (flag_error == 9))//flag_error可能会出现123456789错误
            {
                AllButton_true();
                return;
            }
            /***************************组帧********************************************/
            ArrayForBMS();
            if (flag_error == 101)                                                   //flag_error可能会出现101错误
            {
                MessageBox.Show("标准模式或特殊模式不能设为关闭，请更正！", "提示！");//此错误只需要在遥控按键中添加
                AllButton_true();
                return;
            }
            ArrayForCaiJiQi();
            ArrayForZhuZhan();
            /*************************发送数据***************************************/
            SendData_ShowData();
            if (flag_error == 102)                                                  //flag_error可能会出现102错误（发送数据过程中检测到通讯中断）
            {
                AllButton_true();
                return;
            }
            /***************************接收数据并判断是否合格*********************************/
            ReciviedRight = RecData_ShowData();                                     //接收数据并判断是否合格
            if (flag_error == 103)                                                 //flag_error可能会出现103错误（接收数据过程中检测到通讯中断）
            {
                AllButton_true();
                return;
            }
            /**************************显示数据，显示操作结果*******************************/
            Result_Show();                                                        //弹出结果提示框，正确与否都回弹出
            if (ReciviedRight == 0)                                               //如果接收数据正确，显示在桌面
                Message_show();

            AllButton_true();
        }

        private void btn_ReadDayEnergy_Click(object sender, EventArgs e)//读取日发电量按键
        {
            AllButton_false();
            flag_btnclick = "召日发电量";
            /***************************纠错，保证参数文本不为空************************/
            Error_Check();
            if ((flag_error == 1) || (flag_error == 2) || (flag_error == 3) || (flag_error == 4) || (flag_error == 5) || (flag_error == 6) || (flag_error == 7) || (flag_error == 8) || (flag_error == 9))//flag_error可能会出现123456789错误
            {
                AllButton_true();
                return;
            }
            /***************************组帧********************************************/
            ArrayForBMS();
            if (flag_error == 101)                                                   //flag_error可能会出现101错误
            {
                MessageBox.Show("标准模式或特殊模式不能设为关闭，请更正！", "提示！");//此错误只需要在遥控按键中添加
                AllButton_true();
                return;
            }
            ArrayForCaiJiQi();
            ArrayForZhuZhan();
            /*************************发送数据***************************************/
            SendData_ShowData();
            if (flag_error == 102)                                                  //flag_error可能会出现102错误（发送数据过程中检测到通讯中断）
            {
                AllButton_true();
                return;
            }
            /***************************接收数据并判断是否合格*********************************/
            ReciviedRight = RecData_ShowData();                                     //接收数据并判断是否合格
            if (flag_error == 103)                                                 //flag_error可能会出现103错误（接收数据过程中检测到通讯中断）
            {
                AllButton_true();
                return;
            }
            /**************************显示数据，显示操作结果*******************************/
            Result_Show();                                                        //弹出结果提示框，正确与否都回弹出
            if (ReciviedRight == 0)                                               //如果接收数据正确，显示在桌面
                Message_show();

            AllButton_true();
        }

        private void btn_ClearData_Click(object sender, EventArgs e)//清除数据信息按键
        {
            AllButton_false();
            flag_btnclick = "清除数据信息";
            /***************************纠错，保证参数文本不为空************************/
            Error_Check();
            if ((flag_error == 1) || (flag_error == 2) || (flag_error == 3) || (flag_error == 4) || (flag_error == 5) || (flag_error == 6) || (flag_error == 7) || (flag_error == 8) || (flag_error == 9))//flag_error可能会出现123456789错误
            {
                AllButton_true();
                return;
            }
            /***************************组帧********************************************/
            ArrayForBMS();
            if (flag_error == 101)                                                   //flag_error可能会出现101错误
            {
                MessageBox.Show("标准模式或特殊模式不能设为关闭，请更正！", "提示！");//此错误只需要在遥控按键中添加
                AllButton_true();
                return;
            }
            ArrayForCaiJiQi();
            ArrayForZhuZhan();
            /*************************发送数据***************************************/
            SendData_ShowData();
            if (flag_error == 102)                                                  //flag_error可能会出现102错误（发送数据过程中检测到通讯中断）
            {
                AllButton_true();
                return;
            }
            /***************************接收数据并判断是否合格*********************************/
            ReciviedRight = RecData_ShowData();                                     //接收数据并判断是否合格
            if (flag_error == 103)                                                 //flag_error可能会出现103错误（接收数据过程中检测到通讯中断）
            {
                AllButton_true();
                return;
            }
            /**************************显示数据，显示操作结果*******************************/
            Result_Show();                                                        //弹出结果提示框，正确与否都回弹出
            if (ReciviedRight == 0)                                               //如果接收数据正确，显示在桌面
                Message_show();

            AllButton_true();
        }

        private void btn_ClearConfig_Click(object sender, EventArgs e)//清除配置信息按键
        {
            AllButton_false();
            flag_btnclick = "清除配置信息";
            /***************************纠错，保证参数文本不为空************************/
            Error_Check();
            if ((flag_error == 1) || (flag_error == 2) || (flag_error == 3) || (flag_error == 4) || (flag_error == 5) || (flag_error == 6) || (flag_error == 7) || (flag_error == 8) || (flag_error == 9))//flag_error可能会出现123456789错误
            {
                AllButton_true();
                return;
            }
            /***************************组帧********************************************/
            ArrayForBMS();
            if (flag_error == 101)                                                   //flag_error可能会出现101错误
            {
                MessageBox.Show("标准模式或特殊模式不能设为关闭，请更正！", "提示！");//此错误只需要在遥控按键中添加
                AllButton_true();
                return;
            }
            ArrayForCaiJiQi();
            ArrayForZhuZhan();
            /*************************发送数据***************************************/
            SendData_ShowData();
            if (flag_error == 102)                                                  //flag_error可能会出现102错误（发送数据过程中检测到通讯中断）
            {
                AllButton_true();
                return;
            }
            /***************************接收数据并判断是否合格*********************************/
            ReciviedRight = RecData_ShowData();                                     //接收数据并判断是否合格
            if (flag_error == 103)                                                 //flag_error可能会出现103错误（接收数据过程中检测到通讯中断）
            {
                AllButton_true();
                return;
            }
            /**************************显示数据，显示操作结果*******************************/
            Result_Show();                                                        //弹出结果提示框，正确与否都回弹出
            if (ReciviedRight == 0)                                               //如果接收数据正确，显示在桌面
                Message_show();

            AllButton_true();
        }

        private void btn_ClearRichTextBox_Click(object sender, EventArgs e)//清空报文区
        {

            textBox_fasong.Text = "";
            Form1_Load(null, null);
        }

        private void btn_ClearDataShow_Click(object sender, EventArgs e)//清空显示的数据
        {
            switch (flag_ClearDataShow)
            {
                case 1:
                    //dataGridView_YX.DataSource = new List<Fruit>() {
                    //new Fruit(){名字1 = "BMS运行状态" ,      状态1  = 65536.ToString (), 名字2 = "C4低压保护", 状态2 = 65536.ToString (), 名字3 = "C8高压告警",     状态3 = 65536.ToString (), 名字4 = "C12高压保护", 状态4 =65536.ToString (),名字5 = "充电测温点2低温保护" ,          状态5  = 65536.ToString (), 名字6 = "环境温度低保护",       状态6 = 65536.ToString ()},
                    //new Fruit(){名字1 = "C1低压告警" ,       状态1  = 65536.ToString (), 名字2 = "C5低压保护", 状态2 =65536.ToString (), 名字3 = "C9高压告警",      状态3 = 65536.ToString (), 名字4 = "电池组低压告警", 状态4 = 65536.ToString (),名字5 = "放测温点1高温告警" ,          状态5  =65536.ToString (), 名字6 = "通讯故障",       状态6 = 65536.ToString ()},
                    //new Fruit(){名字1 = "C2低压告警" ,       状态1  = 65536.ToString (), 名字2 = "C6低压保护", 状态2 = 65536.ToString (), 名字3 = "C10高压告警",    状态3 = 65536.ToString (), 名字4 = "电池组高压告警", 状态4 =65536.ToString (),名字5 = "放测温点1高温保护" ,          状态5  =65536.ToString (), 名字6 = "I2C故障",           状态6 = 65536.ToString ()},
                    //new Fruit(){名字1 = "C3低压告警" ,       状态1  = 65536.ToString (), 名字2 = "C7低压保护", 状态2 = 65536.ToString (), 名字3 = "C11高压告警",    状态3 =65536.ToString (), 名字4 = "电池组高压保护", 状态4 = 65536.ToString (),名字5 = "放测温点1低温告警" ,        状态5  = 65536.ToString (), 名字6 = "MOS管故障",       状态6 = 65536.ToString ()},
                    //new Fruit(){名字1 = "C低压告警" ,        状态1  = 65536.ToString (), 名字2 = "C8低压保护", 状态2 =65536.ToString (), 名字3 = "C12高压告警",     状态3 = 65536.ToString (), 名字4 = "电池组低压保护", 状态4 = 65536.ToString (),名字5 = "放测温点1低温保护" ,         状态5  = 65536.ToString (), 名字6 = "电池管理芯片故障",           状态6 =65536.ToString ()},
                    //new Fruit(){名字1 = "C5低压告警" ,       状态1  = 65536.ToString (), 名字2 = "C9低压保护", 状态2 = 65536.ToString (), 名字3 = "C1高压保护",     状态3 = 65536.ToString (), 名字4 = "电池充电过流告警", 状态4 = 65536.ToString (),名字5 = "放测温点2高温告警" ,         状态5  =65536.ToString (), 名字6 = "存储故障",      状态6 = 65536.ToString ()},
                    //new Fruit(){名字1 = "C6低压告警" ,       状态1  = 65536.ToString (), 名字2 = "C10低压保护",状态2 =65536.ToString (), 名字3 = "C2高压保护",      状态3 =65536.ToString (), 名字4 = "电池充电过流保护", 状态4 = 65536.ToString (),名字5 = "放测温点2高温保护" ,     状态5  = 65536.ToString (), 名字6 = "采样故障",      状态6 = 65536.ToString ()},
                    //new Fruit(){名字1 = "C7低压告警" ,       状态1  =  65536.ToString (), 名字2 = "C11低压保护",  状态2 = 65536.ToString (), 名字3 = "C3高压保护",  状态3 = 65536.ToString (), 名字4 = "电池放电过流告警", 状态4 = 65536.ToString (),名字5 = "放测温点2低温告警" ,     状态5  = 65536.ToString (), 名字6 = "时钟故障",      状态6 = 65536.ToString ()},
                    //new Fruit(){名字1 = "C低压告警" ,        状态1  =  65536.ToString (), 名字2 = "C12低压保护",  状态2 = 65536.ToString (), 名字3 = "C4高压保护",  状态3 = 65536.ToString (), 名字4 = "电池放电过流保护", 状态4 =65536.ToString (),名字5 = "放测温点2低温保护" ,  状态5  = 65536.ToString (), 名字6 = "充电允许标志",       状态6 = 65536.ToString ()},
                    //new Fruit(){名字1 = "C9低压告警" ,       状态1  = 65536.ToString (), 名字2 = "C1高压告警",  状态2 =65536.ToString (), 名字3 = "C5高压保护",     状态3 = 65536.ToString (), 名字4 = "充电测温点1高温告警", 状态4 =65536.ToString (),名字5 = "左侧均衡板高温告警" ,  状态5  = 65536.ToString (), 名字6 = "充电结束标志",       状态6 = 65536.ToString ()},
                    //new Fruit(){名字1 = "C10低压告警" ,      状态1  =  65536.ToString (), 名字2 = "C2高压告警", 状态2 = 65536.ToString (), 名字3 = "C6高压保护",    状态3 =65536.ToString (), 名字4 = "充电测温点1高温保护", 状态4 = 65536.ToString (),名字5 = "左侧均衡板高温保护" ,  状态5  = 65536.ToString ()},
                    //new Fruit(){名字1 = "C11低压告警" ,      状态1  =  65536.ToString (), 名字2 = "C3高压告警", 状态2 = 65536.ToString (), 名字3 = "C7高压保护",    状态3 = 65536.ToString (), 名字4 = "充电测温点1低温告警", 状态4 = 65536.ToString (),名字5 = "右侧均衡板高温告警" ,  状态5  = 65536.ToString ()},
                    //new Fruit(){名字1 = "C12低压告警" ,      状态1  =  65536.ToString (), 名字2 = "C4高压告警", 状态2 =65536.ToString (), 名字3 = "C8高压保护",     状态3 = 65536.ToString (), 名字4 = "充电测温点1低温保护", 状态4 = 65536.ToString (),名字5 = "右侧均衡板高温保护" ,     状态5  = 65536.ToString (), },
                    //new Fruit(){名字1 = "C1低压保护" ,       状态1  = 65536.ToString (), 名字2 = "C5高压告警",  状态2 = 65536.ToString (), 名字3 = "C9高压保护",    状态3 = 65536.ToString (), 名字4 = "充电测温点2高温告警", 状态4 = 65536.ToString (),名字5 = "环境温度高告警" ,      状态5  = 65536.ToString (), },
                    //new Fruit(){名字1 = "C2低压保护" ,       状态1  = 65536.ToString (), 名字2 = "C6高压告警",  状态2 =65536.ToString (), 名字3 = "C10高压保护",    状态3 = 65536.ToString (), 名字4 = "充电测温点2高温保护", 状态4 = 65536.ToString (),名字5 = "环境温度高保护" ,      状态5  = 65536.ToString (),},
                    //new Fruit(){名字1 = "C3低压保护" ,       状态1  = 65536.ToString (), 名字2 = "C7高压告警",  状态2 = 65536.ToString (), 名字3 = "C11高压保护",   状态3 = 65536.ToString (), 名字4 = "充电测温点2低温告警", 状态4 = 65536.ToString (),名字5 = "环境温度低告警" , 状态5  = 65536.ToString ()}};
                    break;
                case 2:

                    //  new Fruit(){名字1 = "CELL1电池电压" , 状态1  = ((double)GetYC(Data_Rec[0],Data_Rec[1])).ToString ("F1"),  名字2 = "CELL7电池电压", 状态2 = ((double)GetYC(Data_Rec[54],Data_Rec[55])/100).ToString ("F2")},
                    break;
                case 4:
                    dataGridView_SOE.Rows.Clear();
                    dataGridView_SOE.DataSource = null;
                    break;
                case 5:
                //dataGridView_SettedData.DataSource = new List<Fruit>() {
                //new Fruit(){名字1 = "标准/特殊保护模式" , 状态1  = 65535.ToString (), 名字2 = "预留4",          状态2 = 65535.ToString (), 名字3 = "频率保护最小值",       状态3 = 65535.ToString (), 名字4 = "母线电压校准系数1",     状态4 = 65535.ToString (),名字5 = "DSP交流B电流校准系数1" , 状态5  = 65535.ToString (), 名字6 = "ARM交流B电压校准系数1", 状态6 = 65535.ToString (), 名字7 = "预留3", 状态7 = 65535.ToString (), 名字8 = "预留13", 状态8 = 65535.ToString ()},
                //new Fruit(){名字1 = "限功率开关" ,        状态1  = 65535.ToString (), 名字2 = "预留5",          状态2 = 65535.ToString (), 名字3 = "漏电流增加限值",       状态3 = 65535.ToString (), 名字4 = "母线电压校准系数2",     状态4 = 65535.ToString (),名字5 = "DSP交流B电流校准系数2" , 状态5  = 65535.ToString (), 名字6 = "ARM交流B电压校准系数2", 状态6 = 65535.ToString (), 名字7 = "预留4", 状态7 = 65535.ToString (), 名字8 = "预留14", 状态8 = 65535.ToString ()},
                //new Fruit(){名字1 = "逆变器启停" ,        状态1  = 65535.ToString (), 名字2 = "预留6",          状态2 = 65535.ToString (), 名字3 = "直流1侧电压校准系数1", 状态3 = 65535.ToString (), 名字4 = "母线电流校准系数1",     状态4 = 65535.ToString (),名字5 = "DSP交流C电压校准系数1" , 状态5  = 65535.ToString (), 名字6 = "ARM交流B电流校准系数1", 状态6 = 65535.ToString (), 名字7 = "预留5", 状态7 = 65535.ToString (), 名字8 = "预留15", 状态8 = 65535.ToString ()},
                //new Fruit(){名字1 = "MPPT开关" ,          状态1  = 65535.ToString (), 名字2 = "预留7",          状态2 = 65535.ToString (), 名字3 = "直流1侧电压校准系数2", 状态3 = 65535.ToString (), 名字4 = "母线电流校准系数2",     状态4 = 65535.ToString (),名字5 = "DSP交流C电压校准系数2" , 状态5  = 65535.ToString (), 名字6 = "ARM交流B电流校准系数2", 状态6 = 65535.ToString (), 名字7 = "预留6", 状态7 = 65535.ToString (), 名字8 = "预留16", 状态8 = 65535.ToString ()},
                //new Fruit(){名字1 = "孤岛开关" ,          状态1  = 65535.ToString (), 名字2 = "预留8",          状态2 = 65535.ToString (), 名字3 = "直流1侧电流校准系数1", 状态3 = 65535.ToString (), 名字4 = "DSP交流A电压校准系数1", 状态4 = 65535.ToString (),名字5 = "DSP交流C电流校准系数1" , 状态5  = 65535.ToString (), 名字6 = "ARM交流C电压校准系数1", 状态6 = 65535.ToString (), 名字7 = "预留7", 状态7 = 65535.ToString (), 名字8 = "预留17", 状态8 = 65535.ToString ()},
                //new Fruit(){名字1 = "绝缘阻抗检测开关" ,  状态1  = 65535.ToString (), 名字2 = "限功率输出",     状态2 = 65535.ToString (), 名字3 = "直流1侧电流校准系数2", 状态3 = 65535.ToString (), 名字4 = "DSP交流A电压校准系数2", 状态4 = 65535.ToString (),名字5 = "DSP交流C电流校准系数2" , 状态5  = 65535.ToString (), 名字6 = "ARM交流C电压校准系数2", 状态6 = 65535.ToString (), 名字7 = "预留8", 状态7 = 65535.ToString (), 名字8 = "预留18", 状态8 = 65535.ToString ()},
                //new Fruit(){名字1 = "计量板开关" ,        状态1  = 65535.ToString (), 名字2 = "功率因数",       状态2 = 65535.ToString (), 名字3 = "直流2侧电压校准系数1", 状态3 = 65535.ToString (), 名字4 = "DSP交流A电流校准系数1", 状态4 = 65535.ToString (),名字5 = "ARM交流A电压校准系数1" , 状态5  = 65535.ToString (), 名字6 = "ARM交流C电压校准系数1", 状态6 = 65535.ToString (), 名字7 = "预留9", 状态7 = 65535.ToString (), 名字8 = "预留19", 状态8 = 65535.ToString ()},
                //new Fruit(){名字1 = "预留1" ,             状态1  = 65535.ToString (), 名字2 = "电压保护最大值", 状态2 = 65535.ToString (), 名字3 = "直流2侧电压校准系数2", 状态3 = 65535.ToString (), 名字4 = "DSP交流A电流校准系数2", 状态4 = 65535.ToString (),名字5 = "ARM交流A电压校准系数2" , 状态5  = 65535.ToString (), 名字6 = "ARM交流C电流校准系数2", 状态6 = 65535.ToString (), 名字7 = "预留10", 状态7 = 65535.ToString (), 名字8 = "预留20", 状态8 = 65535.ToString ()},
                //new Fruit(){名字1 = "预留2" ,             状态1  = 65535.ToString (), 名字2 = "电压保护最小值", 状态2 = 65535.ToString (), 名字3 = "直流2侧电流校准系数1", 状态3 = 65535.ToString (), 名字4 = "DSP交流B电压校准系数1", 状态4 = 65535.ToString (),名字5 = "ARM交流A电流校准系数1" , 状态5  = 65535.ToString (), 名字6 = "预留1",                 状态6 = 65535.ToString (), 名字7 = "预留11", 状态7 = 65535.ToString (), 名字8 = "预留21", 状态8 = 65535.ToString ()},
                //new Fruit(){名字1 = "预留3" ,             状态1  = 65535.ToString (), 名字2 = "频率保护最大值", 状态2 = 65535.ToString (), 名字3 = "直流2侧电流校准系数2", 状态3 = 65535.ToString (), 名字4 = "DSP交流B电压校准系数2", 状态4 = 65535.ToString (),名字5 = "ARM交流A电流校准系数2" , 状态5  = 65535.ToString (), 名字6 = "预留2",                 状态6 = 65535.ToString (), 名字7 = "预留12", 状态7 = 65535.ToString (), 名字8 = "预留22", 状态8 = 65535.ToString ()}};
                //break;
                case 6:
                    //textBox_Version.Text = "";
                    //textBox_ReadTime.Text = "";
                    // textBox_Totle.Text = "";
                    //textBox_Day.Text = "";
                    break;
            }
            //textBox8.ForeColor = System.Drawing.Color.Black;
            //textBox11.ForeColor = System.Drawing.Color.Black;
            //textBox6.ForeColor = System.Drawing.Color.Black;
            //textBox7.ForeColor = System.Drawing.Color.Black;
            //textBox8.Text = "待检测";
            //textBox11.Text = "待检测";
            //textBox6.Text = "待检测";
            //textBox7.Text = "待检测";
            //textBox9.Text = "V0.0-00000000";
            //textBox10.Text = "V0.00-00000000";
            //textBox12.Text = "V0.0-00000000";
            //textBox13.Text = "V0.00-00000000";

        }

        public void Time_Delay_S(int s)//S级延时函数
        {
            var timeout = DateTime.Now.AddSeconds(s);
            while (true)
            {
                System.Windows.Forms.Application.DoEvents();
                if (DateTime.Now >= timeout)
                    return;
            }
        }
        /// <summary>
        /// 延时ms级
        /// </summary>
        /// <param name="s">延时多少毫秒</param> 
        public void Time_Delay_MS(int ms)
        {
            var timeout = DateTime.Now.AddMilliseconds(ms);
            while (true)
            {
                System.Windows.Forms.Application.DoEvents();
                if (DateTime.Now >= timeout)
                    return;
            }
        }


        private void serialPort_7112_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

        }

        private void Button_serialport_OPENorCLOSE_Click(object sender, EventArgs e)
        {

            if (this.button_serialport_OPENorCLOSE.Text == "打开串口")
            {
                switch (this.comboBox3.Text)
                {
                    case "4800":
                        this.serialPort_7112.BaudRate = 4800;
                        break;
                    case "9600":
                        this.serialPort_7112.BaudRate = 9600;
                        break;
                    case "19200":
                        this.serialPort_7112.BaudRate = 19200;
                        break;
                    case "57600":
                        this.serialPort_7112.BaudRate = 57600;
                        break;
                    case "115200":
                        this.serialPort_7112.BaudRate = 1152000;
                        break;
                    default: break;
                }
                switch (comboBox2.Text)
                {
                    case "None":
                        serialPort_7112.Parity = Parity.None;
                        break;
                    case "Odd":
                        serialPort_7112.Parity = Parity.Odd;
                        break;
                    case "Even":
                        serialPort_7112.Parity = Parity.Even;
                        break;
                    case "Mark":
                        serialPort_7112.Parity = Parity.Mark;
                        break;
                    case "Space":
                        serialPort_7112.Parity = Parity.Space;
                        break;
                    default: break;
                }
                switch (comboBox1.Text)
                {
                    case "com1":
                        serialPort_7112.PortName = "com1";
                        break;
                    case "com2":
                        serialPort_7112.PortName = "com2";
                        break;
                    case "com3":
                        serialPort_7112.PortName = "com3";
                        break;
                    case "com4":
                        serialPort_7112.PortName = "com4";
                        break;
                    case "com5":
                        serialPort_7112.PortName = "com5";
                        break;
                    case "com6":
                        serialPort_7112.PortName = "com6";
                        break;
                    case "com7":
                        serialPort_7112.PortName = "com7";
                        break;
                    case "com8":
                        serialPort_7112.PortName = "com8";
                        break;
                    case "com9":
                        serialPort_7112.PortName = "com9";
                        break;
                    default: break;
                }
                switch (comboBox4.Text)
                {/*
                        case "500ms":
                            Timeout = 500;
                            break;
                        case "800ms":
                            Timeout = 800;
                            break;
                        case "1s":
                            Timeout = 1000;
                            break;
                        case "2s":
                            Timeout = 2000;
                            break;
                        case "3s":
                            Timeout = 3000;
                            break;*/
                    default: break;
                }
              //  serialPort_7112.DataReceived += serialPort_7112_DataReceived;
                try
                {
                    serialPort_7112.Open();
                }
                catch
                {
                 //   serialPort_7112.DataReceived -= serialPort_7112_DataReceived;
                    MessageBox.Show("串口<" + serialPort_7112.PortName + ">无法打开", "打开失败", MessageBoxButtons.OK);
                    return;
                }

                button_serialport_OPENorCLOSE.Text = "关闭串口";
                comboBox1.Enabled = false;
                comboBox2.Enabled = false;
                comboBox3.Enabled = false;
                comboBox4.Enabled = false;
                pictureBox2.Image = global::充电机电源板生产测试软件.Properties.Resources.green;

            }
            else if (serialPort_7112.IsOpen)
            {
               // serialPort_7112.DataReceived -= serialPort_7112_DataReceived;
                serialPort_7112.Close();

                button_serialport_OPENorCLOSE.Text = "打开串口";
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
                comboBox3.Enabled = true;
                comboBox4.Enabled = true;
                pictureBox2.Image = global::充电机电源板生产测试软件.Properties.Resources.red;
            }
            else
            {
                pictureBox2.Image = global::充电机电源板生产测试软件.Properties.Resources.black;
            }
        }

        private void Label5_Click(object sender, EventArgs e)
        {

        }

        private void TextBox_FeiKong_TextChanged(object sender, EventArgs e)
        {

        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void TextBox_fasong_TextChanged(object sender, EventArgs e)
        {



        }

        private void TextBox_jieshou_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBox_YCNum_TextChanged(object sender, EventArgs e)
        {

        }

        private void DataGridView_SOE_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void CombB_YKName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            System.Timers.Timer pTimer = new System.Timers.Timer(15000);//每隔5秒执行一次，没用winfrom自带的
            pTimer.Elapsed += pTimer_Elapsed;//委托，要执行的方法
            pTimer.AutoReset = true;//获取该定时器自动执行
            pTimer.Enabled = true;//这个一定要写，要不然定时器不会执行的
            Control.CheckForIllegalCrossThreadCalls = false;//这个不太懂，有待研究

        }

        private void pTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //button1_Click(null, null);
            if (flag_btnclick == "遥测")
            {


                /*   ArrayForBMS();

                   /*************************发送数据***************************************
                   SendData_ShowData();
                   if (flag_error == 102)                                                  //flag_error可能会出现102错误（发送数据过程中检测到通讯中断）
                   {
                       AllButton_true();
                   }
                   /***************************接收数据并判断是否合格*********************************
                   RecData_ShowData_New();

                   AllButton_true(); */
                this.btn_YC_Click(null, null);


            }
            else if (flag_btnclick == "遥信")
            {
                //  btn_YX_Click(null, null);
            }

        }

        private void TextBox_YXNum_TextChanged(object sender, EventArgs e)
        {

        }

        private void Label4_Click(object sender, EventArgs e)
        {

        }

        private void Label15_Click(object sender, EventArgs e)
        {

        }

        private void Label50_Click(object sender, EventArgs e)
        {
        }

        private void Label67_Click(object sender, EventArgs e)
        {

        }

        private void Label9_Click(object sender, EventArgs e)
        {

        }

        private void Label68_Click(object sender, EventArgs e)
        {

        }

        private void Label65_Click(object sender, EventArgs e)
        {

        }

        private void Label21_Click(object sender, EventArgs e)
        {

        }

        private void TextBox_CellV(object sender, EventArgs e)
        {
            //TextBox_CellV.
        }

        private void TextBox4_TextChanged(object sender, EventArgs e)
        {
        }

        private void TextBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBox_Totle_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBox_TotleEnergy_TextChanged(object sender, EventArgs e)
        {

        }



        private void Button13_Click(object sender, EventArgs e)
        {
            string Ver_buffer;
            AllButton_false();
            flag_btnclick = "读版本号";
            /***************************纠错，保证参数文本不为空************************/
            Error_Check();
            if ((flag_error == 1) || (flag_error == 2) || (flag_error == 3) || (flag_error == 4) || (flag_error == 5) || (flag_error == 6) || (flag_error == 7) || (flag_error == 8) || (flag_error == 9))//flag_error可能会出现123456789错误
            {
                AllButton_true();
                return;
            }
            ArrayForBMS();
            SendData_ShowData();
            Com_Over_Time_Pro();
            RecData_ShowData_New();

            textBox12.Text = "";
            textBox13.Text = "";
            if (Ver[0].ToString() == "Y" && Ver[1].ToString() == "T")
            {
                //textBox9.Text += "V";
                //textBox9.Text += Ver[6].ToString();
                //textBox9.Text += ".";
                //textBox9.Text += Ver[7].ToString();
                //textBox9.Text += "-";
                //textBox9.Text += "20";
                //textBox9.Text += Ver[10].ToString();
                //textBox9.Text += Ver[11].ToString();
                //textBox9.Text += Ver[12].ToString();
                //textBox9.Text += Ver[13].ToString();
                //textBox9.Text += Ver[14].ToString();
                //textBox9.Text += Ver[15].ToString();

                //textBox10.Text += "V";
                //textBox10.Text += Ver[19].ToString();
                //textBox10.Text += ".";
                //textBox10.Text += Ver[20].ToString();
                //textBox10.Text += Ver[21].ToString();
                //textBox10.Text += "-";
                //textBox10.Text += Ver[23].ToString();
                //textBox10.Text += Ver[24].ToString();
                //textBox10.Text += Ver[25].ToString();
                //textBox10.Text += Ver[26].ToString();
                //textBox10.Text += Ver[28].ToString();
                //textBox10.Text += Ver[29].ToString();
                //textBox10.Text += Ver[30].ToString();
                //textBox10.Text += Ver[31].ToString();
            }
            else if (Ver[0].ToString() == "K" && Ver[1].ToString() == "E")
            {
                textBox12.Text += "V";
                textBox12.Text += Ver[6].ToString();
                textBox12.Text += ".";
                textBox12.Text += Ver[7].ToString();
                textBox12.Text += "-";
                textBox12.Text += "20";
                textBox12.Text += Ver[10].ToString();
                textBox12.Text += Ver[11].ToString();
                textBox12.Text += Ver[12].ToString();
                textBox12.Text += Ver[13].ToString();
                textBox12.Text += Ver[14].ToString();
                textBox12.Text += Ver[15].ToString();

                textBox13.Text += "V";
                textBox13.Text += Ver[19].ToString();
                textBox13.Text += ".";
                textBox13.Text += Ver[20].ToString();
                textBox13.Text += Ver[21].ToString();
                textBox13.Text += "-";
                textBox13.Text += Ver[23].ToString();
                textBox13.Text += Ver[24].ToString();
                textBox13.Text += Ver[25].ToString();
                textBox13.Text += Ver[26].ToString();
                textBox13.Text += Ver[28].ToString();
                textBox13.Text += Ver[29].ToString();
                textBox13.Text += Ver[30].ToString();
                textBox13.Text += Ver[31].ToString();
            }
            else
            {
                textBox12.Text += "V";
                textBox12.Text += 0.ToString();
                textBox12.Text += ".";
                textBox12.Text += 0.ToString();
                textBox12.Text += "-";
                textBox12.Text += "20";
                textBox12.Text += 0.ToString();
                textBox12.Text += 0.ToString();
                textBox12.Text += 0.ToString();
                textBox12.Text += 0.ToString();
                textBox12.Text += 0.ToString();
                textBox12.Text += 0.ToString();

                textBox13.Text += "V";
                textBox13.Text += 0.ToString();
                textBox13.Text += ".";
                textBox13.Text += 0.ToString();
                textBox13.Text += 0.ToString();
                textBox13.Text += "-";
                textBox13.Text += 0.ToString();
                textBox13.Text += 0.ToString();
                textBox13.Text += 0.ToString();
                textBox13.Text += 0.ToString();
                textBox13.Text += 0.ToString();
                textBox13.Text += 0.ToString();
                textBox13.Text += 0.ToString();
                textBox13.Text += 0.ToString();

                //textBox9.Text += "V";
                //textBox9.Text += 0.ToString();
                //textBox9.Text += ".";
                //textBox9.Text += 0.ToString();
                //textBox9.Text += "-";
                //textBox9.Text += "20";
                //textBox9.Text += 0.ToString();
                //textBox9.Text += 0.ToString();
                //textBox9.Text += 0.ToString();
                //textBox9.Text += 0.ToString();
                //textBox9.Text += 0.ToString();
                //textBox9.Text += 0.ToString();

                //textBox10.Text += "V";
                //textBox10.Text += 0.ToString();
                //textBox10.Text += ".";
                //textBox10.Text += 0.ToString();
                //textBox10.Text += 0.ToString();
                //textBox10.Text += "-";
                //textBox10.Text += 0.ToString();
                //textBox10.Text += 0.ToString();
                //textBox10.Text += 0.ToString();
                //textBox10.Text += 0.ToString();
                //textBox10.Text += 0.ToString();
                //textBox10.Text += 0.ToString();
                //textBox10.Text += 0.ToString();
                //textBox10.Text += 0.ToString();
            }

            AllButton_true();
        }

        private void TextBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void Label23_Click(object sender, EventArgs e)
        {

        }

        private void Label24_Click(object sender, EventArgs e)
        {

        }

        private void Label25_Click(object sender, EventArgs e)
        {

        }

        private void TextBox13_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void Label66_Click(object sender, EventArgs e)
        {

        }

        private void GroupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            flag_btnclick = "自动模式";
            ArrayForBMS();
            SendData_ShowData();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            flag_btnclick = "维护模式";
            ArrayForBMS();
            SendData_ShowData();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  Para_Disy = Devce1[0];
            int i = 0;

            string strPath = Application.StartupPath + @"\Device.ini";
            string strRead = GetStringFromINI("设备型号", i.ToString(), strPath);

            Device1[0] = strRead;
            while (strRead != "")
            {
                i++;
                strRead = GetStringFromINI("设备型号", i.ToString(), strPath);
                Device1[i] = strRead;
            }
            Device1[i] = "null_RES";

            //switch (comboBox5.Text)
            //{
            //    case "默认参数":
            //        Para_Disy.Text = Device1[0];
            //        break;
            //    case "参数1":
            //        Para_Disy.Text = Device1[1];
            //        break;
            //    case "参数2":
            //        Para_Disy.Text = Device1[2];
            //        break;
            //    case "参数3":
            //        Para_Disy.Text = Device1[3];
            //        break;
            //    case "参数4":
            //        Para_Disy.Text = Device1[4];
            //        break;
            //    case "参数5":
            //        Para_Disy.Text = Device1[5];
            //        break;
            //    case "参数6":
            //        Para_Disy.Text = Device1[6];
            //        break;
            //    case "参数7":
            //        Para_Disy.Text = Device1[7];
            //        break;
            //    case "参数8":
            //        Para_Disy.Text = Device1[8];
            //        break;
            //    case "参数9":
            //        Para_Disy.Text = Device1[9];
            //        break;
            //    case "参数10":
            //        Para_Disy.Text = Device1[10];
            //        break;
            //    case "参数11":
            //        Para_Disy.Text = Device1[11];
            //        break;
            //    default:
            //        Para_Disy.Text = Device1[0];
            //        break;

            //}

            //
            //textBox17.Text = strRead;


        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox27_TextChanged(object sender, EventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {

        }

        //private void button14_Click_1(object sender, EventArgs e)
        //{
        //    dgvShowPPD.DataSource = new SpiderCenter.PPDController().SearchPPDModel(0);
        //}

        //private void button15_Click(object sender, EventArgs e)
        //{
        //    dataGridView2.DataSource = new SpiderCenter.PPDController().SearchPPDModel(10);
        //}

        //private void button16_Click(object sender, EventArgs e)
        //{
        //    dataGridView3.DataSource = new SpiderCenter.PPDController().SearchPPDModel(100);
        //}
        //private void tabControl2_SelectedIndexChanged(object sender, TabControlEventArgs e)
        //{
        //    if (e.TabPage == tabPage9)
        //    {
        //        dgvShowPPD.DataSource = new SpiderCenter.PPDController().SearchPPDModel(0);
        //    }
        //    if (e.TabPage == tabPage10)
        //    {
        //        dataGridView2.DataSource = new SpiderCenter.PPDController().SearchPPDModel(10);
        //    }
        //    if (e.TabPage == tabPage11)
        //    {
        //        dataGridView3.DataSource = new SpiderCenter.PPDController().SearchPPDModel(100);
        //    }

        //}

        //private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (tabControl2.SelectedTab == tabPage9)
        //    {
        //        Form_Main.Recorder_path = "Data Source = " + 电源生产测试软件.Form_Main.BMS_Sn + "故障记录" + ".db";
        //        dgvShowPPD.DataSource = new SpiderCenter.PPDController().SearchPPDModel(0);
        //    }
        //    if (tabControl2.SelectedTab == tabPage10)
        //    {
        //        Form_Main.Recorder_path = "Data Source = " + 电源生产测试软件.Form_Main.BMS_Sn + "充电数据记录" + ".db";
        //        dataGridView2.DataSource = new SpiderCenter.PPDController().SearchPPDModel(0);
        //    }
        //    if (tabControl2.SelectedTab == tabPage11)
        //    {
        //        Form_Main.Recorder_path = "Data Source = " + 电源生产测试软件.Form_Main.BMS_Sn + "放电数据记录" + ".db";
        //        dataGridView3.DataSource = new SpiderCenter.PPDController().SearchPPDModel(0);
        //    }
        //}

        private void 导出数据库ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 导入数据库ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripContainer1_TopToolStripPanel_Click(object sender, EventArgs e)
        {

        }

        private void button14_Click_2(object sender, EventArgs e)
        {
            AllButton_false();
            flag_btnclick = "读取故障记录长度";
            ArrayForBMS();
            SendData_ShowData();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
            if (Recorder_len > 1664)
            {
                Recorder_len = 1664;
            }
            for (int i = 0; i < Recorder_len; i++)
            {
                flag_btnclick = "读取故障记录数据";
                Recorder_num = (UInt16)(i + 1);
                ArrayForBMS();
                SendData_ShowData();
                Com_Over_Time_Pro();
                RecData_ShowData_New();
            }

            flag_btnclick = "读充电记录长度";
            ArrayForBMS();
            SendData_ShowData();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
            if (Recorder_len > 1728)
            {
                Recorder_len = 1728;
            }
            for (int i = 0; i < Recorder_len; i++)
            {
                flag_btnclick = "读充电记录数据";
                Recorder_num = (UInt16)(i + 1);
                ArrayForBMS();
                SendData_ShowData();
                Com_Over_Time_Pro();
                RecData_ShowData_New();
            }

            flag_btnclick = "读放电记录长度";
            ArrayForBMS();
            SendData_ShowData();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
            if (Recorder_len > 3584)
            {
                Recorder_len = 3584;
            }
            for (int i = 0; i < Recorder_len; i++)
            {
                flag_btnclick = "读放电记录数据";
                Recorder_num = (UInt16)(i + 1);
                ArrayForBMS();
                SendData_ShowData();
                Com_Over_Time_Pro();
                RecData_ShowData_New();
            }
            AllButton_true();
        }
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            int n = serialPort_7112.BytesToRead;
            if (n > 0)
            {
                Time_Delay_MS(150);
                n = serialPort_7112.BytesToRead;
            }
            else
            {
                return;
            }
            byte[] Com_Rc_Data1 = new byte[n];
            serialPort_7112.Read(Com_Rc_Data1, 0, n);
            Com_Rc_Data_Lenth = (UInt16)Com_Rc_Data1.Length;
            if (Com_Rc_Data_Lenth > Com_Rc_Data.Length)
            {
                Com_Rc_Data_Lenth = (UInt16)Com_Rc_Data.Length;
            }
            for (int i = 0; i < Com_Rc_Data1.Length; i++)
            {
                Com_Rc_Data[i] = Com_Rc_Data1[i];
            }
            if (Com_Rc_Data_Lenth > 3)
            {
                Flag_Com = 1;
            }
        }

        private void serialPort2_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            int n = 负载串口.BytesToRead;
            if (n > 0)
            {
                Time_Delay_MS(150);
                n = 负载串口.BytesToRead;
            }
            else
            {
                return;
            }
            byte[] Com_Rc_Data1 = new byte[n];
            负载串口.Read(Com_Rc_Data1, 0, n);
            Com_Rc_Data_Lenth = (UInt16)Com_Rc_Data1.Length;
            if (Com_Rc_Data_Lenth > Com_Rc_Data.Length)
            {
                Com_Rc_Data_Lenth = (UInt16)Com_Rc_Data.Length;
            }
            for (int i = 0; i < Com_Rc_Data1.Length; i++)
            {
                Com_Rc_Data[i] = Com_Rc_Data1[i];
            }
            if (Com_Rc_Data_Lenth > 3)
            {
                Flag_Com = 1;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            flag_btnclick = "进入校准模式";
            ArrayForBMS();
            SendData_ShowData();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            flag_btnclick = "退出校准模式";
            ArrayForBMS();
            SendData_ShowData();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            flag_btnclick = "维护模式";
            ArrayForBMS();
            SendData_ShowData();
            Com_Over_Time_Pro();
            RecData_ShowData_New();

            flag_btnclick = "进入校准模式";
            ArrayForBMS();
            SendData_ShowData();
            Com_Over_Time_Pro();
            RecData_ShowData_New();

            flag_btnclick = "恢复默认参数";
            ArrayForBMS();
            SendData_ShowData();
            Com_Over_Time_Pro();
            RecData_ShowData_New();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            flag_btnclick = "第一次给定电压";
            Back_Date = 0;
            byte[] a = StringToByte(textBox2.Text);
            if (a.Length == 1)
            {
                Back_Date = a[0];
            }
            else if (a.Length == 2)
            {
                Back_Date = a[0];
                Back_Date = (UInt16)((Back_Date << 8) + a[1]);
                Back_Date = Back_Date;
            }
            U_Adj_set1 = (UInt16)(Back_Date);
            ArrayForBMS();
            SendData_ShowData();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            flag_btnclick = "第一次实际电压";
            Back_Date = 0;
            byte[] a = StringToByte(textBox3.Text);
            if (a.Length == 1)
            {
                Back_Date = a[0];
            }
            else if (a.Length == 2)
            {
                Back_Date = a[0];
                Back_Date = (UInt16)((Back_Date << 8) + a[1]);
                Back_Date = Back_Date;
            }
            U_Samp1 = (UInt16)(Back_Date);
            ArrayForBMS();
            SendData_ShowData();
            Com_Over_Time_Pro();
            RecData_ShowData_New();

        }

        private void button9_Click(object sender, EventArgs e)
        {
            flag_btnclick = "第二次给定电压";
            Back_Date = 0;
            byte[] a = StringToByte(textBox4.Text);
            if (a.Length == 1)
            {
                Back_Date = a[0];
            }
            else if (a.Length == 2)
            {
                Back_Date = a[0];
                Back_Date = (UInt16)((Back_Date << 8) + a[1]);
                Back_Date = Back_Date;
            }
            U_Adj_set2 = (UInt16)(Back_Date);
            ArrayForBMS();
            SendData_ShowData();
            Com_Over_Time_Pro();
            RecData_ShowData_New();

        }

        private void button8_Click(object sender, EventArgs e)
        {
            flag_btnclick = "第二次实际电压";
            Back_Date = 0;
            byte[] a = StringToByte(textBox5.Text);
            if (a.Length == 1)
            {
                Back_Date = a[0];
            }
            else if (a.Length == 2)
            {
                Back_Date = a[0];
                Back_Date = (UInt16)((Back_Date << 8) + a[1]);
                Back_Date = Back_Date;
            }
            U_Samp2 = (UInt16)(Back_Date);
            ArrayForBMS();
            SendData_ShowData();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            flag_btnclick = "第一次给定电流";
            Back_Date = 0;
            byte[] a = StringToByte(textBox6.Text);
            if (a.Length == 1)
            {
                Back_Date = a[0];
            }
            else if (a.Length == 2)
            {
                Back_Date = a[0];
                Back_Date = (UInt16)((Back_Date << 8) + a[1]);
                Back_Date = Back_Date;
            }
            I_Adj_set1 = (UInt16)(Back_Date);
            ArrayForBMS();
            SendData_ShowData();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            flag_btnclick = "第一次实际电流";
            Back_Date = 0;
            byte[] a = StringToByte(textBox7.Text);
            if (a.Length == 1)
            {
                Back_Date = a[0];
            }
            else if (a.Length == 2)
            {
                Back_Date = a[0];
                Back_Date = (UInt16)((Back_Date << 8) + a[1]);
                Back_Date = Back_Date;
            }
            I_Samp1 = (UInt16)(Back_Date);
            ArrayForBMS();
            SendData_ShowData();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
        }

        private void button11_Click(object sender, EventArgs e)
        {

            flag_btnclick = "第二次给定电流";
            byte[] a = StringToByte(textBox8.Text);
            Back_Date = 0;
            if (a.Length == 1)
            {
                Back_Date = a[0];
            }
            else if (a.Length == 2)
            {
                Back_Date = a[0];
                Back_Date = (UInt16)((Back_Date << 8) + a[1]);
                Back_Date = Back_Date;
            }
            I_Adj_set2 = (UInt16)(Back_Date);
            ArrayForBMS();
            SendData_ShowData();
            Com_Over_Time_Pro();
            RecData_ShowData_New();

        }

        private void button10_Click(object sender, EventArgs e)
        {
            flag_btnclick = "第二次实际电流";

            byte[] a = StringToByte(textBox9.Text);
            Back_Date = 0;
            if (a.Length == 1)
            {
                Back_Date = a[0];
            }
            else if (a.Length == 2)
            {
                Back_Date = a[0];
                Back_Date = (UInt16)((Back_Date << 8) + a[1]);
                Back_Date = Back_Date;
            }
            I_Samp2 = (UInt16)(Back_Date);
            ArrayForBMS();
            SendData_ShowData();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
            AllButton_true();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            byte[] a = StringToByte(textBox10.Text);

            Back_Date = 0;
            if (a.Length == 1)
            {
                Back_Date = a[0];
            }
            else if (a.Length == 2)
            {
                Back_Date = a[0];
                Back_Date = (UInt16)((Back_Date << 8) + a[1]);
                Back_Date = Back_Date;
            }
            U_set = (UInt16)(Back_Date);
            if (U_set > 0x6100)
            {
                textBox10.Text = "设定值超限";
            }

            byte[] b = StringToByte(textBox1.Text);
            Back_Date = 0;
            if (b.Length == 1)
            {
                Back_Date = b[0];
            }
            else if (b.Length == 2)
            {
                Back_Date = b[0];
                Back_Date = (UInt16)((Back_Date << 8) + b[1]);
                Back_Date = Back_Date;
            }
            I_set = (UInt16)(Back_Date);
            if (I_set > 0x2300)
            {
                textBox1.Text = "设定值超限";
            }
            flag_btnclick = "输出给定";
            ArrayForBMS();
            SendData_ShowData();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
            AllButton_true();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button14_Click_1(object sender, EventArgs e)
        {
            textBox_fasong.Text = "";
            Form1_Load(null, null);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            textBox10.Text = "56.00";
            textBox1.Text = "3.00";
            textBox2.Text = "48.00";
            textBox3.Text = "";
            textBox4.Text = "56.00";
            textBox5.Text = "";
            textBox6.Text = "12.00";
            textBox7.Text = "";
            textBox8.Text = "20.00";
            textBox9.Text = "";

            textBox12.Text = "V0.0-00000000";
            textBox13.Text = "V0.00-00000000";
        }

        private void button18_Click(object sender, EventArgs e)
        {
            flag_btnclick = "遥测";
            /***************************组帧********************************************/
            while (flag_btnclick == "遥测")
            {
                ArrayForBMS();
                /*************************发送数据***************************************/
                SendData_ShowData();
                Time_Delay_MS(500);
                /***************************接收数据并判断是否合格*********************************/
                RecData_ShowData_New();
                AllButton_true();
            }
        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView_YX_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button19_Click(object sender, EventArgs e)         //控制板遥测
        {
            flag_btnclick = "控制遥测";
            /***************************组帧********************************************/
            while (flag_btnclick == "控制遥测")
            {
                ArrayForBMS();
                /*************************发送数据***************************************/
                SendData_ShowData();
                Time_Delay_MS(500);
                /***************************接收数据并判断是否合格*********************************/
                RecData_ShowData_New();
                if ((YX_CTL<<8)== 0)
                {
                  //  pictureBox1.Image = global::充电机电源板生产测试软件.Properties.Resources.green;
                }
                else
                {
                 //   pictureBox1.Image = global::充电机电源板生产测试软件.Properties.Resources.red;
                }
                if ((YX_CTL >> 8) != 0)
                {
                    MessageBox.Show("铁电时钟检测失败", "检测失败", MessageBoxButtons.OK);
                }
           
                AllButton_true();
            }
        }

        private void label47_Click(object sender, EventArgs e)
        {

        }

        private void label50_Click_1(object sender, EventArgs e)
        {

        }

        private void label65_Click_1(object sender, EventArgs e)
        {

        }

        private void label63_Click(object sender, EventArgs e)
        {

        }

        private void button29_Click(object sender, EventArgs e)
        {
            /////////////////STEP1 设定维护模式///////////////////////////
            flag_btnclick = "维护模式";
            ArrayForBMS();
            SendData_ShowData();
            Com_Over_Time_Pro();
            RecData_ShowData_New();

            /////////////////STEP2 第一次设定源的电压///////////////////////////
            byte[] a = StringToByte(负载电压.Text);
            byte[] b = StringToTen(负载电压.Text);
            Back_Date = 0;
            if (a.Length == 1)
            {
                Back_Date = a[0];
            }
            else if (a.Length == 2)
            {
                Back_Date = a[0];
                Back_Date = (UInt16)((Back_Date << 8) + a[1]);
                Back_Date = Back_Date;
            }
            U_set = (UInt16)(Back_Date);
            U_set_pwr = b[0];
            U_set_pwr = (UInt16)((U_set_pwr * 10 + b[1]) * 100);
            if (U_set > 0x5800)
            {
                //textBox10.Text = "设定值超限";
            }

            I_set = 0x0500;
            if (I_set > 0x2300)
            {
                //textBox1.Text = "设定值超限";
            }
            flag_btnclick = "输出给定";
            ArrayForBMS();
            SendData_ShowData();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
            AllButton_true();

            /////////////////STEP3 等待电压稳定///////////////////////////
            flag_btnclick = "遥测";
            /***************************组帧********************************************/
            while (flag_btnclick == "遥测")
            {
                ArrayForBMS();
                /*************************发送数据***************************************/
                SendData_ShowData();
                Time_Delay_MS(500);
                /***************************接收数据并判断是否合格*********************************/
                RecData_ShowData_New();
                AllButton_true();
                负载电流.Text =(U_Power / 100).ToString() + "." + (U_Power % 100).ToString();
                if (U_Power >= U_set_pwr)
                {
                    Time_Delay_MS(500);
                    break;
                }
            }
            ////////////////Step4 第一给值/////////////////////////////////
            ///
            Time_Delay_MS(2500);
            flag_btnclick = "控制第一次给电压值";

            U_Samp1 = (UInt16)(Back_Date);
            ArrayForBMS();
            SendData_ShowData();
            Com_Over_Time_Pro();
            RecData_ShowData_New();

            /////////////////STEP5 第二次设定源的电压///////////////////////////
            a = StringToByte(CC设置.Text);
            b = StringToTen(CC设置.Text);

            U_set_pwr = b[0];
            U_set_pwr = (UInt16)((U_set_pwr * 10 + b[1]) * 100);
            Back_Date = 0;
            if (a.Length == 1)
            {
                Back_Date = a[0];
            }
            else if (a.Length == 2)
            {
                Back_Date = a[0];
                Back_Date = (UInt16)((Back_Date << 8) + a[1]);
                Back_Date = Back_Date;
            }
            U_set = (UInt16)(Back_Date);
            if (U_set > 0x5800)
            {
               // textBox10.Text = "设定值超限";
            }

            I_set = 0x0500;
            if (I_set > 0x2300)
            {
              //  textBox1.Text = "设定值超限";
            }
            flag_btnclick = "输出给定";
            ArrayForBMS();
            SendData_ShowData();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
            AllButton_true();

            /////////////////STEP6 等待电压稳定///////////////////////////
            flag_btnclick = "遥测";
            /***************************组帧********************************************/
            while (flag_btnclick == "遥测")
            {
                ArrayForBMS();
                /*************************发送数据***************************************/
                SendData_ShowData();
                Time_Delay_MS(500);
                /***************************接收数据并判断是否合格*********************************/
                RecData_ShowData_New();
                AllButton_true();
               // textBox35.Text = U_Power.ToString();
                CV设置.Text = (U_Power / 100).ToString() + "." + (U_Power % 100).ToString();
                if (U_Power >= U_set_pwr)
                {
                    Time_Delay_MS(500);
                    break;
                }
            }
            Time_Delay_MS(2500);
            ////////////////Step5 第二给值/////////////////////////////////
            flag_btnclick = "控制第二次给电压值";

            U_Samp1 = (UInt16)(Back_Date);
            ArrayForBMS();
            SendData_ShowData();
            Com_Over_Time_Pro();
            RecData_ShowData_New();



            flag_btnclick = "控制遥测";
            /***************************组帧********************************************/
            while (flag_btnclick == "控制遥测")
            {
                ArrayForBMS();
                /*************************发送数据***************************************/
                SendData_ShowData();
                Time_Delay_MS(500);
                /***************************接收数据并判断是否合格*********************************/
                RecData_ShowData_New();
                //if (textBox29.Text == "0")
                //{
                //    pictureBox1.Image= global::充电机电源板生产测试软件.Properties.Resources.green;
                //}
                //else
                //{
                //    pictureBox1.Image = global::充电机电源板生产测试软件.Properties.Resources.red;
                //}
                AllButton_true();
            }
               


        }

        private void textBox34_TextChanged(object sender, EventArgs e)
        {

        }

        private void _TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void label64_Click(object sender, EventArgs e)
        {

        }

        private void button20_Click(object sender, EventArgs e)
        {
            flag_btnclick = "恢复控制默认参数";
            ArrayForBMS();
            SendData_ShowData();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
        }

        private void button21_Click(object sender, EventArgs e)
        {
            Adjust_Flag = 0;
          
            电流校准.Image = global::充电机电源板生产测试软件.Properties.Resources.red;
            电压校准.Image = global::充电机电源板生产测试软件.Properties.Resources.red;
            下垂测试.Image = global::充电机电源板生产测试软件.Properties.Resources.red;
            短路测试.Image = global::充电机电源板生产测试软件.Properties.Resources.red;
            ///////////////设置电子负载为程控模式//////////////////////////// step0
            flag_btnclick = "程控模式";
            ArrayForDc_elec_load();

            SendData_ShowData_Elec_load();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
            AllButton_true();
            ////////////////设置电源模块进入维护模式//////////////////////////// step1
            flag_btnclick = "维护模式";
            ArrayForBMS();
            SendData_ShowData();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
            ////////////////设置电源模块进入校准模式///////////////////////////step2
            flag_btnclick = "进入校准模式";
            ArrayForBMS();
            SendData_ShowData();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
            ////////////////电压校准准备//////////////////// ///////////////////step3
            flag_btnclick = "CR";
            ArrayForDc_elec_load();

            SendData_ShowData_Elec_load();
            Com_Over_Time_Pro();
            RecData_ShowData_New();

            DC_set_32 = 50000;//设置负载为50R
            flag_btnclick = "设CR值";
            ArrayForDc_elec_load();
            SendData_ShowData_Elec_load();
            Com_Over_Time_Pro();
            RecData_ShowData_New();

            flag_btnclick = "开机";
            ArrayForDc_elec_load();

            SendData_ShowData_Elec_load();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
            /////////////////第一次电压给定//////////////////////////////////step4
            flag_btnclick = "第一次给定电压";////
            Back_Date = 0;
            byte[] a = StringToByte(textBox2.Text);
            if (a.Length == 1)
            {
                Back_Date = a[0];
            }
            else if (a.Length == 2)
            {
                Back_Date = a[0];
                Back_Date = (UInt16)((Back_Date << 8) + a[1]);
                Back_Date = Back_Date;
            }
            U_Adj_set1 = (UInt16)(Back_Date);
            ArrayForBMS();
            SendData_ShowData();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
            Time_Delay_MS(1000);//延时1.5S


            Read_elec_load();//读取负载电压

            flag_btnclick = "第一次实际电压";////
            Back_Date = 0;
            textBox3.Text = 负载电压.Text;
            U_Samp1 = Convert_to_setUI(textBox3.Text);
            ArrayForBMS();
            SendData_ShowData();
            Com_Over_Time_Pro();
            RecData_ShowData_New();

            /////////////////第二次电压给定//////////////////////////////////step5
            flag_btnclick = "第二次给定电压";////
            Back_Date = 0;
             a = StringToByte(textBox4.Text);
            if (a.Length == 1)
            {
                Back_Date = a[0];
            }
            else if (a.Length == 2)
            {
                Back_Date = a[0];
                Back_Date = (UInt16)((Back_Date << 8) + a[1]);
                Back_Date = Back_Date;
            }
            U_Adj_set2 = (UInt16)(Back_Date);
            ArrayForBMS();
            SendData_ShowData();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
            Time_Delay_MS(1000);//延时1.5S
            Read_elec_load();//读取负载电压

            flag_btnclick = "第二次实际电压";

            textBox5.Text = 负载电压.Text;
            U_Samp2= Convert_to_setUI(textBox5.Text);
            ArrayForBMS();
            SendData_ShowData();
            Com_Over_Time_Pro();
            RecData_ShowData_New();

            /////////////////第三次电压给定判据////////////////////////////////step6

            U_set = 0x5800;// 设定电压 58V

            I_set = 0x600; // 设定电流 10A
            flag_btnclick = "输出给定";
            ArrayForBMS();
            SendData_ShowData();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
            Time_Delay_MS(1000);//延时1.5S
            Read_elec_load();//读取负载电压
            U_Samp_adjust = Convert_to_Uint16(负载电压.Text);

            if (U_Samp_adjust > U_Samp_min && U_Samp_adjust < U_Samp_max)
            {
                电压校准.Image = global::充电机电源板生产测试软件.Properties.Resources.green;

            }
            else
            {
                电压校准.Image = global::充电机电源板生产测试软件.Properties.Resources.red;
                Adjust_Flag |= 0x1;
            }

            ////////////////电流校准准备///////////////////////////////////////step7
            flag_btnclick = "CV";
            ArrayForDc_elec_load();

            SendData_ShowData_Elec_load();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
            
            DC_set_32 = 53000;//设置负载为53V
            flag_btnclick = "设CV值";
            ArrayForDc_elec_load();
            SendData_ShowData_Elec_load();
            Com_Over_Time_Pro();
            RecData_ShowData_New();

            ////////////////第一次给定电流////////////////////////////////////step8
            flag_btnclick = "第一次给定电流";
            Back_Date = 0;
            a = StringToByte(textBox6.Text);
            if (a.Length == 1)
            {
                Back_Date = a[0];
            }
            else if (a.Length == 2)
            {
                Back_Date = a[0];
                Back_Date = (UInt16)((Back_Date << 8) + a[1]);
                Back_Date = Back_Date;
            }
            I_Adj_set1 = (UInt16)(Back_Date);
            ArrayForBMS();
            SendData_ShowData();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
            Time_Delay_MS(1000);//延时1.5S
            Read_elec_load();//读取负载电流

            flag_btnclick = "第一次实际电流";
            textBox7.Text= 负载电流.Text;
          
            I_Samp1 = Convert_to_setUI(textBox7.Text);
            ArrayForBMS();
            SendData_ShowData();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
            ////////////////第二次给定电流////////////////////////////////////step9
            flag_btnclick = "第二次给定电流";
            a = StringToByte(textBox8.Text);
            Back_Date = 0;
            if (a.Length == 1)
            {
                Back_Date = a[0];
            }
            else if (a.Length == 2)
            {
                Back_Date = a[0];
                Back_Date = (UInt16)((Back_Date << 8) + a[1]);
                Back_Date = Back_Date;
            }
            I_Adj_set2 = (UInt16)(Back_Date);
            ArrayForBMS();
            SendData_ShowData();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
            Time_Delay_MS(1000);//延时1.5S
            Read_elec_load();//读取负载电流

            flag_btnclick = "第二次实际电流";

            textBox9.Text = 负载电流.Text;

            I_Samp2 = Convert_to_setUI(textBox9.Text);
            ArrayForBMS();
            SendData_ShowData();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
            AllButton_true();



            ////////////////第三次给定电流并判据////////////////////////////////////step10
            flag_btnclick = "退出校准模式";
            ArrayForBMS();
            SendData_ShowData();
            Com_Over_Time_Pro();
            RecData_ShowData_New();

            U_set = 0x5800;// 设定电压 58V
            I_set = 0x2000; // 设定电流 00A
            flag_btnclick = "输出给定";
            ArrayForBMS();
            SendData_ShowData();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
            Time_Delay_MS(1000);//延时1.5S

            Read_elec_load();//读取负载电流
            I_Samp_adjust = Convert_to_Uint16(负载电流.Text);

            if (I_Samp_adjust > I_Samp_min && I_Samp_adjust < I_Samp_max)
            {
                电流校准.Image = global::充电机电源板生产测试软件.Properties.Resources.green;
            }
            else
            {
                电流校准.Image = global::充电机电源板生产测试软件.Properties.Resources.red;
                Adjust_Flag |= 0x2;
            }
            ////////////////下垂功能测试///////////////////////////////////////step11

            DC_set_32 = 25000;//设置负载为20V
            flag_btnclick = "设CV值";
            ArrayForDc_elec_load();
            SendData_ShowData_Elec_load();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
            Time_Delay_MS(1500);//延时1.5S
            Read_elec_load();//读取负载电流
            下垂电流.Text = 负载电流.Text;
            I_Samp_drop = Convert_to_Uint16(负载电流.Text);
            
            if (I_Samp_drop > I_Samp_drop_min && I_Samp_drop < I_Samp_drop_max)
            {
                下垂测试.Image = global::充电机电源板生产测试软件.Properties.Resources.green;
            }
            else
            {
                下垂测试.Image = global::充电机电源板生产测试软件.Properties.Resources.red;
                Adjust_Flag |= 0x4;
            }

            ////////////////短路功能测试////////////////////////////////////step11
            DC_set_32 = 58000;//设置负载为58V
            flag_btnclick = "设CV值";
            ArrayForDc_elec_load();
            SendData_ShowData_Elec_load();
            Com_Over_Time_Pro();
            RecData_ShowData_New();

            Time_Delay_MS(3500);//延时1.5S
            Read_elec_load();//读取负载电流
            DC_set_32 = 1000;//设置负载为1V 短路
            flag_btnclick = "设CV值";
            ArrayForDc_elec_load();
            SendData_ShowData_Elec_load();
            Com_Over_Time_Pro();
            RecData_ShowData_New();

            Read_elec_load();//读取负载电流
            短路电压.Text = 负载电压.Text;

            Time_Delay_MS(2500);//延时2.5S

            U_Samp_short = Convert_to_Uint16(负载电压.Text);

            if (U_Samp_short <U_Samp_short_min )
            {
                短路测试.Image = global::充电机电源板生产测试软件.Properties.Resources.green;
            }
            else
            {
                短路测试.Image = global::充电机电源板生产测试软件.Properties.Resources.red;
                Adjust_Flag |= 0x8;
            }

            DC_set_32 = 53000;//设置负载为50V
            flag_btnclick = "设CV值";
            ArrayForDc_elec_load();
            SendData_ShowData_Elec_load();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
            Time_Delay_MS(9000);//延时2.5S
            Read_elec_load();//读取负载电流
            短路电压.Text = 负载电压.Text;

            U_Samp_short = Convert_to_Uint16(负载电压.Text);

            if (U_Samp_short > U_Samp_short_max)
            {
                短路测试.Image = global::充电机电源板生产测试软件.Properties.Resources.green;
            }
            else
            {
                短路测试.Image = global::充电机电源板生产测试软件.Properties.Resources.red;
                Adjust_Flag |= 0x8;
            }

            if (Adjust_Flag == 0)
            {
                MessageBox.Show("检测通过！", "恭喜");
            }
            else 
            {
                MessageBox.Show("检测失败，请检查问题！", "遗憾");
            }

            ////////////////关机测试结束////////////////////////////////////step12
            U_set = 0x5800; ;// 设定电压 0V
            I_set = 0x300; // 设定电流 00A
            flag_btnclick = "输出给定";
            ArrayForBMS();
            SendData_ShowData();
            Com_Over_Time_Pro();
            RecData_ShowData_New();

            flag_btnclick = "CR";
            ArrayForDc_elec_load();

            SendData_ShowData_Elec_load();
            Com_Over_Time_Pro();
            RecData_ShowData_New();

            DC_set_32 = 40000;//设置负载为800R
            flag_btnclick = "设CR值";
            ArrayForDc_elec_load();
            SendData_ShowData_Elec_load();
            Com_Over_Time_Pro();
            RecData_ShowData_New();

            //flag_btnclick = "关机";
            //ArrayForDc_elec_load();

            //SendData_ShowData_Elec_load();
            //Com_Over_Time_Pro();
            //RecData_ShowData_New();





        }

        private void label74_Click(object sender, EventArgs e)
        {

        }

        private void button19_Click_1(object sender, EventArgs e)
        {
       
            flag_btnclick = "关机";
            ArrayForDc_elec_load();

            SendData_ShowData_Elec_load();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
            AllButton_true();
        }

        private void button22_Click(object sender, EventArgs e)
        {

        }

        private void button24_Click(object sender, EventArgs e)
        {
            flag_btnclick = "程控模式";
            ArrayForDc_elec_load();

            SendData_ShowData_Elec_load();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
            AllButton_true();
        }

        private void button25_Click(object sender, EventArgs e)
        {
            flag_btnclick = "手动模式";
            ArrayForDc_elec_load();

            SendData_ShowData_Elec_load();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
            AllButton_true();
        }

        private void button26_Click(object sender, EventArgs e)
        {
            flag_btnclick = "开机";
            ArrayForDc_elec_load();

            SendData_ShowData_Elec_load();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
            AllButton_true();
        }

        private void button20_Click_1(object sender, EventArgs e)
        {
            flag_btnclick = "读取电压";
            while (flag_btnclick == "读取电压")
            {          
                ArrayForDc_elec_load();
                SendData_ShowData_Elec_load();
                Com_Over_Time_Pro();
                RecData_ShowData_New();
                Time_Delay_MS(1500);
                AllButton_true();
            }
        }

         private void button23_Click(object sender, EventArgs e)
        {
            flag_btnclick = "CC";
            ArrayForDc_elec_load();

            SendData_ShowData_Elec_load();
            Com_Over_Time_Pro();
            RecData_ShowData_New();

            Back_Date32 = 0;
            byte[] a = StringToByte32(CC设置.Text);
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
            shi= 10*((Back_Date32>>4) & 0xF);
            bai = 100 * ((Back_Date32 >> 8) & 0xF);
            qian = 1000 * ((Back_Date32 >> 12) & 0xF);
            wan = 10000 * ((Back_Date32 >> 16) & 0xF);
            shiwan = 100000 * ((Back_Date32 >> 20) & 0xF);
            baiwan = 1000000 * ((Back_Date32 >> 24) & 0xF);
            qianwan = 10000000 * ((Back_Date32 >> 28) & 0xF);
   
            DC_set_32 = ge+ shi+ bai+ qian+ wan+ shiwan+ baiwan+qianwan;
            flag_btnclick = "设CC值";
            ArrayForDc_elec_load();
            SendData_ShowData_Elec_load();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
            AllButton_true();
        }

        private void button22_Click_1(object sender, EventArgs e)
        {
            flag_btnclick = "CV";
            ArrayForDc_elec_load();

            SendData_ShowData_Elec_load();
            Com_Over_Time_Pro();
            RecData_ShowData_New();

            Back_Date32 = 0;
            byte[] a = StringToByte32(CV设置.Text);
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


            DC_set_32 = ge + shi + bai + qian + wan + shiwan + baiwan + qianwan;
            DC_set_32 = DC_set_32 / 10;

            flag_btnclick = "设CV值";
            ArrayForDc_elec_load();
            SendData_ShowData_Elec_load();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
            AllButton_true();
        }

        private void button27_Click(object sender, EventArgs e)
        {
            flag_btnclick = "CR";
            ArrayForDc_elec_load();

            SendData_ShowData_Elec_load();
            Com_Over_Time_Pro();
            RecData_ShowData_New();

            Back_Date32 = 0;
            byte[] a = StringToByte32(CR设置.Text);
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


            DC_set_32 = ge + shi + bai + qian + wan + shiwan + baiwan + qianwan;
            DC_set_32 = DC_set_32 / 10;
            flag_btnclick = "设CR值";
            ArrayForDc_elec_load();
            SendData_ShowData_Elec_load();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
            AllButton_true();
        }

        private void button28_Click(object sender, EventArgs e)
        {
            flag_btnclick = "CW";
            ArrayForDc_elec_load();
            SendData_ShowData_Elec_load();
            Com_Over_Time_Pro();
            RecData_ShowData_New();

            Back_Date32 = 0;
            byte[] a = StringToByte32(CW设置.Text);
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


            DC_set_32 = ge + shi + bai + qian + wan + shiwan + baiwan + qianwan;
            DC_set_32 = DC_set_32 / 10;
            flag_btnclick = "设CW值";
            ArrayForDc_elec_load();
            SendData_ShowData_Elec_load();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
            AllButton_true();
        }

        

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox35_TextChanged(object sender, EventArgs e)
        {

        }

        private void CC设置_TextChanged(object sender, EventArgs e)
        {

        }

        private void label69_Click(object sender, EventArgs e)
        {

        }

        private void 连接负载_Click(object sender, EventArgs e)
        {
            if (this.连接负载.Text == "连接负载")
            {
                switch (this.comboBox3.Text)
                {
                    case "4800":
                        this.负载串口.BaudRate = 4800;
                        break;
                    case "9600":
                        this.负载串口.BaudRate = 9600;
                        break;
                    case "19200":
                        this.负载串口.BaudRate = 19200;
                        break;
                    case "57600":
                        this.负载串口.BaudRate = 57600;
                        break;
                    case "115200":
                        this.负载串口.BaudRate = 1152000;
                        break;
                    default: break;
                }
                switch (comboBox2.Text)
                {
                    case "None":
                        负载串口.Parity = Parity.None;
                        break;
                    case "Odd":
                        负载串口.Parity = Parity.Odd;
                        break;
                    case "Even":
                        负载串口.Parity = Parity.Even;
                        break;
                    case "Mark":
                        负载串口.Parity = Parity.Mark;
                        break;
                    case "Space":
                        负载串口.Parity = Parity.Space;
                        break;
                    default: break;
                }
                switch (comboBox1.Text)
                {
                    case "com1":
                        负载串口.PortName = "com1";
                        break;
                    case "com2":
                        负载串口.PortName = "com2";
                        break;
                    case "com3":
                        负载串口.PortName = "com3";
                        break;
                    case "com4":
                        负载串口.PortName = "com4";
                        break;
                    case "com5":
                        负载串口.PortName = "com5";
                        break;
                    case "com6":
                        负载串口.PortName = "com6";
                        break;
                    case "com7":
                        负载串口.PortName = "com7";
                        break;
                    case "com8":
                        负载串口.PortName = "com8";
                        break;
                    case "com9":
                        负载串口.PortName = "com9";
                        break;
                    default: break;
                }
                switch (comboBox4.Text)
                {/*
                        case "500ms":
                            Timeout = 500;
                            break;
                        case "800ms":
                            Timeout = 800;
                            break;
                        case "1s":
                            Timeout = 1000;
                            break;
                        case "2s":
                            Timeout = 2000;
                            break;
                        case "3s":
                            Timeout = 3000;
                            break;*/
                    default: break;
                }
               // 负载串口.DataReceived += 负载串口_DataReceived;
                try
                {
                    负载串口.Open();
                }
                catch
                {
                  //  负载串口.DataReceived -= 负载串口_DataReceived;
                    MessageBox.Show("串口<" + 负载串口.PortName + ">无法打开", "打开失败", MessageBoxButtons.OK);
                    return;
                }

                连接负载.Text = "断开负载";
                //comboBox1.Enabled = false;
                //comboBox2.Enabled = false;
                //comboBox3.Enabled = false;
                //comboBox4.Enabled = false;
                负载指示.Image = global::充电机电源板生产测试软件.Properties.Resources.green;

            }
            else if (负载串口.IsOpen)
            {
               // 负载串口.DataReceived -= 负载串口_DataReceived;
                负载串口.Close();
                连接负载.Text = "连接负载";
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
                comboBox3.Enabled = true;
                comboBox4.Enabled = true;
                负载指示.Image = global::充电机电源板生产测试软件.Properties.Resources.red;
            }
            else
            {
                负载指示.Image = global::充电机电源板生产测试软件.Properties.Resources.black;
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void 负载连接_Enter(object sender, EventArgs e)
        {

        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            flag_btnclick_pwr = "开机";
            ArrayForDc_elec_pwr();
            SendData_ShowData_Elec_load();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
            AllButton_true();
        }

        private void button11_Click_1(object sender, EventArgs e)
        {
            flag_btnclick_pwr = "关机";
            ArrayForDc_elec_pwr();

            SendData_ShowData_Elec_load();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
            AllButton_true();
        }
       
        private void button12_Click_1(object sender, EventArgs e)
        {
            Array_Load.Clear();//清空逆变器动态数组
            char[] a = String_to_char(直流源电流设定值.Text);
            Array_Load.Add(Convert.ToByte('C'));//1
            Array_Load.Add(Convert.ToByte('U'));//2
            Array_Load.Add(Convert.ToByte('R'));//3
            Array_Load.Add(Convert.ToByte('R'));//4
            Array_Load.Add(Convert.ToByte(' '));//5
            for (int i = 0; i < a.Length; i++)
            {
                Array_Load.Add(Convert.ToByte(a[i]));//5
            }
            Array_Load.Add(Convert.ToByte(0x0D));//6
            Array_Load.Add(Convert.ToByte(0x0A));//6

            SendData_ShowData_Elec_load();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
            AllButton_true();

        }

        private void button15_Click_1(object sender, EventArgs e)
        {
            Array_Load.Clear();//清空逆变器动态数组
            char[] a = String_to_char(直流源电压设定值.Text);
            Array_Load.Add(Convert.ToByte('V'));//1
            Array_Load.Add(Convert.ToByte('O'));//2
            Array_Load.Add(Convert.ToByte('L'));//3
            Array_Load.Add(Convert.ToByte('T'));//4
            Array_Load.Add(Convert.ToByte(' '));//5
            for (int i = 0; i < a.Length; i++)
            { 
                Array_Load.Add(Convert.ToByte(a[i]));//5
            }
            Array_Load.Add(Convert.ToByte(0x0D));//6
            Array_Load.Add(Convert.ToByte(0x0A));//6

            SendData_ShowData_Elec_load();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
            AllButton_true();
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            flag_btnclick_pwr = "手动模式";
            ArrayForDc_elec_pwr();
            SendData_ShowData_Elec_load();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
            AllButton_true();
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            flag_btnclick_pwr = "程控模式";
            ArrayForDc_elec_pwr();

            SendData_ShowData_Elec_load();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
            AllButton_true();

        }

        private void button21_Click_1(object sender, EventArgs e)
        {
            Array_Load.Clear();//清空逆变器动态数组
            char[] a = String_to_char(直流源电压设定值.Text);//FETC:CURR?

            flag_btnclick_pwr = "读取电压";
            ArrayForDc_elec_pwr();
            SendData_ShowData_Elec_load();
            Com_Over_Time_Pro();
            flag_btnclick_pwr = "读取电流";
            ArrayForDc_elec_pwr();
            SendData_ShowData_Elec_load();
            Com_Over_Time_Pro();
            flag_btnclick_pwr = "读取功率";
            ArrayForDc_elec_pwr();
            SendData_ShowData_Elec_load();
            Com_Over_Time_Pro();
            RecData_ShowData_New();
            AllButton_true();
        }

        private void textBox19_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
