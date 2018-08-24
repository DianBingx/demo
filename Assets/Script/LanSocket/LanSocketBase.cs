using System.Threading;
using UnityEngine;

/*
 *轻量级局域网服务器。 
 * 协议如下
 * 消息头前2字节保存当前消息长度
 * 后面跟4字节表示消息ID
 * 再后面是消息实质内容
 */

namespace LanSocket
{
    public class LanSocketBase
    {
        public static int m_MaxOnePackBuff = 1024 * 3;
        public static int m_MaxAllBuff = 1024 * 100;
        public static int m_HeadSize = 6;
        protected static bool m_HasInit = false;
        private static Mutex m_Mutex;

        public static void BaseInit()
        {
            m_HasInit = true;
            m_Mutex = new Mutex();
        }

        public static void BaseRelease()
        {
            m_Mutex.Close();
        }

        protected static void Lock()
        {
            m_Mutex.WaitOne();
            //MonoBehaviour.print("Lock:" + Thread.CurrentThread.ManagedThreadId.ToString());
        }

        protected static void UnLock()
        {
            m_Mutex.ReleaseMutex();
            //MonoBehaviour.print("Unlock:" + Thread.CurrentThread.ManagedThreadId.ToString());
        }
    }
}
