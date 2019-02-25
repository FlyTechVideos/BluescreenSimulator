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
using BluescreenSimulator.ViewModels;

namespace BluescreenSimulator.Views
{
    /// <summary>
    /// Interaction logic for BluescreenWindow9x.xaml
    /// </summary>
    public partial class BluescreenWindow9x : Window
    {
        private Windows9xBluescreenViewModel _vm;
        public BluescreenWindow9x(Windows9xBluescreenViewModel vm)
        {
            DataContext = _vm = vm ?? new Windows9xBluescreenViewModel();
            InitializeComponent();
        }
    }
}
