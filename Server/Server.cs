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
            TcpListener serverSocket = new TcpListener(IPAddress.Any, 7777);
            TcpClient clientSocket = default(TcpClient);
            int counter = 0;
            byte[] bytesFrom = new byte[1024];
            string dataFromClient = "";

            serverSocket.Start(); 
            Console.WriteLine("Server Started...");

            while (true)
            {
                counter += 1;
                clientSocket = serverSocket.AcceptTcpClient();
                dataFromClient = "";

                counter = userCnt;
                userCnt++;

                ClientHandle client = new ClientHandle();
                clientsDictionary.Add(counter, client);

                client.startClient(clientSocket, counter);
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
        if (clientsDictionary.ContainsKey(id))
        {
            ClientHandle hc = clientsDictionary[id];
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
            foreach (var Item in clientsDictionary.Values)
            {
                TcpClient broadcastSocket;
                ClientHandle hc = Item;
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
        Console.WriteLine(clientNo + "Joined");
    }

    public static void UserLeft(int userID, string clientNo)
    {
        if (clientsDictionary.ContainsKey(userID))
        {
            broadcast(clientNo + "$#Left#", clientNo, false);
            Console.WriteLine("Client Left: " + clientNo);

            TcpClient clientSocket = GetSocket(userID);

            clientsDictionary.Remove(userID);
            clientSocket.Close();
        }
    }
}
