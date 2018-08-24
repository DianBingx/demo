using System.Net.Sockets;
using System.Threading;
using System.Net;

public class SocketBase
{
    protected bool m_HasInit = false;
    protected Socket m_Socket;
    protected Thread m_LinstenThread;
    protected IPEndPoint m_IP;
    protected Mutex m_Mutex;
}
