using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using BluescreenSimulator;
namespace BluescreenSimulator.ViewModels
{
    public class BluescreenViewModelBase<T> : ViewModelBase<T>, IBluescreenViewModel where T : BluescreenBase, new()
    {
        public virtual string StyleName => "Bluescreen";

        public BluescreenViewModelBase() : this(null)
        {

        }
        public BluescreenViewModelBase(T model = null) : base(model)
        {
            ExecuteCommand = new DelegateCommand(async p => await Execute(p));
            ResetAllCommand = new DelegateCommand(ResetAll);
            InterruptCommand = new DelegateCommand(Interrupt, () => IsWaiting);
        }

        public DelegateCommand ExecuteCommand { get; }
        private CancellationTokenSource _source = new CancellationTokenSource();
        public DelegateCommand ResetAllCommand { get; }
        public DelegateCommand InterruptCommand { get; }
        public async Task Execute(object p)
        {
            if (!(p is Action show)) return;
            Interrupt();
            _source = new CancellationTokenSource();
            Progress = StartingProgress;
            if (Delay <= 0)
            {
                show();
                return;
            }
            IsWaiting = true;
            var token = _source.Token;
            await Task.Delay(Delay * 1000, token).ContinueWith(_ => { });
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
        [CmdParameter("-d")]
        public int Delay
        {
            get => Model.Delay;
            set
            {
                value = Math.Min(Math.Max(value, 0), 86400); SetModelProperty(value);
            }
        }

        public string CreateCommandParameters()
        {
            var commandBuilder = new StringBuilder();
            commandBuilder.Append("--direct ");
            var type = GetType();
            var @default = Activator.CreateInstance(type);
            foreach (var info in type.GetProperties().Select(p => new
            {
                DefaultValue = p.GetValue(@default),
                Value = p.GetValue(this),
                p.GetCustomAttribute<CmdParameterAttribute>()?.Parameter,
                IsStandalone = p.PropertyType == typeof(bool),
                p.Name
            }).Where(p => p.Parameter != null && p.Value != null))
            {
                if (info.Value is false || info.Value == info.DefaultValue || (info.Value?.Equals(info.DefaultValue) ?? false)) continue; // is default
                var value = info.Value.ToString();
                if (value.Contains(' ') || value.Any(c => !char.IsLetterOrDigit(c))) value = $@"""{value}"""; // something like `my string with spaces` => "my string with spaces"
                commandBuilder.Append($"{info.Parameter} {(info.Value is bool ? "" : value)} ");
            }
            return commandBuilder.ToString().Trim();
        }
        private void ResetAll()
        {
            Model = new T();
            foreach (var property in GetType().GetProperties())
            {
                OnPropertyChanged(property.Name);
            }
        }
        private int _progress;

        public virtual int Progress
        {
            get => _progress;
            set { _progress = Math.Min(value, 100); OnPropertyChanged(); }
        }
        private int _startingProgress;

        public int StartingProgress
        {
            get { return _startingProgress; }
            set { _startingProgress = Math.Min(value, 100); OnPropertyChanged(); }
        }

        public bool EnableUnsafe
        {
            get => Model.EnableUnsafe;
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

        public bool RainbowMode
        {
            get => Model.RainbowMode;
            set => SetModelProperty(value);
        }
        public virtual bool SupportsRainbow => false;

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
    }
}