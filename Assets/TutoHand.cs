using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutoHand : MonoBehaviour {

	public static TutoHand hande;
    private bool vanished = false;
    private int step = -1;
    public GameObject[] hands;

    // 0 - build
    // 1 - drag plank
    // 2 - rotate plank
    // 3 - play
    // 4 - drag map
    // 5 - stop

    // Use this for initialization

    public static Gamb hand = new Gamb();

	public class Gamb
	{
		public TutoHand hand;
        public void advance()
        {
            if (hand != null)
                hand.advance();
        }
		public int getStep() {
			if(hand==null)
				return 999;
			else
				return hand.getStep ();
		}
	}



    void Start()
    {
        hand.hand = this;
        for(int x = 0; x < hands.Length; x++)
        {
			if(hands[x]!=null)
            hands[x].active = false;
        }
        advance();
    }

    public void advance()
    {
        if(step>=0)
        {
			if(hands[step]!=null)
            	hands[step].active = false;
        }
        if (step == hands.Length - 1)
        {
            return;
        }
        step++;
		if(hands[step]!=null)
        	hands[step].active = true;
    }


    public int getStep()
    {
        return step;
    }

	// Update is called once per frame
	void Update () {
	
	}
}
