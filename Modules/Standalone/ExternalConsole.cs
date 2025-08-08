using LUXED.Wrappers;
using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace LUXED.Modules.Standalone
{
    internal class ExternalConsole
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool FreeConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleOutputCP(uint wCodePageID);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCP(uint wCodePageID);

        private const int STD_OUTPUT_HANDLE = -11;
        private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;

        public static void Init()
        {
            IntPtr consoleWindow = GetConsoleWindow();
            if (consoleWindow == IntPtr.Zero)
            {
                if (!AllocConsole())
                {
                    HDLogger.LogError("Failed to allocate console.");
                    return;
                }
                SetConsoleOutputCP(65001);
                SetConsoleCP(65001);

                var stdHandle = GetStdHandle(STD_OUTPUT_HANDLE);
                var safeHandle = new SafeFileHandle(stdHandle, true);
                var fileStream = new FileStream(safeHandle, FileAccess.Write);
                var encoding = System.Text.Encoding.UTF8;
                var writer = new StreamWriter(fileStream, encoding) { AutoFlush = true };
                Console.SetOut(writer);
                Console.SetError(writer);

                if (GetConsoleMode(stdHandle, out uint mode))
                {
                    SetConsoleMode(stdHandle, mode | ENABLE_VIRTUAL_TERMINAL_PROCESSING);
                }

                Console.CursorVisible = false;
                Console.Title = "Melexed";

                Console.WriteLine(@"
 ▄▀▀▄ ▄▀▄  ▄▀▀█▄▄▄▄  ▄▀▀▀▀▄     ▄▀▀█▄▄▄▄  ▄▀▀▄  ▄▀▄  ▄▀▀█▄▄▄▄  ▄▀▀█▄▄  
█  █ ▀  █ ▐  ▄▀   ▐ █    █     ▐  ▄▀   ▐ █    █   █ ▐  ▄▀   ▐ █ ▄▀   █ 
▐  █    █   █▄▄▄▄▄  ▐    █       █▄▄▄▄▄  ▐     ▀▄▀    █▄▄▄▄▄  ▐ █    █ 
  █    █    █    ▌      █        █    ▌       ▄▀ █    █    ▌    █    █ 
▄▀   ▄▀    ▄▀▄▄▄▄     ▄▀▄▄▄▄▄▄▀ ▄▀▄▄▄▄       █  ▄▀   ▄▀▄▄▄▄    ▄▀▄▄▄▄▀ 
█    █     █    ▐     █         █    ▐     ▄▀  ▄▀    █    ▐   █     ▐  
▐    ▐     ▐          ▐         ▐         █    ▐     ▐        ▐        

      _
     |n|
   __|i|__
   \+-g-+/       Beautiful girls, dead feelings...
    ~|g|~        One day the sun is gonna explode and all this was for nothing.
     |a|         Ported to melon by that's cool guy :3.
      \|
");
            }
            else
            {
                var stdHandle = GetStdHandle(STD_OUTPUT_HANDLE);
                if (GetConsoleMode(stdHandle, out uint mode))
                {
                    SetConsoleMode(stdHandle, mode | ENABLE_VIRTUAL_TERMINAL_PROCESSING);
                }
                HDLogger.Log("A console has already been allocated.", HDLogger.LogsType.Info);
            }
        }
    }
}
