using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BluescreenSimulator.ViewModels;
using static System.Windows.Input.Key;
namespace BluescreenSimulator.Views
{
    public partial class BluescreenWindow : Window
    {
        private readonly Windows10BluescreenViewModel _vm;
        private readonly CancellationTokenSource _source = new CancellationTokenSource();
        private bool _realClose;
        public BluescreenWindow(Windows10BluescreenViewModel data)
        {
            DataContext = _vm = data;
            InitializeComponent();
            Cursor = Cursors.None;
            Loaded += Bluescreen_Loaded;
            Closing += Close;
            KeyDown += Window_PreviewKeyDown;
            HookKeyboard();
        }

        private void Close(object sender, CancelEventArgs e)
        {
            e.Cancel = !_realClose; // no. 
            _source.Cancel();         
            if (_realClose) Focus();
        }
        private async void Bluescreen_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await _vm.StartProgress(_source.Token);
                Close(); // we're done
            }
            catch (TaskCanceledException)
            {
                _vm.Progress = 0;
            }
        }

        private static readonly Key[] BlockingKeys = { Key.System, F4, LWin, RWin, Tab, LeftAlt, RightAlt };
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (_realClose) return;
            if (e.Key is F7)
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
            int scanCode;
            public int flags;
            int time;
            int dwExtraInfo;
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
        LowLevelKeyboardProcDelegate hookProc; // prevent gc
        const int WH_KEYBOARD_LL = 13;
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


        private void Window_Closed(object sender, EventArgs e)
        {
            UnhookWindowsHookEx(hHook); // release keyboard hook
        }

        #endregion
    }
}
