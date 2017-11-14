using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {

	public static bool rot = false;

	// Use this for initialization
	void Start ()
	{
		//gameObject.active = false;
	}

    private Transform getClicked()
    {
        RaycastHit2D ht = Physics2D.Raycast(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0);
        if (ht != null && ht.transform != null)
        {
            return ht.transform;
        }
        return null;
    }

    void Update ()
	{

		if (Hud.selected != null) {
			transform.localScale = new Vector3(1, 1, 1);

			//transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y-2,10);
		} else {
			//gameObject.SetActive(false);
			transform.localScale = new Vector3(0, 0, 0);
		}

		bool turtle = false;
		RaycastHit hit;
		
		if (Input.GetMouseButtonDown(0))
		{
            Transform col = getClicked();
			if(col!=null && col.tag == "Rotator") {
				rot = true;
			}
		}
		if (Input.GetMouseButtonUp (0) && rot) {
			//if(!Hud.drag)
			//	rot = false;
		}
		if (rot) {
            if(TutoHand.hand.getStep()==2)
            {
                TutoHand.hand.advance();
            }
			float x = -Input.GetAxis("Mouse X");
			float y = -Input.GetAxis("Mouse Y");
			float speed = 10;
			transform.rotation *= Quaternion.AngleAxis(x*speed, Vector3.forward);
			transform.rotation *= Quaternion.AngleAxis(y*speed, Vector3.forward);
			if(Hud.selected!=null) {
				Hud.selected.transform.rotation *= Quaternion.AngleAxis(x*speed, Vector3.forward);
				Hud.selected.transform.rotation *= Quaternion.AngleAxis(y*speed, Vector3.forward);
			}
		}
	}
	
}
