using System.Diagnostics.CodeAnalysis;
using BluescreenSimulator.Views;

namespace BluescreenSimulator.ViewModels
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [CmdParameter("--win9x", Description = "Uses a windows 9x blue screen.")]
    [BluescreenView(typeof(BluescreenWindow9x))]
    public class Windows9xBluescreenViewModel : BluescreenViewModelBase<Windows9xBluescreen>
    {
        public override string StyleName => "Windows 9x Style";

        public Windows9xBluescreenViewModel() : this(null)
        {
            
        }

        public Windows9xBluescreenViewModel(Windows9xBluescreen model = null) : base(model)
        {
        }

        [CmdParameter("-e", Description = "The error, displayed on the bottom.", FullAlias = "error")]
        public string Error
        {
            get => Model.Error;
            set => SetModelProperty(value);
        }
        [CmdParameter("-bh", Description = "The header, displayed on a gray background, defaults to Windows.", FullAlias = "header")]
        public string Header
        {
            get => Model.Header;
            set => SetModelProperty(value);
        }
        [CmdParameter("-i1", Description = "The first line of information.", FullAlias = "infoline1")]
        public string InfoLine1
        {
            get => Model.InfoLine1;
            set => SetModelProperty(value);
        }
        [CmdParameter("-i2", Description = "The second line of information.", FullAlias = "infoline2")]
        public string InfoLine2
        {
            get => Model.InfoLine2;
            set => SetModelProperty(value);
        }
        [CmdParameter("-i", Description = "The instructions to follow.", FullAlias = "instructions")]
        public string Instructions
        {
            get => Model.Instructions;
            set => SetModelProperty(value);
        }
        [CmdParameter("-tc", Description = "The command to continue. Displayed on the bottom of the screen.", FullAlias = "continue")]
        public string ToContinue
        {
            get => Model.ToContinue;
            set => SetModelProperty(value);
        }
    }
}