using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PInvokeGL
{
    public enum PrimitiveType : int
    {
        QUADS = GLConsts.GL_QUADS,
        LINES = GLConsts.GL_LINES,
        TRIANGLES = GLConsts.GL_TRIANGLES,
        LINE_STRIP = 0x3,
        TRIANGLE_STRIP = 0x5,
        LINE_LOOP = 0x2
    }

    public class GL
    {
        public static void Begin(PrimitiveType primitiveType)
        {
            Opengl32.glBegin((int)primitiveType);
        }

        public static void End()
        {
            Opengl32.glEnd();
        }

        public static void Vertex2(float x, float y)
        {
            Opengl32.glVertex2f(x, y);
        }

        public static void Vertex2(System.Drawing.PointF p)
        {
            Opengl32.glVertex2f(p.X, p.Y);
        }

        public static void Color(float red, float green, float blue)
        {
            Opengl32.glColor3f(red, green, blue);
        }

        public static void Color(float red, float green, float blue, float alpha)
        {
            Opengl32.glColor4f(red, green, blue, alpha);
        }

        public static void Color(System.Drawing.Color c)
        {
            Opengl32.glColor4f(c.R / 255f, c.G / 255f, c.B / 255f, c.A / 255f);
        }

        public static void TexCoord(float s, float t)
        {
            Opengl32.glTexCoord2f(s, t);
        }
    }
}
