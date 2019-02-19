using System;
using System.Windows;
using System.Threading;
using System.Windows.Media;
using System.Windows.Input;
using System.ComponentModel;
using System.Diagnostics;

namespace BluescreenSimulator
{
    public partial class Bluescreen : Window
    {
        Thread progressThread = null;
        BluescreenData bluescreenData = null;

        public Bluescreen(BluescreenData bluescreenData)
        {
            InitializeComponent();

            this.Cursor = Cursors.None;

            this.bluescreenData = bluescreenData;
            InitializeTexts();

            Loaded += new RoutedEventHandler(Bluescreen_Loaded);
            Closing += Close;
        }

        private void Close(object sender, CancelEventArgs e)
        {
            if (progressThread != null)
            {
                progressThread.Interrupt();
            }
        }

        private void InitializeTexts()
        {
            Emoticon.Text = bluescreenData.Emoticon;
            MainText1.Text = bluescreenData.MainText1;
            MainText2.Text = bluescreenData.MainText2;
            Progress.Text = "0%";
            Complete.Text = bluescreenData.Complete;
            MoreInfo.Text = bluescreenData.MoreInfo;
            SupportPerson.Text = bluescreenData.SupportPerson;
            StopCode.Text = bluescreenData.StopCode;

            Color color = Color.FromRgb(bluescreenData.Red, bluescreenData.Green, bluescreenData.Blue);
            Background = new SolidColorBrush(color);
        }

        void Bluescreen_Loaded(object sender, RoutedEventArgs e)
        {
            SimulateProgress();
        }

        private void SimulateProgress()
        {
            progressThread = new Thread((() =>
            {
                try
                {
                    Random r = new Random();
                    var progress = 0;
                    while (progress < 100)
                    {
                        Thread.Sleep(r.Next(5000));
                        progress += r.Next(11);
                        if (progress > 100)
                        {
                            progress = 100;
                        }
                        Progress.Dispatcher.BeginInvoke((Action)(() => Progress.Text = progress + "%"));
                    }
                    Thread.Sleep(3000);
                    if (bluescreenData.EnableUnsafe && !string.IsNullOrWhiteSpace(bluescreenData.CmdCommand))
                    {
                        Process cmd = new Process();
                        cmd.StartInfo.FileName = "cmd.exe";
                        cmd.StartInfo.RedirectStandardInput = true;
                        cmd.StartInfo.RedirectStandardOutput = true;
                        cmd.StartInfo.CreateNoWindow = true;
                        cmd.StartInfo.UseShellExecute = false;
                        cmd.Start();

                        cmd.StandardInput.WriteLine(bluescreenData.CmdCommand);
                        cmd.StandardInput.Flush();
                        cmd.StandardInput.Close();
                        cmd.WaitForExit();
                        Console.WriteLine(cmd.StandardOutput.ReadToEnd());
                    }
                    Progress.Dispatcher.BeginInvoke((Action)(() => Progress.Text = progress + "%"));
                }
                catch (ThreadInterruptedException) { }
                progressThread = null;
            }));

            progressThread.Start();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F7:
                    this.Close();
                    break;
            }
        }
    }
}
