using System.Windows;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Threading;
using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.IO;
using System.Diagnostics;
using Microsoft.VisualBasic;

namespace BluescreenSimulator
{
    public partial class MainWindow : Window
    {
        bool enableUnsafe = false;
        Thread delayThread = null;

        public MainWindow(bool enableUnsafe)
        {
            InitializeComponent();
            this.enableUnsafe = enableUnsafe;
            CommandContainer.Visibility = enableUnsafe ? Visibility.Visible : Visibility.Hidden;

            string title = "BluescreenSimulator v2.1";
            if (enableUnsafe)
            {
                title += " (Unsafe Mode)";
            }

            MainWindowFrame.Title = title;

            Closing += WarnClose;
        }

        private void ShowBSOD(object sender, RoutedEventArgs e)
        {
            BluescreenData data = new BluescreenData();
            bool success = PopulateData(data);
            if (success)
            {
                StartBluescreenWithDelay(data);
            }
        }

        private void CancelBSOD(object sender, RoutedEventArgs e)
        {
            InterruptBluescreenDelay();
        }

        private void OpenAbout(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.Show();
        }

        private void GenerateExe(object sender, RoutedEventArgs e)
        {
            string response = Interaction.InputBox("Please enter the file name of the EXE-File", "Enter Filename", "BSOD", 5, 5);
            string optionfilecontent = "[Version]\n" +
                                        "Class = IEXPRESS\n" +
                                        "SEDVersion = 3\n" +
                                        "[Options]\n" +
                                        "PackagePurpose = InstallApp\n" +
                                        "ShowInstallProgramWindow = 1\n" +
                                        "HideExtractAnimation = 1\n" +
                                        "UseLongFileName = 1\n" +
                                        "InsideCompressed = 0\n" +
                                        "CAB_FixedSize = 0\n" +
                                        "CAB_ResvCodeSigning = 0\n" +
                                        "RebootMode = N\n" +
                                        "InstallPrompt =% InstallPrompt %\n" +
                                        "DisplayLicense =% DisplayLicense %\n" +
                                        "FinishMessage =% FinishMessage %\n" +
                                        "TargetName =% TargetName %\n" +
                                        "FriendlyName =% FriendlyName %\n" +
                                        "AppLaunched =% AppLaunched %\n" +
                                        "PostInstallCmd =% PostInstallCmd %\n" +
                                        "AdminQuietInstCmd =% AdminQuietInstCmd %\n" +
                                        "UserQuietInstCmd =% UserQuietInstCmd %\n" +
                                        "SourceFiles = SourceFiles\n" +
                                        "[Strings]\n" +
                                        "InstallPrompt =\n" +
                                        "DisplayLicense =\n" +
                                        "FinishMessage =\n" +
                                        "TargetName = File.exe\n" +
                                        "FriendlyName = etlksnslgdfg\n" +
                                        "AppLaunched = BluescreenSimulatorV2.1.exe " + GetOwO(0) + GetOwO(1) + GetOwO(2) + GetOwO(3) + GetOwO(4) + GetOwO(5) + GetOwO(6) + GetOwO(7) + GetOwO(8) + GetOwO(9) + GetOwO(10) + GetOwO(11) + GetOwO(12) + GetOwO(13) + GetOwO(14) + GetOwO(15) + "\n" +
                                        "PostInstallCmd =<None>\n" +
                                        "AdminQuietInstCmd =\n" +
                                        "UserQuietInstCmd =\n" +
                                        "FILE0 = BluescreenSimulatorV2.1.exe\n" +
                                        "[SourceFiles]\n" +
                                        "SourceFiles0 = " + Path.GetTempPath() + "\n" + 
                                        "[SourceFiles0]\n" +
                                        "%FILE0%=\n";
            File.WriteAllText(Path.GetTempPath() + "\\optionfile.SED", optionfilecontent);
            string thisFile = System.AppDomain.CurrentDomain.FriendlyName;
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\" + thisFile;
            string Filepath = Path.GetTempPath() + "\\BluescreenSimulatorV2.1.exe";
            try
            {
                if (!File.Exists(Filepath))
                {
                    System.IO.File.Copy(path, Filepath);
                }
            }
            catch (Exception ignored)
            {
                
            }
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C iexpress.exe /N " + Path.GetTempPath() + "\\optionfile.SED";
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            File.Move("File.exe", response + ".exe");
            File.Delete(Path.GetTempPath() + "\\optionfile.SED");
            File.Delete(Path.GetTempPath() + "\\BluescreenSimulatorV2.1.exe");
            File.Delete(Path.GetTempPath() + "\\File.exe");
            MessageBox.Show("Your EXE-File has been created.", "Bluescreen Simulator");
        }

        private string GetOwO(int what)
        {
            if (what == 0)
            {
                if (Emoticon.Text.Length > 0)
                {
                    return "--emoticon \"" + Emoticon.Text + "\" ";
                } else
                {
                    return "";
                }
            } else if (what == 1)
            {
                if (MainText1.Text.Length > 0)
                {
                    return "--m1 \"" + MainText1.Text + "\" ";
                }
                else
                {
                    return "";
                }
            } else if (what == 2)
            {
                if (MainText2.Text.Length > 0)
                {
                    return "--m2 \"" + MainText2.Text + "\" ";
                }
                else
                {
                    return "";
                }
            } else if (what == 3)
            {
                if (Complete.Text.Length > 0)
                {
                    return "--progress \"" + Complete.Text + "\" ";
                }
                else
                {
                    return "";
                }
            } else if (what == 4)
            {
                if (MoreInfo.Text.Length > 0)
                {
                    return "--mi \"" + MoreInfo.Text + "\" ";
                }
                else
                {
                    return "";
                }
            } else if (what == 5)
            {
                if (SupportPerson.Text.Length > 0)
                {
                    return "--supportperson \"" + SupportPerson.Text + "\" ";
                }
                else
                {
                    return "";
                }
            } else if (what == 6)
            {
                if (StopCode.Text.Length > 0)
                {
                    return "--sc \"" + StopCode.Text + "\" ";
                }
                else
                {
                    return "";
                }
            } else if (what == 7)
            {
                if (BgRed.Text.Length > 0)
                {
                    return "--br \"" + BgRed.Text + "\" ";
                }
                else
                {
                    return "";
                }
            } else if (what == 8)
            {
                if (BgGreen.Text.Length > 0)
                {
                    return "--bg \"" + BgGreen.Text + "\" ";
                }
                else
                {
                    return "";
                }
            } else if (what == 9)
            {
                if (BgBlue.Text.Length > 0)
                {
                    return "--bb \"" + BgBlue.Text + "\" ";
                }
                else
                {
                    return "";
                }
            } else if (what == 10)
            {
                if (FgRed.Text.Length > 0)
                {
                    return "--fr \"" + FgRed.Text + "\" ";
                }
                else
                {
                    return "";
                }
            } else if (what == 11)
            {
                if (FgGreen.Text.Length > 0)
                {
                    return "--fg \"" + FgGreen.Text + "\" ";
                }
                else
                {
                    return "";
                }
            } else if (what == 12)
            {
                if (FgBlue.Text.Length > 0)
                {
                    return "--fb \"" + FgBlue.Text + "\" ";
                }
                else
                {
                    return "";
                }
            } else if (what == 13)
            {
                if (UseOriginalQR.IsChecked.Value)
                {
                    return "--oq ";
                }
                else
                {
                    return "";
                }
            } else if (what == 14)
            {
                if (HideQR.IsChecked.Value)
                {
                    return "--hq ";
                }
                else
                {
                    return "";
                }
            } else if (what == 15)
            {
                if (Delay.Text.Length > 0)
                {
                    return "--delay " + Delay.Text;
                } else
                {
                    return "";
                }
            }
            else
            {
                return "notices buldge";
            }
        }

        private void UseOriginalQR_Click(object sender, RoutedEventArgs e)
        {
            HandleCheckboxes(UseOriginalQR, HideQR);
        }

        private void HideQR_Click(object sender, RoutedEventArgs e)
        {
            HandleCheckboxes(HideQR, UseOriginalQR);
        }

        private void HandleCheckboxes(CheckBox clicked, CheckBox other)
        {
            if (clicked.IsChecked == true) // it's a 'bool?' so we need explicit boolean comparison
            {
                other.IsChecked = false;
                other.IsEnabled = false;
            }
            else
            {
                other.IsEnabled = true;
            }
        }

        private void WarnClose(object sender, CancelEventArgs e)
        {
            if (delayThread != null) { 
                MessageBoxResult messageBoxResult = MessageBox.Show("Do you want to exit? The scheduled BSOD will remain scheduled. If you want to interrupt it, you have to kill the process.",
                    "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void InterruptBluescreenDelay()
        {
            if (delayThread != null)
            {
                delayThread.Interrupt();
                ConfirmButton.IsEnabled = true;
                CancelButton.IsEnabled = false;
                delayThread = null;
            }
        }

        private void StartBluescreenWithDelay(BluescreenData data)
        {
            Bluescreen bluescreen = new Bluescreen(data);
            ConfirmButton.IsEnabled = false;
            CancelButton.IsEnabled = true;
            delayThread = new Thread((() =>
            {
                try
                {
                    Thread.Sleep(data.Delay * 1000);
                    Application.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        bluescreen.Show();
                        ConfirmButton.IsEnabled = true;
                        CancelButton.IsEnabled = false;
                    }));
                }
                catch (ThreadInterruptedException)
                {
                    Application.Current.Dispatcher.Invoke((Action)(() => bluescreen.Close()));
                }
                bluescreen = null;
                delayThread = null;
            }));
            delayThread.Start();
        }

        private bool PopulateData(BluescreenData data)
        {
            if (!string.IsNullOrEmpty(Emoticon.Text))
            {
                data.Emoticon = Emoticon.Text;
            }
            if (!string.IsNullOrEmpty(MainText1.Text))
            {
                data.MainText1 = MainText1.Text;
            }
            if (!string.IsNullOrEmpty(MainText2.Text))
            {
                data.MainText2 = MainText2.Text;
            }
            if (!string.IsNullOrEmpty(Complete.Text))
            {
                data.Complete = Complete.Text;
            }
            if (!string.IsNullOrEmpty(MoreInfo.Text))
            {
                data.MoreInfo = MoreInfo.Text;
            }
            if (!string.IsNullOrEmpty(SupportPerson.Text))
            {
                data.SupportPerson = SupportPerson.Text;
            }
            if (!string.IsNullOrEmpty(StopCode.Text))
            {
                data.StopCode = StopCode.Text;
            }
            if (!string.IsNullOrEmpty(BgRed.Text))
            {
                try
                {
                    byte red = byte.Parse(BgRed.Text);
                    data.BgRed = red;
                }
                catch (OverflowException) {
                    MessageBox.Show("Background color value for red is invalid! Must be a number between 0 and 255.", "Invalid color value", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                };
            }
            if (!string.IsNullOrEmpty(BgGreen.Text))
            {
                try
                {
                    byte green = byte.Parse(BgGreen.Text);
                    data.BgGreen = green;
                }
                catch (OverflowException) {
                    MessageBox.Show("Background color value for green is invalid! Must be a number between 0 and 255.", "Invalid color value", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                };
            }
            if (!string.IsNullOrEmpty(BgBlue.Text))
            {
                try
                {
                    byte blue = byte.Parse(BgBlue.Text);
                    data.BgBlue = blue;
                }
                catch (OverflowException) {
                    MessageBox.Show("Background color value for blue is invalid! Must be a number between 0 and 255.", "Invalid color value", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                };
            }
            if (!string.IsNullOrEmpty(FgRed.Text))
            {
                try
                {
                    byte red = byte.Parse(FgRed.Text);
                    data.FgRed = red;
                }
                catch (OverflowException)
                {
                    MessageBox.Show("Foreground color value for red is invalid! Must be a number between 0 and 255.", "Invalid color value", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                };
            }
            if (!string.IsNullOrEmpty(FgGreen.Text))
            {
                try
                {
                    byte green = byte.Parse(FgGreen.Text);
                    data.FgGreen = green;
                }
                catch (OverflowException)
                {
                    MessageBox.Show("Foreground color value for green is invalid! Must be a number between 0 and 255.", "Invalid color value", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                };
            }
            if (!string.IsNullOrEmpty(FgBlue.Text))
            {
                try
                {
                    byte blue = byte.Parse(FgBlue.Text);
                    data.FgBlue = blue;
                }
                catch (OverflowException)
                {
                    MessageBox.Show("Foreground color value for blue is invalid! Must be a number between 0 and 255.", "Invalid color value", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                };
            }
            if (!string.IsNullOrEmpty(Delay.Text))
            {
                try
                {
                    int delay = int.Parse(Delay.Text);
                    if (delay > 86400) {
                        MessageBox.Show("Please enter a number between 0 and 86,400 (= 24 hours)", "Invalid delay", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                    data.Delay = delay;
                }
                catch (OverflowException) { };
            }
            if (enableUnsafe && !string.IsNullOrEmpty(CmdCommand.Text.Trim()))
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Using a CMD command can be dangerous. " +
                    "I will not be responsible for any data loss or other damage arising from irresponsible or careless use of the CMD command option. " +
                    "Please re-check your command to make sure that you execute what you intended:\r\n\r\n" + CmdCommand.Text.Trim() + "\r\n\r\n" + "Do you want to proceed?", 
                    "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (messageBoxResult == MessageBoxResult.No)
                {
                    return false;
                }
                data.CmdCommand = CmdCommand.Text.Trim();
            }
            data.EnableUnsafe = enableUnsafe;
            data.HideQR = HideQR.IsChecked == true; // is a bool? -> need explicit check
            data.UseOriginalQR = UseOriginalQR.IsChecked == true; // is a bool? -> need explicit check
            return true;
        }

        private void ValidateNumber(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void RemoveSpaces(object sender, TextChangedEventArgs e)
        {
            ((TextBox)sender).Text = ((TextBox)sender).Text.Replace(" ", "");
        }
    }
}
