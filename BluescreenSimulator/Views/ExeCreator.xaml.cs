using System.IO;
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
            Loaded += (sender, args) => { FileName.Text = Path.GetRandomFileName(); };
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
