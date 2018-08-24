using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour {

    private float fps = 10.0f;
    private float time;
    [Tooltip("一组动画的贴图")]
    public Texture2D[] animations;
    [Tooltip("进度条")]
    public Slider slider;

    private int nowFram;
    private float curProgressVaule = 0;//计数器
    //异步对象
    AsyncOperation async;

    //读取场景的进度，它的取值范围在0 - 1 之间。
    int progress = 0;

    void Start()
    {
        //在这里开启一个异步任务，
        //进入loadScene方法。
        StartCoroutine(loadScene());
    }

    //注意这里返回值一定是 IEnumerator
    IEnumerator loadScene()
    {
        //异步读取场景。
        //Globe.loadName 就是A场景中需要读取的C场景名称。LoadSceneAsync
        async = SceneManager.LoadSceneAsync(GameManager.loadName);
        async.allowSceneActivation = false;//当game场景加载到90%时，不让它直接跳转到game场景。
        //读取完毕后返回， 系统会自动进入C场景
        yield return async;

    }

    void OnGUI()
    {
        //因为在异步读取场景，
        //所以这里我们可以刷新UI
        DrawAnimation(animations);

    }

    void Update()
    {

        //在这里计算读取的进度，
        //progress 的取值范围在0.1 - 1之间， 但是它不会等于1
        //也就是说progress可能是0.9的时候就直接进入新场景了
        //所以在写进度条的时候需要注意一下。
        //为了计算百分比 所以直接乘以100即可
        ///progress = (int)(async.progress * 100);
        float progressVaule = 0;

        if (async.progress < 0.9f)
        {
            progressVaule = async.progress ;
            curProgressVaule = progressVaule;
        }
        else
        {
            progressVaule = 1;
        }

        if (curProgressVaule < progressVaule)
        {
            curProgressVaule += 0.1f;
        }

        if (curProgressVaule > 1f)
        {
            async.allowSceneActivation = true;
        }

        slider.value = curProgressVaule;

        //有了读取进度的数值，大家可以自行制作进度条啦。
        Debug.Log(curProgressVaule + "进度" + async.progress);

    }
    //这是一个简单绘制2D动画的方法，没什么好说的。
    void DrawAnimation(Texture2D[] tex)
    {

        time += Time.deltaTime;

        if (time >= 1.0 / fps)
        {

            nowFram++;

            time = 0;

            if (nowFram >= tex.Length)
            {
                nowFram = 0;
            }
        }
        GUI.DrawTexture(new Rect(100, 100, 40, 60), tex[nowFram]);

        //在这里显示读取的进度。
        GUI.Label(new Rect(100, 180, 300, 60), "lOADING!!!!!" + progress);

    }
}
