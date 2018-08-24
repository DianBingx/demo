//第一个摄像头用于显示Canvas上，
//这个脚本挂在第二个摄像头上，仅仅作用于游戏中的“Sprite Renderer”精灵上，

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeCameraMove : MonoBehaviour {

    public float mimMapX;
    public float maxMapX;

    public float mimMapY;
    public float maxMapY;

    float sensitivityY = 1;

    float minimumY = -80;
    float maximumY = 80;

    private float rotationX = 0;
    private float rotationY = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0))
        {      
            rotationX +=Input.GetAxis("Mouse X");
            rotationX = Mathf.Clamp(rotationX, minimumY, maximumY);
            rotationY += Input.GetAxis("Mouse Y");
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            if (mimMapX > rotationX)
            {
                rotationX = mimMapX;
            }
            if (maxMapX < rotationX)
            {
                rotationX = maxMapX;
            }
            if (mimMapY > rotationY)
            {
                rotationY = mimMapY;
            }
            if (maxMapY < rotationY)
            {
                rotationY = maxMapY;
            }

            transform.position = new Vector3(rotationX, rotationY, -10);    
        }
    }
}
