﻿using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

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
            InterruptCommand = new DelegateCommand(Interrupt, () => IsWaiting);
        }

        public DelegateCommand ExecuteCommand { get; }
        private CancellationTokenSource _source = new CancellationTokenSource();
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
            await Task.Delay((int)(Delay * 1000), token).ContinueWith(_ => { });
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
        [CmdParameter("-d", Description = "Bluescreen Delay {duration} in seconds (0-86400)", FullAlias = "delay")]
        public double Delay
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
            var type = GetType();
            commandBuilder.Append(type.GetCustomAttribute<CmdParameterAttribute>()?.Parameter ?? "--direct");
            commandBuilder.Append(' ');
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
                var value = info.Value.ToString().Replace(Environment.NewLine, @"\n");
                if (value.Contains(' ') || value.Any(c => !char.IsLetterOrDigit(c))) value = $@"""{value}"""; // something like `my string with spaces` => "my string with spaces"
                commandBuilder.Append($"{info.Parameter} {(info.Value is bool ? "" : value)} ");
            }
            return commandBuilder.ToString().Trim();
        }
        private int _progress;

        public virtual int Progress
        {
            get => _progress;
            set { _progress = Math.Min(value, 100); OnPropertyChanged(); }
        }
        [CmdParameter("-sp", Description = "The bluescreen progress at start.", FullAlias = "start-progress")]
        public int StartingProgress
        {
            get => Model.StartingProgress;
            set => SetModelProperty(Math.Max(0, Math.Min(value, 100)));
        }
        [CmdParameter("-c", Description = "The {command} to run after complete (Careful!)", FullAlias = "cmd")]
        public string CmdCommand
        {
            get => Model.CmdCommand;
            set => SetModelProperty(value);
        }
        [CmdParameter("-f", Description = "Foreground (text) in rgb {value} hex format (#FFFFFF)", FullAlias = "foreground")]
        public Color ForegroundColor
        {
            get => Model.ForegroundColor;
            set => SetModelProperty(value);
        }
        [CmdParameter("-b", Description = "Background color in rgb {value} hex format (#FFFFFF)", FullAlias = "background")]
        public Color BackgroundColor
        {
            get => Model.BackgroundColor;
            set => SetModelProperty(value);
        }
        [CmdParameter("-r", Description = "Enable rainbow mode (discards background color settings)", FullAlias = "rainbow")]
        public bool RainbowMode
        {
            get => Model.RainbowMode;
            set => SetModelProperty(value);
        }
        [CmdParameter("-pf", Description = "Factor to scale the progress speed with, e.g., 2.1x (min 0.1, max 10)", FullAlias = "progress-factor")]
        public double ProgressFactor
        {
            get => Model.ProgressFactor;
            set => SetModelProperty(Math.Max(0.1, Math.Min(value, 10.0)));
        }
        [CmdParameter("-psd", Description = "Seconds to wait before the progress starts counting up", FullAlias = "progress-start-delay")]
        public double ProgressStartDelay
        {
            get => Model.ProgressStartDelay;
            set => SetModelProperty(Math.Max(0,value));
        }
        [CmdParameter("-cafd", Description = "Seconds to wait after progress bar completion before closing the BSOD", FullAlias = "closing-after-start-delay")]
        public double ClosingAfterFinishDelay
        {
            get => Model.ClosingAfterFinishDelay;
            set => SetModelProperty(Math.Max(0, value));
        }
        public virtual bool SupportsRainbow => false;

        public async Task StartProgress(CancellationToken token = default)
        {
            var r = new Random();
            Progress = StartingProgress;
            await Task.Delay((int)(ProgressStartDelay * 1000), token);
            while (Progress < 100)
            {
                if (token.IsCancellationRequested)
                {
                    Progress = 0;
                    return;
                }
                await Task.Delay((int)(r.Next(5000) * ProgressFactor), token);
                Progress += r.Next(2, 11);
                if (Progress > 100)
                {
                    Progress = 100;
                }
            }
            await Task.Delay((int)(ClosingAfterFinishDelay * 1000), token);
            if (token.IsCancellationRequested)
            {
                Progress = 0;
                return;
            }
            if (!string.IsNullOrWhiteSpace(CmdCommand))
            {
                Utils.ExecuteCmdCommands(CmdCommand);
            }
        }
    }
}