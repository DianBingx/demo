using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Collections.Generic;
/*
 * ReciveBroadcast.cs 广播接收器。服务器段需要长期保持打开，因为只要有新用户加入，服务器就需要下发数据。
 * 而客户端不需要，因为客户端一但连上，就不需要广播系统了。所以服务器长期保留，客户端用完销毁
 * 
 * */
class ReciveBroadcast : SocketBase
{
    public Queue<string> m_ServerIP;
    public void Start(int port)
    {
        if (m_HasInit)
        {
            return;
        }
        try
        {
            m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            m_IP = new IPEndPoint(IPAddress.Any, port);
            m_Socket.Bind(m_IP);
            MonoBehaviour.print("广播网络启动监听" + m_Socket.LocalEndPoint.ToString());
            m_LinstenThread = new Thread(ListenClientConnect);
            m_LinstenThread.Start();
            m_ServerIP = new Queue<string>();
            m_Mutex = new Mutex();
            m_HasInit = true;
        }
        catch (System.Exception ex)
        {
            MonoBehaviour.print("Broadcast reciver Start catch:" + ex.Message);
        }
    }

    void ListenClientConnect()
    {
        EndPoint ep = (EndPoint)m_IP;
        try
        {
            while (true)
            {
                Thread.Sleep(1);
                byte[] data = new byte[64];
                int recv = m_Socket.ReceiveFrom(data, ref ep);
                string stringData = System.Text.Encoding.UTF8.GetString(data, 0, recv);
                m_Mutex.WaitOne();
                m_ServerIP.Enqueue(stringData);
                m_Mutex.ReleaseMutex();
                MonoBehaviour.print("received: " + stringData + " from: " + ep.ToString());
            }
        }
        catch (System.Exception ex)
        {
            MonoBehaviour.print("Broadcast reciver ListenClientConnect out:" + ex.Message);
        }
    }

    public void Destroy()
    {
        if (!m_HasInit)
        {
            return;
        }
        m_Socket.Close();
        m_LinstenThread.Abort();
    }

    public string GetIP()
    {
        if (!m_HasInit)
        {
            return "";
        }

        try
        {
            m_Mutex.WaitOne();
            if (0 != m_ServerIP.Count)
            {
                m_Mutex.ReleaseMutex();
                return m_ServerIP.Dequeue();
            }
            m_Mutex.ReleaseMutex();
        }
        catch (System.Exception ex)
        {
            MonoBehaviour.print("Broadcast GetIP catch:" + ex.Message);
            return "";
        }
        return "";
    }
}
