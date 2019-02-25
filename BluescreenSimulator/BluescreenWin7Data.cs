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
        private string _ErrorCode = "IRQL_NOT_LESS_OR_EQUAL";
        private string _Step1 = "Check to make sure any hardware or software is properly installed.";
        private string _Step2 = "If this is a new installation.ask your hardware or software manufacturer for any windows updates you might need.";
        private string _tip = "If problems continue, disable or remove any newly installed hardware or software. Disable BIOS memory options such as caching or shadowing. If you need to use Safe Mode to remove or disable components, restart your computer, press F8 to select Advanced Startup Options, and then select Safe Mode.";
        private string _StopCode = "*** STOP: 0x000000FE (0x00000008, 0x000000006, 0x00000009, 0x847075cc)";
        #endregion


        #region Get&Set Strings
        public string ErrorCode
        {
            get
            {
                return _ErrorCode;
            }
            set
            {
                _ErrorCode = value;
            }
        }

        public string Step1
        {
            get
            {
                return _Step1;
            }
            set
            {
                _Step1 = value;
            }
        }

        public string Step2
        {
            get
            {
                return _Step2;
            }
            set
            {
                _Step2 = value;
            }
        }

        public string Tip
        {
            get
            {
                return _tip;
            }
            set
            {
                _tip = value;
            }
        }

        public string StopCode
        {
            get
            {
                return _StopCode;
            }
            set
            {
                _StopCode = value;
            }
        }
        #endregion

    }
}
