using UnityEngine;
using System.Collections;

public class Bouncer : MonoBehaviour {

    private float x;
	private float y;
    public float limit = 5;
	public Vector3 speed = new Vector3(0.3f,0,0);
    bool up = true;
	// Use this for initialization
	void Start () {
        x = transform.position.x;
		y = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
	    if(up)
        {
			transform.position += speed;
            if(transform.position.x > x+limit || transform.position.y > y+limit)
            {
				transform.position -= speed;
                up = false;
            }
        } else
        {
			transform.position -= speed;
            if (transform.position.x < x - limit || transform.position.y < y - limit)
            {
				transform.position += speed;
                up = true;
            }
        }
	}
}
