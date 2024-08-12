using System;
using System.Runtime.InteropServices;
using AssetStudio;

namespace AssetStudioGUI
{
    internal static class ConsoleWindow
    {
        private enum CtrlSignalType
        {
            CTRL_C_EVENT,
            CTRL_BREAK_EVENT,
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        private delegate bool EventHandler(CtrlSignalType ctrlSignal);
        private static EventHandler eventHandler;
        private static IntPtr ConsoleWindowHandle;
        private static readonly int SW_HIDE = 0;
        private static readonly int SW_SHOW = 5;

        private static bool CloseEventHandler(CtrlSignalType ctrlSignal)
        {
            switch (ctrlSignal)
            {
                case CtrlSignalType.CTRL_C_EVENT:
                case CtrlSignalType.CTRL_BREAK_EVENT:
                    return true;
                default:
                    Logger.Verbose("Closing AssetStudio");
                    return false;
            }
        }

        public static void RunConsole(bool showConsole)
        {
            AllocConsole();
            ConsoleWindowHandle = GetConsoleWindow();
            eventHandler += CloseEventHandler;
            SetConsoleCtrlHandler(eventHandler, true);

            if (!showConsole)
                HideConsoleWindow();
        }

        public static void ShowConsoleWindow()
        {
            ShowWindow(ConsoleWindowHandle, SW_SHOW);
        }

        public static void HideConsoleWindow()
        {
            ShowWindow(ConsoleWindowHandle, SW_HIDE);
        }
    }
}
