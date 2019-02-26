using System.Windows.Media;
using Strings = BluescreenSimulator.Properties.Windows7BluescreenResources;
namespace BluescreenSimulator
{
    public class Windows7Bluescreen : BluescreenBase
    {
        public string Header { get; set; } = Strings.Header;

        public string ErrorCode { get; set; } = Strings.ErrorCode;

        public string StepsHeader { get; set; } = Strings.StepsHeader;

        public string Steps { get; set; } = Strings.Steps;

        public string TechnicalInfoHeader { get; set; } = Strings.TechnicalInfoHeader;
        public string StopCode { get; set; } = Strings.StopCode;
        public string DumpStart { get; set; } = Strings.DumpStart;

        public string DumpProgress { get; set; } = Strings.DumpProgress;

        public string DumpComplete { get; set; } = Strings.DumpComplete;


        public override Color BackgroundColor { get; set; } = Colors.DarkBlue;
    }
}