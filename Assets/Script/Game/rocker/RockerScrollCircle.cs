using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RockerScrollCircle : ScrollRect
{
    [SerializeField]
    private GameObject player;

    public delegate void TouchDelegate(Vector2 value);
    public event TouchDelegate TouchEvent;
    // 半径
    private float _mRadius = 0f;
 
     // 距离
     private const float Dis = 0.5f;
    private float smoothing = 1;

    protected override void Start()
    {
        base.Start();
        _mRadius = (transform as RectTransform).sizeDelta.x * Dis;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);

        // 获取摇杆，根据锚点的位置。
        var contentPosition = content.anchoredPosition;

        // 判断摇杆的位置 是否大于 半径
         if (contentPosition.magnitude > _mRadius)
         {
             // 设置摇杆最远的位置
             contentPosition = contentPosition.normalized * _mRadius;
             SetContentAnchoredPosition(contentPosition);
         }
        // 最后 v2.x/y 就跟 Input中的 Horizontal Vertical 获取的值一样 
        var v2 = content.anchoredPosition.normalized;
        float v = v2.y / v2.x;

        Vector2 ve2 = new Vector2(v2.x, v2.y);

        if (TouchEvent != null)
        {
            TouchEvent(ve2);
        }

        //Vector3 dir = new Vector3(v2.x, 0, v2.y);
        ////旋转赋给物体
        //Quaternion qua = Quaternion.LookRotation(dir.normalized);//※  将Vector3类型转换四元数类型
        //player.transform.rotation = Quaternion.Lerp(player.transform.rotation, qua, Time.deltaTime * smoothing);//四元数的插值，实现平滑过渡

    }

}
