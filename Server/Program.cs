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

        while (true) { }
    }
}
