using System;
using System.Windows;

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
