using UnityEngine;
using System.Collections;

public class Mouse : MonoBehaviour
{

    public static bool mouseDown = false;
    public static bool drag = false;
    public static bool placed = false;
    Vector3 newPos, oldPos;

    // Use this for initialization
    void Start()
    {

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

    // Update is called once per frame
    void Update()
    {
        // LEFT CLICK 
        if (Input.GetMouseButton(0) && GUIUtility.hotControl == 0)
        {

            // FIRST CLICK
            if (!mouseDown)
            {
                oldPos = Vector3.zero;
                newPos = Vector3.zero;
                Transform col = getClicked();
                if (col != null)
                {
			
                    if (col.tag == "Buildable")
                    {
                     
                        if (Hud.selected == null)
                        {
                            Hud.select(col.transform);
                        } else
                        {
                            if(Hud.selected!=col.transform)
                            {
                                Hud.clearSelected();
                                Hud.select(col.transform);
                            }
                        }
                    }
                } else
                {
                    // nothing was clicked
                    if(Hud.selected!=null)
                    {

						if (TutoHand.hand.getStep() != 2)
						{
							Hud.clearSelected();
						}
                       
                    }
                }
                mouseDown = true;
            // DRAG CLICK
            }
            else
            {

                if (Hud.selected != null && !Spawn.spawning)
                {
                    // IF IM SELECTING SOMETHING AND NOT DRAGGIN YET
                    if (!drag)
                    {
                        Transform col = getClicked();
                        if (col == null || col.tag != "Buildable")
                        {
                            if (placed)
                            {
                                placed = false;
                            }
                            else
                            {
                                if (col == null || col.tag != "Rotator")
                                {
                                    Hud.clearSelected();
                                }
                            }

                        }
                        // already draggin something
                    }
                    else
                    {
						if(TutoHand.hand.getStep ()==1) {
							TutoHand.hand.advance();
						}


                        if (!Rotator.rot)
                        {

                           
                    
                            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                            if(oldPos == Vector3.zero)
                            {
                                oldPos = mousePos;
                            }
                            newPos = mousePos - oldPos;
                            oldPos = mousePos;

                            Hud.selected.Translate(new Vector3(newPos.x, newPos.y, 0), Space.World);
                           // Hud.selected.position = new Vector3(mousePos.x+newPos.x, mousePos.y+newPos.y, 10);
                          


                        }
                    }

                    drag = true;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
			if(drag && mouseDown) {
				if (TutoHand.hand.getStep() == 1)
				{
					TutoHand.hand.advance();
				}
			}
            mouseDown = false;
            drag = false;

            // i have something selected
            if (Hud.selected != null)
            {
                // im not rotating
                if (!Rotator.rot)
                {

                    Transform col = getClicked();
                    if (col == null || col.tag != "Buildable")
                    {
                        if (Hud.hasPlacedBlock)
                        {
                            Hud.hasPlacedBlock = false;
                        }
                        else
                        {
                            Hud.clearSelected();

                        }
                    }

                }
                if (Rotator.rot)
                {
                    Rotator.rot = false;
                }
            } 

        }


    }
}
