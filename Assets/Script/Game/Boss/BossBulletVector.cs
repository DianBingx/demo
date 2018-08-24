using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBulletVector : MonoBehaviour {

    public Vector3 bullet_vector=Vector3.right;

    int speed = 1;
    private Rigidbody2D RigBullet;

	// Use this for initialization
	void Start () {
        RigBullet = this.GetComponent<Rigidbody2D>();

    }
	
	// Update is called once per frame
	void Update () {
        if (this.gameObject.activeSelf)
        {
            RigBullet.velocity = transform.TransformDirection(bullet_vector * speed);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "wall")
        {
            this.gameObject.SetActive(false);
        }
        else if (other.tag == "player")
        {

        }
    }
}
