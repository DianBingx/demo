using UnityEngine;
/*
 * 通信协议
 * 消息头前2字节保存当前消息长度
 * 后面跟4字节表示消息ID
 * 再后面是消息实质内容
 */

namespace LanSocket
{
    class ClientMsgUnPack : MsgUnPack
    {
        long m_UserID;
        public ClientMsgUnPack()
        {
            m_UserID = -1;
        }

        public ClientMsgUnPack(byte[] mBuff, ushort len, int userID)
        {
            m_UserID = userID;
            UnPack(mBuff, len);
        }

        public ClientMsgUnPack(byte[] mBuff, ushort offset, ushort len, int userID)
        {
            m_UserID = userID;
            UnPack(mBuff, offset, len);
        }

        public long GetUserID()
        {
            return m_UserID;
        }

        public void SetUserID(long userID)
        {
            m_UserID = userID;
        }
    }
}
