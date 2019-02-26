using System.Windows.Media;

namespace BluescreenSimulator
{
    public class Windows10Bluescreen : BluescreenBase
    {
        public string Emoticon { get; set; } = ":(";

        public string MainText1 { get; set; } = "Your PC ran into a problem and needs to restart. We're just";

        public string MainText2 { get; set; } = "collecting some error info, and then we'll restart for you.";

        public string Complete { get; set; } = "complete";

        public string MoreInfo { get; set; } = "For more information about this issue and possible fixes, visit https://www.windows.com/stopcode";

        public string SupportPerson { get; set; } = "If you call a support person, give them this info:";

        public string StopCode { get; set; } = "Stop code: DRIVER_IRQL_NOT_LESS_OR_EQUAL";

        public bool HideQR { get; set; } = false;

        public bool UseOriginalQR { get; set; }
    }
}
