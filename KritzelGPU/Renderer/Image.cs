using PInvokeGL;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if SLIMDX
using d2d = SlimDX.Direct2D;
using SlimDX;
#endif
using gdi = System.Drawing;

namespace Kritzel.Renderer
{
    public class Image : IDisposable
    {
        public gdi.Bitmap GdiBitmap { get; private set; }
#if SLIMDX
        public d2d.Bitmap GPUBitmap { get; private set; }
#endif
        public bool IsDisposed { get; private set; } = false;
        public int GLTextureID { get; private set; }

        public Image(gdi.Bitmap bmp)
        {
            this.GdiBitmap = bmp;
            GLTextureID = 0;
#if SLIMDX
            GPUBitmap = null;
#endif
        }

        ~Image()
        {
            if (GLTextureID != 0)
                PInvokeGL.Util.Leackage(this);
        }

#if SLIMDX
        public static d2d.Bitmap LoadBitmap(d2d.RenderTarget renderTarget, gdi.Bitmap gdiBmp)
        {
            d2d.Bitmap result = null;

            //Lock the gdi resource
            BitmapData drawingBitmapData = gdiBmp.LockBits(
                new gdi.Rectangle(0, 0, gdiBmp.Width, gdiBmp.Height),
                ImageLockMode.ReadOnly, gdi.Imaging.PixelFormat.Format32bppPArgb);

            //Prepare loading the image from gdi resource
            DataStream dataStream = new DataStream(
                drawingBitmapData.Scan0,
                drawingBitmapData.Stride * drawingBitmapData.Height,
                true, false);
            d2d.BitmapProperties properties = new d2d.BitmapProperties();
            properties.PixelFormat = new SlimDX.Direct2D.PixelFormat(
                SlimDX.DXGI.Format.B8G8R8A8_UNorm,
                d2d.AlphaMode.Premultiplied);

            //Load the image from the gdi resource
            result = new d2d.Bitmap(
                renderTarget,
                new gdi.Size(gdiBmp.Width, gdiBmp.Height),
                dataStream, drawingBitmapData.Stride,
                properties);

            //Unlock the gdi resource
            gdiBmp.UnlockBits(drawingBitmapData);

            return result;
        }
#endif

        public void Dispose()
        {
            if (IsDisposed) return;
            IsDisposed = true;
            GdiBitmap.Dispose();
            UnloadGPU();

#if SLIMDX
            if(GPUBitmap != null) GPUBitmap.Dispose();
#endif
        }

        public void UnloadGPU()
        {
            if (GLTextureID != 0)
            {
                int id = GLTextureID;
                Opengl32.glDeleteTextures(1, ref id);
                GLTextureID = 0;
            }
        }

        public void LoadGPU()
        {
            UnloadGPU();
            int id = PInvokeGL.Util.LoadTexture(GdiBitmap);
            GLTextureID = id;
        }

#if SLIMDX
        public void UnloadGPU()
        {
            d2d.Bitmap tmp = GPUBitmap;
            GPUBitmap = null;
            tmp?.Dispose();
        }

        public bool LoadGPU(d2d.RenderTarget renderTarget)
        {
            if (IsDisposed) return false;
            GPUBitmap?.Dispose();
            GPUBitmap = LoadBitmap(renderTarget, GdiBitmap);
            return true;
        }
#endif
    }
}
