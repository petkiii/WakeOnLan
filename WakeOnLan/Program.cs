﻿namespace WakeOnLan;

internal static class Program
{
    private static readonly Option Back = new("Back", stop: true);

    private static void Main()
    {
        Console.Title = "WakeOnLan";

        DataManager.EnsureCreated();
        DataManager.Load();

        var mainMenuOptions = GetMainMenuOptions();
        var mainMenu = Menu.CreateMenu(mainMenuOptions);

        mainMenu.RunMenu();
    }

    private static Menu GetDeleteMenu()
    {
        var options = DataManager.Targets
            .Select(x => new Option(x.Name, () => TargetManager.DeleteTarget(x), true))
            .Append(Back)
            .ToArray();
        var deleteMenu = Menu.CreateMenu(options);

        return deleteMenu;
    }

    private static Menu GetWakeMenu()
    {
        var options = DataManager.Targets
            .Select(x => new Option(x.Name, x.Wake))
            .Append(Back)
            .ToArray();
        var wakeMenu = Menu.CreateMenu(options);

        return wakeMenu;
    }

    private static Option[] GetMainMenuOptions() =>
        new[]
        {
            new Option("Wake", () => GetWakeMenu().RunMenu()),
            new Option("Add", () => TargetManager.AddTarget()),
            new Option("Remove", () => GetDeleteMenu().RunMenu()),
            new Option("Exit", () => Environment.Exit(0))
        };
}
