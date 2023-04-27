using System.Net;
using System.Net.Sockets;

namespace Server
{
    public class Listener
    {
        private TcpListener? tcpListener = null;
        
        public void Init(IPEndPoint iPEndPoint)
        {
            tcpListener = new(iPEndPoint);
            tcpListener.Start();

            Console.WriteLine($"Server Online: {tcpListener.LocalEndpoint}");

            
        }

        private void AcceptClient()
        {
            while (true)
            {
                TcpClient tcpClient = tcpListener!.AcceptTcpClient();
                NetworkStream networkStream = tcpClient.GetStream();
                byte[] buffer = new byte[1024];
            }
        }
    }
}
