using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        BluescreenWin7Data Win7Data = null;
        public BluescreenWin7(BluescreenWin7Data win7Data)
        {
            InitializeComponent();
            this.Win7Data = win7Data;
            SetupScreen();
        }

        private void SetupScreen()
        {
            ErrorCode.Text = Win7Data.ErrorCode;
            Step1.Text = Win7Data.Step1;
            Step2.Text = Win7Data.Step2;
            Tip.Text = Win7Data.Tip;
            StopCode.Text = Win7Data.StopCode;
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
