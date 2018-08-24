using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameBoss : MonoBehaviour {

    [Tooltip("玩家的位置")]
    public GameObject playerPot;

    [Tooltip("扇形子弹")]
    public GameObject Bullet;
    //扇形子弹 List
    private List<GameObject> bulletList = new List<GameObject>();

    [Tooltip("BOSS Hp")]
    public int Hp = 100;

    // Use this for initialization
    void Start () {
        //sectorBullet();
        CommonBullet();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public GameObject getCanUsedBullet()
    {
        foreach (GameObject obj in bulletList)
        {
            if (obj.activeSelf == false)
            {
                return obj;
            }
        }

        GameObject bullet = GameObject.Instantiate(Bullet);
        bullet.SetActive(false);
        bullet.AddComponent<BossBulletVector>();
        bulletList.Add(bullet);
        return bullet;
    }

    //扇形子弹方法
    private void sectorBullet()
    {
        float bulletWaveNum = 0;
        Vector3 oriVec = playerPot.transform.position - this.transform.position;
        oriVec = oriVec.normalized;
        for (int n = 0; n < 7; n++)
        {
            StartCoroutine(CreateAllChip(oriVec, bulletWaveNum));
            bulletWaveNum += 2;
        }
        bulletWaveNum = 0;
    }
    
    IEnumerator CreateAllChip(Vector3 oriVec, float waittime)
    {
        
        yield return new WaitForSeconds(waittime);
        for (int i = 1; i < 10; i++)
        {
            GameObject bulets = getCanUsedBullet();
            bulets.transform.SetParent(this.transform);
            bulets.transform.localPosition = Vector3.zero;
            bulets.transform.rotation=new Quaternion(0,0,0,0);
            Vector3 bulletPot = BulletVector(i, oriVec);
            double angleOfLine = Math.Atan2((bulletPot.y), (bulletPot.x)) * 180 / Math.PI;
            bulets.transform.Rotate(new Vector3(0, 0, (float)angleOfLine));
            bulets.SetActive(true);
        }
    }

    private Vector3 BulletVector(float angle, Vector3 oriVec)
    {
        Vector3 axis = Vector3.forward;
        int i = (int)angle % 2;
        int n = (int)angle / 2;
        if (i == 0)
        {
            Vector3 newVec = Quaternion.AngleAxis(n * 10, axis) * oriVec;
            return newVec;
        }
        else
        {
            Vector3 newVec = Quaternion.AngleAxis(n * -10, axis) * oriVec;
            return newVec;
        }
    }

    //Invoke(string method,int Secondtimes) 过Secondtimes 秒后触发method 函数

    //普通子弹
    private void CommonBullet()
    {
        float bulletWaveNum = 0;
        
        for (int n = 0; n < 7; n++)
        {
            StartCoroutine(CommonCreateAllChip(bulletWaveNum));
            bulletWaveNum += 2;
        }
        bulletWaveNum = 0;
    }

    IEnumerator CommonCreateAllChip(float waittime)
    {

        yield return new WaitForSeconds(waittime);

        Vector3 oriVec = playerPot.transform.position - this.transform.position;
        oriVec = oriVec.normalized;

        GameObject bulets = getCanUsedBullet();
        bulets.transform.SetParent(this.transform);
        bulets.transform.localPosition = Vector3.zero;
        bulets.transform.rotation = new Quaternion(0, 0, 0, 0);
        double angleOfLine = Math.Atan2((oriVec.y), (oriVec.x)) * 180 / Math.PI;
        bulets.transform.Rotate(new Vector3(0, 0, (float)angleOfLine));   
        bulets.SetActive(true);

    }

}
