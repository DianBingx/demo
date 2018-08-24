using UnityEngine;
using System.Collections;

public class ServerMain : MonoBehaviour
{
    bool m_Destroy;
    ServerEventDispath m_ClientMsg;
    ReciveBroadcast m_Reciver;
    SendBroadcast m_Sender;
    LanSocket.Server m_GameNet;
    byte[] m_AudioData;
    int m_AudioOffset;
    int m_AudioLen;
    void Start()
    {
        m_Destroy = false;
        //广播
        m_Reciver = new ReciveBroadcast();
        m_Reciver.Start(6666);
        m_Sender = new SendBroadcast();
        m_Sender.Start(6688);

        //游戏网络
        m_GameNet = new LanSocket.Server();
        m_GameNet.Start(8888);

        m_AudioData = new byte[44100000];
        m_AudioOffset = 0;
        m_AudioLen = 0;

        m_ClientMsg = new ServerEventDispath();
        m_ClientMsg.RegistEvent((int)NetMsgID.C2S_ASK_SEND_AUDIO_BEGIN, Action_C2S_ASK_SEND_AUDIO_BEGIN);
        m_ClientMsg.RegistEvent((int)NetMsgID.C2S_ASK_SEND_AUDIO, Action_C2S_ASK_SEND_AUDIO);
        m_ClientMsg.RegistEvent((int)NetMsgID.C2S_ASK_SEND_AUDIO_END, Action_C2S_ASK_SEND_AUDIO_END);
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_Destroy)
        {
            LanSocket.ClientMsgUnPack clientMsg = null;
            m_GameNet.GetMsg(ref clientMsg);
            if (null != clientMsg)
            {
                //print("Msg:" + clientMsg.GetMsgID() + " from: " + clientMsg.GetUserID());

                EventNode mNode = new EventNode();
                mNode.m_EventID = clientMsg.GetMsgID();
                mNode.msg = clientMsg;

                MonoBehaviour.print("Update EventID " + mNode.m_EventID + " ID: " + clientMsg.GetMsgID());
                m_ClientMsg.AddEvent(mNode);
            }

            if (!"".Equals(m_Reciver.GetIP()))
            {
                m_Sender.Send();
            }

            m_ClientMsg.Proccess();
        }
    }

    void OnDestroy()
    {
        m_Destroy = true;
        m_GameNet.Destroy();
        m_Reciver.Destroy();
        m_Sender.Destroy();
    }

    void Action_123(LanSocket.ClientMsgUnPack msg)
    {
        long userID = msg.GetUserID();
        ushort accountLen = msg.ReadUShort();
        string account = msg.ReadString(accountLen);
        ushort passLen = msg.ReadUShort();
        string pass = msg.ReadString(passLen);

        print("Action_123 account: " + account + " pass word: " + pass + " from user: " + userID);

        LanSocket.MsgPack sendMsg = new LanSocket.MsgPack();
        sendMsg.SetHead(123);
        string strAccount = "test account";
        sendMsg.Pack16bit((ushort)strAccount.Length);
        sendMsg.PackString(strAccount, (ushort)strAccount.Length);
        string strPass = "test pass word";
        sendMsg.Pack16bit((ushort)strPass.Length);
        sendMsg.PackString(strPass, (ushort)strPass.Length);
        sendMsg.PackEnd();
        m_GameNet.SendTo(ref sendMsg, msg.GetUserID());
    }

    void Action_C2S_ASK_SEND_AUDIO_BEGIN(LanSocket.ClientMsgUnPack msg)
    {
        m_AudioOffset = 0;
        m_AudioLen = 0;
    }

    void Action_C2S_ASK_SEND_AUDIO(LanSocket.ClientMsgUnPack msg)
    {
        long userID = msg.GetUserID();
        ushort dataLen = msg.ReadUShort();
        byte[] audioData = msg.ReadByte(dataLen);

        //         string result = string.Empty;
        //         for (int i = 0; i < dataLen; i++)
        //         {
        //             result += System.Convert.ToString(audioData[i], 16) + " ";
        //         }
        //         print(result);

        System.Buffer.BlockCopy(audioData, 0, m_AudioData, m_AudioOffset, (int)dataLen);
        m_AudioOffset += dataLen;
    }

    void Action_C2S_ASK_SEND_AUDIO_END(LanSocket.ClientMsgUnPack msg)
    {
        print("audio length: " + m_AudioOffset);
        short[] decodeData = new short[m_AudioOffset / 2];
        for (int i = 0; i < decodeData.Length; ++i)
        {
            decodeData[i] = System.BitConverter.ToInt16(m_AudioData, i * 2);
        }
        //MicroPhoneInput.getInstance().PlayClipData(decodeData);

        //         string result = string.Empty;
        //         for (int i = 0; i < m_AudioOffset; i++)
        //         {
        //             result += System.Convert.ToString(m_AudioData[i], 16) + " ";
        //         }
        //         print(result);
    }
}
