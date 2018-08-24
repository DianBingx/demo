using UnityEngine;
/*
 * 通信协议
 * 消息头前2字节保存当前消息长度
 * 后面跟4字节表示消息ID
 * 再后面是消息实质内容
 */

namespace LanSocket
{
    public class MsgPack : PackBase
    {
        public MsgPack()
        {
            m_OnePackIndex = LanSocketBase.m_HeadSize;
        }

        public void SetHead(int ID)
        {
            byte[] mBuff = System.BitConverter.GetBytes(ID);
            System.Buffer.BlockCopy(mBuff, 0, m_OnePack, 2, 4);
        }

        public void PackEnd()
        {
            byte[] mBuff = System.BitConverter.GetBytes(m_OnePackIndex);
            System.Buffer.BlockCopy(mBuff, 0, m_OnePack, 0, 2);
        }

        public void Packbool(bool data)
        {
            ushort curDatalen = 1;
            if (m_OnePackIndex + curDatalen > m_MaxOnePackBuff)
            {
                MonoBehaviour.print("Packbool() longer lager than Max buff len");
                return;
            }
            byte[] mBuff = System.BitConverter.GetBytes(data);
            Pack(mBuff, curDatalen);
        }

        public void Pack16bit(short data)
        {
            ushort curDatalen = 2;
            if (m_OnePackIndex + curDatalen > m_MaxOnePackBuff)
            {
                MonoBehaviour.print("Pack16bit(short) longer lager than Max buff len");
                return;
            }
            byte[] mBuff = System.BitConverter.GetBytes(data);
            Pack(mBuff, curDatalen);
        }
        public void Pack16bit(ushort data)
        {
            ushort curDatalen = 2;
            if (m_OnePackIndex + curDatalen > m_MaxOnePackBuff)
            {
                MonoBehaviour.print("Pack16bit(ushort) longer lager than Max buff len");
                return;
            }
            byte[] mBuff = System.BitConverter.GetBytes(data);
            Pack(mBuff, curDatalen);
        }
        public void Pack32bit(int data)
        {
            ushort curDatalen = 4;
            if (m_OnePackIndex + curDatalen > m_MaxOnePackBuff)
            {
                MonoBehaviour.print("Pack32bit(int) longer lager than Max buff len");
                return;
            }
            byte[] mBuff = System.BitConverter.GetBytes(data);
            Pack(mBuff, curDatalen);
        }
        public void Pack32bit(uint data)
        {
            ushort curDatalen = 4;
            if (m_OnePackIndex + curDatalen > m_MaxOnePackBuff)
            {
                MonoBehaviour.print("Pack32bit(uint) longer lager than Max buff len");
                return;
            }
            byte[] mBuff = System.BitConverter.GetBytes(data);
            Pack(mBuff, curDatalen);
        }
        public void Pack32bit(float data)
        {
            ushort curDatalen = 4;
            if (m_OnePackIndex + curDatalen > m_MaxOnePackBuff)
            {
                MonoBehaviour.print("Pack32bit(float) longer lager than Max buff len");
                return;
            }
            byte[] mBuff = System.BitConverter.GetBytes(data);
            Pack(mBuff, curDatalen);
        }
        public void Pack64bit(double data)
        {
            ushort curDatalen = 8;
            if (m_OnePackIndex + curDatalen > m_MaxOnePackBuff)
            {
                MonoBehaviour.print("Pack64bit(double) longer lager than Max buff len");
                return;
            }
            byte[] mBuff = System.BitConverter.GetBytes(data);
            Pack(mBuff, curDatalen);
        }
        public void Pack64bit(long data)
        {
            ushort curDatalen = 8;
            if (m_OnePackIndex + curDatalen > m_MaxOnePackBuff)
            {
                MonoBehaviour.print("Pack64bit(long) longer lager than Max buff len");
                return;
            }
            byte[] mBuff = System.BitConverter.GetBytes(data);
            Pack(mBuff, curDatalen);
        }

        public void PackString(string data, ushort len)
        {
            ushort curDatalen = len;
            if (m_OnePackIndex + curDatalen > m_MaxOnePackBuff)
            {
                MonoBehaviour.print("PackString() longer lager than Max buff len");
                return;
            }
            byte[] mBuff = System.Text.Encoding.UTF8.GetBytes(data);
            Pack(mBuff, curDatalen);
        }

        public void PackByte(byte[] data, int offset, ushort len)
        {
            if (m_OnePackIndex + len > m_MaxOnePackBuff)
            {
                MonoBehaviour.print("PackByte() longer lager than Max buff len");
                return;
            }
            System.Buffer.BlockCopy(data, offset, m_OnePack, m_OnePackIndex, len);
            m_OnePackIndex += len;
        }

        void Pack(byte[] data, ushort len)
        {
            System.Buffer.BlockCopy(data, 0, m_OnePack, m_OnePackIndex, len);
            m_OnePackIndex += len;
        }

        public byte[] GetByte()
        {
            return m_OnePack;
        }

        public int GetByteLen()
        {
            return m_OnePackIndex;
        }
    }
}
