using System;
using System.Windows;

namespace BluescreenSimulator
{
    public class StatefulResourceDictionary : ResourceDictionary
    {
        private static readonly Uri EmptyUri = new Uri("/BluescreenSimulator;component/Resources/Empty.xaml", UriKind.Relative);
        private Uri _lastSource;
        private bool _isEnabled;

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value; if (Source != EmptyUri) _lastSource = Source;
                Source = value ?  _lastSource : EmptyUri;
            }
        }
    }
}
