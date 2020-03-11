using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Kritzel
{
    public class Matrix3x3
    {
        float[] elements = new float[] { 1, 0, 0, 1, 0, 0 };

        public float this[int ind]
        {
            get { return elements[ind]; }
            set { elements[ind] = value; }
        }

        public Matrix3x3()
        {
            
        }

        public Matrix3x3(float m11, float m12, float m21, float m22, float dx, float dy)
        {
            elements = new float[] { m11, m12, m21, m22, dx, dy };
        }

        public static Matrix3x3 operator *(Matrix3x3 a, Matrix3x3 b)
        {
            return new Matrix3x3(a[0] * b[0] + a[1] * b[2], a[0] * b[1] + a[1] * b[3],
                a[2] * b[0] + a[3] * b[2], a[2] * b[1] + a[3] * b[3],
                b[4] + a[4] * b[0] + a[5] * b[2], b[5] + a[4] * b[1] + a[5] * b[3]);
        }

        public static Matrix3x3 operator /(Matrix3x3 a, Matrix3x3 b)
        {
            b = b.GetInverse();
            return a * b;
        }

        public void Multiply(Matrix3x3 b)
        {
            Matrix3x3 a = this;
            elements = new float[] {a[0] * b[0] + a[1] * b[2], a[0] * b[1] + a[1] * b[3],
                a[2] * b[0] + a[3] * b[2], a[2] * b[1] + a[3] * b[3],
                b[4] + a[4] * b[0] + a[5] * b[2], b[5] + a[4] * b[1] + a[5] * b[3]};
        }

        /*public static Matrix3x3(Matrix3x3 a, PointF b)
        {
            return new PointF(a[0] * b.X + a[1] * b.Y, a[2] * b.X + a[3] * b.Y);
        }*/

        public static PointF operator *(PointF p, Matrix3x3 m)
        {
            return new PointF(m[4] + m[0] * p.X + m[2] * p.Y, m[5] + m[1] * p.X + m[3] * p.Y);
        }

        public Matrix3x3 GetInverse()
        {
            float a0 = elements[0];
            float a1 = elements[1];
            float a2 = elements[2];
            float a3 = elements[3];
            float ax = elements[4];
            float ay = elements[5];
            return new Matrix3x3(a3 / (a0 * a3 - a1 * a2), -a1 / (a0 * a3 - a1 * a2),
                -a2 / (a0 * a3 - a1 * a2), a0 / (a0 * a3 - a1 * a2),
                -(a3 * ax - a2 * ay) / (a0 * a3 - a1 * a2), (a1 * ax - a0 * ay) / (a0 * a3 - a1 * a2));
        }

        public Matrix CreateGdiMatrix()
        {
            Matrix m = new Matrix(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5]);
            return m;
        }

        public override string ToString()
        {
            return string.Format("[({0},{1},0)({2},{3},0),({4},{5},1)]",
                elements[0], elements[1], elements[2], elements[3], elements[4], elements[5]);
        }

        public static Matrix3x3 Translation(float x, float y)
        {
            Matrix3x3 m = new Matrix3x3();
            m[4] = x;
            m[5] = y;
            return m;
        }

        public static Matrix3x3 Scale(float x, float y)
        {
            Matrix3x3 m = new Matrix3x3();
            m[0] = x;
            m[3] = y;
            return m;
        }

        public static Matrix3x3 Scale(float s)
        {
            Matrix3x3 m = new Matrix3x3();
            m[0] = s;
            m[3] = s;
            return m;
        }

        public static Matrix3x3 Rotate(float a)
        {
            float s = (float)Math.Sin(a);
            float c = (float)Math.Cos(a);
            Matrix3x3 m = new Matrix3x3();
            m[0] = c;
            m[1] = -s;
            m[2] = s;
            m[3] = c;
            return m;
        }

        public static Matrix3x3 RotateD(float d)
        {
            return Rotate(d * (float)Math.PI / 180f);
        }

        public void Transform(ref float x, ref float y)
        {
            float nx = elements[4] + elements[0] * x + elements[2] * y;
            float ny = elements[5] + elements[1] * x + elements[3] * y;
        }

        public void Transform(ref PointF pt)
        {
            float nx = elements[4] + elements[0] * pt.X + elements[2] * pt.Y;
            float ny = elements[5] + elements[1] * pt.X + elements[3] * pt.Y;
            pt.X = nx;
            pt.Y = ny;
        }

        public void Transform(PointF[] pts)
        {
            for(int i = 0; i < pts.Length; i++)
            {
                Transform(ref pts[i]);
            }
        }

        public void TransformTranslate(float x, float y)
        {
            this.Multiply(Matrix3x3.Translation(x, y));
        }

        public void TransformScale(float s)
        {
            this.Multiply(Matrix3x3.Scale(s));
        }

        public void TransformScaleAt(float scaleX, float scaleY, float shiftX, float shiftY)
        {
            this.Multiply(Matrix3x3.Translation(-shiftX, -shiftY));
            this.Multiply(Matrix3x3.Scale(scaleX, scaleY));
            this.Multiply(Matrix3x3.Translation(shiftX, shiftY));
        }

        public void TransformRotate(float a)
        {
            this.Multiply(Matrix3x3.Rotate(a));
        }

        public void TransformRotateAt(float a, float x, float y)
        {
            this.Multiply(Matrix3x3.Translation(-x, -y));
            this.Multiply(Matrix3x3.Rotate(a));
            this.Multiply(Matrix3x3.Translation(x, y));
        }

        public Matrix3x3 Clone()
        {
            return new Matrix3x3(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5]);
        }

        public void Split(out Matrix3x3 translation, out Matrix3x3 scalerotate)
        {
            scalerotate = new Matrix3x3(elements[0], elements[1], elements[2], elements[3], 0, 0);
            translation = new Matrix3x3(1, 0, 0, 1, elements[4], elements[5]);
        }

        public void Split(out Matrix3x3 translation, out Matrix3x3 scale, out Matrix3x3 rotate)
        {
            Matrix3x3 scalerotate;
            Split(out translation, out scalerotate);
            rotate = Matrix3x3.Rotate(scalerotate.GetRotation());
            scale = scalerotate / rotate;
        }

        public PointF GetTranslation()
        {
            return new PointF(elements[4], elements[5]);
        }

        public float GetRotation()
        {
            PointF p1 = new PointF(0, 0);
            PointF p2 = new PointF(1, 0);
            this.Transform(ref p1);
            this.Transform(ref p2);
            float dx = p2.X - p1.X;
            float dy = p1.Y - p2.Y;
            return (float)Math.Atan2(dy, dx);
        }

        public float GetScale()
        {
            PointF p1 = new PointF(0, 0);
            PointF p2 = new PointF(1, 0);
            this.Transform(ref p1);
            this.Transform(ref p2);
            float dx = p2.X - p1.X;
            float dy = p1.Y - p2.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        public float[] GetColumnMatrix()
        {
            return new float[]
            {
                elements[0], elements[2],elements[4],
                elements[1], elements[3],elements[5],
                0,0,1
            };
        }

        public float[] GetRowMatrix()
        {
            return new float[]
            {
                elements[0], elements[1], 0,
                elements[2], elements[3], 0,
                elements[4], elements[5], 1
            };
        }
    }
}
