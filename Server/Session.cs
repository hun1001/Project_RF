using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
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
                //Program.Broadcast(message);
            }

        }
    }

    public struct TankInfo 
    {
        public float HP;

        public float tankPositionX;
        public float tankPositionY;
        public float tankPositionZ;

        public float tankRotationX;
        public float tankRotationY;
        public float tankRotationZ;

        public float turretRotationX;
        public float turretRotationY;
        public float turretRotationZ;
    }
}
