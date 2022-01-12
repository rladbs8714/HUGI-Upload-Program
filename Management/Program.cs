using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;
using System.IO;

namespace Management
{
    static class Program
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;
        private static bool lastKeyWasLetter = false;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Paths.Init();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            _hookID = SetHook(_proc);
            Application.Run(new Main());

            UnhookWindowsHookEx(_hookID);
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private static void ToggleCapsLock()
        {
            const int KEYEVENTF_EXTENDEDKEY = 0x1;
            const int KEYEVENTF_KEYUP = 0x2;

            UnhookWindowsHookEx(_hookID);
            keybd_event(0x14, 0x45, KEYEVENTF_EXTENDEDKEY, (UIntPtr)0);
            keybd_event(0x14, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, (UIntPtr)0);
            _hookID = SetHook(_proc);
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                Keys key = (Keys)Marshal.ReadInt32(lParam);
                if (key == Keys.F1)
                {
                    string title = FileManager.LoadFile(Paths.valueInputTextPath)["제목"];
                    Clipboard.SetText(title);
                    SendKeys.Send("^v");
                    return (IntPtr)1;
                }
                else if (key == Keys.F2)
                {
                    string contents = FileManager.LoadFile(Paths.valueInputTextPath)["경험담"];
                    Clipboard.SetText(contents);
                    SendKeys.Send("^v");
                    return (IntPtr)1;
                }
                else if (key == Keys.F3)
                {
                    string date = string.Format("{0:D2}{1:D2}{2:D2}", DateTime.Now.Year.ToString().Substring(2), DateTime.Now.Month, DateTime.Now.Day);
                    Clipboard.SetText(date.ToString());
                    SendKeys.Send("^v");
                    return (IntPtr)1;
                }
                else if (key == Keys.D1)
                {
                    if (new FileInfo(Paths.shortKeyListPath + "\\code.txt").Exists)
                    {
                        try
                        {
                            string contents = File.ReadAllText(Paths.shortKeyListPath + "\\code.txt");
                            Clipboard.SetText(contents);
                            SendKeys.Send("^v");
                        }
                        catch(ArgumentNullException e)
                        {
                            return (IntPtr)1;
                        }
                    }
                    return (IntPtr)1;
                }
                else if (key == Keys.D2)
                {
                    SendKeys.Send("^s");
                    return (IntPtr)1;
                }
                else if (key == Keys.D3)
                {
                    SendKeys.Send("%{F4}");
                    return (IntPtr)1;
                }
                else if (key == Keys.Oemtilde)
                {
                    Clipboard.SetText("asqw1234");
                    SendKeys.Send("^v");
                    return (IntPtr)1;
                }
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }
    }
}
