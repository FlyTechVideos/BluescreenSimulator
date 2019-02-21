namespace BluescreenSimulator
{
    public class BluescreenData
    {
        private string _Emoticon = ":(";
        private string _MainText1 = "Your PC ran into a problem and needs to restart. We're just";
        private string _MainText2 = "collecting some error info, and then we'll restart for you.";
        private string _Complete = "complete";
        private string _MoreInfo = "For more information about this issue and possible fixes, visit https://www.windows.com/stopcode";
        private string _SupportPerson = "If you call a support person, give them this info:";
        private string _StopCode = "Stop code: DRIVER_IRQL_NOT_LESS_OR_EQUAL";
        private string _CmdCommand = null;

        private byte _BgRed = 10;
        private byte _BgGreen = 112;
        private byte _BgBlue = 169;

        private byte _FgRed = 255;
        private byte _FgGreen = 255;
        private byte _FgBlue = 255;

        private int _Delay = 0;
        private bool _EnableUnsafe = false;

        private bool _HideQR = false;
        private bool _UseOriginalQR = false;

        public string Emoticon
        {
            get
            {
                return _Emoticon;
            }

            set
            {
                _Emoticon = value;
            }
        }

        public string MainText1
        {
            get
            {
                return _MainText1;
            }

            set
            {
                _MainText1 = value;
            }
        }

        public string MainText2
        {
            get
            {
                return _MainText2;
            }

            set
            {
                _MainText2 = value;
            }
        }

        public string Complete
        {
            get
            {
                return _Complete;
            }

            set
            {
                _Complete = value;
            }
        }

        public string MoreInfo
        {
            get
            {
                return _MoreInfo;
            }

            set
            {
                _MoreInfo = value;
            }
        }

        public string SupportPerson
        {
            get
            {
                return _SupportPerson;
            }

            set
            {
                _SupportPerson = value;
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

        public string CmdCommand
        {
            get
            {
                return _CmdCommand;
            }

            set
            {
                _CmdCommand = value;
            }
        }

        public byte BgRed
        {
            get
            {
                return _BgRed;
            }

            set
            {
                _BgRed = value;
            }
        }

        public byte BgGreen
        {
            get
            {
                return _BgGreen;
            }

            set
            {
                _BgGreen = value;
            }
        }

        public byte BgBlue
        {
            get
            {
                return _BgBlue;
            }

            set
            {
                _BgBlue = value;
            }
        }

        public byte FgRed
        {
            get
            {
                return _FgRed;
            }

            set
            {
                _FgRed = value;
            }
        }

        public byte FgGreen
        {
            get
            {
                return _FgGreen;
            }

            set
            {
                _FgGreen = value;
            }
        }

        public byte FgBlue
        {
            get
            {
                return _FgBlue;
            }

            set
            {
                _FgBlue = value;
            }
        }

        public int Delay
        {
            get
            {
                return _Delay;
            }

            set
            {
                _Delay = value;
            }
        }

        public bool EnableUnsafe
        {
            get
            {
                return _EnableUnsafe;
            }

            set
            {
                _EnableUnsafe = value;
            }
        }

        public bool HideQR
        {
            get
            {
                return _HideQR;
            }

            set
            {
                _HideQR = value;
            }
        }

        public bool UseOriginalQR
        {
            get
            {
                return _UseOriginalQR;
            }

            set
            {
                _UseOriginalQR = value;
            }
        }
    }
}
