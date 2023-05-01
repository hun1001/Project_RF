using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

public class Session
{
    private int _sessionId = 0;

    private TcpClient? _tcpClient = null;
    private NetworkStream? _networkStream = null;

    private TankInfo _tankInfo;

    public Session(TcpClient tcpClient, int sessionId)
    {
        _sessionId = sessionId;
        _tcpClient = tcpClient;
        _networkStream = tcpClient.GetStream();
        _tankInfo = new TankInfo();
        Task.Run(Receive);
    }

    private async Task Receive()
    {
        while (_tcpClient!.Connected)
        {
            byte[] buffer = new byte[1024];
            int length = await _networkStream!.ReadAsync(buffer, 0, buffer.Length);
            string message = Encoding.UTF8.GetString(buffer, 0, length);
            
            Console.WriteLine(message);

            TankInfo tankInfo = new TankInfo();
            tankInfo = JsonSerializer.Deserialize<TankInfo>(message);

            if(_tankInfo.Equals(tankInfo))
            {

            }
            else
            {
                _tankInfo = tankInfo;
                SessionManager.Instance.Broadcast(message);
            }
        }
    }

    public void Send(string message)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(message);
        _networkStream!.Write(buffer, 0, buffer.Length);
        _networkStream!.Flush();
    }
}

[Serializable]
public struct TankInfo 
{
    public float HP;

    public float tankPositionX;
    public float tankPositionY;

    public float tankRotationZ;

    public float turretRotationZ;

    public bool Equals(TankInfo other) => this.HP == other.HP && this.tankPositionX == other.tankPositionX && this.tankPositionY == other.tankPositionY && this.tankRotationZ == other.tankRotationZ && this.turretRotationZ == other.turretRotationZ;
}
