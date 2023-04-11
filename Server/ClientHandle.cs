using System.Net.Sockets;
using System.Text;

namespace Server;

public class ClientHandle
{
    
    const char CHAR_TERMINATOR = ';';

    public TcpClient clientSocket;
    public int userID;
    public string clientID;

    private bool noConnection = false;

    public void StartClient(TcpClient inClientSocket, int userSerial)
    {
        clientSocket = inClientSocket;
        userID = userSerial;

        Thread ctThread = new Thread(Recv);
        ctThread.Start();
    }

    bool SocketConnected(Socket e)
    {
        bool part1 = e.Poll(1000, SelectMode.SelectRead);
        bool part2 = (e.Available == 0);
        if (part1 && part2)
        {
            return false;
        }
        else return true;
    }

    private void Recv()
    {
        byte[] bytesFrom = new byte[1024];
        NetworkStream networkStream = clientSocket.GetStream();
        Packet packet = new Packet();

        while (!noConnection)
        {
            try
            {
                int numBytesRead;
                if (!SocketConnected(clientSocket.Client))
                {
                    noConnection = true;
                }
                else
                {
                    if (networkStream.DataAvailable)
                    {
                        while (networkStream.DataAvailable)
                        {
                            numBytesRead = networkStream.Read(bytesFrom, 0, bytesFrom.Length);
                            packet = Packet.Deserialize(bytesFrom);
                        }

                        Console.WriteLine($"ID: {packet.ID} | Cmd: {packet.Command} | Data: {packet.Data}");

                        switch (packet.Command)
                        {
                            case Command.COMMAND_REGISTER:
                                clientID = packet.ID;
                                Server.UserAdd(clientID);
                                break;
                            case Command.COMMAND_LEFT:
                                Server.UserLeft(userID, clientID);
                                break;
                            case Command.COMMAND_MOVE:
                            case Command.COMMAND_ATTACK:
                            case Command.COMMAND_DAMAGED:
                                Server.Broadcast(packet);
                                break;
                            default:
                                break;
                        }

                        packet.Clear();
                    }
                }
            }
            catch (Exception e)
            {
                noConnection = true;
                Console.WriteLine(e.ToString());
            }
        }
        Server.UserLeft(userID, clientID);
    }
}
