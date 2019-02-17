using System;
using System.Windows;

namespace BluescreenSimulator
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, EventArgs e)
        {
            bool enableUnsafe = false;

            foreach (string arg in Environment.GetCommandLineArgs())
            {
                if (arg.Equals("--enable-unsafe"))
                {
                    enableUnsafe = true;
                }
            }

            if (enableUnsafe)
            {
                MessageBox.Show("You are entering Unsafe Mode. Be careful!", "Careful", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            MainWindow mainWindow = new MainWindow(enableUnsafe);
            mainWindow.Show();
        }
    }
}
