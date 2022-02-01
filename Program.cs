namespace ProcessMonitor
{
    internal class Program
    {
        static int Main(string[] args)
        {
            Tasks tasks = new Tasks();

            if (args.Length == 0)
            {
                Tasks.HelpMenu();
                return 1;
            }

            switch (args[0].ToLower())
            {
                case COMMANDS.LIST:
                    tasks.ListProcesses();
                    break;

                case COMMANDS.ADD:
                    Console.WriteLine(tasks.AddProcesses(args[1]));
                    break;

                // TODO
                // ~~Need to verify this is actually working~~
                // Verified. It does not work :'(
                case COMMANDS.START:
                    if (args.Length > 1 && args[1] != "background") //TODO
                    {
                        Tasks.NewWindow("start background"); //TODO
                        return 0;
                    }

                    tasks.Start();
                    break;

                case COMMANDS.ALERT:
                    tasks.Alarm(args[1]);
                    break;

                case COMMANDS.HELP:
                default:
                    Tasks.HelpMenu();
                    break;
            }

            return 0; 
        }
    }
}