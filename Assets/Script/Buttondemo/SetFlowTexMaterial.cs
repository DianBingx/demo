using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetFlowTexMaterial : MonoBehaviour
{
    private float widthRate = 1;
    private float heightRate = 1;
    private float xOffsetRate = 0;
    private float yOffsetRate = 0;
    public Shader shader;
    public Color color = Color.yellow;
    public float power = 0.55f;
    public float speed = 0.5f;
    public float largeWidth = 0.003f;
    public float littleWidth = 0.0003f;
    public float length = 0.1f;
    public float skewRadio = 0.2f;//倾斜
    public float moveTime = 0;
    private MaskableGraphic maskableGraphic;
    //Material imageMat = null;
    void Awake()
    {
        maskableGraphic = GetComponent<MaskableGraphic>();
        if (maskableGraphic)
        {
            Image image = maskableGraphic as Image;
            if (image)
            {
                image.material = new Material(Shader.Find("Custom/Flowlight"));
                //imageMat = new Material(shader);
                widthRate = image.sprite.textureRect.width * 1.0f / image.sprite.texture.width;
                heightRate = image.sprite.textureRect.height * 1.0f / image.sprite.texture.height;
                xOffsetRate = (image.sprite.textureRect.xMin) * 1.0f / image.sprite.texture.width;
                yOffsetRate = (image.sprite.textureRect.yMin) * 1.0f / image.sprite.texture.height;
            }
        }
        //image.material = imageMat;
    }
    
    void Start()
    {
        SetShader();
    }
    void Update()
    {
        moveTime = Time.time;
        SetShader();
    }
    public void SetShader()
    {
        skewRadio = Mathf.Clamp(skewRadio, 0, 1);
        length = Mathf.Clamp(length, 0, 0.5f);
        //imageMat.SetColor("_FlowlightColor", color);
        maskableGraphic.material.SetColor("_FlowlightColor", color);
        maskableGraphic.material.SetFloat("_Power", power);
        maskableGraphic.material.SetFloat("_MoveSpeed", speed);
        maskableGraphic.material.SetFloat("_LargeWidth", largeWidth);
        maskableGraphic.material.SetFloat("_LittleWidth", littleWidth);
        maskableGraphic.material.SetFloat("_SkewRadio", skewRadio);
        maskableGraphic.material.SetFloat("_Lengthlitandlar", length);
        maskableGraphic.material.SetFloat("_MoveTime", moveTime);

        maskableGraphic.material.SetFloat("_WidthRate", widthRate);
        maskableGraphic.material.SetFloat("_HeightRate", heightRate);
        maskableGraphic.material.SetFloat("_XOffset", xOffsetRate);
        maskableGraphic.material.SetFloat("_YOffset", yOffsetRate);

    }
}
