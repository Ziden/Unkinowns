using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lemming : MonoBehaviour {

	public bool isLemming = true;
	public Vector2 force = new Vector2(2.8f,0);
	public bool onlyForceOnce = false;
	private bool completeSpawn = false;
	private bool move = true;
	private bool standing = false;

    private static Object[] faces = null;
    private static Object[] frames = null;

	private bool hasUsedTheForce = false;

	// Use this for initialization
	void Start () {
		if (isLemming) {
		
        int random = Random.Range(1, 9);
        //GetComponent<SpriteRenderer>().sprite = sprites[random];
        if(faces == null)
            faces = Resources.LoadAll("Sprite/Faces");
        Debug.Log(faces.Length);
       
        
        if(frames==null)
            frames = Resources.LoadAll("Sprite/Smiley");
        Debug.Log("_________"+frames.Length);
        transform.Find("Face").GetComponent<SpriteRenderer>().sprite = (Sprite)faces[random];
		}
        //GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/Smiley/Smiley_1");
    }

	void StandUp() {
		this.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,120));
		standing = true;
		hasUsedTheForce = false;
	}

	void Update() {

	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (isLemming) {
						if (GetComponent<Rigidbody2D> ().velocity.y < 0) {
								GetComponent<SpriteRenderer> ().sprite = (Sprite)frames [3];
						} else if (GetComponent<Rigidbody2D> ().velocity.y > 0) {
								GetComponent<SpriteRenderer> ().sprite = (Sprite)frames [2];
						} else {
								GetComponent<SpriteRenderer> ().sprite = (Sprite)frames [1];
						}
				}
		if(move && completeSpawn && this.GetComponent<Rigidbody2D>().transform.rotation.eulerAngles.z > 85 && this.GetComponent<Rigidbody2D>().transform.rotation.eulerAngles.z < 285) {
			move = false;
			Invoke ("StandUp", 2);
		}
		else if(standing) {
			this.GetComponent<Rigidbody2D>().transform.rotation = Quaternion.Lerp(this.GetComponent<Rigidbody2D>().transform.rotation, Quaternion.identity, 8f * Time.deltaTime);
			if(this.GetComponent<Rigidbody2D>().transform.rotation.eulerAngles.z < 1 || this.GetComponent<Rigidbody2D>().transform.rotation.z > 359) {
				standing = false;
				move = true;
				hasUsedTheForce = false;
				this.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,100));
			}
		}
		if(!completeSpawn && isLemming) return;
		if(!hasUsedTheForce)
			if(this.GetComponent<Rigidbody2D>().velocity.x<3 && move) {
				this.GetComponent<Rigidbody2D>().AddForce(force);
                if (onlyForceOnce)
			        hasUsedTheForce = true;
		}
		if(transform.position.y < -10)
			GameObject.Destroy (this);
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if(!isLemming) return;

		if(move) {

			foreach(ContactPoint2D contact in coll.contacts) {
				Vector2 differencePoint = new Vector2(contact.point.x-this.transform.position.x, contact.point.y-this.transform.position.y);
				// collided with floor
				if(differencePoint.y < -0.1f) {
					this.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 100));
					completeSpawn = true;
				}
				break;
				//this.rigidbody2D.AddTorque(20);
				//Debug.Log ("DIFF X"+(contact.point.x-this.transform.position.x)+" Y"+(contact.point.y-this.transform.position.y));
				//this.rigidbody2D.AddForce (new Vector2(0, 100));
			}
		}


		
		if(coll.gameObject.name == "Door") {
			GameObject.Destroy (this.gameObject);
			Hud.instance.homed += 1;
			if(Hud.instance.homed==Hud.instance.lemmingsCount){
				Hud.instance.Advance();
			}
		}
	}
}
