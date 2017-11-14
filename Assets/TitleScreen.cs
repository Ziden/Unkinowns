using UnityEngine;
using System.Collections;

public class TitleScreen : MonoBehaviour {

    public GameObject texto;
	private float time;
    private float passou = 0;
	public GUISkin skin;
	private bool show = false;
	// Use this for initialization
	void Start () {
        time = Time.time;
        Spawn.spawning = true;
        texto.active = false;
        
	}
	
	// Update is called once per frame
	void Update () {
        passou = (Time.time) - time;
        if (passou > 4)
        {
            texto.active = true;
        }

		if (Input.GetMouseButton (0) || Input.touchCount > 0)
        {
            Spawn.spawning = false;
            Fader.scene("Level1");
        }
						
	}
}
