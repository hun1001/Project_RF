using System.Net;
using System.Net.Sockets;
using System.Text;

public class Listener
{
    private TcpListener? tcpListener = null;

    public void Init(IPEndPoint iPEndPoint)
    {
        tcpListener = new(iPEndPoint);
        tcpListener.Start();

        Console.WriteLine($"Server Online: {tcpListener.LocalEndpoint}");

        Task.Run(AcceptClient);
    }

    private async Task AcceptClient()
    {
        while (true)
        {
            TcpClient tcpClient = await tcpListener!.AcceptTcpClientAsync();
            
            SessionManager.Instance.GenerateSession(tcpClient);
        }
    }
}
