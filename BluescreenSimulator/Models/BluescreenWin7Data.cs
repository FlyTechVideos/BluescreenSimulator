using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluescreenSimulator
{
    public class BluescreenWin7Data
    {
        #region Strings
        public string _ErrorCode { get; set; } = "IRQL_NOT_LESS_OR_EQUAL";
        public string _Step1 { get; set; } = "Check to make sure any hardware or software is properly installed.";
        public string _Step2 { get; set; } = "If this is a new installation.ask your hardware or software manufacturer for any windows updates you might need.";
        public string _tip { get; set; } = "If problems continue, disable or remove any newly installed hardware or software. Disable BIOS memory options such as caching or shadowing. If you need to use Safe Mode to remove or disable components, restart your computer, press F8 to select Advanced Startup Options, and then select Safe Mode.";
        public string _StopCode { get; set; } = "*** STOP: 0x000000FE (0x00000008, 0x000000006, 0x00000009, 0x847075cc)";
        #endregion

    }
}