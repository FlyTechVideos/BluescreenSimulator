using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BluescreenSimulator.ViewModels;

namespace BluescreenSimulator.Views
{
    public partial class BluescreenWindow : Window
    {
        private readonly BluescreenDataViewModel _vm;
        private readonly CancellationTokenSource _source = new CancellationTokenSource();
        public BluescreenWindow(BluescreenDataViewModel data)
        {
            DataContext = _vm = data;
            InitializeComponent();
            Loaded += Bluescreen_Loaded;
            Closing += Close;
        }

        private void Close(object sender, CancelEventArgs e)
        {
            e.Cancel = true; // no. 
            Focus();
        }
        private async void Bluescreen_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await _vm.StartProgress(_source.Token);
            }
            catch (TaskCanceledException)
            {
                _vm.Progress = 0;
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key is Key.F7) Close();
        }
    }
}
