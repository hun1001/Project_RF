using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class NetworkManager : MonoSingleton<NetworkManager>
{
    private TcpClient client = null;
    private NetworkStream stream = null;

    private int _id = 0;

    private Dictionary<int, OtherPlayer> otherPlayers = new Dictionary<int, OtherPlayer>();

    public void ConnectToServer()
    {
        _id = Random.Range(1, 10000);
        client = new TcpClient();
        client.Connect(IPAddress.Parse("172.31.1.200"), 7777);
        stream = client.GetStream();

        string d = "Enter|" + _id;
        byte[] outStream = Encoding.UTF8.GetBytes(d);

        stream.Write(outStream, 0, outStream.Length);
        stream.Flush();

        StartCoroutine(GetMessage());
    }

    private IEnumerator GetMessage()
    {
        byte[] inStream = new byte[1024];
        int numBytesRead;

        while (client.Connected)
        {
            if (stream.DataAvailable)
            {
                numBytesRead = stream.Read(inStream, 0, inStream.Length);
                string returnData = Encoding.UTF8.GetString(inStream, 0, numBytesRead);
                Debug.Log(returnData);
            }
            yield return null;
        }
    }
}
