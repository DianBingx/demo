using UnityEngine;

public class GameManager : MonoBehaviour
{

    static GameManager cur;

    public static string loadName;

    void Awake()
    {
        if (cur) Destroy(cur.gameObject);
        cur = this;
    }
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        
    }

    public void OnDestroy()
    {
        
    }
}