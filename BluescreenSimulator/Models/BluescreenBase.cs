using System.Windows.Media;

namespace BluescreenSimulator
{
    public class BluescreenBase
    {
        public string CmdCommand { get; set; } = null;

        public Color BackgroundColor { get; set; } = Color.FromRgb(10, 112, 169);

        public Color ForegroundColor { get; set; } = Colors.White;

        public int Delay { get; set; } = 0;
        public bool EnableUnsafe { get; set; } = false;
    }
}