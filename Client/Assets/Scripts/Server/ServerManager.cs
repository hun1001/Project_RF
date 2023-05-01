using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class ServerManager : MonoSingleton<ServerManager>
{
    private TcpClient client = null;
    private NetworkStream stream = null;

    private Dictionary<int, OtherPlayer> otherPlayers = new Dictionary<int, OtherPlayer>();

    public void ConnectToServer()
    {
        client = new TcpClient();
        client.Connect(IPAddress.Parse("172.31.1.200"), 7777);
        stream = client.GetStream();

        string d = "Enter";
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

[SerializeField]
public struct TankInfo
{
    public float HP;

    public float tankPositionX;
    public float tankPositionY;

    public float tankRotationZ;
    public float turretRotationZ;

    public bool Equals(TankInfo other) => this.HP == other.HP && this.tankPositionX == other.tankPositionX && this.tankPositionY == other.tankPositionY && this.tankRotationZ == other.tankRotationZ && this.turretRotationZ == other.turretRotationZ;
}
