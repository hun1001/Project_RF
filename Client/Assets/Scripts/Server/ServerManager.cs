using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
using System.Text;
using Pool;

public class ServerManager : MonoSingleton<ServerManager>
{
    public const string COMMAND_ENTER = "#Enter#";
    public const string COMMAND_MOVE = "#Move#";
    public const char CHAR_TERMINATOR = ';';

    private TcpClient client = null;
    private NetworkStream stream = null;

    private string _id = "Player";
    public string ID => _id;
    private bool isPlayingGame = false;

    private Queue<string> commandQueue = new Queue<string>();
    private Dictionary<string, OtherPlayer> otherPlayers = new Dictionary<string, OtherPlayer>();

    public void ConnectToServer()
    {
        _id = "Player" + Random.Range(0, 1000);
        client = new TcpClient();
        IPAddress ip = IPAddress.Parse("172.31.1.200");
        client.Connect(ip, 7777);
        stream = client.GetStream();

        byte[] outStream = Encoding.UTF8.GetBytes(_id + '$');
        stream.Write(outStream, 0, outStream.Length);
        stream.Flush();

        isPlayingGame = true;
        StartCoroutine(GetMessage());
        StartCoroutine(ProcessQueue());
    }

    public void SendToServer(string message)
    {
        message = _id + "$" + message;
        byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
        stream.Write(data, 0, data.Length);
    }

    private IEnumerator GetMessage()
    {
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
            }
            commandQueue.Enqueue(returnData);
            returnData = "";
            yield return null;
        }
    }

    private IEnumerator ProcessQueue()
    {
        while (true)
        {
            if (commandQueue.Count > 0)
            {
                string command = commandQueue.Dequeue();
                ProcessCommand(command);
            }
            yield return null;
        }
    }

    private void ProcessCommand(string cmd)
    {
        if (isPlayingGame == false)
        {
            return;
        }

        int nameIdx = cmd.IndexOf("$");
        string id = "";
        if (nameIdx > 0)
        {
            id = cmd.Substring(0, nameIdx);
        }

        int cmdIdx1 = cmd.IndexOf("#");
        if (cmdIdx1 > nameIdx)
        {
            int cmdIdx2 = cmd.IndexOf("#", cmdIdx1 + 1);
            if (cmdIdx2 > cmdIdx1)
            {
                string command = cmd.Substring(cmdIdx1 + 1, cmdIdx2 - cmdIdx1 - 1);

                string remain = "";
                string nextCommand;
                int endIdx = cmd.IndexOf(CHAR_TERMINATOR, cmdIdx2 + 1);
                if (endIdx > cmdIdx2)
                {
                    remain = cmd.Substring(cmdIdx2 + 1, endIdx - cmdIdx2 - 1);
                    nextCommand = cmd.Substring(endIdx + 1);
                }
                else
                {
                    nextCommand = cmd.Substring(cmdIdx2 + 1);
                }

                Debug.Log($"command = {command}, id = {id}, remain = {remain}, nextCommand = {nextCommand}");

                if (id.CompareTo(id) != 0)
                {
                    switch (command)
                    {
                        case "Enter":
                            Debug.Log($"Enter {id}");
                            AddOtherPlayer(id);
                            break;
                        case "Move":
                            Debug.Log($"Move {id}");
                            string[] t = remain.Split(',');
                            Vector3 position = new Vector3(float.Parse(t[0]), float.Parse(t[1]), float.Parse(t[2]));
                            Quaternion rotation = new Quaternion(float.Parse(t[3]), float.Parse(t[4]), float.Parse(t[5]), float.Parse(t[6]));
                            UpdateOtherPlayerTransform(id, position, rotation);
                            break;
                        case "Left":
                            Debug.Log($"Left {id}");
                            LeftOtherPlayer(id);
                            break;
                        case "Attack":
                            Debug.Log($"Attack {id}");
                            break;
                        case "Damage":
                            Debug.Log($"Damage {id}");
                            break;
                        case "Dead":
                            Debug.Log($"Dead {id}");
                            break;
                    }
                }
                else
                {
                    Debug.Log("Skip");
                }
            }
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

    private void AddOtherPlayer(string _id)
    {
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
            otherPlayers[_id].transform.position = position;
            otherPlayers[_id].transform.rotation = rotation;
        }
    }

    public void SendTransform(Transform transform)
    {
        string message = COMMAND_MOVE + transform.position.x + "," + transform.position.y + "," + transform.position.z + "," + transform.rotation.x + "," + transform.rotation.y + "," + transform.rotation.z + "," + transform.rotation.w;
        SendToServer(message);
    }

    public void Disconnect()
    {
        if (client != null)
        {
            SendToServer("Left");
            isPlayingGame = false;
            stream.Close();
            client.Close();
            client = null;
        }
    }
}
