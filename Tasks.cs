using System.Diagnostics;
using System.Timers;

namespace ProcessMonitor
{
    internal class Tasks
    {
        /// <summary>
        /// Open a new Console windows of the application
        /// </summary>
        /// <param name="args">arguments to pass to the application</param>
        public static void NewWindow(string args, bool hidden = true)
        {
            using (Process newProcess = new Process())
            {
                var currentProcessModule = Process.GetCurrentProcess().MainModule;

                // Sanity Null Checks
                // In theory, should never happen.
                if(currentProcessModule == null) throw new Exception("Honestly... Idk what happened here.");
                if(currentProcessModule.FileName == null) throw new Exception("Honestly... Idk what happened here.");

                newProcess.StartInfo = new ProcessStartInfo(currentProcessModule.FileName);
                newProcess.StartInfo.UseShellExecute = !hidden;
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
                Console.WriteLine($"Process Name: {proc.ProcessName}");
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

                return $"{process} has been added";
            }

            return $"A process by the name {process} could not be found.";
        }

        /// <summary>
        /// Displays missing processes and plays a beeps to alert
        /// Current 10 beeps
        /// </summary>
        /// <param name="message">missing processes</param>
        public void Alarm(string message)
        {
            Console.WriteLine($"The follow processes are missing: \n{message}");
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

            // TODO
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

            if(!settings.settings.AlertOnEmpty && missing.Count != settings.settings.Processes.Count)
            {
                if(missing.Count > 0)
                {
                    Tasks.NewWindow($"alert {string.Join(",", missing)}", false);
                }
            }
            
        }
    }
}