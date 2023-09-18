namespace WakeOnLan;

internal static class Program
{
    private static readonly Option Back = new("Back", Console.Clear, true);

    private static void Main()
    {
        Console.Title = "WakeOnLan";

        DataContext.Initialize();

        var mainMenu = Menu.CreateMenu(MainMenuOptions);
        mainMenu.RunMenu();
    }

    private static Menu GetDeleteMenu()
    {
        var options = DataContext.Targets
            .Select(x => new Option(x.Name, () => TargetManager.DeleteTarget(x), true))
            .Append(Back)
            .ToArray();
        var deleteMenu = Menu.CreateMenu(options);

        return deleteMenu;
    }

    private static Menu GetWakeMenu()
    {
        var options = DataContext.Targets
            .Select(x => new Option(x.Name, x.Wake))
            .Append(Back)
            .ToArray();
        var wakeMenu = Menu.CreateMenu(options);

        return wakeMenu;
    }

    private static readonly Option[] MainMenuOptions =
    {
        new("Wake", () => GetWakeMenu().RunMenu()),
        new("Add", () => TargetManager.AddTarget()),
        new("Remove", () => GetDeleteMenu().RunMenu()),
        new("Exit", () => Environment.Exit(0))
    };
}