using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace Chronos_As_Read
{
    class Draw
    {
        public static Bitmap bMap = new Bitmap(1500, 1000);
        public static Graphics gph = Graphics.FromImage(bMap);
        public static Pen mypenX = new Pen(Color.Red, 3);
        public static Pen mypenY = new Pen(Color.Blue, 3);
        public static Pen mypenZ = new Pen(Color.Green, 3);
        public static PointF cPt = new PointF(35, 350);//原点
        static  double RawXold, RawYold, RawZold;
        public void display()
        {
            gph.Clear(Color.White);
            PointF[] xPt = new PointF[3] { new PointF(1200, cPt.Y), new PointF(1200 - 8, cPt.Y - 8), new PointF(1200 - 8, cPt.Y + 8) };//X轴三角形
            PointF[] yPt = new PointF[3] { new PointF(cPt.X, 60), new PointF(cPt.X - 8, 70), new PointF(cPt.X + 8, 70) };//Y轴三角形
            gph.DrawString("三轴加速度中 X Y Z归一化值变化趋势", new Font("宋体", 14), Brushes.Black, new PointF(450, 55));//标题

            //画X轴
            gph.DrawLine(Pens.Black, cPt.X, cPt.Y, 1200, cPt.Y);
            gph.DrawPolygon(Pens.Black, xPt);        //画三角形多边形
            gph.FillPolygon(new SolidBrush(Color.Black), xPt);
            gph.DrawString("时间/s", new Font("宋体", 12), Brushes.Black, new PointF(1160, cPt.Y + 10));
            //画Y轴
            gph.DrawLine(Pens.Black, cPt.X, 650, cPt.X, 70);
            gph.DrawPolygon(Pens.Black, yPt);        //画三角形多边形
            gph.FillPolygon(new SolidBrush(Color.Black), yPt);
            gph.DrawString("加速度归一化值", new Font("宋体", 12), Brushes.Black, new PointF(0, 30));

            //标注那个颜色代表XYZ的加速度
            gph.DrawLine(Pens.Red, 1000, 55, 1020, 55);
            gph.DrawString("X轴", new Font("宋体", 12), Brushes.Black, new PointF(1022, 47));
            gph.DrawLine(Pens.Blue, 1000, 70, 1020, 70);
            gph.DrawString("Y轴", new Font("宋体", 12), Brushes.Black, new PointF(1022, 62));
            gph.DrawLine(Pens.Green, 1000, 85, 1020, 85);
            gph.DrawString("Z轴", new Font("宋体", 12), Brushes.Black, new PointF(1022, 77));

            for (float i = 1; i <= 11; i++)
            {
                //画Y/X轴刻度           
                    gph.DrawString((-0.5+(i-1) * 0.1).ToString(), new Font("宋体", 11), Brushes.Black, new PointF(cPt.X - 32, 650 - i * 50 - 6));   //Y轴刻度及数值
                    gph.DrawLine(Pens.Black, cPt.X + 5, 650 - i * 50, cPt.X, 650 - i * 50);
                    gph.DrawString(i.ToString(), new Font("宋体", 11), Brushes.Black, new PointF(cPt.X + i * 100 - 5, cPt.Y + 8));   //X轴刻度及数值
                    gph.DrawLine(Pens.Black, cPt.X + i * 100, cPt.Y, cPt.X + i * 100, cPt.Y - 5);
               
            }
        }
        public void Picture(uint count, double RawX, double RawY, double RawZ)
        {
            gph.DrawEllipse(mypenX, cPt.X + count * 5, cPt.Y - (float)RawX * 10f * 50f, 2, 2);
            gph.DrawEllipse(mypenY, cPt.X + count * 5, cPt.Y - (float)RawY * 10f * 50f, 2, 2);
            gph.DrawEllipse(mypenZ, cPt.X + count * 5, cPt.Y - (float)RawZ * 10f * 50f, 2, 2);

            if (count > 1)
            {
                gph.DrawLine(Pens.Red, cPt.X + (count - 1) * 5, cPt.Y - (float)RawXold * 10f * 50f, cPt.X + count * 5, cPt.Y - (float)RawX * 10f * 50f);
                gph.DrawLine(Pens.Blue, cPt.X + (count - 1) * 5, cPt.Y - (float)RawYold * 10f * 50f, cPt.X + count * 5, cPt.Y - (float)RawY * 10f * 50f);
                gph.DrawLine(Pens.Green, cPt.X + (count - 1) * 5, cPt.Y - (float)RawZold * 10f * 50f, cPt.X + count * 5, cPt.Y - (float)RawZ * 10f * 50f);
            }
            RawXold = RawX;
            RawYold = RawY;
            RawZold = RawZ;
        }


    }
}
