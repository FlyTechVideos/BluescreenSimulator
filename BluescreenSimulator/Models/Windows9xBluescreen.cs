using System.Windows.Media;

namespace BluescreenSimulator
{
    public class Windows9xBluescreen : BluescreenBase
    {
        public string Header { get; set; } = "Windows";

        public string InfoLine1 { get; set; } = "An error has occurred. To continue:";

        public string InfoLine2 { get; set; } = "Press Enter to return to Windows, or";

        public string Instructions { get; set; } = 
        @"Press CTRL+ALT+DEL to restart your computer. If you do this, 
you will lose any unsaued information in all open applications.";

        public string Error { get; set; } = "Error : 0E : 016F : BFF9B3D4";

        public string ToContinue { get; set; } = "Press any key to continue";


        public override Color BackgroundColor { get; set; } = Colors.DarkBlue;
    }
}