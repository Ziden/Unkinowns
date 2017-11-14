using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour {

	public static bool spawning = false;

	public Transform lemming;
	public float spawnTime = 0.2f;

	public static float spawnTimer = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(spawning) {
			if(Time.time > spawnTimer) {
				Summon ();
				spawnTimer = Time.time + spawnTime;
			}
		}
	}

	void Summon() {
		if(Hud.instance!=null) {
			if(Hud.instance.summoned==Hud.instance.lemmingsCount) return;
			Hud.instance.summoned++;
		}

		Vector3 spawnPoint = new Vector3(this.transform.position.x,this.transform.position.y-0.5f,this.transform.position.z);
		Transform l = (Transform)Transform.Instantiate(lemming, spawnPoint, this.transform.rotation);
		float r = Random.value;
		float g = Random.value;
		float b = Random.value;
        l.transform.Find("Face").GetComponent<Renderer>().material.color = new Color(r, g, b);
        l.GetComponent<Renderer>().material.color = new Color(r, g, b);

	}
}
