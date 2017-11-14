using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
    float speed = 20f;
    int boundary = 1;
    GameObject bg;
    int width;
    private bool drag = false;
    int height;

    public float minX;
    public float minY;
    public float maxX;
    public float maxY;
    public float bgTraz = 0;
    public float bgFrente = 0;
    private string taAtraz = "BG2";
    private GameObject bg1;
    private GameObject bg2;

    void Start()
    {

        GameObject[] backgrounds = GameObject.FindGameObjectsWithTag("Background");
        if (backgrounds.Length > 0)
        {
            foreach (GameObject b in backgrounds)
            {
                if (b.name == "BG1")
                {
                    bg1 = b;
                    bgTraz = b.transform.position.x;
                }
                else if (b.name == "BG2")
                {
                    bg2 = b;
                    bgFrente = b.transform.position.x;
                }
            }
        }
        width = Screen.width;
        height = Screen.height;
    }

    void Update()
    {

		if (Hud.getLevel () == 1) {
			if(TutoHand.hand.getStep()<2) {
				return;
			}
		}

        if(bg1!=null && bg2!=null)
        {
            GameObject b = bg1;
            GameObject b2 = bg2;
            b.transform.Translate(-0.01f, 0, 0);
            b2.transform.Translate(-0.01f, 0, 0);

            if (b2.name == taAtraz)
            {

                // ele passou do lugar da traz, move o outro
                if (b2.transform.position.x < bgTraz)
                {
                    b.transform.position = new Vector3(bgFrente - 0.1f, b.transform.position.y, b.transform.position.z);
                    taAtraz = b.name;
                }
            }
            else if (b.name == taAtraz)
            {

                // ele passou do lugar da traz, move o outro
                if (b.transform.position.x < bgTraz)
                {
                    b2.transform.position = new Vector3(bgFrente - 0.1f, b2.transform.position.y, b2.transform.position.z);
                    taAtraz = b2.name;
                }
            }

        }
        

        if (Input.touchCount > 0)
        {

            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                drag = true;
            }
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                drag = false;
            }
            if (Input.GetTouch(0).position.y > Screen.height - Hud.yPosition)
            {
                drag = false;
                return;
            }
            if (Hud.selected)
                drag = false;
            if (drag)
            {
				if(TutoHand.hand.getStep ()==4) {
					TutoHand.hand.advance();
				}
                Vector2 var = Input.GetTouch(0).deltaPosition;
                float newX = var.x * Time.deltaTime * 3;
                float newY = var.y * Time.deltaTime * 3;
                float pX = transform.position.x - newX;
                float pY = transform.position.y - newY;
                if (minX != 0 && minY != 0)
                {
                    pX = Mathf.Clamp(pX, minX, maxX);
                    pY = Mathf.Clamp(pY, minY, maxY);
                }
                // newX = Mathf.Clamp(, leftBound, rightBound);
                transform.position = new Vector3(pX, pY, 0);

                //transform.position = new Vector3(pX,pY , 0);
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                drag = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                drag = false;
            }
            if (Input.mousePosition.y > Screen.height - Hud.yPosition)
            {
                drag = false;
                return;
            }
            if (Hud.selected)
                drag = false;
            if (drag)
            {
				if(TutoHand.hand.getStep ()==4) {
					TutoHand.hand.advance();
				}
                float newX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * speed * 1.3f;
                float newY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * speed * 1.3f;
                float pX = transform.position.x - newX;
                float pY = transform.position.y - newY;
                if (minX != 0 && minY != 0)
                {
                    pX = Mathf.Clamp(pX, minX, maxX);
                    pY = Mathf.Clamp(pY, minY, maxY);
                }
                // newX = Mathf.Clamp(, leftBound, rightBound);
                transform.position = new Vector3(pX, pY, 0);
                if (Hud.loadedLevel == 1 && Hud.tutorialStep == 5)
                {
                    Hud.tutorialStep++;
                }
            }
        }

		Debug.Log (transform.position);



    }
}