using System.Windows.Media;

namespace BluescreenSimulator
{
    public class Windows7Bluescreen : BluescreenBase
    {
        public string Header { get; set; } =
            "A problem has been detected and windows has been shutdown to prevent damage to your computer.";

        public string ErrorCode { get; set; } = "IRQL_NOT_LESS_OR_EQUAL";

        public string StepsHeader { get; set; } =
            "If this is the first time you've seen this error screen," +
            "restart your computer. if this screen appears again. follow " +
            "these steps:";

        public string Steps { get; set; } =
            @"Check to make sure any hardware or software is properly installed.
If this is a new installation. ask your hardware or software manufacturer for any windows updates you might need.

If problems continue, disable or remove any newly installed hardware or software. Disable BIOS memory options such as caching or shadowing. If you need to use Safe Mode to remove or disable components, restart your computer, press F8 to select Advanced Startup Options, and then select Safe Mode.";

        public string TechnicalInfoHeader { get; set; } = "Technical information:";
        public string StopCode { get; set; } = "*** STOP: 0x000000FE (0x00000008, 0x000000006, 0x00000009, 0x847075cc)";
        public string DumpStart { get; set; } =
            @"Collecting data for crash dump...
Initializing disk for crash dump...
Beginning dump of physical memory.";

        public string DumpProgress { get; set; } = "Dumping physical memory to disk: @p";

        public string DumpComplete { get; set; } =
            @"Physical memory dump complete.
Contact your system administator or technical support group for further assistance.";


        public override Color BackgroundColor { get; set; } = Colors.DarkBlue;
    }
}