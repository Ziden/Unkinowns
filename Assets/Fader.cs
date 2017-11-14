using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Fader : MonoBehaviour {

	float tempo;
	float inicial = 0;
	bool mudou = false;
	
	// Use this for initialization
	void Start () {

	}

	void Tira() {
		transform.position = new Vector3 (transform.position.x, transform.position.y+900, transform.position.z);
	}

	static string cena = null;


	public static void scene(string nome) {
		GameObject.Find ("Fader").GetComponent<Fader> ().mudaScene (nome);
	}

	void mudaScene(string nome) {
		cena = nome;
		transform.position = new Vector3 (transform.position.x, transform.position.y-900, transform.position.z);
		GetComponent<Image> ().CrossFadeAlpha (1f, 0.9f, false);
		Invoke("go",1);
		
	}

	void go() {
		Application.LoadLevel (cena);
		cena = null;
	}
	
	// Update is called once per frame
	void Update () {
		tempo = Time.time - inicial;
		if (!mudou && tempo > 0) {
			GetComponent<Image> ().CrossFadeAlpha (0f, 0.9f, false);
			Invoke ("Tira", 1);
			mudou = true;
		}
	}
}