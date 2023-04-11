using System;
using System.Text;

[Serializable]
public class Packet
{
    public Packet()
    {
        id = "";
        command = "";
        args = null;
    }

    public Packet(string id, string command, params string[] args)
    {
        this.id = id;
        this.command = command;
        this.args = args;
    }

    public void SetPacket(string id, string command, string[] args)
    {
        this.id = id;
        this.command = command;
        this.args = args;
    }

    public void SetPacket(byte[] data)
    {
        string[] split = Encoding.ASCII.GetString(data).Split('_');
        id = split[0];
        command = split[1];

        args = new string[split.Length - 2];
        for (int i = 2; i < split.Length; i++)
        {
            args[i - 2] = split[i];
        }
    }

    public void SetPacket(int data)
    {
        string[] split = Encoding.ASCII.GetString(BitConverter.GetBytes(data)).Split('_');
        id = split[0];
        command = split[1];

        args = new string[split.Length - 2];
        for (int i = 2; i < split.Length; i++)
        {
            args[i - 2] = split[i];
        }
    }

    public void SetPacket(string str)
    {
        string[] split = str.Split('_');
        id = split[0];
        command = split[1];

        args = new string[split.Length - 2];
        for (int i = 2; i < split.Length; i++)
        {
            args[i - 2] = split[i];
        }
    }

    public byte[] ToBytes()
    {
        string[] split = new string[args.Length + 2];
        split[0] = id + "_";
        split[1] = command + "_";

        for (int i = 0; i < args.Length; i++)
        {
            split[i + 2] = args[i];
        }

        return Encoding.ASCII.GetBytes(string.Join("_", split));
    }

    public void Clear()
    {
        id = "";
        command = "";
        args = null;
    }

    private string id;
    public string ID => id;

    private string command;
    public string Command => command;

    private string[] args;
    public string[] Args => args;
}