using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
using System.Text;

public class ServerManager : MonoSingleton<ServerManager>
{
    public const string COMMAND_ENTER = "#Enter#";
    public const string COMMAND_MOVE = "#Move#";
    public const char CHAR_TERMINATOR = ';';

    private TcpClient client = null;
    private NetworkStream stream = null;

    private string id = "Player";
    private bool isRunning = false;

    public void ConnectToServer()
    {
        id = "Player" + Random.Range(0, 1000);
        client = new TcpClient();
        IPAddress ip = IPAddress.Parse("127.0.0.1");
        client.Connect(ip, 7777);
        stream = client.GetStream();

        byte[] outStream = Encoding.UTF8.GetBytes(id + '$');
        stream.Write(outStream, 0, outStream.Length);
        stream.Flush();

        isRunning = true;
        StartCoroutine(GetMessage());
    }

    public void SendToServer(string message)
    {
        byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
        stream.Write(data, 0, data.Length);
    }

    private IEnumerator GetMessage()
    {
        byte[] inStream = new byte[1024];
        string returnData = "";

        while (true)
        {
            stream = client.GetStream();
            int buffSize = client.ReceiveBufferSize;
            int numBytesRead;

            if (stream.DataAvailable)
            {
                numBytesRead = stream.Read(inStream, 0, inStream.Length);
                returnData += Encoding.UTF8.GetString(inStream, 0, numBytesRead);
            }
            //GameManager.Instance.QueueCommand(returnData);
            Debug.Log(returnData);
            returnData = "";
            yield return null;
        }

        Debug.Log("Disconnected from server");
        isRunning = false;
    }
}
