using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BluescreenSimulator
{
    /// <summary>
    /// Interaction logic for BluescreenWin7.xaml
    /// </summary>
    public partial class BluescreenWin7 : Window
    {
        //Variables
        private bool DoneDumping = false;
        Random r_dump = new Random();
        BluescreenWin7Data Win7Data = null;

        private int tempHeight = 0, tempWidth = 0;
        private int FixHeight = 800, FixWidth = 600;

        public BluescreenWin7(BluescreenWin7Data win7Data)
        {
            System.Windows.Forms.Screen Srn = System.Windows.Forms.Screen.PrimaryScreen;
            // gets the main screen current res
            tempHeight = Srn.Bounds.Width;
            tempWidth = Srn.Bounds.Height;
            InitializeComponent();
            this.Win7Data = win7Data;
            SetupScreen();
        }

        private void SetupScreen()
        {
            // sets the main screen current res to 500*800
            Resolution.CResolution ChangeRes = new Resolution.CResolution(FixHeight, FixWidth);
            DoneDump1.Visibility = Visibility.Hidden; DoneDump2.Visibility = Visibility.Hidden;

            ErrorCode.Text = Win7Data._ErrorCode;
            Step1.Text = Win7Data._Step1;
            Step2.Text = Win7Data._Step2;
            Tip.Text = Win7Data._tip;
            StopCode.Text = Win7Data._StopCode;
            DoneDumping = SimulateDumping();
        }
        private void AfterDumping()
        {
            DoneDump1.Visibility = Visibility.Visible; DoneDump2.Visibility = Visibility.Visible;

        }
        void DumpLoop()
        {
            int MaxDump = 95;
            var CurrentDump = 0;
            while (CurrentDump < MaxDump)
            {
                Thread.Sleep(r_dump.Next(5000));
                CurrentDump += r_dump.Next(15, 25);
                if (CurrentDump > MaxDump)
                {
                    CurrentDump = MaxDump;
                    Application.Current.Dispatcher.InvokeAsync(() => AfterDumping());
                }

                Application.Current.Dispatcher.InvokeAsync(() => Dump.Text = "Dumping physical memory to disk: " + CurrentDump.ToString());
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // sets the main screen res to the defualt res so it can reset to it after whike closing
            Resolution.CResolution ChangeRes = new Resolution.CResolution(tempHeight, tempWidth);
        }

        private bool SimulateDumping()
        {
            Task.Factory.StartNew(this.DumpLoop);
            return true;
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F7)
            {
                this.Close();
            }
        }
    }
}