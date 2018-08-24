using System;
using UnityEngine;

[Serializable]
public class Setting
{
    static Setting mSetting;

    public string playerName;//玩家名字
    public string playerAdditionalWeight;//玩家配重
    public string playerLightness;//玩家轻盈
    public string playerCharge;//玩家充能

    public string gamegrade;//游戏等级
    public string gameAdditionalWeight;
    public string gameLightness;
    public string gameCharge;



    public static Setting setting
    {
        get
        {
            Debuger.isDebug = false;
            if (mSetting == null)
            {
                string str = FileUtils.getInstance().getString(Application.streamingAssetsPath + "/setting.json");
                if (!string.IsNullOrEmpty(str))
                {
                    mSetting = UnityEngine.JsonUtility.FromJson<Setting>(str);
                }
                else
                {
                    mSetting = new Setting();
                }
            }
            return mSetting;
        }
        set
        {           
            mSetting = value;
        }
    }


}
