using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace BluescreenSimulator
{
    public static class Utils
    {

        public static void ShowOnMonitor(this Window window, Screen targetScreen)
        {
            window.WindowStyle = WindowStyle.None;
            window.WindowStartupLocation = WindowStartupLocation.Manual;

            window.WindowState = WindowState.Normal;

            window.Left = targetScreen.Bounds.Left;
            window.Top = targetScreen.Bounds.Top;

            window.SourceInitialized += (snd, arg) => window.WindowState = WindowState.Maximized;

            window.Show();
        }

        public static void ExecuteCmdCommands(params string[] commands)
        {
            var cmd = new Process
            {
                StartInfo =
                {
                    FileName = "cmd.exe",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    UseShellExecute = false
                }
            };
            cmd.Start();
            foreach (var command in commands)
            {
                cmd.StandardInput.WriteLine(command);
                cmd.StandardInput.Flush();
            }
            cmd.StandardInput.Close();
        }

        public static void SetWindowToScreen(Window window, Screen screen)
        {
            window.Left = screen.Bounds.Left + 1;
            window.Top = 0;
        }
    }
}
