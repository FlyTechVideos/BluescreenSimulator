using System.ComponentModel;
using System.Windows.Input;

namespace BluescreenSimulator.ViewModels
{
    public interface IBluescreenViewModel : INotifyPropertyChanged
    {
        string StyleName { get; }
        DelegateCommand ExecuteCommand { get; }
        DelegateCommand InterruptCommand { get; }

        bool IsWaiting { get; }
        bool IsNotWaiting { get; }

        string CreateCommandParameters();
        bool EnableUnsafe { get; set; }
        string CmdCommand { get; set; }
    }
}