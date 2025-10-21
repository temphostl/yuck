using System.Net;
using yuck;

namespace yuck;

internal class Program
{
    private static void Main(string[] args)
    {
        var status = new Pinger("mc.hypixel.net", 25565).SendPacket();

        Console.WriteLine("\n" + status.Description.Text
            + "\n\n" +
            status.Version.Name +
            " (Protocol: " + status.Version.Protocol + ")"
            + "\n" + "Online: " + status.Players.Online
            );
    }
}