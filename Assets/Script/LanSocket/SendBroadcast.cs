using UnityEngine;
using System.Net.Sockets;
using System.Net;

//SendBroadcast.cs 广播发射器

class SendBroadcast : SocketBase
{
    byte[] m_MyIP;
    public void Start(int port)
    {
        if (m_HasInit)
        {
            return;
        }
        try
        {
            m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            m_IP = new IPEndPoint(IPAddress.Broadcast, port);//255.255.255.255
            //m_IP = new IPEndPoint(IPAddress.Parse("192.168.255.255"), 9050);

            string mLocalIP = "";
            string hostname = Dns.GetHostName();
            IPHostEntry localHost = Dns.GetHostEntry(hostname);
            for (int i = 0; i < localHost.AddressList.Length; ++i)
            {
                if (localHost.AddressList[i].AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    //MonoBehaviour.print(localHost.AddressList[i].ToString());
                    mLocalIP = localHost.AddressList[i].ToString();
                    break;
                }
            }

            if ("".Equals(m_MyIP))
            {
                MonoBehaviour.print("网络检测异常。请检查网络设置或接入网络");
                m_Socket.Close();
                m_Socket = null;
                return;
            }
            m_MyIP = System.Text.Encoding.UTF8.GetBytes(mLocalIP);
            m_Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
            m_HasInit = true;
        }
        catch (System.Exception ex)
        {
            MonoBehaviour.print("Broadcast sender Start catch:" + ex.Message);
        }
    }

    public void Send()
    {
        if (null != m_Socket)
        {
            MonoBehaviour.print("send a broadcast");
            m_Socket.SendTo(m_MyIP, m_IP);
        }
    }

    public void Destroy()
    {
        if (!m_HasInit)
        {
            return;
        }
        m_Socket.Close();
    }
}
