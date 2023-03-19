using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using BluescreenSimulator.Properties;
using BluescreenSimulator.ViewModels;

namespace BluescreenSimulator.Views
{
    public partial class MainWindow : Window
    {
        public static bool EnableUnsafe;
        private MainWindowViewModel _vm;
        private IBluescreenViewModel CurrentBluescreen => _vm.SelectedBluescreen;
        public MainWindow(bool enableUnsafe)
        {
            InitializeComponent();
            DataContext = _vm = new MainWindowViewModel();
            EnableUnsafe = enableUnsafe;
            InputBindings.Add(new InputBinding(new DelegateCommand(() =>
            {
                Settings.Default.IsDarkTheme = !Settings.Default.IsDarkTheme;
                Settings.Default.Save();
            }), new KeyGesture(Key.F12)));
        }

        private void ShowBSOD(object sender, RoutedEventArgs e)
        {
            if (CheckData())
            {
                CurrentBluescreen.ShowView();
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
            var commandFile = "command";
            var qrFile = "qr";

            File.WriteAllText(commandFile, command);

            var exeCreator = new ExeCreator { Owner = this, CommandBox = { Text = filename + " " + command } };
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

            var hasCustomQR = false;
            var customQRPath = "";

            if (command.Contains("--custom-qr-path=") || command.Contains("-cqp="))
            {
                hasCustomQR = true;
                string[] cmdArr = command.Split(' ');
                foreach (string cmd in cmdArr)
                {
                    if (cmd.StartsWith("--custom-qr-path") || command.StartsWith("-cqp="))
                    {
                        customQRPath = cmd.Split('=')[0];
                    }
                }
            }

            var iexpressSED = "";

            if (hasCustomQR)
            {
                File.Copy(customQRPath, qrFile, true);
                iexpressSED =
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
                AppLaunched=cmd /c %FILE0% --read-command-file
                PostInstallCmd=<None>
                SourceFiles=SourceFiles
                [Strings]
                FILE0=""{filename}""
                FILE1=""{commandFile}""
                FILE2=""{qrFile}""
                [SourceFiles]
                SourceFiles0 = {folder}
                [SourceFiles0]
                %FILE0%=
                %FILE1%=
                %FILE2%=";
            } else
            {
                iexpressSED =
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
                AppLaunched=cmd /c %FILE0% --read-command-file
                PostInstallCmd=<None>
                SourceFiles=SourceFiles
                [Strings]
                FILE0=""{filename}""
                FILE1=""{commandFile}""
                [SourceFiles]
                SourceFiles0 = {folder}
                [SourceFiles0]
                %FILE0%=
                %FILE1%=";
            }

            var SEDPath = Path.GetTempPath() + "\\optionfile.SED";

            File.WriteAllText(SEDPath, iexpressSED);
            Process p = new Process();
            p.StartInfo.FileName = "C:\\Windows\\system32\\iexpress.exe";
            p.StartInfo.Arguments = $"/N {SEDPath}";
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.Start();
            p.WaitForExit();
            File.Delete(SEDPath);
            File.Delete(commandFile);
            if (File.Exists(qrFile))
                File.Delete(qrFile);
            MessageBox.Show("Your EXE-File has been created.", "BluescreenWindow Simulator", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private string GenerateCommand()
        {
            var success = CheckData();
            if (!success) return "";
            return CurrentBluescreen.CreateCommandParameters();
        }


        private bool CheckData()
        {
            if (EnableUnsafe && !string.IsNullOrEmpty(CurrentBluescreen.CmdCommand.Trim()))
            {
                var messageBoxResult = MessageBox.Show("Using a CMD command can be dangerous. " +
                    "I will not be responsible for any data loss or other damage arising from irresponsible or careless use of the CMD command option. " +
                    "Please re-check your command to make sure that you execute what you intended:\r\n\r\n" + CurrentBluescreen.CmdCommand.Trim() + "\r\n\r\n" + "Do you want to proceed?",
                    "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (messageBoxResult == MessageBoxResult.No)
                {
                    return false;
                }
            }
            return true;
        }

        private void CustomQR_Checked(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Filter = "Image Files (*.jpg, *.png, *.jpeg, *.bmp, *.gif)|*.jpg;*.png;*.jpeg;*.bmp;*.gif";
            ofd.Title = "Select a image file to use for the QR code (Use an image with 1:1 aspect ratio for best results)";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Windows10BluescreenViewModel w10b = CurrentBluescreen as Windows10BluescreenViewModel;
                w10b.CustomQRPath = ofd.FileName;
            } else
            {
                Windows10BluescreenViewModel w10b = CurrentBluescreen as Windows10BluescreenViewModel;
                w10b.CustomQR = false;
                w10b.CustomQRPath = "";
                w10b.UseOriginalQR = true;
            }
        }
    }
}
