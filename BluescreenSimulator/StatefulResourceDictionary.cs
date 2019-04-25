using System;
using System.Windows;

namespace BluescreenSimulator
{
    public class StatefulResourceDictionary : ResourceDictionary
    {
        private static readonly Uri DarkTheme = new Uri("/BluescreenSimulator;component/Resources/DarkTheme.xaml", UriKind.Relative);
        private static readonly Uri LightTheme = new Uri("/BluescreenSimulator;component/Resources/LightTheme.xaml", UriKind.Relative);
        private bool _isEnabled;

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                Source = _isEnabled ? DarkTheme : LightTheme;
            }
        }
    }
}
