using UnityEngine;
/*
 * 通信协议
 * 消息头前2字节保存当前消息长度
 * 后面跟4字节表示消息ID
 * 再后面是消息实质内容
 */

namespace LanSocket
{
    class MsgUnPack : PackBase
    {
        ushort m_PackLen;
        int m_MsgID;
        public MsgUnPack()
        {
        }

        void GetHead()
        {
            m_PackLen = System.BitConverter.ToUInt16(m_OnePack, 0);
            m_MsgID = System.BitConverter.ToInt32(m_OnePack, 2);
            m_OnePackIndex = 6;
        }

        public MsgUnPack(byte[] mBuff, ushort len)
        {
            UnPack(mBuff, len);
        }

        public MsgUnPack(byte[] mBuff, int offset, ushort len)
        {
            UnPack(mBuff, offset, len);
        }

        public void UnPack(byte[] mBuff, ushort len)
        {
            System.Buffer.BlockCopy(mBuff, 0, m_OnePack, 0, len);
            GetHead();
        }

        public void UnPack(byte[] mBuff, int offset, ushort len)
        {
            System.Buffer.BlockCopy(mBuff, offset, m_OnePack, 0, len);
            GetHead();
        }

        public bool Readbool()
        {
            if (m_OnePackIndex + 1 > m_PackLen)
            {
                MonoBehaviour.print("Readbool() longer lager than Max buff len");
                return false;
            }
            bool data = System.BitConverter.ToBoolean(m_OnePack, m_OnePackIndex);
            ++m_OnePackIndex;
            return data;
        }

        public short ReadShort()
        {
            if (m_OnePackIndex + 2 > m_PackLen)
            {
                MonoBehaviour.print("ReadShort() longer lager than Max buff len");
                return 0;
            }
            short data = System.BitConverter.ToInt16(m_OnePack, m_OnePackIndex);
            m_OnePackIndex += 2;
            return data;
        }

        public ushort ReadUShort()
        {
            if (m_OnePackIndex + 2 > m_PackLen)
            {
                MonoBehaviour.print("ReadUShortbit() longer lager than Max buff len");
                return 0;
            }
            ushort data = System.BitConverter.ToUInt16(m_OnePack, m_OnePackIndex);
            m_OnePackIndex += 2;
            return data;
        }

        public int ReadInt()
        {
            if (m_OnePackIndex + 4 > m_PackLen)
            {
                MonoBehaviour.print("ReadInt() longer lager than Max buff len");
                return 0;
            }
            int data = System.BitConverter.ToInt32(m_OnePack, m_OnePackIndex);
            m_OnePackIndex += 4;
            return data;
        }

        public uint ReadUInt()
        {
            if (m_OnePackIndex + 4 > m_PackLen)
            {
                MonoBehaviour.print("ReadUInt() longer lager than Max buff len");
                return 0;
            }
            uint data = System.BitConverter.ToUInt32(m_OnePack, m_OnePackIndex);
            m_OnePackIndex += 4;
            return data;
        }

        public float ReadFloat()
        {
            if (m_OnePackIndex + 4 > m_PackLen)
            {
                MonoBehaviour.print("ReadFloat() longer lager than Max buff len");
                return 0.0f;
            }
            float data = System.BitConverter.ToSingle(m_OnePack, m_OnePackIndex);
            m_OnePackIndex += 4;
            return data;
        }

        public double ReadDouble()
        {
            if (m_OnePackIndex + 8 > m_PackLen)
            {
                MonoBehaviour.print("ReadDouble() longer lager than Max buff len");
                return 0.0f;
            }
            double data = System.BitConverter.ToDouble(m_OnePack, m_OnePackIndex);
            m_OnePackIndex += 8;
            return data;
        }

        public long ReadLong()
        {
            if (m_OnePackIndex + 8 > m_PackLen)
            {
                MonoBehaviour.print("ReadLong() longer lager than Max buff len");
                return 0;
            }
            long data = System.BitConverter.ToInt64(m_OnePack, m_OnePackIndex);
            m_OnePackIndex += 8;
            return data;
        }

        public ulong ReadULong()
        {
            if (m_OnePackIndex + 8 > m_PackLen)
            {
                MonoBehaviour.print("ReadULong() longer lager than Max buff len");
                return 0;
            }
            ulong data = System.BitConverter.ToUInt64(m_OnePack, m_OnePackIndex);
            m_OnePackIndex += 8;
            return data;
        }

        public string ReadString(ushort len)
        {
            if (m_OnePackIndex + len > m_PackLen)
            {
                MonoBehaviour.print("ReadString() longer lager than Max buff len");
                return "";
            }
            string data = System.Text.Encoding.UTF8.GetString(m_OnePack, m_OnePackIndex, len);
            m_OnePackIndex += len;
            return data;
        }

        public byte[] ReadByte(ushort len)
        {
            byte[] mCur = null;
            if (m_OnePackIndex + len > m_PackLen)
            {
                MonoBehaviour.print("ReadByte() longer lager than Max buff len");
                return mCur;
            }
            mCur = new byte[len];
            System.Buffer.BlockCopy(m_OnePack, m_OnePackIndex, mCur, 0, len);
            m_OnePackIndex += len;
            return mCur;
        }

        public int GetMsgID()
        {
            return m_MsgID;
        }
    }
}
