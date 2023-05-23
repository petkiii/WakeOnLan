﻿using System.Globalization;
using System.Net;
using System.Net.Sockets;

namespace WakeOnLan;

public static class WakeOnLan
{
    public static void Wake(this Target target)
    {
        Console.WriteLine("Sending wake...");
        SendWake(target);
        Console.WriteLine("Wake sent.");
    }

    private static void SendWake(Target target)
    {
        try
        {
            var header = Enumerable.Repeat(byte.MaxValue, 6);
            var address = Dns.GetHostAddresses(target.Host).First();
            var data = Enumerable.Repeat(target.MacAddress.GetAddressBytes(), 16).SelectMany(mac => mac);

            var magicPacket = header.Concat(data).ToArray();

            using var client = new UdpClient();
            
            client.Send(magicPacket, magicPacket.Length, new IPEndPoint(address, target.Port));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}