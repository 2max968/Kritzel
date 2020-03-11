using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LineLib
{
    public class MatrixNxM
    {
        public int Columns { get; private set; }
        public int Rows { get; private set; }
        public int Length
        {
            get
            {
                return Rows * Columns;
            }
        }

        double[,] data;

        public double this[int row, int column]
        {
            get
            {
                return data[row, column];
            }
            set
            {
                data[row, column] = value;
            }
        }

        public double this[int i]
        {
            get
            {
                int x = i % Columns;
                int y = i / Columns;
                return data[y, x];
            }
            set
            {
                int x = i % Columns;
                int y = i / Columns;
                data[y, x] = value;
            }
        }

        public MatrixNxM(int rows, int columns)
        {
            this.Columns = columns;
            this.Rows = rows;
            data = new double[rows, columns];
        }

        public MatrixNxM(int rows, int columns, double value)
            : this(rows, columns)
        {
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    this[y, x] = value;
                }
            }
        }

        public MatrixNxM(int columns, params double[] elements)
        {
            float rowsF = elements.Length / (float)columns;
            int rowsI = elements.Length / columns;
            if (rowsF != rowsI)
                throw new Exception("Parameter count not matching");

            this.Columns = columns;
            this.Rows = rowsI;
            this.data = new double[this.Rows, this.Columns];

            for (int r = 0; r < rowsI; r++)
            {
                for (int i = 0; i < columns; i++)
                {
                    this[r, i] = elements[r * columns + i];
                }
            }
        }

        public static MatrixNxM CreateIdentity(int size)
        {
            return CreateIdentity(size, size);
        }

        public static MatrixNxM CreateIdentity(int rows, int columns)
        {
            MatrixNxM m = new MatrixNxM(rows, columns);
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    m[y, x] = (y == x) ? 1 : 0;
                }
            }
            return m;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            for (int y = 0; y < Rows; y++)
            {
                sb.Append("[");
                for (int x = 0; x < Columns; x++)
                {
                    sb.Append(data[y, x]);
                    if (x < Columns - 1)
                        sb.Append(",");
                }
                sb.Append("]");
            }
            sb.Append("]");
            return sb.ToString();
        }

        public string ToString2D()
        {
            StringBuilder sb = new StringBuilder();
            for (int y = 0; y < Rows; y++)
            {
                sb.Append("|");
                for (int x = 0; x < Columns; x++)
                {
                    sb.Append(data[y, x]);
                    if (x < Columns - 1)
                        sb.Append(",\t");
                }
                sb.AppendLine("|");
            }
            return sb.ToString();
        }

        public static MatrixNxM Add(MatrixNxM a, MatrixNxM b)
        {
            if (a.Columns != b.Columns || a.Rows != b.Rows)
                throw new MatrixDimensionMismatchException(a, b);
            MatrixNxM c = new MatrixNxM(a.Rows, a.Columns);
            for (int x = 0; x < c.Columns; x++)
            {
                for (int y = 0; y < c.Rows; y++)
                {
                    c[y, x] = a[y, x] + b[y, x];
                }
            }
            return c;
        }

        public static MatrixNxM Multiply(MatrixNxM a, MatrixNxM b)
        {
            if (a.Columns != b.Rows)
                throw new MatrixDimensionMismatchException(a, b);
            MatrixNxM c = new MatrixNxM(a.Rows, b.Columns);

            for (int x = 0; x < c.Columns; x++)
            {
                for (int y = 0; y < c.Rows; y++)
                {
                    double sum = 0;
                    for (int i = 0; i < a.Columns; i++)
                    {
                        double sa = a[y, i];
                        double sb = b[i, x];
                        sum += sa * sb;
                    }
                    c[y, x] = sum;
                }
            }

            return c;
        }

        public MatrixNxM GetSubmatrix(int y, int x)
        {
            MatrixNxM sub = new MatrixNxM(Rows - 1, Columns - 1);
            for (int j = 0; j < sub.Columns; j++)
            {
                for (int k = 0; k < sub.Rows; k++)
                {
                    int n = j;
                    int m = k;
                    if (n >= x) n++;
                    if (m >= y) m++;
                    sub[k, j] = this[m, n];
                }
            }
            return sub;
        }

        public double GetDeterminant()
        {
            if (Rows != Columns)
                throw new MatrixDimensionExceptionNonSquare(this);

            if (Rows == 0 && Columns == 0)
                return 0;

            if (Rows == 1 && Columns == 1)
                return this[0, 0];

            if (Rows == 2 && Columns == 2)
                return this[0, 0] * this[1, 1] - this[0, 1] * this[1, 0];

            double sum = 0;
            for (int i = 0; i < Columns; i++)
            {
                if (this[0, i] == 0) continue;
                MatrixNxM sub = this.GetSubmatrix(0, i);
                int factor = (i % 2 == 0) ? 1 : -1;
                sum += factor * this[0, i] * sub.GetDeterminant();
            }
            return sum;
        }

        public MatrixNxM GetInverse()
        {
            if (Rows != Columns)
                throw new MatrixDimensionExceptionNonSquare(this);

            double det = this.GetDeterminant();
            MatrixNxM inv = new MatrixNxM(Rows, Columns);
            for(int x = 0; x < Columns; x++)
            {
                for(int y = 0; y < Rows; y++)
                {
                    MatrixNxM sub = this.GetSubmatrix(x, y);
                    int f1 = (x % 2 == 0) ? 1 : -1;
                    int f2 = (y % 2 == 0) ? 1 : -1;
                    inv[y, x] = f1 * f2 * sub.GetDeterminant() / det;
                }
            }

            return inv;
        }

        public static MatrixNxM CreateDiag(params double[] vect)
        {
            MatrixNxM mat = new MatrixNxM(vect.Length, vect.Length, 0);
            for(int i = 0; i < vect.Length; i++)
            {
                mat[i, i] = vect[i];
            }
            return mat;
        }

        public static MatrixNxM CreateDiagOffset(int offset, params double[] vect)
        {
            int size = vect.Length + Math.Abs(offset);
            MatrixNxM mat = new MatrixNxM(size, size);
            int x = Math.Max(0, offset);
            int y = Math.Max(0, -offset);
            for(int i = 0;i < vect.Length; i++)
            {
                mat[i + y, i + x] = vect[i];
            }
            return mat;
        }

        public static MatrixNxM CreateDiagSingleVal(int length, double val)
        {
            MatrixNxM mat = new MatrixNxM(length, length, 0);
            for (int i = 0; i < length; i++)
            {
                mat[i, i] = val;
            }
            return mat;
        }

        public static MatrixNxM CreateDiagOffsetSingleVal(int length, int offset, double val)
        {
            int size = length + Math.Abs(offset);
            MatrixNxM mat = new MatrixNxM(size, size);
            int x = Math.Max(0, offset);
            int y = Math.Max(0, -offset);
            for (int i = 0; i < length; i++)
            {
                mat[i + y, i + x] = val;
            }
            return mat;
        }

        public static MatrixNxM operator +(MatrixNxM a, MatrixNxM b)
        {
            return MatrixNxM.Add(a, b);
        }

        public static MatrixNxM operator *(MatrixNxM a, MatrixNxM b)
        {
            return MatrixNxM.Multiply(a, b);
        }

        public static MatrixNxM operator *(MatrixNxM a, double b)
        {
            MatrixNxM c = new MatrixNxM(a.Rows, a.Columns);
            for (int x = 0; x < a.Columns; x++)
            {
                for (int y = 0; y < a.Rows; y++)
                {
                    c[y, x] = a[y, x] * b;
                }
            }
            return c;
        }

        public static MatrixNxM operator *(double a, MatrixNxM b)
        {
            return b * a;
        }

        public static MatrixNxM operator /(MatrixNxM a, double b)
        {
            return a * (1.0 / b);
        }
    }
}
