﻿using System.Net.NetworkInformation;

namespace WakeOnLan;

internal static class TargetManager
{
    public static void AddTarget()
    {
        try
        {
            Console.CursorVisible = true;

            var target = PromptAddTarget();

            DataManager.Add(target);
            DataManager.Save();
        }
        catch (InvalidInputException e)
        {
            Console.WriteLine($"{e.Message}: {e.Input}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            Console.CursorVisible = false;
        }
    }

    private static Target PromptAddTarget()
    {
        Console.Write("Enter name: ");
        var name = Console.ReadLine();

        if (string.IsNullOrEmpty(name))
            throw new InvalidInputException("Invalid name.", name);

        Console.Write("Enter IP address and port: ");
        var address = Console.ReadLine();

        if (string.IsNullOrEmpty(address))
            throw new InvalidInputException("Invalid IP address.", address);

        var split = address.Split(":", StringSplitOptions.RemoveEmptyEntries);

        if (split.Length != 2)
            throw new InvalidInputException("Invalid IP address.", address);

        //if (!Uri.TryCreate(split[0], UriKind.Absolute, out var host))
        //    throw new InvalidInputException("Invalid hostname.", split[0]);

        if (!int.TryParse(split[1], out var port) || port < 0)
            throw new InvalidInputException("Invalid port.", split[1]);

        Console.Write("Enter MAC address: ");
        var macAddressStr = Console.ReadLine();
        if (!PhysicalAddress.TryParse(macAddressStr, out var macAddress))
            throw new InvalidInputException("Invalid MAC address.", macAddressStr);

        return new Target
        {
            Name = name,
            NormalizedName = name.Replace(" ", "").ToLower(),
            Host = split[0],
            Port = port,
            MacAddress = macAddress
        };
    }

    public static void DeleteTarget(ITarget target)
    {
        try
        {
            DataManager.Remove(target);
            DataManager.Save();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
