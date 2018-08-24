using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManage : MonoBehaviour {

    [Tooltip("生成的预制Cube")]
    public GameObject Cube;

	// Use this for initialization
	void Start () {
        prefabCube();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void prefabCube()
    {
        for(int i = 0; i < 100; i++)
        {
            for(int n = 0; n < 100; n++)
            {
                int z = (i + n) % 2;
                if (z == 1)
                {
                    GameObject gg = Instantiate(Cube);
                    gg.transform.position = new Vector3(i, 0, n);
                    gg.transform.SetParent(this.transform);
                }
            }
        }
    }

}
