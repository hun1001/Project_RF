using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

[Serializable]
public class Packet
{
    public Packet(string id, string command, params string[] args)
    {
        this.id = id;
        this.command = command;
        this.args = args;
    }

    public Packet(byte[] data)
    {
        string[] split = Encoding.ASCII.GetString(data).Split('_');
        id = split[0];
        command = split[1];
        args = split[2].Split(';');
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

    public string id;
    public string command;
    public string[] args;
}
