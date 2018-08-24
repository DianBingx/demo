using UnityEngine;
using System.Collections;
using System.Collections.Generic;

delegate void ServerEventDelagate(LanSocket.ClientMsgUnPack msg);

class EventNode
{
    public int m_EventID;
    public LanSocket.ClientMsgUnPack msg;
}

class EventDispathBase
{
    public static int g_MaxEventNum = 300;
}

class ServerEventDispath : EventDispathBase
{
    List<ServerEventDelagate>[] m_Event;
    Queue<EventNode> m_EventQueue;
    public ServerEventDispath()
    {
        m_Event = new List<ServerEventDelagate>[g_MaxEventNum];
        m_EventQueue = new Queue<EventNode>();
    }

    public void RegistEvent(int eventID, ServerEventDelagate func)
    {
        if (null == m_Event[eventID])
        {
            m_Event[eventID] = new List<ServerEventDelagate>();
        }
        m_Event[eventID].Add(func);
    }

    public void AddEvent(EventNode eventNode)
    {
        m_EventQueue.Enqueue(eventNode);
    }

    public void Proccess()
    {
        if (0 != m_EventQueue.Count)
        {
            EventNode mCur = m_EventQueue.Dequeue();
            if (mCur.m_EventID >= g_MaxEventNum || mCur.m_EventID < 0)
            {
                MonoBehaviour.print("error event ID: " + mCur.m_EventID);
                return;
            }
            if (null == m_Event[mCur.m_EventID])
            {
                MonoBehaviour.print("event ID: " + mCur.m_EventID + " is null");
            }
            else
            {
                List<ServerEventDelagate> curEventDelagate = m_Event[mCur.m_EventID];
                for (int i = 0; i < curEventDelagate.Count; ++i)
                {
                    curEventDelagate[i](mCur.msg);
                }
            }
        }
    }
}


delegate void ClientEventDelagate(LanSocket.MsgUnPack msg);
class ClientEventDispath : EventDispathBase
{
    List<ClientEventDelagate>[] m_Event;
    Queue<EventNode> m_EventQueue;
    public ClientEventDispath()
    {
        m_Event = new List<ClientEventDelagate>[g_MaxEventNum];
        m_EventQueue = new Queue<EventNode>();
    }

    public void RegistEvent(int eventID, ClientEventDelagate func)
    {
        if (null == m_Event[eventID])
        {
            m_Event[eventID] = new List<ClientEventDelagate>();
        }
        m_Event[eventID].Add(func);
    }

    public void AddEvent(EventNode eventNode)
    {
        m_EventQueue.Enqueue(eventNode);
    }

    public void Proccess()
    {
        if (0 != m_EventQueue.Count)
        {
            EventNode mCur = m_EventQueue.Dequeue();
            if (mCur.m_EventID >= g_MaxEventNum || mCur.m_EventID < 0)
            {
                MonoBehaviour.print("error event ID: " + mCur.m_EventID);
                return;
            }
            if (null == m_Event[mCur.m_EventID])
            {
                MonoBehaviour.print("event ID: " + mCur.m_EventID + " is null");
            }
            else
            {
                List<ClientEventDelagate> curEventDelagate = m_Event[mCur.m_EventID];
                for (int i = 0; i < curEventDelagate.Count; ++i)
                {
                    curEventDelagate[i](mCur.msg);
                }
            }
        }
    }
}
