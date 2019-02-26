using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BluescreenSimulator.Properties;

namespace BluescreenSimulator.ViewModels
{
    public class MainWindowViewModel : PropertyChangedObject
    {
        public string Title => AppInfo.AppTitle + $"{(SelectedBluescreen.EnableUnsafe ? "- Unsafe" : "")}";
        public IEnumerable<IBluescreenViewModel> Bluescreens { get; set; } = new IBluescreenViewModel[]
        {
            new Windows10BluescreenViewModel(),
            new Windows7BluescreenViewModel(),
            new Windows9xBluescreenViewModel()
        };
        private IBluescreenViewModel _selectedBluescreen;

        public IBluescreenViewModel SelectedBluescreen
        {
            get { return _selectedBluescreen ?? (_selectedBluescreen = Bluescreens.FirstOrDefault()); }
            set { _selectedBluescreen = value; OnPropertyChanged(); OnPropertyChanged(nameof(Title));}
        }        
    }
}