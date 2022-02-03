using System.ComponentModel.DataAnnotations;

namespace ProcessMonitor
{
    internal enum ARGS
    { 
        [Display(Name = "help", Description = "Displays the avalible commands.")]
        HELP = 0,

        [Display(Name = "list", Description = "List running processes.")]
        LIST = 2, 

        [Display(Name = "add", Description = "Add a process to monitor.")]
        ADD = 4,

        [Display(Name = "start", Description = "Start monitoring processes in background.")]
        START = 6,

        [Display(Name = "alert", Description = "(TEST) Triggers an alert as if a process monitored is missing.", Order = 2)]
        ALERT = 8,

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

        public static ARGS GetField(string arg) => (ARGS)Enum.Parse(typeof(ARGS), arg);
        public static string GetName(this ARGS arg) => GetField(arg).Name ?? "";
        public static string GetDescription(this ARGS arg) => GetField(arg).Description ?? "";

        public static void PrintArgs()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("The following commands are avalible:");
            Console.Write(Environment.NewLine);

            var args = Enum.GetValues<ARGS>();
            foreach(ARGS arg in args)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(arg.GetName());
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\t" + arg.GetDescription());
            }

            Console.Write(Environment.NewLine);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Made with ♥ by Elizabeth");
        }
    }

}