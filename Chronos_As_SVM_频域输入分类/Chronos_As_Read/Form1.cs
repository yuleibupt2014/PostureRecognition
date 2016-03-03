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
using System.Drawing.Drawing2D;
using SVM;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace Chronos_As_Read
{
    
    public partial class Form1 : Form
    {

        uint y = 0;
        uint button1_count=0;   //记录按键次数
        uint same = 0;   //记录判断相同的次数
        uint dat = 0;
        //sp.SpeakChina("磊哥你好");
        uint flaglabel16 = 0;

        eZ430ChronosNet.Chronos ez = new eZ430ChronosNet.Chronos();
        static Draw draw = new Draw();                               //实例化画图
        Music music = new Music();                                   //实例化音乐盒
        voice sp = new voice(100, 0);        //语音实例化
        string[] yuyin = new string[31] {"举手","趴着睡觉","正坐","射击","站立","敬军礼","走","跑","招手","投篮","自由泳","蛙泳","","","","","","","","","","","","","","","","打开音乐盒","暂停或播放音乐","","下一曲" };

        Model model2 = new Model();                                  //读取上一次存储的model
        const string FileName_Model = @"..\..\SavedModel.bin";            //保存支持向量内Model等参数
        double RawX, RawY, RawZ, RawButton;
        uint xianshi = 0;

        double biaozhunchaX = 0, biaozhunchaY = 0, biaozhunchaZ = 0;    //定义标准差、偏度、峰度、相关度
        double pianduX = 0, pianduY = 0, pianduZ = 0;
        double fengduX = 0, fengduY = 0, fengduZ = 0;
        double xiangguanduXY = 0, xiangguanduXZ = 0, xiangguanduYZ = 0;

        private Node[][] _X;
        double[] kNN = new double[600];
        uint count = 0, posture = 1;  //posture表示姿态，count表示收到的有效数据数量
        string[] postureword = new String[6] { "举手","你在睡觉","正坐","用手砍人","站立","敬军礼"};   //定义姿势名称
        double[,] XYZ=new double[6,3]{{-0.0067972,0.2403420,0.0226660},{-0.0608489,-0.0130994,0.2371682},{-0.1575819,-0.1310882,0.1305489},
                                      {-0.2291378,0.0050821,0.0395978},{-0.0520553,-0.2488257,-0.0025767},{0.0541486,0.1324943,0.2019669}};
        double[] junzhi=new double[6];
        double sum = 0;
        uint button2_Click_flag = 0;
        uint button3_count = 0;

        static ArrayList Xlist = new ArrayList(60);           ////////////
        static ArrayList Ylist = new ArrayList(60);
        static ArrayList Zlist = new ArrayList(60);
        public static ArrayList list = new ArrayList(180);
        ArrayList avalue = new ArrayList(180);

        double[] data_r = new double[180];
        double[] data_i = new double[180];
        double[] result_r = new double[180];
        double[] result_i = new double[180];
        FFT fft = new FFT();

        uint train_click_flag = 0;
        double label_old = 0;
        public ArrayList Avalue
        {
            get
            {
                return avalue;
            }
            set
            {
                avalue = value;
            }
        }
        public Node[][] X
        {
            get
            {
                return _X;
            }
            set
            {
                _X = value;
            }
        }
        public ArrayList blist()
        {
            return Avalue;
        }

  
        public Form1()
        {
            InitializeComponent();
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
            if (File.Exists(FileName_Model))
            {
                Stream FileStream_model = File.OpenRead(FileName_Model);
                BinaryFormatter deserializer = new BinaryFormatter();
                model2 = (Model)deserializer.Deserialize(FileStream_model);
                FileStream_model.Close();
            }
        }     //点击连接按键
       
        private void TimGetData_Tick(object sender, EventArgs e)
        {
                    //get data by polling the device 
             uint data = 0;             //Z Y X button 从高到低 例如:Z为data高8位，button为data低8位
             ez.GetData(out data).ToString();             //直到接收到的数据不是255才允许下一步；
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
                RawX /= 256d;
                RawY /= 256d;
                RawZ /= 256d;

                Xlist.Insert(0, RawX);
                Ylist.Insert(0, RawY);
                Zlist.Insert(0, RawZ);
                if(Xlist.Count>=61)
                {
                    Xlist.RemoveAt(60);
                }
                if (Ylist.Count >= 61)
                {
                    Ylist.RemoveAt(60);
                }
                if (Zlist.Count >= 61)
                {
                    Zlist.RemoveAt(60);
                }

                list.Insert(0, RawX);
                list.Insert(0, RawY);
                list.Insert(0, RawZ);     //顺序为Z、Y、X 、Z、Y、X......
                if(list.Count>=183)
                {
                    list.RemoveRange(180, 3);
                }

                if (button2_Click_flag == 1)
                {
                    count++;
                    if (count == 225)
                    {
                        draw.display();
                        count = 0;
                    }
                    this.Acculates();
                    draw.Picture(count, RawX,RawY, RawZ);///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    pictureBox1.Image = Draw.bMap;

                }
                textBox5.Text = Convert.ToString(data);
                textBox4.Text = Convert.ToString(RawButton);
                textBox3.Text = Convert.ToString(RawZ);
                textBox2.Text = Convert.ToString(RawY);
                textBox1.Text = Convert.ToString(RawX);
                if(xianshi==1)
                {
                    Xbiao.Text = biaozhunchaX.ToString();
                    Ybiao.Text = biaozhunchaY.ToString();
                    Zbiao.Text = biaozhunchaZ.ToString();
                }
                if (xianshi == 2)
                {
                    Xbiao.Text = pianduX.ToString();
                    Ybiao.Text = pianduY.ToString();
                    Zbiao.Text = pianduZ.ToString();
                }
                if (xianshi == 3)
                {
                    Xbiao.Text = fengduX.ToString();
                    Ybiao.Text = fengduY.ToString();
                    Zbiao.Text = fengduZ.ToString();
                }
                if (xianshi == 4)
                {
                    Xbiao.Text = xiangguanduXY.ToString();
                    Ybiao.Text = xiangguanduXZ.ToString();
                    Zbiao.Text = xiangguanduYZ.ToString();
                }

            }
            if (data == 255)
                ;
        }     //每50ms采样一次数据

        public void classify_Click(object sender, EventArgs e)
        {
            Problem test3 = Problem.Read("频域测试样例.txt");
            Double zhengquelv = 0d;
            zhengquelv=Prediction.Predict(test3, "results.txt", model2, false);
            Zhengquelv.Text = zhengquelv.ToString();          
        }
        public void Status(double label)
        {
            if (label == label_old)
            {
                same++;
                if (same >=15)          //same=13目前是最好的效果
                {
                    if (label == 1)
                    { 
                        label16.Text = "举手";                    
                     }
                    if (label == 2)
                    {
                        label16.Text = "趴着睡觉";
                    }
                    if (label == 3)
                    {
                        label16.Text = "正坐";
                    }
                    if (label == 4)
                    {
                        label16.Text = "射击";
                    }
                    if (label == 5)
                    {
                        label16.Text = "站立";
                    }
                    if (label == 6)
                    {
                        label16.Text = "敬军礼";
                    }
                    if (label == 7)
                    {
                        label16.Text = "走";
                    }
                    if (label == 8)
                    {
                        label16.Text = "跑";
                    }
                    if (label == 9)
                    {
                        label16.Text = "招手";
                    }
                    if (label == 10)
                    {
                        label16.Text = "投篮";
                    }
                    if (label == 11)
                    {
                        label16.Text = "自由泳";
                    }
                    if (label == 12)
                    {
                        label16.Text = "蛙泳";
                    }
                    if (label == 28)
                    {
                        if (label16.Text != "打开音乐盒")
                            music.Music_Open();
                        label16.Text = "打开音乐盒";
                    }
                    if (label == 29)
                    {
                        if (label16.Text != "播放/暂停")
                            music.Music_Pauseorbroadcast();
                        label16.Text = "播放/暂停";
                    }
                    if (label == 31)
                    {
                        if (label16.Text != "下一曲")
                            music.Music_next();
                        label16.Text = "下一曲";
                    }
                    if(flaglabel16==1&&button3.Text=="已打开语音")
                    {
                        sp.SpeakChina(yuyin[(int)label-1]);
                        flaglabel16 = 0;
                    }
                }
            }
            if (label != label_old)
            {
                same = 0;
                //label16.Text = "未知动作";
            }
            label_old = label;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            draw.display();
            button2_Click_flag = 1;
             pictureBox1.Image = Draw.bMap;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (list.Count >= 180)
            {
               for(int i=0;i<180;i++)
               {
                   data_r[i] = Convert.ToDouble(Form1.list[i]);
               }
               fft.Dit2_FFT(ref data_r, ref data_i, ref result_r, ref result_i);

                Node[] x = new Node[180];
                for (int j = 0; j < 180; j++)
                {
                    x[j] = new Node();
                    x[j].Index = j + 1;                    ///X【j】.index代表第几个特征   坑爹！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！
                    x[j].Value = result_r[j];       //Convert.ToDouble(Form1.list[j]);      //X【j】.value代表第几个特征值
                }
                double v;
                v = Prediction.Pred(model2, x);                   //获取v的值代表第几类
                Status(v);
                DateTime time1 = DateTime.Now;
                label14.Text = time1.ToString();
               // this.Acculates();  //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            }
            
        }

        public void training_Click(object sender, EventArgs e)
        {
           // Double zhengquelv = 0d;
            train_click_flag = 1;
            Problem train = Problem.Read("频域训练样例.txt");       //train里面包含vx.count  vx  vy   train._X=vx(即训练输入数据)   train._Y=vy（训练类标签）
            //Problem test3 = Problem.Read("test3.txt");          //test里面包含vx.count  vx  vy   test._X=vx(即训练输入数据)   test._Y=vy（训练类标签）         

            Parameter parameters = new Parameter();    //初始化参数值
            double C;
            double Gamma;

            ParameterSelection.Grid(train, parameters, "params.txt", out C, out Gamma);    //根据情况自动确定最佳参数值
            parameters.C = C;
            parameters.Gamma = Gamma;

            Model model = Training.Train(train, parameters);                     //model中有关于支持向量和支持向量的参数

            Stream FileStream = File.Create(FileName_Model);              //保存model对象给下次启动程序时使用
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(FileStream, model);
            FileStream.Close();         
        }   
        public void Acculates()
        {
            Acculate acculate = new Acculate(Xlist, Ylist, Zlist);
            acculate.biaozhuncha(out biaozhunchaX, out biaozhunchaY, out biaozhunchaZ);
            acculate.piandu(out pianduX, out pianduY, out pianduZ,biaozhunchaX,biaozhunchaY,biaozhunchaZ);
            acculate.fengdu(out fengduX, out fengduY, out fengduZ, biaozhunchaX, biaozhunchaY, biaozhunchaZ);
            acculate.xiangguandu(out xiangguanduXY, out xiangguanduXZ, out xiangguanduYZ);
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "标准差")
                xianshi = 1;
            if (comboBox1.Text == "偏度")
                xianshi = 2;
            if (comboBox1.Text == "峰度")
                xianshi = 3;
            if (comboBox1.Text == "相关度")
                xianshi = 4;
        }

        private void label16_TextChanged(object sender, EventArgs e)
        {
            flaglabel16 = 1;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3_count++;
            if (button3_count % 2 == 1)
            {
                button3.Text = "已打开语音";
            }
            if (button3_count % 2 == 0)
            {
                button3.Text = "已关闭语音";
            }
            if (button3_count == 100)
            {
                button3_count = 0;
            }
        }
    }
}
