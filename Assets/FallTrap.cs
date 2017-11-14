using UnityEngine;
using System.Collections;

public class FallTrap : MonoBehaviour {

	public Transform trap;
	public Transform blood;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter2D(Collision2D coll) {
		makeItFall(coll.gameObject.transform);
	}


	public void makeItFall(Transform whoActivated) {

		if (whoActivated.tag == "Player") {
			trap.GetComponent<Rigidbody2D>().isKinematic = false;
			Transform.Instantiate(blood, whoActivated.transform.position, Quaternion.identity);
			GameObject.Destroy (whoActivated.gameObject);
		}
	}

}
