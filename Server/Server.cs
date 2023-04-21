using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ProtoBuf;

namespace Server;
static class Server
{
    public static Dictionary<int, ClientHandle> clientsDictionary = new();
    private static int userCnt = 0;
    private static object lockSocket = new();

    [ProtoContract]
    public class Account
    {
        [ProtoMember(1)]
        public string UserID { get; set; }
        
        [ProtoMember(2)]
        public int Age { get; set; }

        [ProtoMember(3)]
        public int Num { get; set; }
    }

    static void Main()
    {
        Account account = new Account
        {
            UserID = "Martin",
            Age = 1337,
            Num = 1
        };



        //  serialize
        MemoryStream serialize = new MemoryStream();
        ProtoBuf.Serializer.Serialize<Account>(serialize, account);
        byte[] byteData = serialize.ToArray();
        Console.WriteLine($"Serialize : {BitConverter.ToString(byteData)}");
        //Console.WriteLine($"Json : {}")


        //  deserialize
        MemoryStream deserialize = new MemoryStream(byteData);
        Account result = ProtoBuf.Serializer.Deserialize<Account>(deserialize);
        Console.WriteLine($"DeSerialize : {result.UserID}, {result.Age}, {result.Num}");
        
        return;
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
                ++counter;
                clientSocket = serverSocket.AcceptTcpClient();
                dataFromClient = "";

                counter = userCnt;
                userCnt++;

                ClientHandle client = new ClientHandle();
                clientsDictionary.Add(counter, client);

                client.StartClient(clientSocket, counter);
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
