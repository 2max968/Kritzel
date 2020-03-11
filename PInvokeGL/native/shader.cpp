#include <GL/glew.h>
#include <GLFW/glfw3.h>
#include <string.h>

#define EXPORT(type) __declspec(dllexport) type __cdecl

char infoLog[512];

#define SHADER_NOERROR          0
#define SHADER_ERROR_VERTEX     1
#define SHADER_ERROR_FRAGMENT   2
#define SHADER_ERROR_LINK       3

EXPORT(int) nglCreateProgram(GLuint *shader, char* vertex, char* fragment)
{
    GLuint prog = glCreateProgram();
    int success;

    // Vertex Shader
    GLuint vShader = glCreateShader(GL_VERTEX_SHADER);
    glShaderSource(vShader, 1, &vertex, NULL);
    glCompileShader(vShader);
    glGetShaderiv(vShader, GL_COMPILE_STATUS, &success);
    if(!success)
    {
        glGetShaderInfoLog(vShader, 512, NULL, infoLog);
        return SHADER_ERROR_VERTEX;
    }
    glAttachShader(prog, vShader);
    glDeleteShader(vShader);

    // Fragment Shader
    GLuint fShader = glCreateShader(GL_FRAGMENT_SHADER);
    glShaderSource(fShader, 1, &fragment, NULL);
    glCompileShader(fShader);
    glGetShaderiv(fShader, GL_COMPILE_STATUS, &success);
    if(!success)
    {
        glGetShaderInfoLog(fShader, 512, NULL, infoLog);
        return SHADER_ERROR_FRAGMENT;
    }
    glAttachShader(prog, fShader);
    glDeleteShader(fShader);

    // Link Program
    glLinkProgram(prog);
    glGetProgramiv(prog, GL_LINK_STATUS, &success);
    if(!success)
    {
        glGetProgramInfoLog(prog, 512, NULL, infoLog);
        return SHADER_ERROR_LINK;
    }

    *shader = prog;
    return SHADER_NOERROR;
}

EXPORT(void) nglUseProgram(GLuint prog)
{
    glUseProgram(prog);
}

EXPORT(char*) nglGetInfoLog(int* len)
{
    *len = strlen(infoLog);
    return infoLog;
}