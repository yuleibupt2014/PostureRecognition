using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Collections;

namespace Chronos_As_Read
{
    public partial class Form1 : Form
    {
        uint button1_count=0;   //记录按键次数
        eZ430ChronosNet.Chronos ez = new eZ430ChronosNet.Chronos();
        eZ430ChronosNet.APStatus status = new eZ430ChronosNet.APStatus();
        static Excel.Application xapp = new Excel.Application();
        static string filepath = @"C:\Users\Administrator\Desktop\训练样例数据库.xlsx";
        Excel.Workbook xbook = xapp.Workbooks._Open(filepath, Type.Missing, false, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        FFT_to_Database FFTtoDatabase = new FFT_to_Database();
        //uint data=0;
        double RawX, RawY, RawZ, RawButton;
        uint icount = 0;    //收到的第icount+1个数据
        uint sheet_count=0;
        uint flag = 0;
        uint label_count = 0;

        public static ArrayList list = new ArrayList(180);

        
        uint ldata = 0;                  //定义ldata,表示上一次50ms读书时读取的非data=255的值，此处用作过滤掉ms中断时表没有发出数据的情况
        public Form1()
        {
            InitializeComponent();
        }
        public void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = null;
            textBox2.Text = null;
            textBox3.Text = null;
            textBox4.Text = null;
            textBox5.Text = null;
            button1.Text="未连接";
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.Items.Add("举手");
            comboBox1.Items.Add("趴桌子睡觉");
            comboBox1.Items.Add("正坐");
            comboBox1.Items.Add("平躺");
            comboBox1.Items.Add("站立");
            comboBox1.Items.Add("敬礼");
            comboBox1.Items.Add("走");
            comboBox1.Items.Add("跑");
            comboBox1.Items.Add("招手");
            comboBox1.Items.Add("投篮");
            comboBox1.Items.Add("自由泳");
            comboBox1.Items.Add("蛙泳");
            comboBox1.Items.Add("射箭");    
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1_count++;
            if (button1_count % 2 == 1)
            {
                button1.Text = "已连接";
            }
            if (button1_count % 2 == 0)
            {
                button1.Text = "未连接";
            }
            if (button1_count == 100)
            {
                button1_count = 0;
            }

            if (button1.Text == "未连接")
            {
                TimGetData.Enabled = false;
                ez.CloseComPort();
            }

            //otherwise, try to connect
            if (button1.Text == "已连接")
            {
                ez.OpenComPort("COM23");
                ez.StartSimpliciTI();
                TimGetData.Enabled = true;
            }
        }

        private void TimGetData_Tick(object sender, EventArgs e)
        {
                    //get data by polling the device 
            uint i = 0; uint data = 0;
            ez.GetData(out data); 
            if (data != 255)
            {

                RawButton = data & 0x00ff; //  will be shown only when change occurs
                RawX = (data >> 8) & 0x00ff;
                RawY = (data >> 16) & 0x00ff;
                RawZ = (data >> 24) & 0x00ff; //direct conversion of every axis

                if (RawX > 128)
                    RawX = -(255 - RawX);
                if (RawY > 128)
                    RawY = -(255 - RawY);
                if (RawZ > 128)
                    RawZ = -(255 - RawZ);

                //RawX = System.Math.Abs(RawX - 128)/128d;      //归一化数据处理
                //RawY = System.Math.Abs(RawY - 128)/128d;
                //RawZ = System.Math.Abs(RawZ - 128)/128d;
                RawX /= 256d;
                RawY /= 256d;
                RawZ /= 256d;

                list.Insert(0, RawX);
                list.Insert(0, RawY);
                list.Insert(0, RawZ);
                if (list.Count >= 183)
                {
                    list.RemoveRange(180, 3);
                }


                textBox5.Text = Convert.ToString(data);
                textBox4.Text = Convert.ToString(RawButton);
                textBox3.Text = Convert.ToString(RawX);
                textBox2.Text = Convert.ToString(RawY);
                textBox1.Text = Convert.ToString(RawZ);
            }
            if (data == 255)
                ;


        }

        public void TimInterrupt_Tick(object sender, EventArgs e)
        {

            if (flag == 1)
            {
                label_count++;
                for (int j = 0; j < 180; j++)
                {
                    xbook.Sheets[Convert.ToInt32(label16.Text)].Cells[label_count, j + 1] = list[j];
                }
                xbook.Sheets[Convert.ToInt32(label16.Text)].Cells[label_count, 181] = Convert.ToInt32(label16.Text);//Convert.ToInt32(label16.Text)
            }

            if (label_count == 50)
            {
                label_count = 1;
                xbook.Save();//(@"C:\Users\Administrator\Desktop\训练样例数据库.xlsx");
               // xapp.Quit();
                //MessageBox.Show("Write Success");
            }
            labelnumber.Text = label_count.ToString();
            
            

            //xapp.Quit();
        }

        private void btnWriteToExcel_Click_1(object sender, EventArgs e)
        {
            // string filepath = @"C:\Users\Administrator\Desktop\database\举手.xlsx";

            flag++;
            if (flag == 2)
                flag = 0;
            if (flag == 1)
                btnWriteToExcel.Text = "正在写入EXCEL";
            if(flag==0)
                btnWriteToExcel.Text = "点击写入EXCEL";


        }




        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "举手")
                label16.Text = "1";
            if (comboBox1.Text == "趴桌子睡觉")
                label16.Text = "2";
            if (comboBox1.Text == "正坐")
                label16.Text = "3";
            if (comboBox1.Text == "平躺")
                label16.Text = "4";
            if (comboBox1.Text == "站立")
                label16.Text = "5";
            if (comboBox1.Text == "敬礼")
                label16.Text = "6";
            if (comboBox1.Text == "走")
                label16.Text = "7";
            if (comboBox1.Text == "跑")
                label16.Text = "8";
            if (comboBox1.Text == "招手")
                label16.Text = "9";
            if (comboBox1.Text == "投篮")
                label16.Text = "10";
            if (comboBox1.Text == "自由泳")
                label16.Text = "11";
            if (comboBox1.Text == "蛙泳")
                label16.Text = "12";
            if (comboBox1.Text == "射箭")
                label16.Text = "13";  
            label_count = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FFTtoDatabase.Convert_to_FFT();
        }
    }
}
