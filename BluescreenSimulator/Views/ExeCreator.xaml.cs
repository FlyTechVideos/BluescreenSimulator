using System.Windows;

namespace BluescreenSimulator.Views
{
    /// <summary>
    /// Interaction logic for ExeCreator.xaml
    /// </summary>
    public partial class ExeCreator : Window
    {
        public ExeCreator()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
