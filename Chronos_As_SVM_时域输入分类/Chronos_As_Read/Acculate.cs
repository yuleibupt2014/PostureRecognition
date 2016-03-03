using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Chronos_As_Read
{
    class Acculate
    {
        double sumX = 0, sumY = 0, sumZ = 0, pingjunX=0, pingjunY=0, pingjunZ=0;
        double zhongjianbiaoX=0, zhongjianbiaoY=0, zhongjianbiaoZ = 0;   
        double zhongjianpianX=0, zhongjianpianY=0, zhongjianpianZ = 0;
        double zhongjianfengX=0, zhongjianfengY=0, zhongjianfengZ = 0;
        double zhongjianxiangXY = 0, zhongjianxiangXZ = 0, zhongjianxiangYZ = 0;
        double zhongjianxiang2X = 0, zhongjianxiang2Y = 0, zhongjianxiang2Z = 0;

        //double biaozhunchaX = 0, biaozhunchaY=0, biaozhunchaZ=0;
        //double pianduX = 0,pianduY = 0, pianduZ = 0;
        //double fengduX = 0, fengduY = 0, fengduZ = 0;
        //double xiangguanduXY=0, xiangguanduXZ=0, xiangguanduYZ=0;

        static ArrayList Xarray = new ArrayList(60);
        static ArrayList Yarray = new ArrayList(60);
        static ArrayList Zarray = new ArrayList(60);
        public Acculate(ArrayList xarray, ArrayList yarray, ArrayList zarray)
        {
            sumX = 0; sumY = 0; sumZ = 0; pingjunX = 0; pingjunY = 0; pingjunZ = 0;
            zhongjianbiaoX = 0; zhongjianbiaoY = 0; zhongjianbiaoZ = 0;
            zhongjianpianX = 0; zhongjianpianY = 0; zhongjianpianZ = 0;
            zhongjianfengX = 0; zhongjianfengY = 0; zhongjianfengZ = 0;
            zhongjianxiangXY = 0; zhongjianxiangXZ = 0; zhongjianxiangYZ = 0;
            zhongjianxiang2X = 0; zhongjianxiang2Y = 0; zhongjianxiang2Z = 0;
            Xarray = xarray; Yarray = yarray; Zarray = zarray;
            this.sumorpingjun();
            
        }
        ////////////////////////////////////////////////////////////////////计算和与平均值
        public void sumorpingjun()
        {
            foreach (double element in Xarray)
            {
                sumX += element;
            }
            foreach (double element in Yarray)
            {
                sumY += element;
            }
            foreach (double element in Zarray)
            {
                sumZ += element;
            }
            pingjunX = sumX / 60;
            pingjunY = sumY / 60;
            pingjunZ = sumZ / 60;
        }
        ////////////////////////////////////////////////////////////////////计算标准差
        public void biaozhuncha(out double biaozhunchaX, out double biaozhunchaY, out double biaozhunchaZ)
        {           
            for (int i = 0; i <= 59; i++)
            {
                zhongjianbiaoX += System.Math.Pow((Convert.ToDouble(Xarray[i]) - pingjunX), 2);
                zhongjianbiaoY += System.Math.Pow((Convert.ToDouble(Yarray[i]) - pingjunY), 2);
                zhongjianbiaoZ += System.Math.Pow((Convert.ToDouble(Zarray[i]) - pingjunZ), 2);
            }
            biaozhunchaX = System.Math.Sqrt(zhongjianbiaoX / 60);
            biaozhunchaY = System.Math.Sqrt(zhongjianbiaoY / 60);
            biaozhunchaZ = System.Math.Sqrt(zhongjianbiaoZ / 60);
        }
        ///////////////////////////////////////////////////////////////////////////////////计算偏度
        public void piandu(out double pianduX, out double pianduY, out double pianduZ, double BiaozhunchaX,  double BiaozhunchaY, double BiaozhunchaZ)
        {
            for (int i = 0; i <= 59; i++)
            {
                zhongjianpianX += System.Math.Pow(((Convert.ToDouble(Xarray[i]) - pingjunX)), 3);
                zhongjianpianY += System.Math.Pow(((Convert.ToDouble(Yarray[i]) - pingjunY)), 3);
                zhongjianpianZ += System.Math.Pow(((Convert.ToDouble(Zarray[i]) - pingjunZ)), 3);
            }
            pianduX = (zhongjianpianX * 60d) / (59d * 58d * BiaozhunchaX * BiaozhunchaX * BiaozhunchaX);
            pianduY = (zhongjianpianY * 60d) / (59d * 58d * BiaozhunchaY * BiaozhunchaY * BiaozhunchaY);
            pianduZ = (zhongjianpianZ * 60d) / (59d * 58d * BiaozhunchaZ * BiaozhunchaZ * BiaozhunchaZ);
        }
        /////////////////////////////////////////////////////////////////////////////////计算峰度
        public void fengdu(out double fengduX, out double fengduY, out double fengduZ, double BiaozhunchaX, double BiaozhunchaY, double BiaozhunchaZ)
        {
            for (int i = 0; i <= 59; i++)
            {
                zhongjianfengX += System.Math.Pow(((Convert.ToDouble(Xarray[i]) - pingjunX)), 4);
                zhongjianfengY += System.Math.Pow(((Convert.ToDouble(Yarray[i]) - pingjunY)), 4);
                zhongjianfengZ += System.Math.Pow(((Convert.ToDouble(Zarray[i]) - pingjunZ)), 4);
            }
            fengduX = (zhongjianfengX ) / (60d * BiaozhunchaX * BiaozhunchaX * BiaozhunchaX*BiaozhunchaX);
            fengduY = (zhongjianfengY ) / (60d * BiaozhunchaY * BiaozhunchaY * BiaozhunchaY*BiaozhunchaY);
            fengduZ = (zhongjianfengZ ) / (60d * BiaozhunchaZ * BiaozhunchaZ * BiaozhunchaZ*BiaozhunchaZ);
        }
        /////////////////////////////////////////////////////////////////////////////////计算相关度
        public void xiangguandu(out double xiangguanduXY,out double xiangguanduXZ,out double xiangguanduYZ)
        {
            for(int i = 0; i <= 59; i++)
            {
                zhongjianxiangXY += (Convert.ToDouble(Xarray[i]) - pingjunX) * (Convert.ToDouble(Yarray[i]) - pingjunY);
                zhongjianxiangXZ += (Convert.ToDouble(Xarray[i]) - pingjunX) * (Convert.ToDouble(Zarray[i]) - pingjunZ);
                zhongjianxiangYZ += (Convert.ToDouble(Yarray[i]) - pingjunY) * (Convert.ToDouble(Zarray[i]) - pingjunZ);

                zhongjianxiang2X += System.Math.Pow(((Convert.ToDouble(Xarray[i]) - pingjunX)), 2);
                zhongjianxiang2Y += System.Math.Pow(((Convert.ToDouble(Yarray[i]) - pingjunY)), 2);
                zhongjianxiang2Z += System.Math.Pow(((Convert.ToDouble(Zarray[i]) - pingjunZ)), 2);
            }
            xiangguanduXY = zhongjianxiangXY / (Math.Sqrt(zhongjianxiang2X * zhongjianxiang2Y));
            xiangguanduXZ = zhongjianxiangXZ / (Math.Sqrt(zhongjianxiang2X * zhongjianxiang2Z));
            xiangguanduYZ = zhongjianxiangYZ /( Math.Sqrt(zhongjianxiang2Y * zhongjianxiang2Z));
        }
    }
}
