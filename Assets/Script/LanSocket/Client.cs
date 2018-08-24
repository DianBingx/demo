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
    class Client : LanSocketBase
    {
        Thread m_ReciveThread;
        Socket m_Connect;
        byte[] m_AllData;
        int m_AllDataHead;
        int m_AllDataEnd;
        int m_MsgNum;
        byte[] m_OnePack;
        int m_OnePackIndex;

        public void Start(string strIP, int port)
        {
            if (m_HasInit)
            {
                return;
            }
            //设定服务器IP地址  
            IPAddress ip = IPAddress.Parse(strIP);
            Socket temp = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                temp.Connect(new IPEndPoint(ip, port)); //配置服务器IP与端口  
                MonoBehaviour.print("连接服务器成功");

                BaseInit();
                m_Connect = temp;
                m_ReciveThread = new Thread(ReceiveMessage);
                m_ReciveThread.Start();
                m_AllData = new byte[LanSocketBase.m_MaxAllBuff + 1];
                m_AllDataHead = 0;
                m_AllDataEnd = 0;
                m_MsgNum = 0;
                m_OnePack = new byte[m_MaxOnePackBuff + 1];
                m_OnePackIndex = 0;
            }
            catch (System.Exception ex)
            {
                MonoBehaviour.print("连接服务器失败: " + ex.Message);
                return;
            }
        }

        private void PutDataToBuff(byte[] mClientSendBuff, int mReceiveNumber)
        {
            if (m_AllDataEnd + mReceiveNumber >= LanSocketBase.m_MaxAllBuff)
            {
                byte[] mCurAllData = new byte[m_AllDataEnd - m_AllDataHead];
                System.Buffer.BlockCopy(m_AllData, m_AllDataHead, mCurAllData, 0, m_AllDataEnd - m_AllDataHead);
                System.Buffer.BlockCopy(mCurAllData, 0, m_AllData, 0, m_AllDataEnd - m_AllDataHead);
                m_AllDataEnd -= m_AllDataHead;
                m_AllDataHead = 0;
            }
            int mOnePackStartPos = 0;
            while (mReceiveNumber > 0)
            {
                if (0 == m_OnePackIndex)
                {
                    ushort datalen = System.BitConverter.ToUInt16(mClientSendBuff, mOnePackStartPos);
                    if (datalen <= mReceiveNumber)
                    {
                        System.Buffer.BlockCopy(mClientSendBuff, mOnePackStartPos, m_AllData, m_AllDataEnd, datalen);
                        m_AllDataEnd += datalen;

                        mOnePackStartPos += datalen;

                        mReceiveNumber -= datalen;
                        ++m_MsgNum;
                    }
                    else
                    {
                        System.Buffer.BlockCopy(mClientSendBuff, mOnePackStartPos, m_OnePack, m_OnePackIndex, mReceiveNumber);
                        m_OnePackIndex += mReceiveNumber;
                        mOnePackStartPos += mReceiveNumber;

                        mReceiveNumber -= mReceiveNumber;
                    }
                }
                else
                {
                    if (m_OnePackIndex < 2)
                    {
                        System.Buffer.BlockCopy(mClientSendBuff, mOnePackStartPos, m_OnePack, m_OnePackIndex, 1);

                        ++m_OnePackIndex;
                        --mReceiveNumber;
                        ++mOnePackStartPos;
                    }
                    ushort datalen = System.BitConverter.ToUInt16(m_OnePack, 0);
                    if (m_OnePackIndex + mReceiveNumber >= datalen)
                    {
                        int mNeedNum = datalen - m_OnePackIndex;
                        System.Buffer.BlockCopy(mClientSendBuff, mOnePackStartPos, m_OnePack, m_OnePackIndex, mNeedNum);
                        mOnePackStartPos += mNeedNum;

                        System.Buffer.BlockCopy(m_OnePack, 0, m_AllData, m_AllDataEnd, datalen);
                        m_OnePackIndex = 0;
                        m_AllDataEnd += datalen;

                        mReceiveNumber -= mNeedNum;
                    }
                    else
                    {
                        System.Buffer.BlockCopy(mClientSendBuff, mOnePackStartPos, m_OnePack, m_OnePackIndex, mReceiveNumber);
                        m_OnePackIndex += mReceiveNumber;
                        mOnePackStartPos += mReceiveNumber;

                        mReceiveNumber -= mReceiveNumber;
                    }
                }
            }
        }

        public void Destroy()
        {
            if (!m_HasInit)
            {
                return;
            }
            BaseRelease();
            ShutDownConnect();
            m_MsgNum = 0;
        }

        public void GetMsg(ref MsgUnPack msg)
        {
            if (!m_HasInit)
            {
                return;
            }
            try
            {
                Lock();
                if (0 != m_MsgNum)
                {
                    ushort datalen = System.BitConverter.ToUInt16(m_AllData, m_AllDataHead);
                    msg = new MsgUnPack(m_AllData, (ushort)m_AllDataHead, (ushort)datalen);
                    m_AllDataHead += datalen;
                    --m_MsgNum;
                }
            }
            finally
            {
                UnLock();
            }
        }

        /// <summary>  
        /// 接收消息  
        /// </summary>  
        public void ReceiveMessage()
        {
            while (true)
            {
                Thread.Sleep(1);
                try
                {
                    //通过clientSocket接收数据  
                    byte[] mClientSendBuff = new byte[m_MaxOnePackBuff + 1];
                    int mReceiveNumber = m_Connect.Receive(mClientSendBuff);
                    if (0 == mReceiveNumber)
                    {
                        MonoBehaviour.print("disconnect");
                        ShutDownConnect();
                    }
                    else if (mReceiveNumber > 0)
                    {
                        try
                        {
                            Lock();
                            PutDataToBuff(mClientSendBuff, mReceiveNumber);
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
                }
                catch (System.Exception ex)
                {
                    MonoBehaviour.print("ReceiveMessage catch: " + ex.Message);
                    ShutDownConnect();
                }
            }
        }

        public void Send(ref MsgPack msg)
        {
            try
            {
                Lock();
                m_Connect.Send(msg.GetByte(), msg.GetByteLen(), SocketFlags.None);
            }
            finally
            {
                UnLock();
            }
        }

        public void ShutDownConnect()
        {
            m_ReciveThread.Abort();
            if (m_Connect.Connected)
            {
                m_Connect.Shutdown(SocketShutdown.Both);
            }
            m_Connect.Close();
        }
    }
}
