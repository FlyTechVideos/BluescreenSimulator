using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using BluescreenSimulator.ViewModels;
using Resolution;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace BluescreenSimulator.Views
{
    /// <summary>
    /// Interaction logic for BluescreenWindowWin7.xaml
    /// </summary>
    public partial class BluescreenWindowWin7 : Window
    {
        //Variables
        private readonly Random _random = new Random();
        private Windows7BluescreenViewModel _vm;

        // for Resolution stuff
        private const int FixHeight = 800, FixWidth = 600;

        public BluescreenWindowWin7(Windows7BluescreenViewModel vm = null)
        {            // gets the main screen current Resolution
            DataContext = _vm = vm ?? new Windows7BluescreenViewModel();
            InitializeComponent();
            
            Task.Run(SetupScreen);
        }

        private readonly CancellationTokenSource _source = new CancellationTokenSource();
        private async Task SetupScreen()
        {
            try
            {
                await _vm.StartProgress(_source.Token);
            }
            catch (TaskCanceledException)
            {
                // ok
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _source.Cancel(); // cancel the current progress.
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F7)
            {
                Close();
            }
        }
    }
}