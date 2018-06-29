using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager cur;
    public int cycleTime = 60;
    private float fixedTime = 0;
    void Awake()
    {
        if (cur) Destroy(cur.gameObject);
        cur = this;
    }
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep; //永不休眠
        Application.targetFrameRate = 30; //帧频设置为30帧
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        
    }

    public void OnDestroy()
    {
        
    }
}