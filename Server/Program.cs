using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server;

internal static class Program
{
    private static TcpListener? tcpListener = null;

    private static List<Session> sessions = new();

    private static void Main(string[] args)
    {
        tcpListener = new(IPAddress.Parse("172.31.1.200"), 7777);

        tcpListener.Start();

        Console.WriteLine($"Server Online: {tcpListener.LocalEndpoint}");

        while (true)
        {
            TcpClient tcpClient = tcpListener.AcceptTcpClient();

            Console.WriteLine("Client connected");

            Session session = new(tcpClient, tcpClient.GetStream(), new byte[1024]);
            sessions.Add(session);
        }
    }

    public static void Broadcast(string d)
    {
        foreach(Session session in sessions)
        {
            session.NetworkStream.Write(Encoding.UTF8.GetBytes(d));
        }
    }
}

public static class BufferQueue
{
    private static Queue<byte[]> _bufferQueue = new Queue<byte[]>();

    public static void Enqueue(byte[] buffer)
    {
        _bufferQueue.Enqueue(buffer);
    }

    public static byte[] Dequeue()
    {
        return _bufferQueue.Dequeue();
    }

    public static int Count => _bufferQueue.Count;
}

public class Session
{
    public TcpClient TcpClient { get; set; }
    public NetworkStream NetworkStream { get; set; }
    public byte[] Buffer { get; set; }
    public string Message { get; set; }

    public Session(TcpClient tcpClient, NetworkStream networkStream, byte[] buffer, string message = "")
    {
        TcpClient = tcpClient;
        NetworkStream = networkStream;
        Buffer = buffer;
        Message = message;

        Thread thread = new Thread(GetMessage);
        thread.Start();
    }

    private void GetMessage()
    {
        while(TcpClient.Connected)
        {
            int bytes = NetworkStream.Read(Buffer, 0, Buffer.Length);
            if (bytes > 0)
            {
                Message = Encoding.UTF8.GetString(Buffer, 0, bytes);
                Console.WriteLine(Message);
                Program.Broadcast(Message);
            }
        }

        Console.WriteLine("Client disconnected");
    }
}