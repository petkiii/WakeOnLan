using System.Net;
using System.Net.Sockets;

namespace WakeOnLan;

internal static class WakeOnLan
{
    public static void Wake(this Target target)
    {
        Console.WriteLine("Sending wake...");

        try
        {
            SendWake(target);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return;
        }

        Console.WriteLine("Wake sent.");
    }

    private static void SendWake(Target target)
    {
        var header = Enumerable.Repeat(byte.MaxValue, 6);
        var address = Dns.GetHostAddresses(target.Host).First();
        var data = Enumerable.Repeat(target.MacAddress.GetAddressBytes(), 16).SelectMany(x => x);

        var magicPacket = header.Concat(data).ToArray();

        using var client = new UdpClient();

        client.Send(magicPacket, magicPacket.Length, new IPEndPoint(address, target.Port));
    }
}
