using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BluescreenSimulator.Views;

namespace BluescreenSimulator.ViewModels
{
    [BluescreenView(typeof(BluescreenWindowWin7))]
    public class Windows7BluescreenViewModel : BluescreenViewModelBase<Windows7Bluescreen>
    {
        public override string StyleName => "Windows 7 Style";

        public string DumpComplete
        {
            get => Model.DumpComplete;
            set => SetModelProperty(value);
        }

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
        public string DumpStart
        {
            get => Model.DumpStart;
            set => SetModelProperty(value);
        }

        public string ErrorCode
        {
            get => Model.ErrorCode;
            set => SetModelProperty(value);
        }

        public string Header
        {
            get => Model.Header;
            set => SetModelProperty(value);
        }

        public string Steps
        {
            get => Model.Steps;
            set => SetModelProperty(value);
        }

        public string StepsHeader
        {
            get => Model.StepsHeader;
            set => SetModelProperty(value);
        }

        public string TechnicalInfoHeader
        {
            get => Model.TechnicalInfoHeader;
            set => SetModelProperty(value);
        }

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
