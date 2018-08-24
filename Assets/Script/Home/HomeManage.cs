using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeManage : MonoBehaviour {

    private GameObject startGame;
    private GameObject player;
    private GameObject attribute;

    private Text AdditionalWeightText;
    private Text LightnessText;
    private Text ChargeText;

    private Slider AdditionalWeight;
    private Slider Lightness;
    private Slider Charge;

    public int closeNum;

    // Use this for initialization
    void Start () {

        startGame = this.transform.Find("StartGame").gameObject;
        //startGame.SetActive(false);
        Button Btnstartgame = startGame.GetComponent<Button>();
        if (Btnstartgame != null)
        {
            Btnstartgame.onClick.AddListener(OnClickStaticGame);
        }
        
        player = this.transform.Find("player").gameObject;
        player.SetActive(false);
        attribute = player.transform.Find("attribute").gameObject;
        GameObject gg = attribute.transform.Find("AdditionalWeightText").gameObject;
        AdditionalWeightText = gg.transform.Find("Text").GetComponent<Text>();
        gg = attribute.transform.Find("AdditionalWeightText").gameObject;
        LightnessText = gg.transform.Find("Text").GetComponent<Text>();
        gg = attribute.transform.Find("AdditionalWeightText").gameObject;
        ChargeText = gg.transform.Find("Text").GetComponent<Text>();

        AdditionalWeight = attribute.transform.Find("AdditionalWeight").GetComponent<Slider>();
        Lightness = attribute.transform.Find("Lightness").GetComponent<Slider>();
        Charge = attribute.transform.Find("Charge").GetComponent<Slider>();

        initAttribute();
        
    }

    private void initAttribute()
    {
        AdditionalWeightText.text = "AdditionalWeight";
        LightnessText.text = "Lightness";
        ChargeText.text = "Charge";

        AdditionalWeight.minValue = 0;
        Lightness.minValue = 0;
        Charge.minValue = 0;

        AdditionalWeight.maxValue = Convert.ToSingle(Setting.setting.playerAdditionalWeight);
        Lightness.maxValue = Convert.ToSingle(Setting.setting.playerLightness);
        Charge.maxValue = Convert.ToSingle(Setting.setting.playerCharge);

        AdditionalWeight.value = Convert.ToSingle(Setting.setting.gameAdditionalWeight);
        Lightness.value = Convert.ToSingle(Setting.setting.gameLightness);
        Charge.value = Convert.ToSingle(Setting.setting.gameCharge);

    }

    public void RangNumber()
    {
        GameCommon.GameData(Setting.setting);
    }

    private void OnClickStaticGame()
    {
        RangNumber();
        GameManager.loadName = "Game";
        SceneManager.LoadScene("Loading");
    }


    // Update is called once per frame
    void Update () {
		
	}
}
