using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class swirl : MonoBehaviour {

    [Tooltip("旋转速度")]
    public float time = 0.2f;
    [Tooltip("旋转周期")]
    public float periodTime = 2f;

    float angle = 0;
    float radius = 0.1f;
    Material mat;
    void Start()
    {
        mat = this.GetComponent<Image>().material;

        //延迟2秒开始，每隔0.2s调用一次
        InvokeRepeating("DoSwirl", 1f, time);
    }

    void DoSwirl()
    {
        angle += 0.25f;
        radius += 0.025f;

        mat.SetFloat("_Angle", angle);
        mat.SetFloat("_Radius", radius);

        //rest
        if (radius >= periodTime)
        {
            angle = 0;
            radius = 0.1f;
        }
    }

}
