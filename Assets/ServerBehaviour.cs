using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerBehaviour : NetworkBehaviour
{
    public const double roundTime = 3;
    private double endRoundTime;
    private bool roundGoing = false;

    public List<GameObject> players = new List<GameObject>();

    // Use this for initialization
    void Start () {
		
	}

    public void StartRound()
    {
        roundGoing = true;
        endRoundTime = Time.fixedTime + roundTime;
    }
	
	// Update is called once per frame
	void Update () {
        if (roundGoing)
        {
            if(endRoundTime < Time.fixedTime)
            {
                //count scores based on cards
                StartRound();
            }
        }
	}
}
