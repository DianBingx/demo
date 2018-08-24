using UnityEngine;
using System.Collections;

enum NetMsgID
{
    NET_MSG_START = 100,
    S2C_SEND_ANIMAL_DATA,
    C2S_SELECT_ANIMAL,
    C2S_ASK_SEND_AUDIO_BEGIN,
    C2S_ASK_SEND_AUDIO,
    C2S_ASK_SEND_AUDIO_END,

    NET_MSG_END,
}
