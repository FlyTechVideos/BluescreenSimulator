using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using BluescreenSimulator.Properties;

namespace BluescreenSimulator.ViewModels
{
    public class MainWindowViewModel : PropertyChangedObject
    {
        public MainWindowViewModel()
        {
            ResetAllCommand = new DelegateCommand(ResetAll);
        }

        private void ResetAll()
        {
            var old = Bluescreens.IndexOf(SelectedBluescreen);
            var @new = Activator.CreateInstance(SelectedBluescreen.GetType()) as IBluescreenViewModel;
            Bluescreens.Insert(old, @new);
            Bluescreens.RemoveAt(old + 1);
            SelectedBluescreen = @new;
        }

        public string Title => AppInfo.AppTitle + $"{(SelectedBluescreen.EnableUnsafe ? "- Unsafe" : "")}";
        public ObservableCollection<IBluescreenViewModel> Bluescreens { get; set; } = new ObservableCollection<IBluescreenViewModel>
        {
            new Windows10BluescreenViewModel(),
            new Windows7BluescreenViewModel(),
            new Windows9xBluescreenViewModel()
        };

        public DelegateCommand ResetAllCommand { get; }

        private IBluescreenViewModel _selectedBluescreen;

        public IBluescreenViewModel SelectedBluescreen
        {
            get { return _selectedBluescreen ?? (SelectedBluescreen = Bluescreens[0]); }
            set { _selectedBluescreen = value; OnPropertyChanged(); OnPropertyChanged(nameof(Title)); }
        }

    }
}