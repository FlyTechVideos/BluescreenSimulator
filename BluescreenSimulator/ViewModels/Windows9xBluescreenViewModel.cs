using System.Diagnostics.CodeAnalysis;

namespace BluescreenSimulator.ViewModels
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Windows9xBluescreenViewModel : BaseBluescreenDataViewModel<Windows9xBluescreen>
    {
        public Windows9xBluescreenViewModel() : this(null)
        {
            
        }

        public Windows9xBluescreenViewModel(Windows9xBluescreen model = null) : base(model)
        {
        }

        public string Error
        {
            get => Model.Error;
            set => SetModelProperty(value);
        }

        public string Header
        {
            get => Model.Header;
            set => SetModelProperty(value);
        }

        public string InfoLine1
        {
            get => Model.InfoLine1;
            set => SetModelProperty(value);
        }
        public string InfoLine2
        {
            get => Model.InfoLine2;
            set => SetModelProperty(value);
        }

        public string Instructions
        {
            get => Model.Instructions;
            set => SetModelProperty(value);
        }

        public string ToContinue
        {
            get => Model.ToContinue;
            set => SetModelProperty(value);
        }
    }
}