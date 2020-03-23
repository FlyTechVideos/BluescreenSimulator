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
            string log = "";


            log = log + "Creating cmd.exe Process variable \n";
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
            log = log + "Starting cmd.exe \n";
            cmd.Start();
            log = log + "writing commands into cmd:  \n";
            foreach (var command in commands)
            {
                log = log + command + "\n";
                cmd.StandardInput.WriteLine(command);
                cmd.StandardInput.Flush();
            }
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            log = log + "cmd exited, output is following: \n";
            log = log + cmd.StandardOutput.ReadToEnd() + "\n";
            log = log + "finished";
            File.WriteAllText("debug.txt", log);
            Console.WriteLine(cmd.StandardOutput.ReadToEnd());
        }

        public static void SetWindowToScreen(Window window, Screen screen)
        {
            window.Left = screen.Bounds.Left + 1;
            window.Top = 0;
        }
    }
}
