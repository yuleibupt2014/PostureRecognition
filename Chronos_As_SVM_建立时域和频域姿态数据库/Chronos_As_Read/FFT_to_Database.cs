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
    class FFT_to_Database
    {
        uint sheet_count = 0;
        uint flag = 0;
        uint label_count = 0;
        
        static Excel.Application xapp = new Excel.Application();
        static string filepath1 = @"C:\Users\Administrator\Desktop\训练样例数据库.xlsx";
        static string filepath2 = @"C:\Users\Administrator\Desktop\训练样例频域数据库.xlsx";
        Excel.Workbook xbook1 = xapp.Workbooks._Open(filepath1, Type.Missing, false, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        Excel.Workbook xbook2 = xapp.Workbooks._Open(filepath2, Type.Missing, false, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        FFT fft = new FFT();
        double[] data_r=new double[180];
        double[] data_i=new double[180];
        double[] result_r=new double[180];
        double[] result_i=new double[180];

        public void Convert_to_FFT()
        {
            for (uint sheet_count = 1; sheet_count <=12; sheet_count++)
            {
                for (uint label_count = 1; label_count <= 50; label_count++)
                {
                    for (uint i = 0; i < 180; i++)
                    {
                        data_r[i] = xbook1.Sheets[sheet_count].Cells[label_count, i + 1].Value;
                    }
                    fft.Dit2_FFT(ref data_r, ref data_i, ref result_r, ref result_i);
                    for (int j = 0; j < 180; j++)
                    {
                        xbook2.Sheets[sheet_count].Cells[label_count, j + 1] = result_r[j];
                    }
                }
            }

            for (uint sheet_count = 28; sheet_count <= 31; sheet_count++)
            {
                for (uint label_count = 1; label_count <= 50; label_count++)
                {
                    for (uint i = 0; i < 180; i++)
                    {
                        data_r[i] = xbook1.Sheets[sheet_count].Cells[label_count, i + 1].Value;
                    }
                    fft.Dit2_FFT(ref data_r, ref data_i, ref result_r, ref result_i);
                    for (int j = 0; j < 180; j++)
                    {
                        xbook2.Sheets[sheet_count].Cells[label_count, j + 1] = result_r[j];
                    }
                }
            }
           // xbook.Save();//(@"C:\Users\Administrator\Desktop\训练样例数据库.xlsx");
            xbook2.Save();//(@"C:\Users\Administrator\Desktop\训练样例频域数据库.xlsx");
            xapp.Quit();
            }
        }
}

