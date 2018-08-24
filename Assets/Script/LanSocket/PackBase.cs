using System.Threading;

/*
 *轻量级局域网服务器。 
 * 协议如下
 * 消息头前2字节保存当前消息长度
 * 后面跟4字节表示消息ID
 * 再后面是消息实质内容
 */
//没什么实际意义，主要就是打包和解包类都会使用到的一些数据
namespace LanSocket
{
    public class PackBase
    {
        protected int m_MaxOnePackBuff;
        protected byte[] m_OnePack;
        protected int m_OnePackIndex;

        public PackBase()
        {
            m_MaxOnePackBuff = LanSocketBase.m_MaxOnePackBuff;
            m_OnePack = new byte[m_MaxOnePackBuff];
            m_OnePackIndex = 0;
        }
    }
}
