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
    /// Interaction logic for bluescreenWin7Menu.xaml
    /// </summary>
    public partial class bluescreenWin7Menu : Window
    {

        public bluescreenWin7Menu()
        {
            InitializeComponent();
        }

        private void Btn_win7bsod_Click(object sender, RoutedEventArgs e)
        {
            BluescreenWin7Data win7Data = new BluescreenWin7Data();
            bool success = SetTexts(win7Data);
            if (success)
            {
                ShowBSOD(win7Data);
            }
        }

        private void ShowBSOD(BluescreenWin7Data data)
        {
            BluescreenWin7 win7 = new BluescreenWin7(data);
            win7.Show();
        }
        private bool SetTexts(BluescreenWin7Data win7Data)
        {
            if (!string.IsNullOrEmpty(txt_ErrorCode.Text))
            {
                win7Data.ErrorCode = txt_ErrorCode.Text;
            }
            if (!string.IsNullOrEmpty(txt_Step1.Text))
            {
                win7Data.Step1 = txt_Step1.Text;
            }
            if (!string.IsNullOrEmpty(txt_Step2.Text))
            {
                win7Data.Step2 = txt_Step2.Text;
            }
            if (!string.IsNullOrEmpty(txt_Tip.Text))
            {
                win7Data.Tip = txt_Tip.Text;
            }
            if (!string.IsNullOrEmpty(txt_StopCode.Text))
            {
                win7Data.StopCode = txt_StopCode.Text;
            }
            return true;
        }
    }
}
