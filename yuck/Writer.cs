namespace yuck;

public static class Writer
{
    public static void WriteVarInt(Stream stream, int value)
    {
        while (true)
        {
            byte temp = (byte)(value & 0b01111111);
            value >>= 7;
            if (value != 0)
                temp |= 0b10000000;
            stream.WriteByte(temp);
            if (value == 0)
                break;
        }
    }

    public static void WriteUShort(Stream stream, ushort value)
    {
        stream.WriteByte((byte)((value >> 8) & 0xFF));
        stream.WriteByte((byte)(value & 0xFF));
    }

    public static void WriteString(Stream stream, string value)
    {
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(value);
        WriteVarInt(stream, bytes.Length);
        stream.Write(bytes, 0, bytes.Length);
    }

    public static int ReadVarInt(Stream stream)
    {
        int numRead = 0;
        int result = 0;
        byte read;
        do
        {
            int b = stream.ReadByte();
            if (b == -1)
                throw new EndOfStreamException();
    
            read = (byte)b;
            int value = read & 0b01111111;
            result |= value << (7 * numRead);
    
            numRead++;
            if (numRead > 5)
                throw new IOException("VarInt too big");
        } while ((read & 0b10000000) != 0);
    
        return result;
    }
}
