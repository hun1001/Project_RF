using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server;
static class Program
{
    public static Dictionary<int, ClientHandle> clientsDictionary = new();
    private static int userCnt = 0;
    private static object lockSocket = new();

    private static TcpListener serverSocket = null;

    static void Main()
    {
        try
        {
            serverSocket = new TcpListener(IPAddress.Any, 7777);
            
            TcpClient clientSocket;
            byte[] bytesFrom = new byte[1024];
            string dataFromClient = "";

            serverSocket.Start(); 
            Console.WriteLine("Server Started...");

            while (true)
            {
                clientSocket = serverSocket.AcceptTcpClient();
                dataFromClient = "";

                ClientHandle client = new ClientHandle();
                clientsDictionary.Add(userCnt, client);

                client.StartClient(clientSocket, userCnt);

                ++userCnt;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    public static TcpClient GetSocket(int id)
    {
        TcpClient socket = null;
        if (clientsDictionary.ContainsKey(id))
        {
            ClientHandle hc = clientsDictionary[id];
            socket = hc.clientSocket;
        }
        return socket;
    }

    public static void Broadcast(Packet packet)
    {
        byte[] data = packet.Serialize();
        
        lock (lockSocket)
        {
            foreach (KeyValuePair<int, ClientHandle> client in clientsDictionary)
            {
                TcpClient socket = client.Value.clientSocket;
                NetworkStream networkStream = socket.GetStream();

                try
                {
                    networkStream.Write(data, 0, data.Length);
                    networkStream.Flush();
                }
                catch(Exception e)
                {
                    socket.Close();
                    networkStream.Close();
                    Console.WriteLine(e.ToString());
                }
            }
        }
    }

    public static void UserAdd(string clientNo)
    {
        string memberIDList = "";

        lock (lockSocket)
        {
            foreach (KeyValuePair<int, ClientHandle> client in clientsDictionary)
            {
                memberIDList += client.Value.clientID + ",";
            }
        }

        Broadcast(new Packet("", Command.COMMAND_ENTER, memberIDList));
    }

    public static void UserLeft(int userID, string clientNo)
    {
        if (clientsDictionary.ContainsKey(userID))
        {
            Broadcast(new Packet("id", Command.COMMAND_LEFT, ""));
            Console.WriteLine("Client Left: " + clientNo);

            TcpClient clientSocket = GetSocket(userID);

            clientsDictionary.Remove(userID);
            clientSocket.Close();
        }
    }
}
