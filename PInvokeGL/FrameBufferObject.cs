using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PInvokeGL
{
    public class FrameBufferObject : IDisposable
    {
        public int Texture { get; private set; }
        public uint FBO { get; private set; }
        public uint WRITE_FBO { get; private set; } = 0;
        public bool Disposed { get; private set; } = false;
        int texture1;
        bool msaa = false;
        int width, height;

        public FrameBufferObject(int width, int height, int samples = 0)
        {
            Console.WriteLine("Create FBO {0}", Thread.CurrentThread.ManagedThreadId);
            this.width = width;
            this.height = height;
            this.msaa = samples > 0;
            if (!this.msaa)
            {
                Texture = Util.LoadTexture(IntPtr.Zero, width, height);
                FBO = NativeGL.nglCreateFBO(Texture, width, height);
            }
            else
            {
                int tex = 0;
                texture1 = Util.LoadTexture(IntPtr.Zero, width, height);
                FBO = NativeGL.nglCreateFBOMultisampling(ref tex, width, height, samples);
                WRITE_FBO = NativeGL.nglCreateFBO(texture1, width, height);
                this.Texture = texture1;
            }
        }

        ~FrameBufferObject()
        {
            if (!Disposed)
                Util.Leackage(this);
        }

        public void Dispose()
        {
            if (Disposed) return;
            Disposed = true;
            NativeGL.nglDeleteFramebuffer(FBO);
            Opengl32.glDeleteTextures(1, new int[] { Texture });
        }

        public void Bind()
        {
            NativeGL.nglBindFramebuffer(FBO);
        }

        public void Unbind()
        {
            NativeGL.nglBindFramebuffer(0);
        }

        public void BindTexture()
        {
            Opengl32.glBindTexture(GLConsts.GL_TEXTURE_2D, Texture);
        }

        public void Blit()
        {
            if (msaa)
            {
                NativeGL.nglBlitFBO(FBO, WRITE_FBO, width, height);
            }
        }
    }
}
