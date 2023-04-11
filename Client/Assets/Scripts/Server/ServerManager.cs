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
    public bool IsPlayingGame => isPlayingGame;

    private Queue<Packet> packetQueue = new Queue<Packet>();
    private Dictionary<string, OtherPlayer> otherPlayers = new Dictionary<string, OtherPlayer>();

    public void ConnectToServer()
    {
        _id = "Player" + Random.Range(0, 1000);
        client = new TcpClient();
        IPAddress ip = IPAddress.Parse("172.31.1.200");
        client.Connect(ip, 7777);
        stream = client.GetStream();

        Packet packet = new Packet(_id, "Register", "");
        SendToServer(packet);

        isPlayingGame = true;
        StartCoroutine(GetMessage());
        StartCoroutine(ProcessQueue());
    }

    public void SendToServer(Packet packet)
    {
        byte[] data = packet.Serialize();
        stream.Write(data, 0, data.Length);
    }

    private IEnumerator GetMessage()
    {
        byte[] inStream = new byte[1024];
        Packet packet;
        int numBytesRead;

        stream = client.GetStream();

        while (isPlayingGame)
        {

            if (stream.DataAvailable)
            {
                while (stream.DataAvailable)
                {
                    numBytesRead = stream.Read(inStream, 0, inStream.Length);
                    packet = Packet.Deserialize(inStream);
                    packetQueue.Enqueue(packet);
                }
            }

            inStream = new byte[1024];
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
        string data = p.Data;

        if (_id.CompareTo(id) == 0 || (otherPlayers.ContainsKey(id) == false && id.CompareTo(id) != 0))
        {
            return;
        }

        Debug.Log($"id: {id}, command: {command}, data: {data}");

        switch (command)
        {
            case "Move":
                MoveOtherPlayer(id, data);
                break;
            case "Enter":
                UpdateMemberList(data);
                break;
            case "Left":
                LeftOtherPlayer(id);
                break;
            case "Attack":
                FireOtherPlayer(id);
                break;
            case "Damage":
                DamageOtherPlayer(id);
                break;
            default:
                Debug.Log($"Unknown command: {command}");
                break;
        }
    }

    private void UpdateMemberList(string id)
    {
        string[] idList = id.Split(',');

        for (int i = 0; i < idList.Length; i++)
        {
            if (idList[i] == _id)
            {
                continue;
            }

            if (otherPlayers.ContainsKey(idList[i]) == false)
            {
                StartCoroutine(AddOtherPlayer(idList[i]));
            }
        }
    }

    private void MoveOtherPlayer(string id, string data)
    {
        if (otherPlayers.ContainsKey(id))
        {
            otherPlayers[id].TransformUpdate(data);
        }
        else
        {
            UpdateMemberList(id);
        }
    }

    private void FireOtherPlayer(string id)
    {
        if (otherPlayers.ContainsKey(id))
        {
            otherPlayers[id].Fire();
        }
        else
        {
            Debug.Log("OtherPlayer not found");
        }
    }

    private void DamageOtherPlayer(string id)
    {
        if (otherPlayers.ContainsKey(id))
        {
            Debug.Log($"{id} is damaged");
        }
        else
        {
            Debug.Log("OtherPlayer not found");
        }
    }

    private void LeftOtherPlayer(string id)
    {
        if (otherPlayers.ContainsKey(id))
        {
            otherPlayers[id].ReturnToPool();
            otherPlayers.Remove(id);
        }
    }

    private IEnumerator AddOtherPlayer(string id)
    {
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "GameScene");
        if (otherPlayers.ContainsKey(id) == false)
        {
            var otherPlayer = PoolManager.Get<OtherPlayer>("Assets/Prefabs/OtherPlayer.prefab");
            otherPlayer.ID = id;
            otherPlayers.Add(id, otherPlayer);
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
