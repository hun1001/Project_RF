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

    private Dictionary<string, OtherPlayer> otherPlayers = new Dictionary<string, OtherPlayer>();
    private Queue<Packet> packetQueue = new Queue<Packet>();
    private Queue<Packet> sendQueue = new Queue<Packet>();

    public void ConnectToServer()
    {
        _id = "Player" + Random.Range(0, 1000);
        client = new TcpClient();
        IPAddress ip = IPAddress.Parse("172.31.1.200");
        client.Connect(ip, 7777);
        stream = client.GetStream();

        Packet packet = new Packet(_id, "Register", "");
        RegisterSendPacket(packet);

        isPlayingGame = true;
        StartCoroutine(GetMessage());
        StartCoroutine(ProcessQueue());
        StartCoroutine(SendToServer());
    }

    public void RegisterSendPacket(Packet packet)
    {
        sendQueue.Enqueue(packet);
    }

    private IEnumerator SendToServer()
    {
        while (true)
        {
            if (sendQueue.Count > 0)
            {
                Packet packet = sendQueue.Dequeue();
                byte[] outStream = packet.Serialize();
                stream.Write(outStream, 0, outStream.Length);
                stream.Flush();
            }
            yield return null;
        }
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

                    if (PacketUtil.IsPacketRight(packet))
                    {
                        packetQueue.Enqueue(packet);
                    }
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

        Debug.Log($"id: {id} | command: {command} | data: {data}");

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
                UpdateOtherTankHP(id, float.Parse(data));
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

        var ot = FindObjectsOfType<OtherPlayer>();
        for (int i = 0; i < ot.Length; i++)
        {
            for (int j = 0; j < idList.Length; j++)
            {
                if (ot[i].ID == idList[j])
                {
                    break;
                }
                if (j == idList.Length - 1)
                {
                    ot[i].ReturnToPool();
                }
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

    private void LeftOtherPlayer(string id)
    {
        if (otherPlayers.ContainsKey(id))
        {
            otherPlayers[id].ReturnToPool();
            otherPlayers.Remove(id);
        }
    }

    public void SendHP(float hp)
    {
        RegisterSendPacket(new Packet(_id, "Damage", hp.ToString()));
    }

    private void UpdateOtherTankHP(string id, float hp)
    {
        if (otherPlayers.ContainsKey(id))
        {
            otherPlayers[id].UpdateHP(hp);
        }
        else
        {
            Debug.Log("OtherPlayer not found");
        }
    }

    public void AttackPlayer()
    {
        RegisterSendPacket(new Packet(_id, "Attack", null));
    }

    private IEnumerator AddOtherPlayer(string id)
    {
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "GameScene");
        if (otherPlayers.ContainsKey(id) == false && id != "")
        {
            var otherPlayer = PoolManager.Get<OtherPlayer>("Assets/Prefabs/OtherPlayer.prefab");
            otherPlayer.ID = id;
            otherPlayers.Add(id, otherPlayer);
        }
    }

    public void SendTransform(Transform tankTransform, Transform turretTransform)
    {
        string message = tankTransform.position.x + "," + tankTransform.position.y + "," + tankTransform.rotation.z + "," + tankTransform.rotation.w + "," + turretTransform.rotation.z + "," + turretTransform.rotation.w;
        Packet packet = new Packet(_id, "Move", message);
        RegisterSendPacket(packet);
    }

    public void Disconnect()
    {
        if (client != null)
        {
            RegisterSendPacket(new Packet(_id, "Left", null));
            isPlayingGame = false;
            stream.Close();
            client.Close();
            client = null;
        }
    }
}
