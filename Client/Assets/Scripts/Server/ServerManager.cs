using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
using System.Text;
using Pool;
using UnityEngine.SceneManagement;

public class ServerManager : MonoSingleton<ServerManager>
{
    private TcpClient client = null;
    private NetworkStream stream = null;

    private string _id = "Player";
    public string ID => _id;
    private bool isPlayingGame = false;

    private Queue<Packet> packetQueue = new Queue<Packet>();
    private Dictionary<string, OtherPlayer> otherPlayers = new Dictionary<string, OtherPlayer>();

    public void ConnectToServer()
    {
        _id = "Player" + Random.Range(0, 1000);
        client = new TcpClient();
        IPAddress ip = IPAddress.Parse("192.168.0.21");
        client.Connect(ip, 7777);
        stream = client.GetStream();

        Packet packet = new Packet(_id, "Enter", null);
        SendToServer(packet);

        isPlayingGame = true;
        StartCoroutine(GetMessage());
        StartCoroutine(ProcessQueue());
    }

    public void SendToServer(Packet packet)
    {
        byte[] data = packet.ToBytes();
        stream.Write(data, 0, data.Length);
    }

    private IEnumerator GetMessage()
    {
        Packet packet = new Packet();
        byte[] inStream = new byte[1024];
        string returnData = "";

        while (isPlayingGame)
        {
            stream = client.GetStream();
            int buffSize = client.ReceiveBufferSize;
            int numBytesRead;

            if (stream.DataAvailable)
            {
                numBytesRead = stream.Read(inStream, 0, inStream.Length);
                returnData += Encoding.UTF8.GetString(inStream, 0, numBytesRead);
                packet.SetPacket(returnData);
                packetQueue.Enqueue(packet);
            }

            inStream = new byte[1024];
            returnData = "";
            yield return null;
        }
    }

    private IEnumerator ProcessQueue()
    {
        while (true)
        {
            if (packetQueue.Count > 0)
            {
                Packet command = packetQueue.Dequeue();
                ProcessCommand(command);
            }
            yield return null;
        }
    }

    private void ProcessCommand(Packet p)
    {
        if (isPlayingGame == false)
        {
            return;
        }

        string id = p.ID;
        string command = p.Command;
        string[] args = p.Args;

        if (_id.CompareTo(id) == 0)
        {
            return;
        }

        switch (command)
        {
            case "Move":
                break;
            case "Enter":
                break;
            case "Left":
                break;
            case "Attack":
                break;
            case "Damage":
                break;
            default:
                break;
        }

        // int nameIdx = cmd.IndexOf("$");
        // string id = "";
        // if (nameIdx > 0)
        // {
        //     id = cmd.Substring(0, nameIdx);
        // }

        // int cmdIdx1 = cmd.IndexOf("#");
        // if (cmdIdx1 > nameIdx)
        // {
        //     int cmdIdx2 = cmd.IndexOf("#", cmdIdx1 + 1);
        //     if (cmdIdx2 > cmdIdx1)
        //     {
        //         string command = cmd.Substring(cmdIdx1 + 1, cmdIdx2 - cmdIdx1 - 1);

        //         string remain = "";
        //         string nextCommand;
        //         int endIdx = cmd.IndexOf(CHAR_TERMINATOR, cmdIdx2 + 1);
        //         if (endIdx > cmdIdx2)
        //         {
        //             remain = cmd.Substring(cmdIdx2 + 1, endIdx - cmdIdx2 - 1);
        //             nextCommand = cmd.Substring(endIdx + 1);
        //         }
        //         else
        //         {
        //             nextCommand = cmd.Substring(cmdIdx2 + 1);
        //         }

        //         Debug.Log($"command = {command}, id = {id}, remain = {remain}, nextCommand = {nextCommand}");

        //         if (_id.CompareTo(id) != 0)
        //         {
        //             switch (command)
        //             {
        //                 case "Enter":
        //                     Debug.Log($"Enter {id}");
        //                     StartCoroutine(AddOtherPlayer(id));
        //                     break;
        //                 case "Move":
        //                     string i = "";
        //                     int index = id.IndexOf('r');
        //                     if (index > 0)
        //                     {
        //                         i = id.Substring(index);
        //                     }
        //                     i = "Player" + id;
        //                     if (otherPlayers.ContainsKey(i) == false)
        //                     {
        //                         StartCoroutine(AddOtherPlayer(i));
        //                     }

        //                     string[] t = remain.Split(',');

        //                     Vector3 position = new Vector3(float.Parse(t[0]), float.Parse(t[1]), float.Parse(t[2]));
        //                     Quaternion rotation = new Quaternion(float.Parse(t[3]), float.Parse(t[4]), float.Parse(t[5]), float.Parse(t[6]));
        //                     UpdateOtherPlayerTransform(id, position, rotation);
        //                     break;
        //                 case "Left":
        //                     Debug.Log($"Left {id}");
        //                     LeftOtherPlayer(id);
        //                     break;
        //                 case "Attack":
        //                     Debug.Log($"Attack {id}");
        //                     break;
        //                 case "Damage":
        //                     Debug.Log($"Damage {id}");
        //                     break;
        //                 case "Dead":
        //                     Debug.Log($"Dead {id}");
        //                     break;
        //             }
        //         }
        //     }
        // }
    }

    private void LeftOtherPlayer(string id)
    {
        if (otherPlayers.ContainsKey(id))
        {
            otherPlayers[id].ReturnToPool();
            otherPlayers.Remove(id);
        }
    }

    private IEnumerator AddOtherPlayer(string _id)
    {
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "GameScene");
        if (otherPlayers.ContainsKey(_id) == false)
        {
            var otherPlayer = PoolManager.Get<OtherPlayer>("Assets/Prefabs/OtherPlayer.prefab");
            otherPlayers.Add(_id, otherPlayer);
        }
    }

    public void UpdateOtherPlayerTransform(string _id, Vector3 position, Quaternion rotation)
    {
        if (otherPlayers.ContainsKey(_id))
        {
            otherPlayers[_id].Tank.transform.position = position;
            otherPlayers[_id].Tank.transform.rotation = rotation;
        }
    }

    public void SendTransform(Transform transform)
    {
        string message = transform.position.x + "," + transform.position.y + "," + transform.position.z + "," + transform.rotation.x + "," + transform.rotation.y + "," + transform.rotation.z + "," + transform.rotation.w;
        Packet packet = new Packet(_id, "Move", message);
        SendToServer(packet);
    }

    public void Disconnect()
    {
        if (client != null)
        {
            SendToServer(new Packet(_id, "Left", null));
            isPlayingGame = false;
            stream.Close();
            client.Close();
            client = null;
        }
    }
}
