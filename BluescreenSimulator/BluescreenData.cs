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

        private byte red = 10;
        private byte green = 112;
        private byte blue = 169;

        private int delay = 0;

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

        public byte Red
        {
            get
            {
                return red;
            }

            set
            {
                red = value;
            }
        }

        public byte Green
        {
            get
            {
                return green;
            }

            set
            {
                green = value;
            }
        }

        public byte Blue
        {
            get
            {
                return blue;
            }

            set
            {
                blue = value;
            }
        }

        public int Delay
        {
            get
            {
                return delay;
            }

            set
            {
                delay = value;
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
    }
}
