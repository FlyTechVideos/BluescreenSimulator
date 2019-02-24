using System.Windows;

namespace BluescreenSimulator
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

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
