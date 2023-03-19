using System.Threading;
using BluescreenSimulator.Views;

namespace BluescreenSimulator.ViewModels
{
    [BluescreenView(typeof(BluescreenWindow))]
    [CmdParameter("--win10", Description = "Uses a windows 10 blue screen.")]
    public class Windows10BluescreenViewModel : BluescreenViewModelBase<Windows10Bluescreen>
    {
        public override string StyleName => "Windows 10 Style";

        public Windows10BluescreenViewModel() : this(null)
        {

        }
        public Windows10BluescreenViewModel(Windows10Bluescreen model = null) : base(model)
        {
            
        }
        private CancellationTokenSource _source = new CancellationTokenSource();

        [CmdParameter("-e", Description = "{Text} for Emoticon", FullAlias = "emoticon")]
        public string Emoticon
        {
            get => Model.Emoticon;
            set => SetModelProperty(value);
        }
        [CmdParameter("-m1", Description = "{Text} for Main Text (Line 1)", FullAlias = "main1")]
        public string MainText1
        {
            get => Model.MainText1;
            set => SetModelProperty(value);
        }
        [CmdParameter("-m2", Description = "{Text} for Main Text (Line 2)", FullAlias = "main2")]
        public string MainText2
        {
            get => Model.MainText2;
            set => SetModelProperty(value);
        }
        [CmdParameter("-p", Description = "{Text} for More Info", FullAlias = "progress")]
        public string Complete
        {
            get => Model.Complete;
            set => SetModelProperty(value);
        }
        [CmdParameter("-mi", Description = "{Text} for More Info", FullAlias = "moreinfo")]
        public string MoreInfo
        {
            get => Model.MoreInfo;
            set => SetModelProperty(value);
        }
        [CmdParameter("-s", Description = "{Text} for Support Person", FullAlias = "supportperson")]
        public string SupportPerson
        {
            get => Model.SupportPerson;
            set => SetModelProperty(value);
        }
        [CmdParameter("-sc", Description = "{Text} for Stop code", FullAlias = "stopcode")]
        public string StopCode
        {
            get => Model.StopCode;
            set => SetModelProperty(value);
        }
        [CmdParameter("-hq", Description = "Hides the QR code", FullAlias = "hideqr")]
        public bool HideQR
        {
            get => Model.HideQR;
            set => SetModelProperty(value, others: nameof(ShowQR));
        }

        public bool ShowQR => !HideQR;
        [CmdParameter("-oq", Description = "Use original QR code", FullAlias = "origqr")]
        public bool UseOriginalQR
        {
            get => Model.UseOriginalQR;
            set => SetModelProperty(value);
        }

        [CmdParameter("-td", Description = "Specify Delay For Text Display", FullAlias = "textdelay")]
        public float TextDelay
        {
            get => Model.TextDelay;
            set => SetModelProperty(value);
        }
        
        public override bool SupportsRainbow => true;
    }
}