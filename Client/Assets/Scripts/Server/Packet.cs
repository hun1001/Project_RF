using System;
using System.IO;
using System.Text;

[Serializable]
public class Packet
{
    private string id;
    public string ID => id;

    private string command;
    public string Command => command;

    private string data;
    public string Data => data;

    public Packet(string id, string command, string data)
    {
        this.id = id;
        this.command = command;
        this.data = data;
    }

    public Packet()
    {
        id = string.Empty;
        command = string.Empty;
        data = string.Empty;
    }

    // 직렬화 메서드
    public byte[] Serialize()
    {
        string str = id + ";" + command + ";" + data + ";";
        return Encoding.UTF8.GetBytes(str);
    }

    // 역직렬화 메서드
    public static Packet Deserialize(byte[] data)
    {
        string str = Encoding.UTF8.GetString(data);
        string[] strArray = str.Split(';');
        return new Packet(strArray[0], strArray[1], strArray[2]);
    }

    public void Clear()
    {
        id = string.Empty;
        command = string.Empty;
        data = string.Empty;
    }
}