using yuck;

internal class Program
{
    private static void Main(string[] args)
    {
        var status = new Pinger("mc.hypixel.net", 25565).SendPacket();

        Console.WriteLine("MOTD: \n" + status.Description.Text);
    }
}