using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassthoughEffect : MonoBehaviour {

    [Range(0, 0.15f)]

    public float distortFactor = 1.0f;

    //扭曲中心(0-1)屏幕空间，默认为中心点

    public Vector2 distortCenter = new Vector2(0.5f, 0.5f);

    public Material _Material;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (_Material)
        {
            _Material.SetFloat("_DistortFactor", distortFactor);
            _Material.SetVector("_DistortCenter", distortCenter);
            Graphics.Blit(source, destination, _Material);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }

}

