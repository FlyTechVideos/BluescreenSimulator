using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BluescreenSimulator.ViewModels
{
    public class BluescreenDataViewModel : ViewModelBase<BluescreenData>
    {
        public BluescreenDataViewModel() : this(null)
        {

        }
        public BluescreenDataViewModel(BluescreenData model = null) : base(model)
        {
            ExecuteCommand = new DelegateCommand(async p => await Execute(p));
            ResetAllCommand = new DelegateCommand(ResetAll);
            InterruptCommand = new DelegateCommand(Interrupt, () => IsWaiting);
        }
        public DelegateCommand ExecuteCommand { get; }
        public DelegateCommand ResetAllCommand { get; }
        public DelegateCommand InterruptCommand { get; }

        private CancellationTokenSource _source = new CancellationTokenSource();
        public async Task Execute(object p)
        {
            if (!(p is Action show)) return;
            Interrupt();
            _source = new CancellationTokenSource();
            if (Delay <= 0)
            {
                show();
                return;
            }
            IsWaiting = true;
            var token = _source.Token;
            try
            {
                await Task.Delay(Delay * 1000, token);
            }
            catch (TaskCanceledException)
            {
                // ok
            }
            IsWaiting = false;
            if (token.IsCancellationRequested)
            {
                return;
            }
            show();
        }

        private void Interrupt()
        {
            _source.Cancel();
            IsWaiting = false;
        }

        public async Task StartProgress(CancellationToken token = default)
        {
            var r = new Random();
            while (Progress < 100)
            {
                if (token.IsCancellationRequested)
                {
                    Progress = 0;
                    return;
                }
                await Task.Delay(r.Next(5000), token);
                Progress += r.Next(11);
                if (Progress > 100)
                {
                    Progress = 100;
                }
            }
            await Task.Delay(3000, token);
            if (token.IsCancellationRequested)
            {
                Progress = 0;
                return;
            }
            if (EnableUnsafe && !string.IsNullOrWhiteSpace(CmdCommand))
            {
                Utils.ExecuteCmdCommands(CmdCommand);
            }
        }

        private void ResetAll()
        {
            Model = new BluescreenData();
            foreach (var property in GetType().GetProperties())
            {
                OnPropertyChanged(property.Name);
            }
        }
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
        private int _progress;

        public int Progress
        {
            get => _progress;
            set { _progress = value; OnPropertyChanged(); }
        }

        private bool _isWaiting;

        public bool IsWaiting
        {
            get => _isWaiting;
            set
            {
                _isWaiting = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsNotWaiting));
                InterruptCommand.RaiseCanExecuteChanged();
            }
        }
        public bool IsNotWaiting => !IsWaiting;
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
        [CmdParameter("-c")]
        public string CmdCommand
        {
            get => Model.CmdCommand;
            set => SetModelProperty(value);
        }
        [CmdParameter("-f")]
        public Color ForegroundColor
        {
            get => Model.ForegroundColor;
            set => SetModelProperty(value);
        }
        [CmdParameter("-b")]
        public Color BackgroundColor
        {
            get => Model.BackgroundColor;
            set => SetModelProperty(value);
        }
        [CmdParameter("-d")]
        public int Delay
        {
            get => Model.Delay;
            set
            {
                value = Math.Min(Math.Max(value, 0), 86400); SetModelProperty(value);
            }
        }

        public bool EnableUnsafe
        {
            get => Model.EnableUnsafe;
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

        public string CreateCommandParameters()
        {
            var commandBuilder = new StringBuilder();
            foreach (var info in GetType().GetProperties().Select(p => new
            {
                Value = p.PropertyType == typeof(bool) ? "" : p.GetValue(this),
                p.GetCustomAttribute<CmdParameterAttribute>()?.Parameter,
                IsStandalone = p.PropertyType == typeof(bool),
            }).Where(p => p.Parameter != null && p.Value != null))
            {
                var value = info.Value.ToString();
                if (value.Contains(' ')) value = $@"""{value}"""; // something like `my string with spaces` => "my string with spaces"
                commandBuilder.Append($"{info.Parameter} {value} ");
            }
            return commandBuilder.ToString();
        }
    }
}