using System.Net.Sockets;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using yuck;
using static yuck.Writer;

internal class Program
{
    private static byte[] BuildHandshake(string ip, ushort port)
    {
        using var packetData = new MemoryStream();

        // packet id
        WriteVarInt(packetData, 0x00);
        // protocol version
        WriteVarInt(packetData, 763);
        // server address
        WriteString(packetData, ip);
        // server port
        WriteUShort(packetData, port);
        // next state
        WriteVarInt(packetData, 1);

        byte[] packetBody = packetData.ToArray();

        using var packet = new MemoryStream();
        WriteVarInt(packet, packetBody.Length);
        packet.Write(packetBody, 0, packetBody.Length);

        return packet.ToArray();
    }

    private static byte[] BuildStatus()
    {
        using var statusPacket = new MemoryStream();
        WriteVarInt(statusPacket, 0x00);
        byte[] body = statusPacket.ToArray();

        using var packet = new MemoryStream();
        WriteVarInt(packet, body.Length);
        packet.Write(body, 0, body.Length);

        return packet.ToArray();
    }

    private static ServerStatus SendPacket(string ip, ushort port)
    {
        using var client = new TcpClient(ip, port);
        using NetworkStream stream = client.GetStream();
    
        // send handshake
        byte[] handshake = BuildHandshake(ip, port);
        stream.Write(handshake, 0, handshake.Length);
    
        // send status request
        byte[] status = BuildStatus();
        stream.Write(status, 0, status.Length);
        stream.Flush();
    
        // read response length and packet id
        int length = ReadVarInt(stream);
        int packetId = ReadVarInt(stream);
    
        // read JSON length and JSON itself
        int jsonLength = ReadVarInt(stream);
        byte[] jsonBytes = new byte[jsonLength];
        int totalRead = 0;
        while (totalRead < jsonLength)
        {
            int read = stream.Read(jsonBytes, totalRead, jsonLength - totalRead);
            if (read <= 0)
                throw new EndOfStreamException("Stream ended before JSON was fully read.");
            totalRead += read;
        }

        string json = Encoding.UTF8.GetString(jsonBytes);
    
        // deserialize
        return JsonConvert.DeserializeObject<ServerStatus>(json);
    }

    private static void Main(string[] args)
    {
        var status = SendPacket("mc.hypixel.net", 25565);

        Console.WriteLine(status.EnforcesSecureChat);
    }
}