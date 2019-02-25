using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BluescreenSimulator.ViewModels
{
    public class Windows10BluescreenViewModel : BaseBluescreenDataViewModel<Windows10Bluescreen>
    {
        public Windows10BluescreenViewModel() : this(null)
        {

        }
        public Windows10BluescreenViewModel(Windows10Bluescreen model = null) : base(model)
        {
            
        }
        private CancellationTokenSource _source = new CancellationTokenSource();

        [CmdParameter("-e")]
        public string Emoticon
        {
            get => Model.Emoticon;
            set => SetModelProperty(value);
        }
        [CmdParameter("-m1")]
        public string MainText1
        {
            get => Model.MainText1;
            set => SetModelProperty(value);
        }
        [CmdParameter("-m2")]
        public string MainText2
        {
            get => Model.MainText2;
            set => SetModelProperty(value);
        }
        [CmdParameter("-p")]
        public string Complete
        {
            get => Model.Complete;
            set => SetModelProperty(value);
        }
        [CmdParameter("-mi")]
        public string MoreInfo
        {
            get => Model.MoreInfo;
            set => SetModelProperty(value);
        }
        [CmdParameter("-s")]
        public string SupportPerson
        {
            get => Model.SupportPerson;
            set => SetModelProperty(value);
        }
        [CmdParameter("-sc")]
        public string StopCode
        {
            get => Model.StopCode;
            set => SetModelProperty(value);
        }
        [CmdParameter("--hideqr")]
        public bool HideQR
        {
            get => Model.HideQR;
            set => SetModelProperty(value, others: nameof(ShowQR));
        }

        public bool ShowQR => !HideQR;
        [CmdParameter("--origqr")]
        public bool UseOriginalQR
        {
            get => Model.UseOriginalQR;
            set => SetModelProperty(value);
        }

        public bool RainbowMode
        {
            get => Model.RainbowMode;
            set => SetModelProperty(value);
        }
    }
}