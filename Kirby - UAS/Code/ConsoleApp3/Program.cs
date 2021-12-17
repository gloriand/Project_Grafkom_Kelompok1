using LearnOpenTK.Common;
using OpenTK.Windowing.Desktop;
using System;

namespace Grafkom
{
	class Program
	{
		static void Main(string[] args)
		{
			var nativeWindowSettings = new NativeWindowSettings()
			{
				Size = new OpenTK.Mathematics.Vector2i(1800, 900),
				Title = "Grafkom"
			};
			using (var window = new Window(GameWindowSettings.Default, nativeWindowSettings))
			{
				window.Run();
			}
		}
	}
}
