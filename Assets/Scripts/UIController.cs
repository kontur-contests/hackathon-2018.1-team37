using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {


    public PlayerController blueCowboy;
    public PlayerController redCowboy;

    public Text blueHP;
    public Text redHP;
    public Text blueReady;
    public Text redReady;

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        GameObject blueGameobj = GameObject.Find("Cowboy(Clone)");
        GameObject redGameobj = GameObject.Find("CowboyRed(Clone)");
        if (blueGameobj != null && redGameobj != null) { 
            blueCowboy = blueGameobj.GetComponent<PlayerController>();
            redCowboy = redGameobj.GetComponent<PlayerController>();
            blueHP.text = blueCowboy.Health.ToString();
            redHP.text = redCowboy.Health.ToString();

            if (blueGameobj.GetComponent<PlayerController>().ready)
            {
                blueReady.text = "Ready";
            }
            else
                blueReady.text = "Not Ready";

            if (redGameobj.GetComponent<PlayerController>().ready)
            {
                redReady.text = "Ready";
            }
            else
                redReady.text = "Not Ready";

        }


    }
}
