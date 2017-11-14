using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class Cannon : MonoBehaviour {

	public float shootTime = 0;
	public float shootDelay = 1;
	public bool right;
	public GameObject cannonBall;
	List<GameObject> balls = new List<GameObject>();
    bool shooting = false;
    Rigidbody2D body;

	void Start () {
        body = GetComponent<Rigidbody2D>();
        if(this.tag=="Resetable")
        {
            shooting = true;
        }
        Debug.Log(this.tag);
    }

    public void resetTime()
    {
        shootTime = 0;
        if (this.tag != "Resetable")
            shooting = false;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if ((coll.relativeVelocity.magnitude > 0.5))
            shooting = true;

        Debug.Log("MAGNI" +coll.relativeVelocity.magnitude);

    }

        void Update () {
        if (!shooting)
            return;
		if(!Spawn.spawning) return;
		if(Time.time > shootTime) {
			Shoot ();
			shootTime = Time.time + shootDelay;
		}
	}

	void Shoot() {
		GameObject ball = (GameObject)Transform.Instantiate(cannonBall, this.transform.position, this.transform.rotation);
		balls.Add (ball);
		if (balls.Count > 3) {
			Destroy(balls[0],1);
			balls.RemoveAt(0);
		}
	}

}
