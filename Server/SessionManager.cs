using System.Net.Sockets;
using System.Text;

public class SessionManager : Singleton<SessionManager>
{
    private Dictionary<int, Session> _sessions = new Dictionary<int, Session>();

    private int _sessionId = 0;

    public void GenerateSession(TcpClient tcpClient)
    {
        Session session = new Session(tcpClient, ++_sessionId);
        _sessions.Add(_sessionId, session);
    }

    public void Remove(int sessionId)
    {
        _sessions.Remove(sessionId);
    }

    public Session Find(int sessionId)
    {
        return _sessions[sessionId];
    }

    public int Count => _sessions.Count;

    public Session[] GetSessions() => _sessions.Values.ToArray();

    public void Broadcast(string message)
    {
        foreach (Session session in _sessions.Values)
        {
            session.Send(message);
        }
    }
}

