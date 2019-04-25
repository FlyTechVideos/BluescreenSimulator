using System.Windows;
using System.Windows.Input;
using BluescreenSimulator.ViewModels;


namespace BluescreenSimulator.Views
{
    /// <summary>
    /// Interaction logic for BluescreenWindow9x.xaml
    /// </summary>
    public partial class BluescreenWindow9x : Window
    {
        private Windows9xBluescreenViewModel _vm;
        public BluescreenWindow9x(Windows9xBluescreenViewModel vm = null)
        {
            DataContext = _vm = vm ?? new Windows9xBluescreenViewModel();
            InitializeComponent();
            KeyDown += BluescreenWindow9x_KeyDown;
        }

        private void BluescreenWindow9x_KeyDown(object sender, KeyEventArgs e)
        {
            Close();
        }
    }
}
