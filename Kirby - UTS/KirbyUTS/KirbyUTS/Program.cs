using System;
using OpenTK;
using OpenTK.Windowing.Desktop;

namespace KirbyUTS
{
    class Program
    {
        static void Main(string[] args)
        {
            var nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new OpenTK.Mathematics.Vector2i(1800, 900),
                Title = "KirbyUTS"
            };
            using (var window = new Window(GameWindowSettings.Default, nativeWindowSettings))
            {
                window.Run();
            }
        }
    }
}
