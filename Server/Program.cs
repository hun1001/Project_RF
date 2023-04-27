using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server;

internal static class Program
{
    private static void Main(string[] args)
    {
        TcpListener tcpListener = new(IPAddress.Parse("127.0.0.1"), 7777);

        tcpListener.Start();

        while (true)
        {
            TcpClient tcpClient = tcpListener.AcceptTcpClient();

            NetworkStream networkStream = tcpClient.GetStream();

            byte[] buffer = new byte[1024];

            int bytes = networkStream.Read(buffer, 0, buffer.Length);

            string message = Encoding.UTF8.GetString(buffer, 0, bytes);

            Console.WriteLine(message);

            networkStream.Close();
            tcpClient.Close();
        }
    }
}