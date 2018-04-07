using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerBehaviour : NetworkBehaviour
{
    public const double roundTime = 3;
    private double endTime;
    public State state = State.Connecting;
    
    public enum State
    {
        Round, Animation, Start, Finish, Pause, Connecting
    }

    public enum PlayerState
    {
        Healed, Damaged, None 
    }

    public List<GameObject> players = new List<GameObject>();

    // Use this for initialization
    void Start () {
		
	}

    public void StartRound()
    {
        state = State.Round;
        endTime = Time.fixedTime + roundTime;
        foreach(GameObject player in players){
            player.GetComponent<PlayerController>().Rpc_ShaffleCards();
        }
    }

    public void Animate()
    {
        state = State.Animation;
        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerController>().Rpc_Animate();
        }
    }

    public void ApplyActions()
    {
        foreach (GameObject playerObject in players)
        {
            PlayerController player = playerObject.GetComponent<PlayerController>();
            foreach(int i in player.SelectedCards)
            {
                Card card = GameObject.Find("GameManager").GetComponent<CardDesk>().cardDesk[i];
            }
        }
    }

    // Update is called once per frame
    void Update () {
        if (state.Equals(State.Start) || state.Equals(State.Animation))
        {
            if (players[0].GetComponent<PlayerController>().ready && players[1].GetComponent<PlayerController>().ready)
                StartRound();
        }
        if (state.Equals(State.Round))
        {
            if(endTime < Time.fixedTime)
            {
                //count scores based on cards
                if (players[0].GetComponent<PlayerController>().IsAlive && players[1].GetComponent<PlayerController>().IsAlive)
                    Animate();
                else
                    state = State.Finish;
            }
        }
        if (state.Equals(State.Finish))
        {
            foreach (GameObject player in players)
            {
                player.GetComponent<PlayerController>().Rpc_Finish();
            }
        }
	}
}
