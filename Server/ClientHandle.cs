using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace Server;

public class ClientHandle
{
    const string COMMAND_ENTER = "#Enter#";
    const string COMMAND_MOVE = "#Move#";
    const string COMMAND_DEAD = "#Dead#";
    const char CHAR_TERMINATOR = ';';

    public TcpClient clientSocket;
    public int userID;
    public string clientID;

    public float posX;
    public float posY;

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
        string dataFromClient = "";
        NetworkStream networkStream = clientSocket.GetStream();

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
                            dataFromClient = Encoding.UTF8.GetString(bytesFrom, 0, numBytesRead);
                        }
                        int idx = dataFromClient.IndexOf('$');

                        if (clientID == null && idx > 0) //닉네임 전송
                        {
                            clientID = dataFromClient.Substring(0, idx);
                            Server.broadcast(clientID + "$" + COMMAND_ENTER, clientID, false);
                            Server.UserAdd(clientID);
                        }
                        else if (idx + 1 < dataFromClient.Length)
                        {
                            dataFromClient = dataFromClient.Substring(idx + 1, dataFromClient.Length - (idx + 1));
                            //Console.WriteLine("From Client - " + clientID + ": " + dataFromClient);
                            ProcessCommand(clientID, dataFromClient);
                            Server.broadcast(dataFromClient, clientID, true);
                        }
                        else
                        {
                            dataFromClient = "";
                        }
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

    private string DeleteTerminator(string remain)
    {
        int idx = remain.IndexOf(CHAR_TERMINATOR);
        if (idx >= 0)
        {
            remain = remain.Substring(0, idx);
        }
        return remain;
    }

    private void ProcessMove(string clientID, string remain)
    {
        var strs = remain.Split(',');
        try
        {
            posX = float.Parse(strs[0]);
            posY = float.Parse(strs[1]);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    private void ProcessCommand(string clientID, string dataFromClient)
    {
        if (dataFromClient[0] == '#')
        {
            string command;
            string remain;
            int idx = dataFromClient.IndexOf('#', 1);
            if (idx > 1)
            {
                command = dataFromClient.Substring(0, idx + 1);
                if (command == COMMAND_MOVE)
                {
                    remain = DeleteTerminator(dataFromClient.Substring(idx + 1));
                    ProcessMove(clientID, remain);
                }
            }
        }
    }
}
