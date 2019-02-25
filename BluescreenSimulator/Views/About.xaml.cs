using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;

namespace BluescreenSimulator.Views
{
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var link = (Hyperlink)sender;
            Process.Start(link.NavigateUri.ToString());
        }
    }
}
