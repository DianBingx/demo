using UnityEngine;
using System.Collections;

public class ClientMain : MonoBehaviour
{

    // Use this for initialization
    ClientEventDispath m_Msg;
    SendBroadcast m_Sender;
    ReciveBroadcast m_Reciver;
    LanSocket.Client m_GameNet;
    string m_GameServerIP;
    bool m_bReady;
    float m_BroadTime;

    void Start()
    {
        m_Sender = new SendBroadcast();
        m_Sender.Start(6666);
        m_Reciver = new ReciveBroadcast();
        m_Reciver.Start(6688);

        m_GameNet = new LanSocket.Client();

        m_GameServerIP = "";

        m_bReady = false;
        m_BroadTime = 0.0f;

        EventDispathBase.g_MaxEventNum = (int)NetMsgID.NET_MSG_END;
        m_Msg = new ClientEventDispath();
        m_Msg.RegistEvent((int)NetMsgID.S2C_SEND_ANIMAL_DATA, Action_S2C_SEND_ANIMAL_DATA);
    }

    void OnGUI()
    {
        //if (GUILayout.Button("Record"))
        //{
        //    MicroPhoneInput.getInstance().StartRecord();
        //}
        //if (GUILayout.Button("End"))
        //{
        //    MicroPhoneInput.getInstance().StopRecord();
        //}
        //if (GUILayout.Button("send"))
        //{
        //    byte[] data = MicroPhoneInput.getInstance().GetClipData();

        //    LanSocket.MsgPack audioBegin = new LanSocket.MsgPack();
        //    audioBegin.SetHead((int)NetMsgID.C2S_ASK_SEND_AUDIO_BEGIN);
        //    audioBegin.PackEnd();
        //    m_GameNet.Send(ref audioBegin);

        //    print("audio length = " + data.Length);
        //    int lastData = data.Length;
        //    int offset = 0;
        //    while (true)
        //    {
        //        ushort sendNum = (ushort)lastData;
        //        if (lastData > 3000)
        //        {
        //            sendNum = 3000;
        //        }
        //        LanSocket.MsgPack audioData = new LanSocket.MsgPack();
        //        audioData.SetHead((int)NetMsgID.C2S_ASK_SEND_AUDIO);
        //        audioData.Pack16bit(sendNum);
        //        audioData.PackByte(data, offset, sendNum);
        //        audioData.PackEnd();
        //        m_GameNet.Send(ref audioData);

        //        //                 string result = string.Empty;
        //        //                 for (int i = 0; i < sendNum; i++)
        //        //                 {
        //        //                     result += System.Convert.ToString(data[offset + i], 16) + " ";
        //        //                 }
        //        //                 print(result);

        //        offset += sendNum;
        //        if (offset >= data.Length)
        //        {
        //            break;
        //        }
        //        lastData -= sendNum;
        //    }
        //    LanSocket.MsgPack audioEnd = new LanSocket.MsgPack();
        //    audioEnd.SetHead((int)NetMsgID.C2S_ASK_SEND_AUDIO_END);
        //    audioEnd.PackEnd();
        //    m_GameNet.Send(ref audioEnd);

        //    //             string result2 = string.Empty;
        //    //             for (int i = 0; i < data.Length; i++)
        //    //             {
        //    //                 result2 += System.Convert.ToString(data[i], 16) + " ";
        //    //             }
        //    //             print(result2);
        //}
        //if (GUILayout.Button("Play"))
        //{
        //    MicroPhoneInput.getInstance().PlayRecord();
        //}
        //if (GUILayout.Button("PlayByData"))
        //{
        //    byte[] data = MicroPhoneInput.getInstance().GetClipData();
        //    short[] decodeData = new short[data.Length / 2];
        //    for (int i = 0; i < decodeData.Length; ++i)
        //    {
        //        decodeData[i] = System.BitConverter.ToInt16(data, i * 2);
        //    }
        //    MicroPhoneInput.getInstance().PlayClipData(decodeData);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (m_bReady)
        {
            LanSocket.MsgUnPack msg = null;
            m_GameNet.GetMsg(ref msg);
            if (null != msg)
            {
                print("here have one msg on client");
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                LanSocket.MsgPack sendMsg = new LanSocket.MsgPack();
                sendMsg.SetHead((int)NetMsgID.C2S_SELECT_ANIMAL);
                sendMsg.Pack16bit(1);
                sendMsg.PackEnd();
                m_GameNet.Send(ref sendMsg);
                print("send 1");
            }
        }
        else
        {
            m_GameServerIP = m_Reciver.GetIP();
            if ("".Equals(m_GameServerIP))
            {
                m_BroadTime -= Time.deltaTime;
                if (m_BroadTime - Time.deltaTime < 0.0f)
                {
                    m_BroadTime = 5.0f;
                    m_Sender.Send();
                }
            }
            else
            {
                print("get broadcast ip:" + m_GameServerIP);
                GameStart();
            }
        }
    }
    void OnDestroy()
    {
        m_GameNet.Destroy();
        if (null != m_Reciver)
        {
            m_Reciver.Destroy();
        }
        if (null != m_Sender)
        {
            m_Sender.Destroy();
        }
    }

    void GameStart()
    {
        m_bReady = true;
        m_GameNet.Start(m_GameServerIP, 8888);
        try
        {
            m_Reciver.Destroy();
            m_Sender.Destroy();
        }
        catch (System.Exception ex)
        {
            MonoBehaviour.print("GameStart catch:" + ex.Message);
        }
        m_Reciver = null;
        m_Reciver = null;
    }

    void Action_S2C_SEND_ANIMAL_DATA(LanSocket.MsgUnPack msg)
    {
    }
}
