using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using BluescreenSimulator.ViewModels;

namespace BluescreenSimulator.Views
{
    public partial class MainWindow : Window
    {
        private bool enableUnsafe;
        private BluescreenDataViewModel _vm;
        public MainWindow(bool enableUnsafe)
        {
            InitializeComponent();
            DataContext = _vm = new BluescreenDataViewModel();
            this.enableUnsafe = enableUnsafe;

            var title = "BluescreenSimulator v2.0";
            if (enableUnsafe)
            {
                title += " (Unsafe Mode)";
            }

            MainWindowFrame.Title = title;

            Closing += WarnClose;
        }

        private void ShowBSOD(object sender, RoutedEventArgs e)
        {
            if (CheckData())
            {
                _vm.ExecuteCommand.Execute(ShowBluescreen);
            }
        }

        private void OpenAbout(object sender, RoutedEventArgs e)
        {
            var about = new About();
            about.Show();
        }

        private void GenerateExe(object sender, RoutedEventArgs e)
        {
            var command = GenerateCommand();
            if (command == null)
            {
                return;
            }

            var path = Environment.GetCommandLineArgs()[0];
            var filenameStartIndex = path.LastIndexOf('\\') + 1;
            var folder = path.Substring(0, filenameStartIndex);
            var filename = path.Substring(filenameStartIndex);

            var exeCreator = new ExeCreator {Owner = this, CommandBox = {Text = filename + " " + command}};
            exeCreator.ShowDialog();

            if (exeCreator.DialogResult != true)
            {
                return;
            }

            var desiredFilename = exeCreator.FileName.Text;
            if (string.IsNullOrEmpty(desiredFilename))
            {
                return;
            }

            var iexpressSED =
                $@"
                [Version]
                Class=IEXPRESS
                SEDVersion=3
                [Options]
                PackagePurpose=InstallApp
                ShowInstallProgramWindow=1
                HideExtractAnimation=1
                UseLongFileName=1
                InsideCompressed=0
                RebootMode=N
                TargetName={folder}\{desiredFilename}.exe
                AppLaunched=cmd /c %FILE0% {command}
                PostInstallCmd=<None>
                SourceFiles=SourceFiles
                [Strings]
                FILE0=""{filename}""
                [SourceFiles]
                SourceFiles0 = {folder}
                [SourceFiles0]
                %FILE0%=";


            var SEDPath = Path.GetTempPath() + "\\optionfile.SED";

            File.WriteAllText(SEDPath, iexpressSED);
            Utils.ExecuteCmdCommands($"iexpress /N {SEDPath}");
            File.Delete(SEDPath);

            MessageBox.Show("Your EXE-File has been created.", "BluescreenWindow Simulator", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private string GenerateCommand()
        {
            var success = CheckData();
            if (!success) return null;
            return _vm.CreateCommandParameters();
        }
        private void WarnClose(object sender, CancelEventArgs e)
        {
            if (_vm.IsWaiting)
            {
                var messageBoxResult = MessageBox.Show("Do you want to exit? The scheduled BSOD will remain scheduled. If you want to interrupt it, you have to kill the process.",
                    "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        public Action ShowBluescreen => ShowBlueScreenImpl;
        
        private void ShowBlueScreenImpl()
        {
            ShowBluescreenWindow(_vm);
        }

        public static void ShowBluescreenWindow(BluescreenDataViewModel vm)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (var s in System.Windows.Forms.Screen.AllScreens)
                {
                    var bluescreenView = new BluescreenWindow(vm);
                    ShowOnMonitor(s, bluescreenView);
                }
            });
        }

        private static void ShowOnMonitor(System.Windows.Forms.Screen screen, Window window)
        {
            window.WindowStyle = WindowStyle.None;
            window.WindowStartupLocation = WindowStartupLocation.Manual;

            window.WindowState = WindowState.Normal;

            window.Left = screen.Bounds.Left;
            window.Top = screen.Bounds.Top;

            window.SourceInitialized += (snd, arg) => window.WindowState = WindowState.Maximized;

            window.Show();
        }

        private bool CheckData()
        {          
            if (_vm.EnableUnsafe && !string.IsNullOrEmpty(CmdCommand.Text.Trim()))
            {
                var messageBoxResult = MessageBox.Show("Using a CMD command can be dangerous. " +
                    "I will not be responsible for any data loss or other damage arising from irresponsible or careless use of the CMD command option. " +
                    "Please re-check your command to make sure that you execute what you intended:\r\n\r\n" + CmdCommand.Text.Trim() + "\r\n\r\n" + "Do you want to proceed?",
                    "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (messageBoxResult == MessageBoxResult.No)
                {
                    return false;
                }
            }
            return true;
        }

    }
}
