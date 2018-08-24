using System.Net.Sockets;
using System.Net;
using System.Threading;
using UnityEngine;
using System.Collections.Generic;
/*
 *轻量级局域网服务器。 
 * 协议如下
 * 消息头前2字节保存当前消息长度
 * 后面跟4字节表示消息ID
 * 再后面是消息实质内容
 */

namespace LanSocket
{
    class ClientConnect
    {
        public byte[] m_AllData;
        public int m_AllDataHead;
        public int m_AllDataEnd;
        public int m_MsgCount;
        public byte[] m_OnePack;
        public int m_OnePackIndex;
        public Socket m_Connect;
        public long m_UserID;

        public ClientConnect()
        {
            m_AllData = new byte[LanSocketBase.m_MaxAllBuff];
            m_AllDataHead = 0;
            m_AllDataEnd = 0;
            m_MsgCount = 0;
            m_OnePack = new byte[LanSocketBase.m_MaxOnePackBuff];
            m_OnePackIndex = 0;
            m_Connect = null;
            m_UserID = 0;
        }

        public void Reset()
        {
            m_AllDataHead = 0;
            m_AllDataEnd = 0;
            m_MsgCount = 0;
            m_OnePackIndex = 0;
            m_Connect = null;
            m_UserID = 0;
        }
    }
    class Server : LanSocketBase
    {
        Queue<int> m_MsgOrder;

        Socket m_ServerSocket;
        Thread m_LinstenThread;
        Thread m_ReciveThread;
        System.Collections.ArrayList m_ServerSocketList;
        System.Collections.ArrayList m_listenSocketList;
        System.Collections.ArrayList m_DeleteSocketList;
        int m_MaxClientConnect = 10;
        ClientConnect[] m_ConnectPool;
        Queue<int> m_EmptyConnect;
        public void Start(int port)
        {
            if (m_HasInit)
            {
                return;
            }
            string mLocalIP = "";

            string mHostName = Dns.GetHostName();
            IPHostEntry localHost = Dns.GetHostEntry(mHostName);
            for (int i = 0; i < localHost.AddressList.Length; ++i)
            {
                if (localHost.AddressList[i].AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    //MonoBehaviour.print(localHost.AddressList[i].ToString());
                    mLocalIP = localHost.AddressList[i].ToString();
                    break;
                }
            }

            if ("".Equals(mLocalIP))
            {
                MonoBehaviour.print("网络检测异常。请检查网络设置或接入网络");
                return;
            }
            BaseInit();
            m_MsgOrder = new Queue<int>();

            //服务器IP地址  
            IPAddress ip = IPAddress.Parse(mLocalIP);
            m_ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            m_ServerSocket.Bind(new IPEndPoint(ip, port));  //绑定IP地址：端口  
            m_ServerSocket.Listen(10);    //设定最多10个排队连接请求  
            MonoBehaviour.print("游戏网络启动监听" + m_ServerSocket.LocalEndPoint.ToString());

            m_ServerSocketList = new System.Collections.ArrayList();
            m_listenSocketList = new System.Collections.ArrayList();
            m_DeleteSocketList = new System.Collections.ArrayList();

            m_ConnectPool = new ClientConnect[m_MaxClientConnect];
            m_EmptyConnect = new Queue<int>();
            for (int i = 0; i < m_MaxClientConnect; ++i)
            {
                m_ConnectPool[i] = new ClientConnect();
                m_EmptyConnect.Enqueue(i);
            }
            //通过Clientsoket发送数据  
            m_ReciveThread = new Thread(ReceiveMessage);
            m_ReciveThread.Start();
            m_LinstenThread = new Thread(ListenClientConnect);
            m_LinstenThread.Start();
        }

        /// <summary>  
        /// 监听客户端连接  
        /// </summary>  
        public void ListenClientConnect()
        {
            while (true)
            {
                Thread.Sleep(500);
                m_ServerSocketList.Add(m_ServerSocket);
                Socket.Select(m_ServerSocketList, null, null, 1000);
                for (int i = 0; i < m_ServerSocketList.Count; ++i)
                {
                    Socket clientSocket = ((Socket)m_ServerSocketList[i]).Accept();
                    if (null != clientSocket)
                    {
                        try
                        {
                            Lock();
                            if (0 == m_EmptyConnect.Count)
                            {
                                MonoBehaviour.print("链接已经达到最大上线，丢弃当前连接");
                                clientSocket.Shutdown(SocketShutdown.Both);
                                clientSocket.Close();
                            }
                            else
                            {
                                //m_listenSocketList.Add(clientSocket);
                                int mSlot = m_EmptyConnect.Dequeue();
                                m_ConnectPool[mSlot].m_Connect = clientSocket;
                                m_ConnectPool[mSlot].m_UserID = System.DateTime.Now.ToFileTime();
                                MonoBehaviour.print("成功连接一个客户端，编号:" + mSlot.ToString());
                            }
                        }
                        finally
                        {
                            UnLock();
                        }
                    }
                }
                m_ServerSocketList.Clear();
            }
        }

        private bool PutDataToBuff(byte[] mClientSendBuff, int mReceiveNumber, Socket client)
        {
            ClientConnect curPlayer = null;
            int mSlot = -1;
            for (int i = 0; i < m_MaxClientConnect; ++i)
            {
                if (client == m_ConnectPool[i].m_Connect)
                {
                    curPlayer = m_ConnectPool[i];
                    mSlot = i;
                    break;
                }
            }
            if (null == curPlayer)
            {
                return false;
            }
            if (curPlayer.m_AllDataEnd + mReceiveNumber >= LanSocketBase.m_MaxAllBuff)
            {
                byte[] mCurAllData = new byte[curPlayer.m_AllDataEnd - curPlayer.m_AllDataHead];
                System.Buffer.BlockCopy(curPlayer.m_AllData, curPlayer.m_AllDataHead, mCurAllData, 0, curPlayer.m_AllDataEnd - curPlayer.m_AllDataHead);
                System.Buffer.BlockCopy(mCurAllData, 0, curPlayer.m_AllData, 0, curPlayer.m_AllDataEnd - curPlayer.m_AllDataHead);
                curPlayer.m_AllDataEnd -= curPlayer.m_AllDataHead;
                curPlayer.m_AllDataHead = 0;

                if (curPlayer.m_AllDataEnd + mReceiveNumber >= LanSocketBase.m_MaxAllBuff)
                {
                    return false;
                }
            }
            int mOnePackStartPos = 0;
            while (mReceiveNumber > 0)
            {
                if (0 == curPlayer.m_OnePackIndex)
                {
                    ushort datalen = System.BitConverter.ToUInt16(mClientSendBuff, mOnePackStartPos);
                    if (datalen > LanSocketBase.m_MaxOnePackBuff || datalen < LanSocketBase.m_HeadSize)
                    {
                        return false;
                    }
                    if (datalen <= mReceiveNumber)
                    {
                        System.Buffer.BlockCopy(mClientSendBuff, mOnePackStartPos, curPlayer.m_AllData, curPlayer.m_AllDataEnd, datalen);
                        curPlayer.m_AllDataEnd += datalen;
                        mOnePackStartPos += datalen;

                        mReceiveNumber -= datalen;

                        m_MsgOrder.Enqueue(mSlot);
                    }
                    else
                    {
                        System.Buffer.BlockCopy(mClientSendBuff, mOnePackStartPos, curPlayer.m_OnePack, curPlayer.m_OnePackIndex, mReceiveNumber);
                        curPlayer.m_OnePackIndex += mReceiveNumber;
                        mOnePackStartPos += mReceiveNumber;

                        mReceiveNumber -= mReceiveNumber;
                    }
                }
                else
                {
                    if (curPlayer.m_OnePackIndex < 2)
                    {
                        System.Buffer.BlockCopy(mClientSendBuff, mOnePackStartPos, curPlayer.m_OnePack, curPlayer.m_OnePackIndex, 1);

                        ++curPlayer.m_OnePackIndex;
                        --mReceiveNumber;
                        ++mOnePackStartPos;
                    }
                    ushort datalen = System.BitConverter.ToUInt16(curPlayer.m_OnePack, 0);
                    if (datalen > LanSocketBase.m_MaxOnePackBuff || datalen < LanSocketBase.m_HeadSize)
                    {
                        return false;
                    }
                    if (curPlayer.m_OnePackIndex + mReceiveNumber >= datalen)
                    {
                        int mNeedNum = datalen - curPlayer.m_OnePackIndex;
                        System.Buffer.BlockCopy(mClientSendBuff, mOnePackStartPos, curPlayer.m_OnePack, curPlayer.m_OnePackIndex, mNeedNum);
                        mOnePackStartPos += mNeedNum;

                        System.Buffer.BlockCopy(curPlayer.m_OnePack, 0, curPlayer.m_AllData, curPlayer.m_AllDataEnd, datalen);
                        curPlayer.m_OnePackIndex = 0;
                        curPlayer.m_AllDataEnd += datalen;

                        mReceiveNumber -= mNeedNum;

                        m_MsgOrder.Enqueue(mSlot);
                    }
                    else
                    {
                        System.Buffer.BlockCopy(mClientSendBuff, mOnePackStartPos, curPlayer.m_OnePack, curPlayer.m_OnePackIndex, mReceiveNumber);
                        curPlayer.m_OnePackIndex += mReceiveNumber;
                        mOnePackStartPos += mReceiveNumber;

                        mReceiveNumber -= mReceiveNumber;
                    }
                }
            }

            return true;
        }

        /// <summary>  
        /// 接收消息  
        /// </summary>  
        public void ReceiveMessage()
        {
            try
            {
                while (true)
                {
                    Thread.Sleep(1);
                    for (int i = 0; i < m_MaxClientConnect; ++i)
                    {
                        if (null != m_ConnectPool[i].m_Connect)
                        {
                            m_listenSocketList.Add(m_ConnectPool[i].m_Connect);
                        }
                    }
                    if (0 == m_listenSocketList.Count)
                    {
                        continue;
                    }
                    Socket.Select(m_listenSocketList, null, null, 1000);
                    for (int i = 0; i < m_listenSocketList.Count; ++i)
                    {
                        Socket mClient = (Socket)m_listenSocketList[i];
                        //try
                        //{
                        //通过clientSocket接收数据  
                        byte[] mClientSendBuff = new byte[m_MaxOnePackBuff];
                        int mReceiveNumber = mClient.Receive(mClientSendBuff);
                        if (0 == mReceiveNumber)
                        {
                            m_DeleteSocketList.Add(mClient);
                        }
                        else if (mReceiveNumber > 0)
                        {
                            try
                            {
                                Lock();
                                bool rt = PutDataToBuff(mClientSendBuff, mReceiveNumber, mClient);
                                if (!rt)
                                {
                                    m_DeleteSocketList.Add(mClient);
                                }
                            }
                            catch (System.Exception ex)
                            {
                                MonoBehaviour.print("PutDataToBuff catch: " + ex.Message);
                            }
                            finally
                            {
                                UnLock();
                            }
                        }
                        else
                        {
                            MonoBehaviour.print("one connect recive a error num: " + mReceiveNumber.ToString());
                        }
                        //}
                        //catch (System.Exception ex)
                        //{
                        //    MonoBehaviour.print("ReceiveMessage catch: " + ex.Message);
                        //    m_DeleteSocketList.Add(mClient);
                        //}
                    }
                    m_listenSocketList.Clear();
                    if (0 != m_DeleteSocketList.Count)
                    {
                        ShutDownConnect();
                    }
                }

            }
            catch (System.Exception ex)
            {
                MonoBehaviour.print("ReceiveMessage out:" + ex.Message);
            }

        }

        /// <summary>  
        /// 程序退出销毁  
        /// </summary>  
        public void Destroy()
        {
            if (!m_HasInit)
            {
                return;
            }
            m_LinstenThread.Abort();
            m_ReciveThread.Abort();
            m_listenSocketList.Clear();

            for (int i = 0; i < m_ServerSocketList.Count; ++i)
            {
                Socket mServer = (Socket)m_ServerSocketList[i];
                if (mServer.Connected)
                {
                    mServer.Shutdown(SocketShutdown.Both);
                }
                mServer.Close();
            }
            m_ServerSocketList.Clear();

            for (int i = 0; i < m_MaxClientConnect; ++i)
            {
                if (null != m_ConnectPool[i].m_Connect)
                {
                    if (m_ConnectPool[i].m_Connect.Connected)
                    {
                        m_ConnectPool[i].m_Connect.Shutdown(SocketShutdown.Both);
                    }
                    m_ConnectPool[i].m_Connect.Close();
                    m_ConnectPool[i].m_Connect = null;
                }
            }
            m_EmptyConnect.Clear();
            BaseRelease();
        }

        /// <summary>  
        /// 销毁一个连接  
        /// </summary>  
        void ShutDownConnect()
        {
            try
            {
                Lock();
                for (int j = 0; j < m_DeleteSocketList.Count; ++j)
                {
                    Socket connect = (Socket)m_DeleteSocketList[j];
                    for (int i = 0; i < m_MaxClientConnect; ++i)
                    {
                        if (connect == m_ConnectPool[i].m_Connect)
                        {
                            connect.Shutdown(SocketShutdown.Both);
                            connect.Close();
                            m_ConnectPool[i].Reset();
                            m_EmptyConnect.Enqueue(i);
                            MonoBehaviour.print("关闭一个连接，编号:" + i.ToString());
                            break;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                MonoBehaviour.print("ShutDownConnect catch: " + ex.Message);
            }
            finally
            {
                m_DeleteSocketList.Clear();
                UnLock();
            }
        }

        /// <summary>  
        /// 获取一个数据  
        /// </summary>  
        public void GetMsg(ref ClientMsgUnPack msg)
        {
            if (!m_HasInit)
            {
                return;
            }
            try
            {
                Lock();
                if (0 != m_MsgOrder.Count)
                {
                    int mSlot = m_MsgOrder.Dequeue();
                    ClientConnect curPlayer = m_ConnectPool[mSlot];
                    ushort mOnePackLen = System.BitConverter.ToUInt16(curPlayer.m_AllData, curPlayer.m_AllDataHead);
                    msg = new ClientMsgUnPack(curPlayer.m_AllData, (ushort)curPlayer.m_AllDataHead, (ushort)mOnePackLen, (int)curPlayer.m_UserID);
                    curPlayer.m_AllDataHead += mOnePackLen;
                }
            }
            finally
            {
                UnLock();
            }
        }

        public void SendTo(ref MsgPack msg, long userID)
        {
            try
            {
                Lock();
                for (int i = 0; i < m_MaxClientConnect; ++i)
                {
                    ClientConnect curPlayer = m_ConnectPool[i];
                    if (null != curPlayer.m_Connect && curPlayer.m_UserID == userID)
                    {
                        curPlayer.m_Connect.Send(msg.GetByte(), msg.GetByteLen(), SocketFlags.None);
                        break;
                    }
                }
            }
            finally
            {
                UnLock();
            }
        }

        public void SendToAll(ref MsgPack msg)
        {
            try
            {
                Lock();
                for (int i = 0; i < m_MaxClientConnect; ++i)
                {
                    ClientConnect curPlayer = m_ConnectPool[i];
                    if (null != curPlayer.m_Connect)
                    {
                        curPlayer.m_Connect.Send(msg.GetByte(), msg.GetByteLen(), SocketFlags.None);
                        break;
                    }
                }
            }
            finally
            {
                UnLock();
            }
        }
    }
}
