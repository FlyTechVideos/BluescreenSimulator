using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using BluescreenSimulator.Properties;
using BluescreenSimulator.ViewModels;
using BluescreenSimulator.Views;
using Resolution;
using Color = System.Windows.Media.Color;
using ColorConverter = System.Windows.Media.ColorConverter;

namespace BluescreenSimulator
{
    public partial class App : Application
    {
        private const int AttachParentProcess = -1;

        [DllImport("kernel32", SetLastError = true)]
        private static extern bool AttachConsole(int dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern bool FreeConsole();

        private StatefulResourceDictionary _darkThemeDictionary 
            => Resources.MergedDictionaries.FirstOrDefault(r => r is StatefulResourceDictionary) as StatefulResourceDictionary;

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            Settings.Default.Save();
        }

        private void Application_Startup(object sender, EventArgs e)
        {
            void SetTheme() => _darkThemeDictionary.IsEnabled = Settings.Default.IsDarkTheme;
           
            Settings.Default.Upgrade();
            Settings.Default.PropertyChanged +=
                (_, __) => SetTheme();
            SetTheme();
            DispatcherUnhandledException += (o, eventArgs) =>
            {
                var m = ShowErrorMessage(eventArgs.Exception);
                eventArgs.Handled = m != MessageBoxResult.Cancel || m != MessageBoxResult.No;
                CResolution.ResetResolution();
            };
            AppDomain.CurrentDomain.UnhandledException +=
                delegate (object o, UnhandledExceptionEventArgs eventArgs)
                {
                    ShowErrorMessage(eventArgs.ExceptionObject as Exception);
                    CResolution.ResetResolution();
                };
            var args = Environment.GetCommandLineArgs();
            if (args.Length > 1) // #0 is file path
            {
                bool showHelp = false;
                void AddHelp(OptionSet set)
                {
                    set.Add("h|help", "Show this message and exit", h => showHelp = h != null);
                }

                IBluescreenViewModel data = null;
                Type type = null;
                var currentSet = CmdParameterAttribute.GetBaseOptionSet(t => type = t, out var bluescreenOptions);
                AddHelp(currentSet);
                //var p = new OptionSet
                //{
                //    { "e|emoticon=", "{Text} for Emoticon", t => bluescreenData.Emoticon = t },
                //    { "m1|main1=", "{Text} for Main Text (Line 1)", t => bluescreenData.MainText1 = t },
                //    { "m2|main2=", "{Text} for Main Text (Line 2)", t => bluescreenData.MainText2 = t },
                //    { "p|progress=", "{Text} for Progress (\"complete\")", t => bluescreenData.Complete = t },
                //    { "mi|moreinfo=", "{Text} for More Info", t => bluescreenData.MoreInfo = t },
                //    { "s|supportperson=", "{Text} for Support Person", t => bluescreenData.SupportPerson = t },
                //    { "sc|stopcode=", "{Text} for Stop code", t => bluescreenData.StopCode = t },
                //    { "b|background=", "Background color in rgb {value} hex format (#FFFFFF)", r => bluescreenData.BackgroundColor = TryGetColor(r, bluescreenData.BackgroundColor) },
                //    { "f|foreground=", "Foreground (text) in rgb {value} hex format (#FFFFFF)", r => bluescreenData.ForegroundColor = TryGetColor(r, bluescreenData.ForegroundColor) },
                //    { "oq|origqr", "Use original QR code", o => bluescreenData.UseOriginalQR = o != null },
                //    { "hq|hideqr", "Hides the QR code", h => bluescreenData.HideQR = h != null },
                //    { "d|delay=", "Bluescreen Delay {duration} in seconds (0-86400)", (int d) => {
                //        if (d > 86400)
                //        {
                //            throw new OptionException("Delay maximum is 86400 seconds (24 hours)", "d|delay=");
                //        }
                //        bluescreenData.Delay = d;
                //    }},
                //    { "c|cmd=", "The {command} to run after complete (Careful!)", c => { bluescreenData.CmdCommand = c; bluescreenData.EnableUnsafe = true; } },
                //    { "r|rainbow", "Enable rainbow mode (discards background color settings)", r => bluescreenData.RainbowMode = r != null },
                //    { "u|enable-unsafe",  "Enable unsafe mode (forces GUI mode and discards all other settings)", eu => enableUnsafe = eu != null },
                //    { "h|help",  "Show this message and exit", h => showHelp = h != null }
                //};
                try
                {
                    foreach (var option in bluescreenOptions)
                    {
                        if (args.Contains(option.Value))
                        {
                            type = option.Key;
                            break;
                        }
                    }
                    if (type is null)
                    {
                        showHelp = true;
                        goto showHelp;
                    }
                    data = Activator.CreateInstance(type) as IBluescreenViewModel;
                    currentSet = CmdParameterAttribute.GetOptionSetFor(type, data);
                    if (showHelp) goto showHelp;
                    AddHelp(currentSet);
                    currentSet.Parse(args);
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
                showHelp:
                if (showHelp)
                {
                    ShowHelp(currentSet);
                    return;
                }

                if (data.EnableUnsafe)
                {
                    MessageBox.Show("You are entering Unsafe Mode. Be careful!", "Careful", MessageBoxButton.OK, MessageBoxImage.Warning);
                    RunGui(true);
                }
                else
                {
                    data.ShowView();
                }
            }
            else
            {
                RunGui(false);
            }
        }
        private static MessageBoxResult ShowErrorMessage(Exception ex)
        {
            string MaxLines(string s, int i)
            {
                return s.Split('\n').Take(i).Aggregate((first, second) => $"{first}\n{second}");
            }
            return MessageBox.Show($"Sorry, some error occured, {ex} ; StackTrace: \n {MaxLines(ex.StackTrace, 4)}\n Do you want the app to continue running?", "Oops",
                MessageBoxButton.OKCancel, MessageBoxImage.Error);
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
