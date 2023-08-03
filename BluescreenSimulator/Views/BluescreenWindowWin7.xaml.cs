using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BluescreenSimulator.ViewModels;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using static System.Windows.Input.Key;
using System.Linq;

namespace BluescreenSimulator.Views
{
    /// <summary>
    /// Interaction logic for BluescreenWindowWin7.xaml
    /// </summary>
    public partial class BluescreenWindowWin7 : Window
    {
        //Variables
        private readonly Windows7BluescreenViewModel _vm;
        private bool _realClose;


        public BluescreenWindowWin7(Windows7BluescreenViewModel vm = null)
        {
            DataContext = _vm = vm ?? new Windows7BluescreenViewModel();
            InitializeComponent();
            Cursor = Cursors.None;
            Loaded += Bluescreen_Loaded;
            Closing += Window_AboutToClose;
            KeyDown += Window_PreviewKeyDown;
            HookKeyboard();
        }

        private readonly CancellationTokenSource _source = new CancellationTokenSource();

        private async void Bluescreen_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await _vm.StartProgress(_source.Token);
                _realClose = true;
                Close(); // we're done
            }
            catch (TaskCanceledException)
            {
                _vm.Progress = 0;
            }
        }

        private void Window_AboutToClose(object sender, CancelEventArgs e)
        {
            e.Cancel = !_realClose; // no.
            if (!e.Cancel)
            {
                _source.Cancel();
                UnhookWindowsHookEx(hHook);
            }
            else
            {
                Focus();
            }
        }

        private static readonly Key[] BlockingKeys = { Key.System, F4, LWin, RWin, Tab, LeftAlt, RightAlt };

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (_realClose) return;
            if (e.Key is Key.F7)
            {
                _realClose = true;
                Close();
            }

            if (BlockingKeys.Contains(e.Key))
            {
                e.Handled = true;
            }
        }


        #region Windows Api

        private void HookKeyboard()
        {
            var hModule = GetModuleHandle(IntPtr.Zero);
            hookProc = LowLevelKeyboardProc;
            hHook = SetWindowsHookEx(WH_KEYBOARD_LL, hookProc, hModule, 0);
            if (hHook == IntPtr.Zero)
            {
                MessageBox.Show("Failed to set hook, error = " + Marshal.GetLastWin32Error());
            }
        }

        private struct KBDLLHOOKSTRUCT
        {
            public int vkCode;
            private int scanCode;
            public int flags;
            private int time;
            private int dwExtraInfo;
        }

        private delegate int LowLevelKeyboardProcDelegate(int nCode, int wParam, ref KBDLLHOOKSTRUCT lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProcDelegate lpfn, IntPtr hMod, int dwThreadId);

        [DllImport("user32.dll")]
        private static extern bool UnhookWindowsHookEx(IntPtr hHook);

        [DllImport("user32.dll")]
        private static extern int CallNextHookEx(int hHook, int nCode, int wParam, ref KBDLLHOOKSTRUCT lParam);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(IntPtr path);

        private IntPtr hHook;
        private LowLevelKeyboardProcDelegate hookProc; // prevent gc
        private const int WH_KEYBOARD_LL = 13;

        private static int LowLevelKeyboardProc(int nCode, int wParam, ref KBDLLHOOKSTRUCT lParam)
        {
            if (nCode >= 0)
                switch (wParam)
                {
                    case 256: // WM_KEYDOWN
                    case 257: // WM_KEYUP
                    case 260: // WM_SYSKEYDOWN
                    case 261: // M_SYSKEYUP
                        if (
                            (lParam.flags == 32) || // alt whatever after
                            (lParam.vkCode == 0x1b && lParam.flags == 32) || // Alt+Esc
                            (lParam.vkCode == 0x73 && lParam.flags == 32) || // Alt+F4
                            (lParam.vkCode == 0x1b && lParam.flags == 0) || // Ctrl+Esc
                            (lParam.vkCode == 0x5b && lParam.flags == 1) || // Left Windows Key
                            (lParam.vkCode == 0x5c && lParam.flags == 1))    // Right Windows Key
                        {
                            return 1; //Do not handle key events
                        }
                        break;
                }
            return CallNextHookEx(0, nCode, wParam, ref lParam);
        }

        #endregion Windows Api
    }
}