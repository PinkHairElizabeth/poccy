using System.Diagnostics;

namespace Poccy
{
    internal static class Installation
    {
        static string Warning = @"
            This will automatically start this application in the location it is currently located in.
            Type 'Y' to proceed. Type another key to exit if you wish to move the application.
        ";

        /// <summary>
        /// This really only works for Windows.
        /// Needs to be updated for future releases.
        /// </summary>
        public static void Install()
        {
            Console.WriteLine(Warning);
            Console.WriteLine("Type 'Y' to proceed: ");

            if (Console.ReadKey().Key == ConsoleKey.Y)
            {
                var currentProcess = Process.GetCurrentProcess().MainModule;

                if (currentProcess == null)
                {
                    Console.WriteLine("Unknown Error!");
                    return;
                }

                var shortcutPath = $"%APPDATA%\\Microsoft\\Windows\\Start Menu\\Programs\\Startup\\{typeof(Installation).Namespace}.lnk";
                var powershellCommand = "-NoProfile -Command " +
                    $"$TargetPath = '{currentProcess.FileName}'; " +
                    $"$ShortcutPath = '{Environment.ExpandEnvironmentVariables(shortcutPath)}'; " +
                    "$wShell = New-Object -ComObject WScript.Shell; " +
                    "$Shortcut = $wShell.CreateShortcut($ShortcutPath); " + 
                    "$Shortcut.TargetPath = $TargetPath; " +
                    "$Shortcut.Arguments = 'start';" +
                    "$Shortcut.Save();";

                using (Process process = new Process())
                {
                    process.StartInfo.FileName = "powershell";
                    process.StartInfo.Arguments = powershellCommand;
                    process.Start();
                }
            }
        }
    }
}