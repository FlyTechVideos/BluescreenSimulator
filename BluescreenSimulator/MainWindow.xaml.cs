using System.Windows;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Threading;
using System;
using System.ComponentModel;

namespace BluescreenSimulator
{
    public partial class MainWindow : Window
    {
        Thread delayThread = null;

        public MainWindow()
        {
            InitializeComponent();
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

        private void WarnClose(object sender, CancelEventArgs e)
        {
            if (delayThread != null) { 
                MessageBoxResult messageBoxResult = MessageBox.Show("Do you want to exit? The scheduled BSOD will be canceled.", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    InterruptBluescreenDelay();
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
            }
        }

        private void StartBluescreenWithDelay(BluescreenData data)
        {
            ConfirmButton.IsEnabled = false;
            CancelButton.IsEnabled = true;
            delayThread = new Thread(delegate ()
            {
                try
                {
                    Thread.Sleep(data.Delay * 1000);
                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        Bluescreen bluescreen = new Bluescreen(data);
                        bluescreen.Show();
                        ConfirmButton.IsEnabled = true;
                        CancelButton.IsEnabled = false;
                    });
                } catch (ThreadInterruptedException) { }
                delayThread = null;
            });
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
            if (!string.IsNullOrEmpty(Red.Text))
            {
                try
                {
                    byte red = byte.Parse(Red.Text);
                    data.Red = red;
                }
                catch (OverflowException) {
                    MessageBox.Show("Color value for red is invalid! Must be a number between 0 and 255.", "Invalid color value", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                };
            }
            if (!string.IsNullOrEmpty(Green.Text))
            {
                try
                {
                    byte green = byte.Parse(Green.Text);
                    data.Green = green;
                }
                catch (OverflowException) {
                    MessageBox.Show("Color value for green is invalid! Must be a number between 0 and 255.", "Invalid color value", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                };
            }
            if (!string.IsNullOrEmpty(Blue.Text))
            {
                try
                {
                    byte blue = byte.Parse(Blue.Text);
                    data.Blue = blue;
                }
                catch (OverflowException) {
                    MessageBox.Show("Color value for blue is invalid! Must be a number between 0 and 255.", "Invalid color value", MessageBoxButton.OK, MessageBoxImage.Error);
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
            if (!string.IsNullOrEmpty(CmdCommand.Text.Trim()))
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
            return true;
        }

        private void ValidateNumber(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
