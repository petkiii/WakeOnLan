namespace WakeOnLan
{
    public static class Program
    {
        private static readonly Option Back = new("Back", Console.Clear, true);

        private static void Main()
        {
            Console.Title = "WakeOnLan";

            var context = new DataContext();
            context.Database.EnsureCreated();

            var mainMenuOptions = GetMainMenuOptions(context);
            var mainMenu = Menu.CreateMenu(mainMenuOptions);

            mainMenu.RunMenu();
        }

        private static Menu GetDeleteMenu(DataContext context)
        {
            var options = context.Targets
                .ToList()
                .Select(x => new Option(x.Name, () => TargetManager.DeleteTarget(context, x), true))
                .Append(Back)
                .ToArray();
            var deleteMenu = Menu.CreateMenu(options);

            return deleteMenu;
        }

        private static Menu GetWakeMenu(DataContext context)
        {
            var options = context.Targets
                .ToList()
                .Select(x => new Option(x.Name, x.Wake))
                .Append(Back)
                .ToArray();
            var wakeMenu = Menu.CreateMenu(options);

            return wakeMenu;
        }

        private static Option[] GetMainMenuOptions(DataContext context) =>
            new[]
            {
                new Option("Wake", () => GetWakeMenu(context).RunMenu()),
                new Option("Add", () => TargetManager.AddTarget(context)),
                new Option("Remove", () => GetDeleteMenu(context).RunMenu()),
                new Option("Exit", () => Environment.Exit(0)),
            };
    }
}