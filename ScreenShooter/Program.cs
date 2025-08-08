using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

class Program
{
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;

    private static IntPtr _hookID = IntPtr.Zero;
    private static LowLevelKeyboardProc _proc = HookCallback;

    static void Main()
    {
        _hookID = SetHook(_proc);
        Console.WriteLine("Screenshooter running in background... \nCtrl + Shift + S to capture. \nCtrl + Shift + C to exit.\n");
        Application.Run();
        UnhookWindowsHookEx(_hookID);
    }

    private static IntPtr SetHook(LowLevelKeyboardProc proc)
    {
        using (Process curProcess = Process.GetCurrentProcess())
        using (ProcessModule curModule = curProcess.MainModule!)
        {
            return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                GetModuleHandle(curModule.ModuleName), 0);
        }
    }

    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

    private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
        {
            int vkCode = Marshal.ReadInt32(lParam);
            var key = (Keys)vkCode;

            bool ctrl = (GetAsyncKeyState((int)Keys.ControlKey) & 0x8000) != 0;
            bool shift = (GetAsyncKeyState((int)Keys.ShiftKey) & 0x8000) != 0;

            if (ctrl && shift && key == Keys.S)
            {
                Screenshot.Capture();
            }

            if (ctrl && shift && key == Keys.C)
            {
                Console.WriteLine("\nExit hotkey pressed.");
                Application.Exit();
            }
        }

        return CallNextHookEx(_hookID, nCode, wParam, lParam);
    }

    [DllImport("user32.dll")]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn,
        IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll")]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll")]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
        IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll")]
    private static extern IntPtr GetModuleHandle(string? lpModuleName);

    [DllImport("user32.dll")]
    private static extern short GetAsyncKeyState(int vKey);
}

