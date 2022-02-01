using System.Diagnostics;
using System.Timers;

namespace ProcessMonitor
{
    // TODO: enum
    internal static class COMMANDS
    {
        public const string HELP = "help";
        public const string LIST = "list";
        public const string ADD = "add";
        public const string START = "start";
        public const string ALERT = "alert";
    }

    internal class Tasks
    {
        /// <summary>
        /// TODO
        /// </summary>
        public static void HelpMenu()
        {
            string result = "The following commands are avalible:\n\n";

            result += COMMANDS.HELP + "\t\tDisplays the avalible commands." + "\n";
            result += COMMANDS.LIST + "\t\tList running processes." + "\n";
            result += COMMANDS.ADD + "\t\tAdd a process to monitor." + "\n";
            result += COMMANDS.START + "\t\tStart monitoring processes in background." + "\n";
            result += COMMANDS.ALERT + "\t\t(TEST) Triggers an alert as if a process monitored is missing." + "\n";
            result += "\n\n";

            Console.WriteLine(result);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="args"></param>
        public static void NewWindow(string args, bool hidden = true)
        {
            //Process.GetCurrentProcess()
            using (Process newProcess = new Process())
            {
                newProcess.StartInfo = new ProcessStartInfo(Process.GetCurrentProcess().MainModule.FileName);
                newProcess.StartInfo.UseShellExecute = true;
                newProcess.StartInfo.Arguments = args;
                newProcess.StartInfo.CreateNoWindow = hidden;
                newProcess.Start();
            }
        }

        /// <summary>
        /// List all avalible processes running
        /// </summary>
        public void ListProcesses()
        {
            Process[] localProcesses = Process.GetProcesses();

            foreach (Process proc in localProcesses)
            {
                Console.WriteLine("Process Name: " + proc.ProcessName);
            }
        }

        /// <summary>
        /// Attempts to find a running process by name and adds to the processes monitored if found.
        /// </summary>
        /// <param name="process">Name of Process</param>
        /// <returns>Return string for user</returns>
        public string AddProcesses(string process)
        {
            Process[] localByName = Process.GetProcessesByName(process);
            if (localByName.Length != 0)
            {
                Settings settings = new Settings();
                settings.settings.Processes.Add(process);
                settings.Save();

                return process + " has been added";
            }

            return "A process by the name " + process + " could not be found.";
        }

        /// <summary>
        /// Displays missing processes and plays a beeps to alert
        /// Current 10 beeps
        /// </summary>
        /// <param name="message">missing processes</param>
        public void Alarm(string message)
        {
            Console.WriteLine("The follow processes are missing: \n" + message);
            for (int i = 0; i <= 10; i++)
            {
                Console.Beep();
            }
            Console.Read();
        }


        /// <summary>
        /// Starts program for running in background.
        /// </summary>
        public void Start()
        {
            Settings settings = new Settings();

            System.Timers.Timer timer = new System.Timers.Timer(settings.settings.PingFrequency * ( 60 * 1000));
            timer.Elapsed += TimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Start();

            // This is not the best way of handling this.
            // This should actually run as a background task.
            // For time sake, this should work for now though.
            Console.Read();
        }

        private static void TimedEvent(Object? source, ElapsedEventArgs? e)
        {
            // It makes sense to read the settings every time in case they have been modified
            Settings settings = new Settings();

            HashSet<string> missing = new HashSet<string>();
            foreach (string process in settings.settings.Processes)
            { 
                if(Process.GetProcessesByName(process).Length == 0)
                {
                    missing.Add(process);
                }
            }

            if(missing.Count > 0)
            {
                Tasks.NewWindow("alert " + string.Join(",", missing), false);
            }
        }
    }
}