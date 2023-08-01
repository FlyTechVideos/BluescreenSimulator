using System.ComponentModel;
using System.Windows.Media;

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
        string CmdCommand { get; set; }

        Color ForegroundColor { get; set; }
        Color BackgroundColor { get; set; }

        double Delay { get; set; }
        int Progress { get; set; }
        int StartingProgress { get; set; }

        bool SupportsRainbow { get; }
        bool RainbowMode { get; }

        double ProgressFactor { get; set; }
        double ProgressStartDelay { get; set; }
        double ClosingAfterFinishDelay { get; set; }

    }
}