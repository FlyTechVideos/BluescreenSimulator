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

namespace BluescreenSimulator.Views
{
    /// <summary>
    /// Interaction logic for BlackWindow.xaml
    /// </summary>
    public partial class BlackWindow : Window
    {
        public BlackWindow(Window owner = null)
        {
            Owner = owner;
            if (Owner != null)
                Owner.Closed += OnParentClosed;
            InitializeComponent();
        }

        private void OnParentClosed(object sender, EventArgs e)
        {
            Close(); // bye :c
        }
    }
}
