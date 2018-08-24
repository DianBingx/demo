using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Craps : MonoBehaviour
{
    public List<Sprite> shaiziData = new List<Sprite>();
    [Tooltip("筛子随机停止的区域")]
    public Vector3 MinPos;
    public Vector3 MaxPos;

    private void Start()
    {
        Vector3 sendPos = new Vector3(-15, -6, 0);
        Vector3 endPos = new Vector3(UnityEngine.Random.Range(MinPos.x, MaxPos.x), UnityEngine.Random.Range(MinPos.y, MaxPos.y), 0);
        PlayAddFriendAnim(sendPos, endPos, 4, 2);

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sendPos"> 玩家头像位置</param>
    /// <param name="endP">筛子停止位置</param>
    /// <param name="i">筛子大小</param>
    /// <param name="radian">弧度</param>
    /// <param name="time">时间</param>
    /// <returns></returns>
    public float PlayAddFriendAnim(Vector3 sendPos, Vector3 endP,int i,int radian = 0, float time = 1.5f)
    {
        GameObject shaizi = this.transform.Find("shaizi_anmi4_0").gameObject;
        shaizi.GetComponent<Animator>().enabled = true;

        SpriteRenderer _shaizhiV = shaizi.GetComponent<SpriteRenderer>();
        float fTime = PlaySendFlyAnim(shaizi, sendPos, endP, radian, time);

        DOTween.Sequence().AppendInterval(fTime).AppendCallback(() =>
        {
            if (shaizi != null)
            {
                shaizi.GetComponent<Animator>().enabled = false;
                _shaizhiV.sprite = shaiziData[i-1];
                StartCoroutine(Destroyshaizi(shaizi));
            }
        });

        return fTime;
    }


    IEnumerator Destroyshaizi(GameObject shaizi)
    {
        yield return new WaitForSeconds(2);
        Destroy(shaizi);
    }

    /**
         * @brief   播放道具飞行动画
         *
         * @param   prop    道具
         * @param   sendPos 起始点
         * @param   endP    终点
         * @param   fHeight 贝塞尔曲线中间点的高度(控制曲线)
         *
         * @return  飞行动画时间
         */
    public float PlaySendFlyAnim(GameObject prop, Vector3 sendPos, Vector3 endP, float fHeight, float time)
    {
        float fTime = 0.0f;
        if (prop == null)
        {
            return fTime;
        }
        
        Vector3 startP = sendPos;
        prop.transform.position = startP;

        float x = Mathf.Min(startP.x, endP.x) + Mathf.Abs(startP.x - endP.x) / 2f;
        float y = Mathf.Min(startP.y, endP.y) + Mathf.Abs(startP.y - endP.y) / 2f;
        float z = startP.z;
        Vector3 midP = new Vector3(x, y, z);
        double length = Math.Sqrt(Math.Pow(sendPos.x - midP.x, 2) + Math.Pow(sendPos.y - midP.y, 2));
        //midP.x = fHeight / (float)length * (sendPos.x - midP.x) + midP.x;
        //midP.y = fHeight / (float)length * (sendPos.y - midP.y) + midP.y;
        int rangeRadomNum = UnityEngine.Random.Range(0, 2);
        if(rangeRadomNum == 1)
        {
            midP.x = fHeight / (float)length * (endP.x - midP.x) + midP.x;
            midP.y = endP.y;
        }
        else
        {
            midP.y = fHeight / (float)length * (endP.y - midP.y) + midP.y;
            midP.x = endP.x;
        }
        //fTime = 2.0f;
        fTime = time;
        List<Vector3> arrRecPos = new List<Vector3>();

        arrRecPos.Add(startP);
        arrRecPos.Add(midP);
        arrRecPos.Add(endP);
        prop.transform.DOPath(arrRecPos.ToArray(), fTime, PathType.CatmullRom, PathMode.Full3D).SetEase(Ease.Linear);

        return fTime;
    }
}