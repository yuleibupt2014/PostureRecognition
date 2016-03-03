
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Globalization;
using Chronos_As_Read;
using System.Collections;
namespace SVM
{
    /// <summary>
    /// Encapsulates a problem, or set of vectors which must be classified.
    /// </summary>
	[Serializable]
	public class Problem
	{
        private int _count;
        private double[] _Y;
        private Node[][] _X;
        private int _maxIndex;
     
        public ArrayList alist = new ArrayList(180);
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="count">Number of vectors</param>
        /// <param name="y">The class labels</param>
        /// <param name="x">Vector data.</param>
        /// <param name="maxIndex">Maximum index for a vector</param>
        public Problem(int count, double[] y, Node[][] x, int maxIndex)
        {
            _count = count;
            _Y = y;
            _X = x;
            _maxIndex = maxIndex;
        }
        /// <summary>
        /// Empty Constructor.  Nothing is initialized.
        /// </summary>
        public Problem()
        {
        }
        /// <summary>
        /// Number of vectors.
        /// </summary>
        public int Count
        {
            get
            {
                return _count;
            }
            set
            {
                _count = value;
            }
        }
        /// <summary>
        /// Class labels.
        /// </summary>
        public double[] Y
        {
            get
            {
                return _Y;
            }
            set
            {
                _Y = value;
            }
        }
        /// <summary>
        /// Vector data.
        /// </summary>
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
        /// <summary>
        /// Maximum index for a vector.
        /// </summary>
        public int MaxIndex
        {
            get
            {
                return _maxIndex;
            }
            set
            {
                _maxIndex = value;
            }
        }

        /// <summary>
        /// Reads a problem from a stream.
        /// </summary>
        /// <param name="stream">Stream to read from</param>
        /// <returns>The problem</returns>
        //public static Problem Read(Stream stream)
        //{
        //    TemporaryCulture.Start();

        //    StreamReader input = new StreamReader(stream);
        //    List<double> vy = new List<double>();
        //    List<Node[]> vx = new List<Node[]>();
        //    int max_index = 0;
           

        //    while (input.Peek() > -1)
        //    {
        //        string[] parts = input.ReadLine().Trim().Split();
        //        char[] separator={':'};

        //        vy.Add(double.Parse(parts[0]));
        //        int m = 180;//parts.Length - 1;      !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //        Node[] x = new Node[m];
        //      //  Form1 form1 = new Form1();
        //       // ArrayList hehe = form1.blist();
        //        for (int j = 0; j < m; j++)
        //        {
        //            x[j] = new Node();
        //           // string[] nodeParts = parts[2+j*2].Split(separator);
        //            x[j].Index = j;                    ///X��j��.index����ڼ�������   
        //            x[j].Value =  Convert.ToDouble(Form1.list[j]);      //X��j��.value����ڼ�������ֵ  !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //        }
        //        if (m > 0)
        //            max_index = Math.Max(max_index, x[m - 1].Index);
        //        vx.Add(x);
        //    }

        //    TemporaryCulture.Stop();

        //    return new Problem(vy.Count, vy.ToArray(), vx.ToArray(), max_index);
        //}

        public static Problem Read(Stream stream)
        {
            TemporaryCulture.Start();

            StreamReader input = new StreamReader(stream);
            List<double> vy = new List<double>();
            List<Node[]> vx = new List<Node[]>();
            int max_index = 0;

            while (input.Peek() > -1)
            {
                string[] parts = input.ReadLine().Trim().Split();
                char[] separator = { ':' };

                vy.Add(double.Parse(parts[0]));
                int m = 180;//parts.Length - 1;      !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                Node[] x = new Node[m];
                for (int j = 0; j < m; j++)
                {
                    x[j] = new Node();
                    string[] nodeParts = parts[2 + j * 2].Split(separator);
                    x[j].Index = int.Parse(nodeParts[0]);                    ///X��j��.index����ڼ�������   


                    x[j].Value = double.Parse(nodeParts[1]);      //X��j��.value����ڼ�������ֵ
                }
                if (m > 0)
                    max_index = Math.Max(max_index, x[m - 1].Index);
                vx.Add(x);
            }

            TemporaryCulture.Stop();

            return new Problem(vy.Count, vy.ToArray(), vx.ToArray(), max_index);
        }

        /// <summary>
        /// Writes a problem to a stream.
        /// </summary>
        /// <param name="stream">The stream to write the problem to.</param>
        /// <param name="problem">The problem to write.</param>
        public static void Write(Stream stream, Problem problem)
        {
            TemporaryCulture.Start();

            StreamWriter output = new StreamWriter(stream);
            for (int i = 0; i < problem.Count; i++)
            {
                output.Write(problem.Y[i]);
                for (int j = 0; j < problem.X[i].Length; j++)
                    output.Write(" {0}:{1}", problem.X[i][j].Index, problem.X[i][j].Value);
                output.WriteLine();
            }
            output.Flush();

            TemporaryCulture.Stop();
        }

        /// <summary>
        /// Reads a Problem from a file.
        /// </summary>
        /// <param name="filename">The file to read from.</param>
        /// <returns>the Probem</returns>
        public static Problem Read(string filename)
        {
            FileStream input = File.OpenRead(filename);
            try
            {
                return Read(input);
            }
            finally
            {
                input.Close();
            }
        }

        /// <summary>
        /// Writes a problem to a file.   This will overwrite any previous data in the file.
        /// </summary>
        /// <param name="filename">The file to write to</param>
        /// <param name="problem">The problem to write</param>
        public static void Write(string filename, Problem problem)
        {
            FileStream output = File.Open(filename, FileMode.Create);
            try
            {
                Write(output, problem);
            }
            finally
            {
                output.Close();
            }
        }
    }
}