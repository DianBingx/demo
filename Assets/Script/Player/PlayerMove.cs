using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerMove : MonoBehaviour
{
    //[Header("摇杆转向")]
    [Tooltip("摇杆转向")]
    public RockerScrollCircle Controller;
    [Tooltip("摇杆攻击")]
    public GameObject Attack;
    [Tooltip("炮口")]
    public GameObject PlayerCannon;
    [Space(5)]
    [Tooltip("局内配重")]
    public float playerAdditionalWeight;
    [Tooltip("局内轻盈")]
    public float playerLightness;
    [Tooltip("局内充能")]
    public float playerCharge;

    [Tooltip("攻击间隔")]
    public float AttackInterval = 1f;
    [Tooltip("质量")]
    public float PlayerMass = 3f;

    private float prevPos = 0;

    private float IntervalTime = 0f;

    private Quaternion targetRotation;

    private Vector2 RotatePos = Vector2.zero;

    void Awake()
    {
        Controller.TouchEvent += cannon_Rotate;

        Button BtnAttack = Attack.GetComponent<Button>();
        if (BtnAttack != null)
        {
            BtnAttack.onClick.AddListener(Move);
        }
        PlayerCannon = this.transform.Find("PlayerCannon").gameObject;
        playerAdditionalWeight = Convert.ToSingle(Setting.setting.playerAdditionalWeight);
        playerLightness = Convert.ToSingle(Setting.setting.playerLightness);
        playerCharge = Convert.ToSingle(Setting.setting.playerCharge);
        
        RotatePos = new Vector2(1, 0);
    }

    private void Start()
    {
        AttackInterval = AttackInterval - playerCharge * 0.1f;
        PlayerMass = PlayerMass + playerLightness - playerAdditionalWeight;
    }

    public void cannon_Rotate(Vector2 value)
    {
        RotatePos = value;
        double angleOfLine = Math.Atan2((value.y), (value.x)) * 180 / Math.PI;
        targetRotation = Quaternion.Euler(0, 0, (float)angleOfLine);
        PlayerCannon.transform.rotation = Quaternion.Slerp(PlayerCannon.transform.rotation, targetRotation, Time.deltaTime * 10);
        
    }

    public void Update()
    {
        
    }

    private void Move()
    {
        if (Time.time - IntervalTime >= AttackInterval)
        {
            
            Vector3 vPot = new Vector3(RotatePos.x, RotatePos.y, 0);
            vPot = vPot.normalized;
            Vector3 goalPot = new Vector3(this.transform.position.x - vPot.x * PlayerMass, this.transform.position.y - vPot.y * PlayerMass, this.transform.position.z + vPot.z);

            Tweener tweener = this.transform.DOMove(goalPot, 1f); 
            tweener.SetUpdate(true);
            tweener.SetEase(Ease.OutQuint);

            AttackInterval = Time.time;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag== "wall")
        {

        }
        else if (other.tag == "obstacle")
        {

        }
        else if (other.tag == "boss")
        {

        }
        else if (other.tag == "Next")
        {

        }
    }

    public void GameOver()
    {
        this.gameObject.SetActive(false);

    }

    void OnDestroy()
    {
        Controller.TouchEvent -= cannon_Rotate;
    }
}
