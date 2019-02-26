using System.Windows.Media;
using Strings = BluescreenSimulator.Properties.Windows9xBluescreenResources;
namespace BluescreenSimulator
{
    public class Windows9xBluescreen : BluescreenBase
    {
        public string Header { get; set; } = Strings.Header;

        public string InfoLine1 { get; set; } = Strings.InfoLine1;

        public string InfoLine2 { get; set; } = Strings.InfoLine2;

        public string Instructions { get; set; } = Strings.Instructions;

        public string Error { get; set; } = Strings.Error;

        public string ToContinue { get; set; } = Strings.ToContinue;


        public override Color BackgroundColor { get; set; } = Colors.DarkBlue;
    }
}