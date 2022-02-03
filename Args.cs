using System.ComponentModel.DataAnnotations;

namespace ProcessMonitor
{
    internal enum ARGS
    { 
        [Display(AutoGenerateField = true)]
        UNKOWN = 0,

        [Display(Name = "help", Description = "Displays the avalible commands.")]
        HELP = 2,

        [Display(Name = "list", Description = "List running processes.")]
        LIST = 4, 

        [Display(Name = "add", Description = "Add a process to monitor.")]
        ADD = 6,

        [Display(Name = "start", Description = "Start monitoring process in background.")]
        START = 8,

        [Display(Name = "stop", Description = "Stops the monitoring process running in background.")]
        STOP = 10,

        [Display(Name = "alert", Description = "(TEST) Triggers an alert as if a process monitored is missing.", Order = 2)]
        ALERT = 12,

        [Display(Name = "settings", Description = "Show current settings.")]
        SETTINGS = 14,

        [Display(Name = "change", Description = "Change a settings value.")]
        CHANGE = 16,

    }

    internal static class Extensions
    {
        private static DisplayAttribute GetField(ARGS arg)
        { 
            var fieldInfo = arg.GetType().GetField(arg.ToString());
            if (fieldInfo == null) throw new Exception("Unknown Arugments");
            var descriptionAttributes = (DisplayAttribute[])fieldInfo.GetCustomAttributes(typeof(DisplayAttribute), false);
            if(descriptionAttributes.Length > 1) throw new Exception("Unknown Arugments");
            return descriptionAttributes[0];
        }

        public static string GetName(this ARGS arg) => GetField(arg).Name ?? "";
        public static string GetDescription(this ARGS arg) => GetField(arg).Description ?? "";

        public static ARGS GetField(string arg)
        {
            if (Enum.TryParse(typeof(ARGS), arg, true, out object? value) && value != null) return (ARGS)value;
            return ARGS.UNKOWN;
        } 

        public static void PrintArgs()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("The following commands are avalible:");
            Console.Write(Environment.NewLine);

            var args = Enum.GetValues<ARGS>();
            foreach(ARGS arg in args)
            {
                if(arg == ARGS.UNKOWN) continue;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(arg.GetName().PadRight(20));
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\t" + arg.GetDescription());
            }

            Console.Write(Environment.NewLine);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Made with ♥ by Elizabeth");
        }
    }

}