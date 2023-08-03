using System.Windows.Media;

namespace BluescreenSimulator
{
    public class BluescreenBase
    {
        public string CmdCommand { get; set; } = null;

        public virtual Color BackgroundColor { get; set; } = Color.FromRgb(10, 112, 169);

        public virtual Color ForegroundColor { get; set; } = Colors.White;

        public double Delay { get; set; } = 0.0;

        public double ProgressFactor { get; set; } = 1.0;

        public double ProgressStartDelay { get; set; } = 0.0;
        public double ClosingAfterFinishDelay { get; set; } = 3.0;

        public bool RainbowMode { get; set; } = false;

        public int StartingProgress { get; set; }
    }
}