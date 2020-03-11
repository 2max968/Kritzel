g++ -DGLEW_STATIC nativegl.cpp -o nativegl.dll -shared -Wall -O2 -I./include -L./lib -lglfw3 -lglew32 -lopengl32 -lkernel32 -luser32 -lgdi32
copy nativegl.dll ..\PInvokeGL\nativegl.dll /Y