﻿using System;

namespace MupdfSharp
{
	public struct Point { public float X, Y; }
	public struct BBox
	{
		public int Left, Top, Right, Bottom;
	}
	public struct Rectangle
	{
		public float Left, Top, Right, Bottom;
		public bool IsEmpty { get { return Left == Right || Top == Bottom; } }
		public bool IsInfinite { get { return Left > Right || Top > Bottom; } }
	}
	public struct Matrix
	{
		public float A, B, C, D, E, F;
		public Matrix (float a, float b, float c, float d, float e, float f) {
			this.A = a;
			this.B = b;
			this.C = c;
			this.D = d;
			this.E = e;
			this.F = f;
		}

		public static readonly Matrix Identity = new Matrix (1, 0, 0, 1, 0, 0);
		private static float Min4 (float a, float b, float c, float d) {
			return Math.Min (Math.Min (a, b), Math.Min (c, d));
		}
		private static float Max4 (float a, float b, float c, float d) {
			return Math.Max (Math.Max (a, b), Math.Max (c, d));
		}
		public static Matrix Concat (Matrix one, Matrix two) {
			return new Matrix (
				one.A * two.A + one.B * two.C,
				one.A * two.B + one.B * two.D,
				one.C * two.A + one.D * two.C,
				one.C * two.B + one.D * two.D,
				one.E * two.A + one.F * two.C + two.E,
				one.E * two.B + one.F * two.D + two.F);
		}
		public static Matrix Scale (float x, float y) {
			return new Matrix (x, 0, 0, y, 0, 0);
		}
		public Matrix ScaleTo (float x, float y) {
			return Concat (this, Scale (x, y));
		}
		public static Matrix Shear (float h, float v) {
			return new Matrix (1, v, h, 1, 0, 0);
		}
		public Matrix ShearTo (float x, float y) {
			return Concat (this, Shear (x, y));
		}
		public static Matrix Rotate (float theta) {
			float s;
			float c;

			while (theta < 0)
				theta += 360;
			while (theta >= 360)
				theta -= 360;

			if (Math.Abs (0 - theta) < Single.Epsilon) {
				s = 0;
				c = 1;
			}
			else if (Math.Abs (90.0f - theta) < Single.Epsilon) {
				s = 1;
				c = 0;
			}
			else if (Math.Abs (180.0f - theta) < Single.Epsilon) {
				s = 0;
				c = -1;
			}
			else if (Math.Abs (270.0f - theta) < Single.Epsilon) {
				s = -1;
				c = 0;
			}
			else {
				s = (float)Math.Sin (theta * Math.PI / 180f);
				c = (float)Math.Cos (theta * Math.PI / 180f);
			}

			return new Matrix (c, s, -s, c, 0, 0);
		}
		public Matrix RotateTo (float theta) {
			return Concat (this, Rotate (theta));
		}
		public static Matrix Translate (float tx, float ty) {
			return new Matrix (1, 0, 0, 1, tx, ty);
		}
		public Matrix TranslateTo (float tx, float ty) {
			return Concat (this, Translate (tx, ty));
		}
		public Point Transform (Point p) {
			Point t;
			t.X = p.X * this.A + p.Y * this.C + this.E;
			t.Y = p.X * this.B + p.Y * this.D + this.F;
			return t;
		}
		public Rectangle Transform (Rectangle rect) {
			Point s, t, u, v;

			if (rect.IsInfinite)
				return rect;

			s.X = rect.Left; s.Y = rect.Top;
			t.X = rect.Left; t.Y = rect.Bottom;
			u.X = rect.Right; u.Y = rect.Bottom;
			v.X = rect.Right; v.Y = rect.Top;
			s = this.Transform (s);
			t = this.Transform (t);
			u = this.Transform (u);
			v = this.Transform (v);
			rect.Left = Min4 (s.X, t.X, u.X, v.X);
			rect.Top = Min4 (s.Y, t.Y, u.Y, v.Y);
			rect.Right = Max4 (s.X, t.X, u.X, v.X);
			rect.Bottom = Max4 (s.Y, t.Y, u.Y, v.Y);
			return rect;
		}

	}
	struct NativePage
	{
		public Matrix Ctm;
		public Rectangle MediaBox;
		public int Rotate;
	}

}
