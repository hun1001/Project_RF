using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class ServerManager : MonoSingleton<ServerManager>
{
    private TcpClient client = null;
    private NetworkStream stream = null;

    public void ConnectToServer()
    {
        client = new TcpClient();
        IPAddress ip = IPAddress.Parse("127.0.0.1");
        client.Connect(ip, 7777);
        stream = client.GetStream();

        Debug.Log("Connected to server");
        string message = "Hello from client";
        byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
        stream.Write(data, 0, data.Length);

    }
}