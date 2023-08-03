using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using BluescreenSimulator.Properties;
using BluescreenSimulator.ViewModels;
using BluescreenSimulator.Views;

namespace BluescreenSimulator
{
    public partial class App : Application
    {
        private const int AttachParentProcess = -1;

        [DllImport("kernel32", SetLastError = true)]
        private static extern bool AttachConsole(int dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern bool FreeConsole();

        private StatefulResourceDictionary DarkThemeDictionary
            => Resources.MergedDictionaries.OfType<StatefulResourceDictionary>().FirstOrDefault();

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            Settings.Default.Save();
        }

        private void Application_Startup(object sender, EventArgs e)
        {
            void SetTheme() => DarkThemeDictionary.IsEnabled = Settings.Default.IsDarkTheme;

            Settings.Default.Upgrade();
            Settings.Default.PropertyChanged +=
                (_, __) => SetTheme();
            SetTheme();
            DispatcherUnhandledException += (o, eventArgs) =>
            {
                var m = ShowErrorMessage(eventArgs.Exception);
                eventArgs.Handled = m != MessageBoxResult.Cancel || m != MessageBoxResult.No;
            };
            AppDomain.CurrentDomain.UnhandledException +=
                delegate (object o, UnhandledExceptionEventArgs eventArgs)
                {
                    ShowErrorMessage(eventArgs.ExceptionObject as Exception);
                };
            var args = Environment.GetCommandLineArgs();
            if (args.Length > 1) // #0 is file path
            {
                bool showHelp = false;
                void AddHelp(OptionSet set)
                {
                    set.Add("h|help", "Show this message and exit", h => showHelp = h != null);
                }
                if (args[1] == "--read-command-file")
                {
                    Process p = new Process();
                    p.StartInfo.FileName = args[0];
                    p.StartInfo.Arguments = File.ReadAllText("command");
                    //p.StartInfo.CreateNoWindow = true;
                    //p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    p.Start();
                    p.WaitForExit();
                    Environment.Exit(0);
                }

                IBluescreenViewModel data = null;
                Type type = null;
                var currentSet = CmdParameterAttribute.GetBaseOptionSet(t => type = t, out var bluescreenOptions);
                AddHelp(currentSet);
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
                    data = (IBluescreenViewModel)Activator.CreateInstance(type);
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
                }
                else
                {
                    data.ShowView();
                }
            }
            else
            {
                RunGui();
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

        private void RunGui()
        {
            var mainWindow = new MainWindow();
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