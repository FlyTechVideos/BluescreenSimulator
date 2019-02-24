using System;
using System.Windows;
using System.Threading;
using System.Windows.Media;
using System.Windows.Input;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Media.Imaging;

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
            InitializeScreen();

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

        private void InitializeScreen()
        {
            Emoticon.Text = bluescreenData.Emoticon;
            MainText1.Text = bluescreenData.MainText1;
            MainText2.Text = bluescreenData.MainText2;
            Progress.Text = "0%";
            Complete.Text = bluescreenData.Complete;
            MoreInfo.Text = bluescreenData.MoreInfo;
            SupportPerson.Text = bluescreenData.SupportPerson;
            StopCode.Text = bluescreenData.StopCode;

            Color foregroundColor = Color.FromRgb(bluescreenData.FgRed, bluescreenData.FgGreen, bluescreenData.FgBlue);
            Foreground = new SolidColorBrush(foregroundColor);

            if (bluescreenData.RainbowMode)
            {
                LinearGradientBrush rainbowBackground = new LinearGradientBrush();
                rainbowBackground.StartPoint = new Point(0, 0);
                rainbowBackground.EndPoint = new Point(1, 1);

                // Create and add Gradient stops

                GradientStop redGS = new GradientStop();
                redGS.Color = Colors.Red;
                redGS.Offset = 0.1;
                rainbowBackground.GradientStops.Add(redGS);

                GradientStop orangeGS = new GradientStop();
                orangeGS.Color = Colors.Orange;
                orangeGS.Offset = 0.2;
                rainbowBackground.GradientStops.Add(orangeGS);

                GradientStop yellowGS = new GradientStop();
                yellowGS.Color = Colors.Yellow;
                yellowGS.Offset = 0.3;
                rainbowBackground.GradientStops.Add(yellowGS);

                GradientStop greenGS = new GradientStop();
                greenGS.Color = Colors.Green;
                greenGS.Offset = 0.5;
                rainbowBackground.GradientStops.Add(greenGS);

                GradientStop cyanGS = new GradientStop();
                cyanGS.Color = Colors.Cyan;
                cyanGS.Offset = 0.6;
                rainbowBackground.GradientStops.Add(cyanGS);

                GradientStop blueGS = new GradientStop();
                blueGS.Color = Colors.Blue;
                blueGS.Offset = 0.7;
                rainbowBackground.GradientStops.Add(blueGS);

                GradientStop purpleGS = new GradientStop();
                purpleGS.Color = Colors.Purple;
                purpleGS.Offset = 0.9;
                rainbowBackground.GradientStops.Add(purpleGS);

                Background = rainbowBackground;
            }
            else
            {
                Color backgroundColor = Color.FromRgb(bluescreenData.BgRed, bluescreenData.BgGreen, bluescreenData.BgBlue);
                Background = new SolidColorBrush(backgroundColor);
            }

            QRCode.Visibility = (bluescreenData.HideQR) ? Visibility.Hidden : Visibility.Visible;
            if (bluescreenData.HideQR)
            {
                QRCode.Width = 0;
                QRCode.Margin = new Thickness(0);
            }

            var qr = new BitmapImage(new Uri("Resources/qr.png", UriKind.RelativeOrAbsolute));
            var qrTransparent = new BitmapImage(new Uri("Resources/qr_transparent.png", UriKind.RelativeOrAbsolute));

            QRCode.Source = (bluescreenData.UseOriginalQR) ? qr : qrTransparent;
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
                        Utils.ExecuteCmdCommands(bluescreenData.CmdCommand);
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
