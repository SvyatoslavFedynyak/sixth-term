using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Чисельні_методи_математичної_фізики
{
    public partial class Form1 : Form
    {
        int nx, ny;
        double a, b, c, d;
        double v, E, h, R;

        int[,] MatrixNT;
        double[,] MatrixNode;
        int[] BoundaryVector;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            nx = Convert.ToInt32(textBox5.Text);
            ny = Convert.ToInt32(textBox6.Text);
            a = Convert.ToDouble(textBox1.Text);
            b = Convert.ToDouble(textBox2.Text);
            c = Convert.ToDouble(textBox3.Text);
            //d = Convert.ToDouble(textBox4.Text);
            d = Math.PI / 25;
            v = Convert.ToDouble(textBox7.Text);
            E = Convert.ToDouble(textBox8.Text);
            h = Convert.ToDouble(textBox9.Text);
            R = Convert.ToDouble(textBox10.Text);

            int NumFE = nx * ny;// кількість FE
            int NumNode = 3 * nx * ny + 2 * nx + 2 * ny + 1;// (nx + 1) * (ny + 1) + (nx * (ny + 1) + ny * (nx + 1));

            CreateMatrixNT(NumFE);
            CreateMatrixNode(NumNode);
            BoundaryVector = CreateBoundaryVector();

            dataGridView1.Columns.Add("index", "index");
            dataGridView1.Columns[0].Width = 30;
            for (int i = 1; i <= 8; ++i)
            {                                                       
                dataGridView1.Columns.Add("columns " + i.ToString(), i.ToString());
                dataGridView1.Columns[i].Width = 24;
            }

            dataGridView1.Rows.Add(NumFE);
            for (int i = 0; i < NumFE; ++i)
            {                                                 
                dataGridView1[0, i].Value = string.Format("{0}", i + 1);
                for (int j = 0; j < 8; ++j)
                {                                                       
                    dataGridView1[j + 1, i].Value = string.Format("{0}", MatrixNT[i, j] + 1);
                }
            }

                                                      
            dataGridView2.Columns.Add("columns x ", "x");
            dataGridView2.Columns.Add("columns y ", "y");
            dataGridView2.Rows.Add(NumNode);       
            for (int i = 0; i < NumNode; ++i)
            {
                dataGridView2[0, i].Value = string.Format("{0:f4}", MatrixNode[i, 0]);
                dataGridView2[1, i].Value = string.Format("{0:f4}", MatrixNode[i, 1]);
            }


            dataGridView3.Columns.Add("index", "index");
            dataGridView3.Columns[0].Width = 30;
                                                    
            dataGridView3.Columns.Add("columns ", "number");
            dataGridView3.Columns[1].Width = 24;
            
            dataGridView3.Rows.Add(NumNode);
            for (int i = 0; i < NumNode; ++i)
            {
                dataGridView3[0, i].Value = string.Format("{0}", i + 1);
                dataGridView3[1, i].Value = string.Format("{0}", BoundaryVector[i]);
            }


            textBox11.Text = (detJakobian(0, -Math.Sqrt(3.0 / 5), -Math.Sqrt(3.0 / 5))).ToString();

            double[,] matrixCL1 = MatrixCLi(1, -Math.Sqrt(3.0 / 5), -Math.Sqrt(3.0 / 5), 0);
            for (int i = 0; i < 6; ++i)
            {
                dataGridView4.Columns.Add(" ", " ");
            }
            dataGridView4.Rows.Add(11);
            for (int i = 0; i < 11; ++i)
            {
                for (int j = 0; j < 6; ++j)
                {
                    dataGridView4[j, i].Value = string.Format("{0}", matrixCL1[i, j]);
                }
            }

            double[,] matrix48_48 = Matrix_48_48(-Math.Sqrt(3.0 / 5), -Math.Sqrt(3.0 / 5), 1);
            using (StreamWriter sw = new StreamWriter("Matrix_48_48.txt"))
            {
                for (int i = 0; i < 48; ++i)
                {
                    for (int j = 0; j < 48; ++j)
                    {
                        sw.Write(String.Format("{0:E}\t", matrix48_48[i, j]));
                    }
                    sw.WriteLine();
                }
            }

            double[,] IntgrMatr = Matrix1FE(1);
            using (StreamWriter sw = new StreamWriter("Matrix1FE.txt"))
            {
                for (int i = 0; i < 48; ++i)
                {
                    for (int j = 0; j < 48; ++j)
                    {
                        sw.Write(String.Format("{0:E}\t", IntgrMatr[i, j]));
                    }
                    sw.WriteLine();
                }
            }

            double[,] GlobalMtr = GlobalMatrix();
            BoundaryCondition(GlobalMtr);
            using (StreamWriter sw = new StreamWriter("Global.txt"))
            {
                for (int i = 0; i < NumNode * 6; ++i)
                {
                    for (int j = 0; j < NumNode * 6; ++j)
                    {
                        sw.Write(String.Format("{0:E}\t", GlobalMtr[i, j]));
                    }
                    sw.WriteLine();
                }
            }
        }


        public void CreateMatrixNT(int NumFE)
        {
            MatrixNT = new int[NumFE, 8];

            int indexFE;
            if (nx < ny)
            {
                for (int i = 0; i < ny; ++i) 
                {
                    for (int j = 0; j < nx; ++j) 
                    {
                        indexFE = i * nx + j;
                        MatrixNT[indexFE, 0] = (3 * nx + 2) * i + 2 * j;
                        MatrixNT[indexFE, 1] = (3 * nx + 2) * i + 2 * j + 1;
                        MatrixNT[indexFE, 2] = (3 * nx + 2) * i + 2 * j + 2;
                        MatrixNT[indexFE, 3] = (3 * nx + 2) * i + (2 * nx + 1) + j + 1;//6
                        MatrixNT[indexFE, 4] = (3 * nx + 2) * i + (3 * nx + 2) + 2 * j + 2;//10
                        MatrixNT[indexFE, 5] = (3 * nx + 2) * i + (3 * nx + 2) + 2 * j + 1;//9
                        MatrixNT[indexFE, 6] = (3 * nx + 2) * i + (3 * nx + 2) + 2 * j;//8
                        MatrixNT[indexFE, 7] = (3 * nx + 2) * i + (2 * nx + 1) + j;//5
                    }
                }
            }
            else
            {
                for (int i = 0; i < nx; ++i)
                {
                    for (int j = 0; j < ny; ++j)
                    {
                        indexFE = i * ny + j;
                        MatrixNT[indexFE, 0] = (3 * ny + 2) * i + 2 * j;
                        MatrixNT[indexFE, 7] = (3 * ny + 2) * i + 2 * j + 1;
                        MatrixNT[indexFE, 6] = (3 * ny + 2) * i + 2 * j + 2;
                        MatrixNT[indexFE, 5] = (3 * ny + 2) * i + (2 * ny + 1) + j + 1;
                        MatrixNT[indexFE, 4] = (3 * ny + 2) * i + (3 * ny + 2) + 2 * j + 2;
                        MatrixNT[indexFE, 3] = (3 * ny + 2) * i + (3 * ny + 2) + 2 * j + 1;
                        MatrixNT[indexFE, 2] = (3 * ny + 2) * i + (3 * ny + 2) + 2 * j;
                        MatrixNT[indexFE, 1] = (3 * ny + 2) * i + (2 * ny + 1) + j;
                    }
                }
            }
        }
        public void CreateMatrixNode(int NumNode)
        {
            MatrixNode = new double[NumNode, 2];
            double hx = (b - a) / nx;
            double hy = (d - c) / ny;

            int index = 0;
            double x = a;
            double y = c;

            if (nx < ny)
            {            
                for (int j = 0; j < 2 * nx + 1; ++j) 
                {
                    MatrixNode[index, 0] = x;
                    MatrixNode[index, 1] = y;
                    x += hx/2;
                    ++index;
                }
                
                for (int i = 0; i < ny; ++i) 
                {
                    x = a;
                    y += hy/2;
                    for (int j = 0; j < nx + 1; ++j)
                    {
                        MatrixNode[index, 0] = x;
                        MatrixNode[index, 1] = y;
                        x += hx ;
                        ++index;
                    }
                    x = a;
                    y += hy / 2;
                    for (int j = 0; j < 2 * nx + 1; ++j) 
                    {
                        MatrixNode[index, 0] = x;
                        MatrixNode[index, 1] = y;
                        x += hx / 2;
                        ++index;
                    }                 
                }            
            }
            else
            {
                for (int j = 0; j < 2 * ny + 1; ++j) 
                {
                    MatrixNode[index, 0] = x;
                    MatrixNode[index, 1] = y;
                    y += hy / 2;
                    ++index;
                }
               
                for (int i = 0; i < nx; ++i)
                {
                    y = c;
                    x += hx / 2;
                    for (int j = 0; j < ny + 1; ++j)
                    {
                        MatrixNode[index, 0] = x;
                        MatrixNode[index, 1] = y;
                        y += hy;
                        ++index;
                    }
                    y = c;
                    x += hx / 2;
                    for (int j = 0; j < 2 * ny + 1; ++j)
                    {
                        MatrixNode[index, 0] = x;
                        MatrixNode[index, 1] = y;
                        y += hy / 2;
                        ++index;
                    }                    
                }
            }
        }
        public int[] CreateBoundaryVector()// по матриці вузлів, ств. матрицю крайових умов
        {
            int size = MatrixNode.GetLength(0);
            int[] BoundaryVector = new int[size];
            for(int i=0; i<size; ++i)
            {
                if(MatrixNode[i, 0] == a)
                {
                    BoundaryVector[i] += 1;
                }
                if(Math.Abs(MatrixNode[i, 0] - b) < 10e-6)
                {
                    BoundaryVector[i] += 4;
                }
                if (MatrixNode[i, 1] == c)
                {
                    BoundaryVector[i] += 7;
                }
                if (MatrixNode[i, 1] == d)
                {
                    BoundaryVector[i] += 2;
                }
            }
            return BoundaryVector;
        }


        public double Ni(int i, double E1, double E2)
        {
            double res = 0;
            switch (i)
            {
                case 0: res = -1.0 / 4 * (1 - E1) * (1 - E2) * (1 + E1 + E2);
                    break;
                case 1:
                    res = 1.0 / 2 * (1 - E1 * E1) * (1 - E2);
                    break;
                case 2:
                    res = -1.0 / 4 * (1 + E1) * (1 - E2) * (1 - E1 + E2);
                    break;
                case 3:
                    res = 1.0 / 2 * (1 - E2 * E2) * (1 + E1);
                    break;
                case 4:
                    res = -1.0 / 4 * (1 + E1) * (1 + E2) * (1 - E1 - E2);
                    break;
                case 5:
                    res = 1.0 / 2 * (1 - E1 * E1) * (1 + E2);
                    break;
                case 6:
                    res = -1.0 / 4 * (1 - E1) * (1 + E2) * (1 + E1 - E2);
                    break;
                case 7:
                    res = 1.0 / 2 * (1 - E2 * E2) * (1 - E1);
                    break;
            }
            return res;
        }
        public double dNidE1(int i, double E1, double E2)
        {
            double res = 0;
            switch (i)
            {
                case 0:
                    res = 1.0 / 4 * (1 - E2) * (2 * E1 + E2);
                    break;
                case 1:
                    res = E1 * (E2 - 1);
                    break;
                case 2:
                    res = 1.0 / 4 * (1 - E2) * (2 * E1 - E2);
                    break;
                case 3:
                    res = 1.0 / 2 * (1 - E2 * E2);
                    break;
                case 4:
                    res = 1.0 / 4 * (1 + E2) * (E2 + 2 * E1);
                    break;
                case 5:
                    res = -E1 * (1 + E2);
                    break;
                case 6:
                    res = 1.0 / 4 * (1 + E2) * (2 * E1 - E2);
                    break;
                case 7:
                    res = 1.0 / 2 * (E2 * E2 - 1);
                    break;
            }
            return res;
        }
        public double dNidE2(int i, double E1, double E2)
        {
            double res = 0;
            switch (i)
            {
                case 0:
                    res = 1.0 / 4 * (1 - E1) * (E1 + 2 * E2);
                    break;
                case 1:
                    res = -1.0 / 2 * (1 - E1 * E1);
                    break;
                case 2:
                    res = 1.0 / 4 * (1 + E1) * (2 * E2 - E1);
                    break;
                case 3:
                    res = -E2 * (1 + E1);
                    break;
                case 4:
                    res = 1.0 / 4 * (1 + E1) * (E1 + 2 * E2);
                    break;
                case 5:
                    res = 1.0 / 2 * (1 - E1 * E1);
                    break;
                case 6:
                    res = 1.0 / 4 * (1 - E1) * (2 * E2 - E1);
                    break;
                case 7:
                    res = -E2 * (1 - E1);
                    break;
            }
            return res;
        }

        public double detJakobian(int indexFE, double x, double y)// indexFE - номерFE; x,y - координати; 
        {
            double sum = 0;
            for (int i = 0; i < 8; ++i) 
            {
                for (int j = 0; j < 8; ++j)
                {
                    if (i != j)
                    {
                        sum += dNidE1(i, x, y) * dNidE2(j, x, y) * (MatrixNode[MatrixNT[indexFE, i], 0] * MatrixNode[MatrixNT[indexFE, j], 1] - MatrixNode[MatrixNT[indexFE, j], 0] * MatrixNode[MatrixNT[indexFE, i], 1]);
                    }
                }
            }
            return sum;
        }
        public double dNida1(int i, double x, double y, int indexFE)
        {
            double sum1 = 0;
            double sum2 = 0;

            for (int k = 0; k < 8; ++k) 
            {                               
                sum1 += dNidE2(k, x, y) * MatrixNode[MatrixNT[indexFE, k], 1];
                sum2 += dNidE1(k, x, y) * MatrixNode[MatrixNT[indexFE, k], 1];
            }
            double res = sum1 * dNidE1(i, x, y) - sum2 * dNidE2(i, x, y);

            return res / detJakobian(indexFE, x, y);
        }
        public double dNida2(int i, double x, double y, int indexFE)
        {
            double sum1 = 0;
            double sum2 = 0;

            for (int k = 0; k < 8; ++k)
            {
                sum1 += dNidE2(k, x, y) * MatrixNode[MatrixNT[indexFE, k], 0];
                sum2 += dNidE1(k, x, y) * MatrixNode[MatrixNT[indexFE, k], 0];
            }
            double res = -sum1 * dNidE1(i, x, y) + sum2 * dNidE2(i, x, y);

            return res / detJakobian(indexFE, x, y);
        }

        public double[,] MultMatrix(double[,] matrix1, double[,] matrix2)
        {
            int n1 = matrix1.GetLength(0);
            int m1 = matrix1.GetLength(1);
            int n2 = matrix2.GetLength(0);
            int m2 = matrix2.GetLength(1);

            if (m1 != n2)
            {
                throw new ArgumentException("Не можна помножити матриці!");
            }

            double[,] res = new double[n1, m2];
            for (int i = 0; i < n1; ++i)
            {
                for (int j = 0; j < m2; ++j)
                {
                    double sum = 0;
                    for (int k = 0; k < n2; ++k)
                    {
                        sum += matrix1[i, k] * matrix2[k, j];
                    }
                    res[i, j] = sum;
                }
            }
            return res;
        }
        public double[,] TransponMatrix(double[,] matrix1)
        {
            int n = matrix1.GetLength(0);
            int m = matrix1.GetLength(1);

            double[,] res = new double[m, n];
            for (int i = 0; i < m; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    res[i, j] = matrix1[j, i];
                }
            }
            return res;
        }

        public double[,] MatrixB()
        {
            double[,] resMatrix = new double[11, 11];

            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    if (i == j)
                    {
                        resMatrix[i, j] = (1 - v) * E * h / ((1 + v) * (1 - 2 * v));
                    }
                    else
                    {
                        resMatrix[i, j] = v * E * h / ((1 + v) * (1 - 2 * v));
                    }
                }
            }

            for (int i = 3; i < 6; ++i)
            {
                resMatrix[i, i] = 2 * E * h / (1 + v);
            }

            for (int i = 6; i < 8; ++i)
            {
                for (int j = 6; j < 8; ++j)
                {
                    if (i == j)
                    {
                        resMatrix[i, j] = (1 - v) * E * Math.Pow(h, 3) / (12 * (1 + v) * (1 - 2 * v));
                    }
                    else
                    {
                        resMatrix[i, j] = v * E * Math.Pow(h, 3) / (12 * (1 + v) * (1 - 2 * v));
                    }
                }
            }

            for (int i = 8; i < 11; ++i)
            {
                resMatrix[i, i] = 2 * E * Math.Pow(h, 3) / (12 * (1 + v));
            }

            return resMatrix;
        }
        public double[,] MatrixCLi(int i, double E1, double E2, int indexFE)
        {
            double[,] resMatrix = new double[11, 6];

            resMatrix[0, 0] = dNida1(i, E1, E2, indexFE);
            resMatrix[1, 1] = dNida2(i, E1, E2, indexFE) / R;
            resMatrix[1, 2] = Ni(i, E1, E2) / R;
            resMatrix[2, 5] = Ni(i, E1, E2);
            resMatrix[3, 0] = dNida2(i, E1, E2, indexFE) / (2 * R);
            resMatrix[3, 1] = dNida1(i, E1, E2, indexFE) / 2;
            resMatrix[4, 2] = dNida1(i, E1, E2, indexFE) / 2;
            resMatrix[4, 3] = Ni(i, E1, E2) / 2;
            resMatrix[5, 1] = -Ni(i, E1, E2) / (2 * R);
            resMatrix[5, 2] = dNida2(i, E1, E2, indexFE) / (2 * R);
            resMatrix[5, 4] = Ni(i, E1, E2) / 2;
            resMatrix[6, 3] = dNida1(i, E1, E2, indexFE);
            resMatrix[7, 4] = dNida2(i, E1, E2, indexFE) / R;
            resMatrix[7, 5] = Ni(i, E1, E2) / R;
            resMatrix[8, 1] = dNida1(i, E1, E2, indexFE) / (2 * R);
            resMatrix[8, 3] = dNida2(i, E1, E2, indexFE) / (2 * R);
            resMatrix[8, 4] = dNida1(i, E1, E2, indexFE) / 2;
            resMatrix[9, 5] = dNida1(i, E1, E2, indexFE) / 2;
            resMatrix[10, 5] = dNida2(i, E1, E2, indexFE) / (2 * R);

            return resMatrix;
        }
        public double[,] Kij(int i, int j, double E1, double E2, int indexFE)// 6*6
        {
            double[,] matrix1 = MultMatrix(MatrixB(), MatrixCLi(j, E1, E2, indexFE));
            double[,] matrix2 = TransponMatrix(MatrixCLi(i, E1, E2, indexFE));
            return MultMatrix(matrix2, matrix1);
        }

        public double[,] Matrix_48_48(double E1, double E2, int indexFE)// В конкретному вузлі Гаусса
        {
            double[,] resMatrix = new double[48, 48];

            for (int i = 0; i < 8; ++i)
            {
                for (int j = 0; j < 8; ++j)
                {
                    for (int k = 0; k < 6; ++k)
                    {
                        for (int l = 0; l < 6; ++l)
                        {
                            resMatrix[i * 6 + k, j * 6 + l] = Kij(i, j, E1, E2, indexFE)[k, l]; 
                        }
                    }
                }
            }
            return resMatrix;
        }

        public double[,] IntegrateKij(int i1, int j1, int indexFE)
        {
            double[,] res = new double[6, 6];

            double[] t = new double[3];
            t[0] = -Math.Sqrt(3.0 / 5);
            t[1] = 0;
            t[2] = Math.Sqrt(3.0 / 5);

            double[] c = new double[3];
            c[0] = 5.0 / 9;
            c[1] = 8.0 / 9;
            c[2] = 5.0 / 9;

            for (int k = 0; k < 6; ++k)
            {
                for (int l = 0; l < 6; ++l)
                {
                    res[k, l] = 0;
                    for (int i = 0; i < 3; ++i)
                    {
                        for (int j = 0; j < 3; ++j)
                        {                            
                            res[k, l] += Kij(i1, j1, t[i], t[j], indexFE)[k, l] * detJakobian(indexFE, t[i], t[j]) * c[i] * c[j] * R;
                        }
                    }
                }
            }
            return res;
        }
        public double[,] Matrix1FE(int indexFE)
        {
            double[,] resMatrix = new double[48, 48];

            for (int i = 0; i < 8; ++i)
            {
                for (int j = 0; j < 8; ++j)
                {
                    for (int k = 0; k < 6; ++k)
                    {
                        for (int l = 0; l < 6; ++l)
                        {
                            resMatrix[i * 6 + k, j * 6 + l] = IntegrateKij(i, j, indexFE)[k, l];
                        }
                    }
                }
            }
            return resMatrix;
        }

        public double[,] GlobalMatrix()
        {
            int sizeMatrix = MatrixNode.GetLength(0) * 6;
            double[,] res = new double[sizeMatrix, sizeMatrix];

            int countFE = MatrixNT.GetLength(0);
            for (int i = 0; i < countFE; ++i)
            {
                double[,] matrix = Matrix1FE(i); 
                for (int k = 0; k < 8; ++k)
                {
                    int numNode_k = MatrixNT[i, k];
                    for (int l = 0; l < 8; ++l)
                    {
                        int numNode_l = MatrixNT[i, l];
                        for (int p = 0; p < 6; ++p)
                        {
                            for (int q = 0; q < 6; ++q)
                            {
                                res[numNode_k * 6 + p, numNode_l * 6 + q] += matrix[k * 6 + p, l * 6 + q];
                            }
                        }
                    }
                }
            }
            return res;
        }
        public void BoundaryCondition(double[,] glbMatrix)
        {
            for (int i = 0; i < BoundaryVector.Length; ++i)
            {
                switch (BoundaryVector[i])
                {                 
                    case 1:
                        {
                            glbMatrix[i * 6, i * 6] = double.MaxValue;
                            glbMatrix[i * 6 + 3, i * 6 + 3] = double.MaxValue;
                            break;
                        }
                    case 2:
                    case 7:
                        {
                            glbMatrix[i * 6 + 1, i * 6 + 1] = double.MaxValue;
                            glbMatrix[i * 6 + 4, i * 6 + 4] = double.MaxValue;
                            break;
                        }
                    case 4:
                        {
                            glbMatrix[i * 6 + 2, i * 6 + 2] = double.MaxValue;
                            glbMatrix[i * 6 + 5, i * 6 + 5] = double.MaxValue;
                            break;
                        }
                    case 3:
                    case 8:
                        {
                            glbMatrix[i * 6, i * 6] = double.MaxValue;
                            glbMatrix[i * 6 + 3, i * 6 + 3] = double.MaxValue;
                            glbMatrix[i * 6 + 1, i * 6 + 1] = double.MaxValue;
                            glbMatrix[i * 6 + 4, i * 6 + 4] = double.MaxValue;
                            break;
                        }
                    case 6:
                    case 11:
                        {
                            glbMatrix[i * 6 + 2, i * 6 + 2] = double.MaxValue;
                            glbMatrix[i * 6 + 5, i * 6 + 5] = double.MaxValue;
                            glbMatrix[i * 6 + 1, i * 6 + 1] = double.MaxValue;
                            glbMatrix[i * 6 + 4, i * 6 + 4] = double.MaxValue;
                            break;
                        }
                }
            }
        }
    }
}
