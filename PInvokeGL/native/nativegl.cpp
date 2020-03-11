#include <GL/glew.h>
#include <GLFW/glfw3.h>
#include <windows.h>
#include <inttypes.h>
#include <stdio.h>

#define EXPORT(type) __declspec(dllexport) type __cdecl

bool initialized = false;

extern "C"
{
    #include "shader.cpp"

    EXPORT(int) nglInit()
    {
        if(initialized) return 0;
        initialized = true;
        printf("NGL Initialize.");
        return glewInit();
    }

    EXPORT(GLuint) nglCreateFBO(int tex, int width, int height)
    {
        GLuint FBO = 0;
        glGenFramebuffers(1, &FBO);
        glBindFramebuffer(GL_FRAMEBUFFER, FBO);
        glFramebufferTexture(GL_FRAMEBUFFER, GL_COLOR_ATTACHMENT0, tex, 0);
        GLenum DrawBuffers[1] = {GL_COLOR_ATTACHMENT0};
        glDrawBuffers(1, DrawBuffers);
        glBindFramebuffer(GL_FRAMEBUFFER, FBO);
        glViewport(0,0, width, height);
        if(glCheckFramebufferStatus(GL_FRAMEBUFFER) !=GL_FRAMEBUFFER_COMPLETE)
        {
            return 0;
        }
        return FBO;
    }

    EXPORT(GLuint) nglCreateFBOMultisampling(GLuint *tex, int width, int height, int samples)
    {
        *tex = 0;
        glGenTextures(1, tex);
        glBindTexture(GL_TEXTURE_2D_MULTISAMPLE, *tex);
        glTexImage2DMultisample(GL_TEXTURE_2D_MULTISAMPLE, samples, GL_RGBA, width, height, GL_FALSE);
        
        GLuint FBO = 0;
        glGenFramebuffers(1, &FBO);
        glBindFramebuffer(GL_FRAMEBUFFER, FBO);
        glFramebufferTexture(GL_FRAMEBUFFER, GL_COLOR_ATTACHMENT0, *tex, 0);
        GLenum DrawBuffers[1] = {GL_COLOR_ATTACHMENT0};
        glDrawBuffers(1, DrawBuffers);
        glBindFramebuffer(GL_FRAMEBUFFER, FBO);
        glViewport(0,0, width, height);
        if(glCheckFramebufferStatus(GL_FRAMEBUFFER) !=GL_FRAMEBUFFER_COMPLETE)
        {
            return 0;
        }
        return FBO;
    }

    EXPORT(void) nglBlitFBO(GLuint fbo_read, GLuint fbo_draw, int width, int height)
    {
        glBindTexture(GL_TEXTURE_2D, 0);
        glBindFramebufferEXT(GL_READ_FRAMEBUFFER_EXT, fbo_read);
        glBindFramebufferEXT(GL_DRAW_FRAMEBUFFER_EXT, fbo_draw);
        glBlitFramebufferEXT(0, 0, width, height,
                             0, 0, width, height,
                             GL_COLOR_BUFFER_BIT,
                             GL_NEAREST);

        glBindFramebufferEXT(GL_READ_FRAMEBUFFER_EXT, 0);
        glBindFramebufferEXT(GL_DRAW_FRAMEBUFFER_EXT, 0);
    }

    EXPORT(void) nglBindFramebuffer(GLuint fbo)
    {
        glBindFramebuffer(GL_FRAMEBUFFER, fbo);
    }

    EXPORT(void) nglBindFramebufferExt(int target, GLuint fbo)
    {
        glBindFramebuffer(target, fbo);
    }

    EXPORT(void) nglDeleteFramebuffer(GLuint fbo)
    {
        auto fboTemp = fbo;
        glDeleteFramebuffers(1, &fboTemp);
    }

    EXPORT(void) nglVertex2f(int n, float *x, float *y)
    {
        for(int i = 0; i < n; i++)
        {
            glVertex2f(x[i], y[i]);
        }
    }

    EXPORT(void) nglVertex2fTrans(int n, float *x, float *y, float dx, float dy, float fx, float fy)
    {
        for(int i = 0; i < n; i++)
        {
            glVertex2f(dx + fx * x[i], dy + fy * y[i]);
        }
    }

    EXPORT(void) nglVertexf(int n, float *v)
    {
        for(int i = 0; i < n; i+=2)
        {
            glVertex2f(v[i], v[i+1]);
        }
    }

    EXPORT(void) nglARGBtoRGBA(uint32_t* dst, uint32_t* src, int pixels)
    {
        for(int i = 0; i < pixels; i++)
        {
            dst[i] = (src[i] & 0xff00ff00) | ((src[i] & 0xff) << 16) | ((src[i] & 0xff0000) >> 16);
        }
    }

    EXPORT(void) nglProjection(int w, int h)
    {
        glViewport(0, 0, w, h);
        glMatrixMode(GL_PROJECTION);
        glLoadIdentity();
        glOrtho(0, w, h, 0, -1, 1);
        glMatrixMode(GL_MODELVIEW);
    }

    EXPORT(void) nglLineWidth(float w)
    {
        glLineWidthx(w);
    }
}