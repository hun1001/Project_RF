using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server;

internal static class Program
{
    private static void Main(string[] args)
    {
        Listener tcpListener = new Listener();
        tcpListener.Init(new IPEndPoint(IPAddress.Parse("172.31.1.200"), 7777));
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

public class Session1
{
    public TcpClient TcpClient { get; set; }
    public NetworkStream NetworkStream { get; set; }
    public byte[] Buffer { get; set; }
    public string Message { get; set; }

    public Session1(TcpClient tcpClient, NetworkStream networkStream, byte[] buffer, string message = "")
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