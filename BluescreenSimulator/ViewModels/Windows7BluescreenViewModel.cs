using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BluescreenSimulator.Views;

namespace BluescreenSimulator.ViewModels
{
    [BluescreenView(typeof(BluescreenWindowWin7))]
    [CmdParameter("--win7", Description = "Uses a windows 7 bluescreen.")]
    public class Windows7BluescreenViewModel : BluescreenViewModelBase<Windows7Bluescreen>
    {
        public override string StyleName => "Windows 7 Style";

        [CmdParameter("-dc", Description = "Text when the dump is complete.", FullAlias = "--dumpcomplete")]
        public string DumpComplete
        {
            get => Model.DumpComplete;
            set => SetModelProperty(value);
        }
        [CmdParameter("-dp", Description = "Text used to indicate the progress (@p is replaced by the current progress)", FullAlias = "--dumpprogress")]
        public string DumpProgress
        {
            get => Model.DumpProgress.Replace("@p", Progress.ToString());
            set => SetModelProperty(value, others: nameof(DumpProgressEdit));
        }
        public string DumpProgressEdit
        {
            get => Model.DumpProgress;
            set => DumpProgress = value;
        }
        [CmdParameter("-ds", Description = "Text used to indicate the dump initializing.", FullAlias = "--dumpstart")]
        public string DumpStart
        {
            get => Model.DumpStart;
            set => SetModelProperty(value);
        }
        [CmdParameter("-ec", Description = "The error code, shown on the second line.", FullAlias = "--error")]
        public string ErrorCode
        {
            get => Model.ErrorCode;
            set => SetModelProperty(value);
        }
        [CmdParameter("-bh", Description = "The header, used on the very first line.", FullAlias = "--header")]
        public string Header
        {
            get => Model.Header;
            set => SetModelProperty(value);
        }
        [CmdParameter("-s", Description = "The steps to follow.", FullAlias = "--steps")]
        public string Steps
        {
            get => Model.Steps;
            set => SetModelProperty(value);
        }
        [CmdParameter("-sh", Description = "The header preceding the steps.", FullAlias = "--stepsheader")]
        public string StepsHeader
        {
            get => Model.StepsHeader;
            set => SetModelProperty(value);
        }
        [CmdParameter("-th", Description = "The header used right before the stop code.", FullAlias = "--techheader")]
        public string TechnicalInfoHeader
        {
            get => Model.TechnicalInfoHeader;
            set => SetModelProperty(value);
        }
        [CmdParameter("-sc", Description = "The stop code", FullAlias = "--stopcode")]
        public string StopCode
        {
            get => Model.StopCode;
            set => SetModelProperty(value);
        }
        public override int Progress
        {
            get => base.Progress;
            set { base.Progress = value; OnPropertyChanged(nameof(IsDumpComplete)); OnPropertyChanged(nameof(DumpProgress));}
        }

        public bool IsDumpComplete => Progress >= 100;
    }
}
