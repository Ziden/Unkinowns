using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class Hud : MonoBehaviour {

	private float timeGlow = 0;
	public static Transform selected = null;
	public static Hud instance = null;
	public int lemmingsCount = 3;
	public int summoned = 0;
	public int homed = 0;
	public static int loadedLevel = 0;
	public Transform [] buildable;
   
	public GameObject [] buildButtons;
	public static bool hasPlacedBlock = false;
	public static int tutorialStep = 0;
	public static float yPosition = Screen.height / 12;
	public int [] qty;
	private int [] startQty;
    public GameObject playButton;
	private Canvas canvas;
	public static List<Transform> built = new List<Transform>();
	private List<Vector3> inicialPositions = new List<Vector3>();
	private List<Quaternion> inicialRotations = new List<Quaternion>();
    private static Material defaultMat;

	//	private Vector3 cameraVelocity = Vector3.zero;
	// Use this for initialization
	void Start () {

        GameObject[] builds = GameObject.FindGameObjectsWithTag("Resetable");
        foreach (GameObject build in builds)
        {
            Transform box = build.transform.Find("Box");
            if(box!=null && box.GetComponent<SpriteRenderer>()!=null)
                box.GetComponent<SpriteRenderer>().enabled = false;
        }

        instance = this;
	    selected = null;
		loadedLevel = 0;
		int tutorialStep = 0;
		bool hasPlacedBlock = false;
		yPosition = Screen.height / 12;
		playButton = GameObject.Find("PlayBtn");
		canvas = GameObject.Find ("Canvas").GetComponent<Canvas>();
		startQty = (int[])qty.Clone();
		built = new List<Transform>();
		defaultMat = new Material(Shader.Find("Sprites/Default"));
		loadedLevel = getLevel ();
		Debug.Log ("LOADED LEVEL "+loadedLevel);
		for (int x = 0; x < buildButtons.Length; x++) {
			int indice = x;
			
			float iniX = bX;
			
			for (int qt = 0; qt < startQty[indice]; qt++)
			{
				GameObject button = (GameObject)Instantiate(buildButtons[x], Vector3.zero, Quaternion.identity);
				button.transform.parent = canvas.transform;
				button.GetComponent<RectTransform>().anchoredPosition = new Vector2(bX, bY);
				bX += 2;
				button.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
				button.GetComponent<Button>().onClick.AddListener(() =>
				                                                  {
					
					if (Spawn.spawning)
						return;
					if (getLevel() == 1 && TutoHand.hand.getStep() == 0)
					{
						TutoHand.hand.advance();
					}
					
					Transform t = buildable[indice];
					Transform generated = (Transform)Transform.Instantiate(t, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0), Quaternion.identity);
					generated.SetParent(GameObject.Find("LevelBase").transform);
					generated.position = new Vector3(generated.position.x, generated.position.y, 10);
					hasPlacedBlock = true;
					
					select(generated);
					//qty[indice]--;
					//if (qty[indice] == 0)
					//{
					Destroy(button);
					//}
				});
			}
			bX += 60;
		}
	}

    public static void clearSelected()
    {
        //if (Hud.selected.GetComponent<Renderer>() != null && Hud.selected.GetComponent<Renderer>().material.color != Color.white)
        //{
        //    Hud.selected.GetComponent<Renderer>().material.color = Color.white;
        //}

		if (TutoHand.hand.getStep() == 2)
		 {
			return;
		}
        Hud.selected.Find("Box").GetComponent<SpriteRenderer>().material = null;
        Hud.selected.Find("Box").GetComponent<SpriteRenderer>().enabled = false;
        if (!Hud.built.Contains(Hud.selected))
            Hud.built.Add(Hud.selected);
        Hud.selected = null;
        Debug.Log("CLEAR SELECTED");
    }

    public static void select(Transform t)
    {
        if(Spawn.spawning)
        {
            return;
        }
        Hud.selected = t;
        Hud.selected.Find("Box").GetComponent<SpriteRenderer>().material = defaultMat;
        Hud.selected.Find("Box").GetComponent<SpriteRenderer>().enabled = true;

    }

	private float bX = 38;
	private float bY = -33;



    

	public void Advance() {
		Stop ();
		int lvl = getLevel ();
		lvl++;
		Application.LoadLevel ("Level"+lvl); 
	}

	public static int getLevel() {
		string sceneName = Application.loadedLevelName;
		if(sceneName.Contains ("Level")) {
			int lvlNumber = Int32.Parse(sceneName.Replace ("Level", ""));
			return lvlNumber;
		}
		return 0;
	}

	private bool mouseDown = false;

	private RaycastHit hit;

	// Update is called once per frame
	void Update () {

		Vector3 inputPosition = Input.mousePosition; 

		Vector3 rayc = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - inputPosition.x, Screen.height - inputPosition.y, -10));

		// rotating the object on PC
		if(Input.GetAxis("Mouse ScrollWheel") < 0) {
			if(selected!=null) {
				selected.Rotate(new Vector3(0,0,15));
			} 
			if(loadedLevel==1 && tutorialStep==4) {
				tutorialStep++;
			}
		} else if(Input.GetAxis("Mouse ScrollWheel") > 0) {
				if(selected!=null) {
					selected.Rotate(new Vector3(0,0,-15));
			}
		}
	}

    private bool play = false;

    public void clickPlay()
    {
        if(!play)
        {
            if(TutoHand.hand.getStep()==3)
            {
                TutoHand.hand.advance();
            }
            var colors = playButton.GetComponent<Button>().colors;
            colors.normalColor = Color.red;
            colors.pressedColor = Color.red;
            colors.highlightedColor = Color.red;
            GameObject.Find("PlayText").GetComponent<Text>().text = "Stop";
            playButton.GetComponent<Button>().colors = colors;
            play = true;
            Play();
        }
        else
        {
			if(TutoHand.hand.getStep()==5)
			{
				TutoHand.hand.advance();
			}
            var colors = playButton.GetComponent<Button>().colors;
            colors.normalColor = Color.green;
            colors.pressedColor = Color.green;
            colors.highlightedColor = Color.green;
            GameObject.Find("PlayText").GetComponent<Text>().text = "Play";
            playButton.GetComponent<Button>().colors = colors;
            play = false;
            Stop();
        }
    }

    private Transform reSelect = null;

    void Play() {

	

        if (Hud.selected != null)
        {
            reSelect = Hud.selected;
        }

		inicialPositions = new List<Vector3>();
		inicialRotations = new List<Quaternion>();
		foreach(Transform t in built) {
			inicialPositions.Add (t.position);
			inicialRotations.Add (t.rotation);
		}

		Spawn.spawning = true;
		if(selected!=null) {
			if(selected.GetComponent<Renderer>().material.color != Color.white) {
				selected.GetComponent<Renderer>().material.color = Color.white;
			}
		}


		selected = null;

        GameObject[] buildable = GameObject.FindGameObjectsWithTag("Buildable");
        foreach (GameObject r in buildable)
        {
            if(r.GetComponent<Cannon>()!=null)
            {
                r.GetComponent<Cannon>().resetTime();
            }
        }
        GameObject [] resetable = GameObject.FindGameObjectsWithTag("Resetable");
		foreach(GameObject r in resetable) {

            if (r.GetComponent<Cannon>() != null)
            {
                r.GetComponent<Cannon>().resetTime();
            }

            built.Add(r.transform);
			inicialPositions.Add (r.transform.position);
			inicialRotations.Add (r.transform.rotation);
		}
		GameObject [] r2 = GameObject.FindGameObjectsWithTag("ResetableKinetic");
		foreach(GameObject r in r2) {
			built.Add(r.transform);
			inicialPositions.Add (r.transform.position);
			inicialRotations.Add (r.transform.rotation);
		}
		foreach(Transform t in built) {
			if(t.gameObject.tag != "ResetableKinetic")
				t.GetComponent<Rigidbody2D>().isKinematic = false;
		}


	}
	

	void Stop() {

		if(loadedLevel==1 && tutorialStep==1) {
			tutorialStep++;
		}

		Spawn.spawning = false;
		Spawn.spawnTimer = 0f;
		GameObject [] lemmings = GameObject.FindGameObjectsWithTag("Player");
		foreach(GameObject lemming in lemmings) {
			GameObject.Destroy(lemming);
		}
		GameObject [] r = GameObject.FindGameObjectsWithTag("Removable");
		foreach(GameObject rr in r) {
			GameObject.Destroy(rr);
		}

		for(int x = 0 ; x < built.Count ; x++) {
						
			Transform inMap = built[x];
			Vector3 inicialPos = inicialPositions[x];
			Quaternion inicialRot = inicialRotations[x];
			inMap.position = inicialPos;
			inMap.rotation = inicialRot;

			inMap.GetComponent<Rigidbody2D> ().WakeUp ();
			inMap.GetComponent<Rigidbody2D> ().velocity = Vector3.zero;
			inMap.GetComponent<Rigidbody2D> ().angularVelocity = 0f;
			inMap.GetComponent<Rigidbody2D>().isKinematic = true;
			
		}
		foreach(GameObject o in GameObject.FindGameObjectsWithTag("Resetable")) {
			int index = built.IndexOf (o.transform);
			inicialPositions.RemoveAt(index);
			inicialRotations.RemoveAt (index);
			built.Remove (o.transform);
		}
		foreach(GameObject o in GameObject.FindGameObjectsWithTag("ResetableKinetic")) {
			int index = built.IndexOf (o.transform);
			inicialPositions.RemoveAt(index);
			inicialRotations.RemoveAt (index);
			built.Remove (o.transform);
		}

		//qty = (int[])startQty.Clone();
		
		//built = new List<Transform>();
		summoned = 0;
		homed = 0;

        if (reSelect != null)
        {
            Hud.select(reSelect);
        } else
        {
            selected = null;
        }
    }

	/*
	void OnGUI() {

		GUI.skin = hudSkin;

		// Tutorial Box
		if(loadedLevel==1 && tutorialStep==0) {
			GUI.Label(new Rect (Screen.width/2, Screen.height/10, 500, 100),"Click PLAY to let things flow !",styleTutorial);
		} else if(loadedLevel==1 && tutorialStep==1) {
			GUI.Label(new Rect (Screen.width/2, Screen.height/10, 100, 100),"Click STOP to reset the world !",styleTutorial);
		} else if(loadedLevel==1 && tutorialStep==2) {
			GUI.Label(new Rect (20, Screen.height/10, 100, 100),"You have 1x plank, you can use it !",styleTutorial);
		} else if(loadedLevel==1 && tutorialStep==3) {
			GUI.Label(new Rect (Screen.width/3, Screen.height/8, 100, 100),"Drag your plank to move it !",styleTutorial);
		} else if(loadedLevel==1 && tutorialStep==4) {
			GUI.Label(new Rect (Screen.width/3, Screen.height/8, 100, 100),"Use the white wheel to rotate it !",styleTutorial);
		}else if(loadedLevel==1 && tutorialStep==5) {
			GUI.Label(new Rect (Screen.width/5, Screen.height/8, 100, 500),"Drag the screen to move it ! !",styleTutorial);
		}

	    yPosition = Screen.height / 12;
		GUI.Box(new Rect(0,0,Screen.width,Screen.height/12), GUIContent.none);
		GUI.color = Color.red;

		GUI.Label (new Rect(10,yPosition,80,80), "Fellas: "+lemmingsCount, commonStyle);
		GUI.color = Color.magenta;
		GUI.Label (new Rect(120-20,yPosition,100,80), "Summoned: "+summoned, commonStyle);
		GUI.color = Color.green;
		GUI.Label (new Rect(250-20,yPosition,80,80), "Safe: "+homed, commonStyle);
		GUI.color = Color.yellow;
		if(!Spawn.spawning && selected==null) {
			int x = 10;
			int index = 0;
			if(!Spawn.spawning) {
				foreach(Transform t in buildable) {
					int qtd = qty[index];
					if(qtd!=0) {
						
						if (GUI.RepeatButton (new Rect (x, 10, Screen.width/10, Screen.height/15), t.name+" x"+qtd)) {
							float xx = (Camera.main.transform.position.x-6.5f);
							float yy = (Camera.main.transform.position.y-6.5f);
							Transform generated = (Transform)Transform.Instantiate(t, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y,10), Quaternion.identity);
							placed = true;
							selected = generated;		
							qty[index]--;

							if(loadedLevel==1 && tutorialStep==2) {
								tutorialStep++;
							}
						}
						//GUI.DrawTexture(new Rect (x, 10, 100, 30), r.sprite.texture);
						x+= Screen.width/10 + 5;
					}
					index++;
				}
			}

		}


		if(selected==null) {
			if(!Spawn.spawning) {
				GUI.color = Color.green;
				if (GUI.Button (new Rect (Screen.width-Screen.width/10-5, 10, Screen.width/10, Screen.height/15), "Play")) {
					Play ();
				}
			} else {
				GUI.color = Color.magenta;
				if (GUI.Button (new Rect (Screen.width-Screen.width/10-5, 10, Screen.width/10, Screen.height/15), "Stop")) {
					Stop ();
				}
			}
		} else {

		}

	}
*/


}
