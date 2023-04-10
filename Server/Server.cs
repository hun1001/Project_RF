using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;

namespace Server;
class Server
{
    const string STRING_TERMINATOR = ";";
    public static Dictionary<int, ClientHandle> clientsDictionary = new();
    private static int userCnt = 0;
    private static object lockSocket = new();

    static void Main()
    {
        try
        {
            TcpListener serverSocket = new TcpListener(IPAddress.Any, 8888);
            TcpClient clientSocket = default(TcpClient);
            int counter = 0;
            byte[] bytesFrom = new byte[1024];
            string dataFromClient = "";

            serverSocket.Start(); //listen
            Console.WriteLine("C# Server Started...");

            while (true)
            {
                counter += 1;
                clientSocket = serverSocket.AcceptTcpClient();
                dataFromClient = "";

                counter = userCnt;
                userCnt++;

                ClientHandle client = new ClientHandle();
                clientsDictionary.Add(counter, client);

                client.startClient(clientSocket, clientsList, counter);
            }
            clientSocket.Close();
            serverSocket.Stop();
            Console.WriteLine("Exit");
            Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    public static TcpClient GetSocket(int id)
    {
        TcpClient socket = null;
        if (clientsList.ContainsKey(id))
        {
            ClientHandle hc = (ClientHandle)clientsList[id];
            socket = hc.clientSocket;
        }
        return socket;
    }
    
    public static void broadcast(string msg, string uName, bool flag)
    {
        Byte[] broadcastBytes = null;

        if (flag == true) //클라이언트
        {
            broadcastBytes = Encoding.UTF8.GetBytes(uName + "$" + msg + STRING_TERMINATOR);
        }
        else //서버
        {
            broadcastBytes = Encoding.UTF8.GetBytes(msg + STRING_TERMINATOR);
        }

        lock (lockSocket)
        {
            foreach (DictionaryEntry Item in clientsList)
            {
                TcpClient broadcastSocket;
                ClientHandle hc = (ClientHandle)Item.Value;
                broadcastSocket = hc.clientSocket;

                NetworkStream broadcastStream = broadcastSocket.GetStream();

                try
                {

                    broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);
                    broadcastStream.Flush();
                }
                catch (Exception ex)
                {
                    broadcastStream.Close();
                    broadcastSocket.Close();
                }

            }
        }
    }

    public static void UserAdd(string clientNo)
    {
        broadcast(clientNo + "$#Enter#", "", false);
        Console.WriteLine(clientNo + "Joined chat room");
    }

    public static void UserLeft(int userID, string clientNo)
    {
        if (clientsList.ContainsKey(userID))
        {
            broadcast(clientNo + "$#Left#", clientNo, false);
            Console.WriteLine("Client Left: " + clientNo);

            TcpClient clientSocket = GetSocket(userID);

            clientsList.Remove(userID);
            clientSocket.Close();
        }
    }
}

public class ClientHandle
{
    const string COMMAND_ENTER = "#Enter#";
    const string COMMAND_HISTORY = "#HISTORY#";
    const string COMMAND_MOVE = "#Move#";
    const char CHAR_TERMINATOR = ';';

    public TcpClient clientSocket;
    public int userID;
    public string clientID;

    public float posX;
    public float posY;

    private Hashtable clientsList;
    private bool noConnection = false;

    public void startClient(TcpClient inClientSocket,
        Hashtable cList, int userSerial)
    {
        clientSocket = inClientSocket;
        userID = userSerial;
        clientsList = cList;

        Thread ctThread = new Thread(doChat);
        ctThread.Start();
    }

    bool SocketConnected(Socket e)
    {
        bool part1 = e.Poll(1000, SelectMode.SelectRead);
        bool part2 = (e.Available == 0);
        if (part1 && part2)
        {
            return false;
        }
        else return true;
    }

    private void doChat()
    {
        byte[] bytesFrom = new byte[1024];
        string dataFromClient = "";
        NetworkStream networkStream = clientSocket.GetStream();

        while (!noConnection)
        {
            try
            {
                int numBytesRead;
                if (!SocketConnected(clientSocket.Client))
                {
                    noConnection = true;
                }
                else
                {
                    if (networkStream.DataAvailable)
                    {
                        while (networkStream.DataAvailable)
                        {
                            numBytesRead = networkStream.Read(bytesFrom, 0, bytesFrom.Length);
                            dataFromClient = Encoding.UTF8.GetString(bytesFrom, 0, numBytesRead);
                        }
                        int idx = dataFromClient.IndexOf('$');

                        if (clientID == null && idx > 0) //닉네임 전송
                        {
                            clientID = dataFromClient.Substring(0, idx);
                            Server.broadcast(clientID + "$" + COMMAND_ENTER, clientID, false);
                            Console.WriteLine(clientID + "$" + COMMAND_ENTER);
                            Server.UserAdd(clientID);
                        }
                        else if (idx + 1 < dataFromClient.Length)// 채팅 내용
                        {
                            dataFromClient = dataFromClient.Substring(idx + 1, dataFromClient.Length - (idx + 1));
                            Console.WriteLine("From Client - " + clientID + ": " + dataFromClient);
                            ProcessCommand(clientID, dataFromClient);
                            Server.broadcast(dataFromClient, clientID, true);
                        }
                        else
                        {
                            dataFromClient = "";
                        }


                    }
                }
            }
            catch (Exception e)
            {
                noConnection = true;
                Console.WriteLine(e.ToString());
            }
        }
        Server.UserLeft(userID, clientID);
    }
    private string DeleteTerminator(string remain)
    {
        int idx = remain.IndexOf(CHAR_TERMINATOR);
        if (idx >= 0)
        {
            remain = remain.Substring(0, idx);
        }
        return remain;
    }


    private void ProcessMove(string clientID, string remain)
    {
        var strs = remain.Split(',');
        try
        {
            posX = float.Parse(strs[0]);
            posY = float.Parse(strs[1]);
            Console.WriteLine("User Move - " + clientID + " to " + posX + "," + posY);
        }
        catch (Exception e)
        {

        }
    }

    private void ProcessCommand(string clientID, string dataFromClient)
    {
        if (dataFromClient[0] == '#')
        {
            string command;
            string remain;
            int idx = dataFromClient.IndexOf('#', 1);
            if (idx > 1)
            {
                command = dataFromClient.Substring(0, idx + 1);
                if (command == COMMAND_MOVE)
                {
                    remain = DeleteTerminator(dataFromClient.Substring(idx + 1));
                    ProcessMove(clientID, remain);
                }
            }
        }
    }
}
