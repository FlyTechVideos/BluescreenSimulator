using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;

namespace BluescreenSimulator
{
    class Utils
    {

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
            cmd.WaitForExit();
            Console.WriteLine(cmd.StandardOutput.ReadToEnd());
        }

        public static void SetWindowToScreen(Window window, Screen screen)
        {
            window.Left = screen.Bounds.Left + 1;
            window.Top = 0;
        } 
    }
}
