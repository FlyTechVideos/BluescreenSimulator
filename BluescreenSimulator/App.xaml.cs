using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using BluescreenSimulator.ViewModels;
using BluescreenSimulator.Views;
using static BluescreenSimulator.Views.MainWindow;
namespace BluescreenSimulator
{
    public partial class App : Application
    {
        private const int AttachParentProcess = -1;

        [DllImport("kernel32", SetLastError = true)]
        private static extern bool AttachConsole(int dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern bool FreeConsole();


        private void Application_Startup(object sender, EventArgs e)
        {

            var args = Environment.GetCommandLineArgs();
            if (args.Length > 1) // #0 is file path
            {
                var bluescreenData = new BluescreenDataViewModel();
                var showHelp = false;
                var enableUnsafe = false;

                var p = new OptionSet
                {
                    { "e|emoticon=", "{Text} for Emoticon", t => bluescreenData.Emoticon = t },
                    { "m1|main1=", "{Text} for Main Text (Line 1)", t => bluescreenData.MainText1 = t },
                    { "m2|main2=", "{Text} for Main Text (Line 2)", t => bluescreenData.MainText2 = t },
                    { "p|progress=", "{Text} for Progress (\"complete\")", t => bluescreenData.Complete = t },
                    { "mi|moreinfo=", "{Text} for More Info", t => bluescreenData.MoreInfo = t },
                    { "s|supportperson=", "{Text} for Support Person", t => bluescreenData.SupportPerson = t },
                    { "sc|stopcode=", "{Text} for Stop code", t => bluescreenData.StopCode = t },
                    { "b|background=", "Background color in rgb {value} hex format (#FFFFFF)", r => bluescreenData.BackgroundColor = (Color)(ColorConverter.ConvertFromString(r) ?? bluescreenData.BackgroundColor) },
                    { "f|foreground=", "Foreground (text) in rgb {value} hex format (#FFFFFF)", r => bluescreenData.ForegroundColor = (Color)(ColorConverter.ConvertFromString(r) ?? bluescreenData.ForegroundColor) },
                    { "oq|origqr", "Use original QR code", o => bluescreenData.UseOriginalQR = o != null },
                    { "hq|hideqr", "Hides the QR code", h => bluescreenData.HideQR = h != null },
                    { "d|delay=", "Bluescreen Delay {duration} in seconds (0-86400)", (int d) => {
                        if (d > 86400)
                        {
                            throw new OptionException("Delay maximum is 86400 seconds (24 hours)", "d|delay=");
                        }
                        bluescreenData.Delay = d;
                    }},
                    { "c|cmd=", "The {command} to run after complete (Careful!)", c => { bluescreenData.CmdCommand = c; bluescreenData.EnableUnsafe = true; } },
                    { "r|rainbow", "Enable rainbow mode (discards background color settings)", r => bluescreenData.RainbowMode = r != null },
                    { "u|enable-unsafe",  "Enable unsafe mode (forces GUI mode and discards all other settings)", eu => enableUnsafe = eu != null },
                    { "h|help",  "Show this message and exit", h => showHelp = h != null }
                };

                List<string> extra;
                try
                {
                    extra = p.Parse(args); // todo use it actually?
                }
                catch (OptionException ex)
                {
                    if (AttachConsole(AttachParentProcess))
                    {
                        Console.WriteLine("\n");
                        Console.Write("BluescreenSimulator: ");
                        Console.WriteLine(ex.Message);
                        Console.WriteLine("Try `--help' for more information.");
                        Console.WriteLine();
                        FreeConsole();
                    }
                    Shutdown(1);
                    return;
                }

                if (showHelp)
                {
                    ShowHelp(p);
                    return;
                }

                if (enableUnsafe)
                {
                    MessageBox.Show("You are entering Unsafe Mode. Be careful!", "Careful", MessageBoxButton.OK, MessageBoxImage.Warning);
                    RunGui(true);
                }
                else
                {
                    void Act() => ShowBluescreenWindow(bluescreenData);
                    bluescreenData.ExecuteCommand.Execute((Action) Act);
                }
            }
            else
            {
                RunGui(false);
            }
        }

        private void RunGui(bool enableUnsafe)
        {
            var mainWindow = new MainWindow(enableUnsafe);
            mainWindow.Show();
        }

        private void ShowHelp(OptionSet p)
        {

            if (AttachConsole(AttachParentProcess))
            {
                Console.WriteLine("\n");
                Console.WriteLine("BluescreenSimulator Options:");
                p.WriteOptionDescriptions(Console.Out);
                Console.WriteLine();
                Console.WriteLine("IMPORTANT:");
                Console.WriteLine(" - If you specify a delay and want to abort, you need to kill the process yourself.");
                Console.WriteLine(" - To use spaces in your text, wrap your text in quotes \"like this\".");
                Console.WriteLine();
                FreeConsole();
            }
            Shutdown();
        }

    }
}
