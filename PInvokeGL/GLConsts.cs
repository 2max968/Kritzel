using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PInvokeGL
{
    public class GLConsts
    {
        public const uint GL_COLOR_BUFFER_BIT = 0x4000;
        public const uint GL_DEPTH_BUFFER_BIT = 0x100;
        public const uint GL_ACCUM_BUFFER_BIT = 0x200;
        public const uint GL_STENCIL_BUFFER_BIT = 0x400;

        public const int GL_TRIANGLES = 0x4;
        public const int GL_QUADS = 0x7;
        public const int GL_LINES = 0x1;

        public const int GL_TEXTURE_2D = 0xDE1;
        public const int GL_RGBA = 0x1908;
        public const int GL_UNSIGNED_BYTE = 0x1401;
        public const int GL_TEXTURE_MAG_FILTER = 0x2800;
        public const int GL_TEXTURE_MIN_FILTER = 0x2801;
        public const int GL_NEAREST = 0x2600;
        public const int GL_LINEAR = 0x2601;

        public const int GL_FRAMEBUFFER = 0x8D40;
        public const int GL_COLOR_ATTACHMENT0 = 0x8CE0;
        public const int GL_FRAMEBUFFER_COMPLETE = 0x8CD5;
        public const int GL_DRAW_FRAMEBUFFER = 0x8CA9;
        public const int GL_READ_FRAMEBUFFER = 0x8CA8;

        public const int GL_MODELVIEW = 0x1700;
        public const int GL_PROJECTION = 0x1701;

        public const int GL_BLEND = 0xBE2;
        public const int GL_MULTISAMPLE = 0x809D;

        public const int GL_SRC_COLOR = 0x300;
        public const int GL_ONE_MINUS_SRC_COLOR = 0x301;
        public const int GL_SRC_ALPHA = 0x302;
        public const int GL_ONE_MINUS_SRC_ALPHA = 0x303;
    }
}
