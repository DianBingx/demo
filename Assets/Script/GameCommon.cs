using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCommon{
    //保存游戏进程数据json
	
    public static void GameData(Setting data)
    {

        string path = Application.streamingAssetsPath + "/setting.json";

        string[] ss = {"{",
            "\"playerName\":\"" +data.playerName+"\",",
            "\"playerAdditionalWeight\":\"" +data.playerAdditionalWeight+"\",",
            "\"playerLightness\":\"" +data.playerLightness+"\",",
            "\"playerCharge\":\"" +data.playerCharge+"\",",
            "\"gamegrade\":\""+data.gamegrade+"\",",
            "\"gameAdditionalWeight\":\"" +data.gameAdditionalWeight+"\",",
            "\"gameLightness\":\"" +data.gameLightness+"\",",
            "\"gameCharge\":\"" +data.gameCharge+"\"" ,
            "}" };

        FileUtils.getInstance().setString(path, ss);
    }

    //
    public static void TheatricalProperty(int grade)
    {
        if (grade == 1)
        {

        }
        else if(grade == 2)
        {

        }
        else if (grade == 3)
        {

        }
    }


}
