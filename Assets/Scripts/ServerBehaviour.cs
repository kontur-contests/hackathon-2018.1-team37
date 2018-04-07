using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerBehaviour : NetworkBehaviour
{
    public const double roundTime = 3;
    private double endTime;
    public State state = State.Connecting;

    private struct PlayerActionStruct
    {
        public int damage;
        public int heal;
        public int evade;
    }
    
    public enum State
    {
        Round, Animation, Start, Finish, Pause, Connecting
    }

    public enum PlayerState
    {
        Healed, Damaged, NoDamaged 
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

    public void Animate(PlayerState nextState)
    {
        state = State.Animation;
        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerController>().ready = false;
            player.GetComponent<PlayerController>().Rpc_Animate(nextState);
        }
    }

    public PlayerState ApplyActions()
    {
        PlayerActionStruct[] playerActions = new PlayerActionStruct[2];
        for (int i = 0; i < players[0].GetComponent<PlayerController>().SelectedCards.Count; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                int cdIndex = players[j].GetComponent<PlayerController>().SelectedCards[i];
                if (cdIndex < 0)
                    continue;
                Card card = GameObject.Find("GameManager").GetComponent<CardDesk>().cardDesk[cdIndex];
                switch (card._ActionType)
                {
                    case Action.DamageBoth:
                        playerActions[j].damage += (int)card._value;
                        playerActions[1 - j].damage += (int)card._value;
                        break;
                    case Action.DamageEnemy:
                        playerActions[1 - j].damage += (int)card._value;
                        break;
                    case Action.DamageSelf:
                        playerActions[j].damage += (int)card._value;
                        break;
                    case Action.Evade:
                        playerActions[j].evade += (int)card._value;
                        break;
                    case Action.Heal:
                        playerActions[j].heal += (int)card._value;
                        break;
                }
            }
        }
        int player0HP = players[0].GetComponent<PlayerController>().Health - 
            Mathf.Max(0, playerActions[0].damage - playerActions[0].evade) +
            playerActions[0].heal;
        player0HP = Mathf.Min(players[0].GetComponent<PlayerController>()._maxHealth, player0HP);
        if (player0HP > players[0].GetComponent<PlayerController>().Health)
            return PlayerState.Healed;
        if (player0HP < players[0].GetComponent<PlayerController>().Health)
            return PlayerState.Damaged;
        return PlayerState.NoDamaged;
    }

    public void FinishRound()
    {
        PlayerState nextState = ApplyActions();
        if (players[0].GetComponent<PlayerController>().IsAlive && players[1].GetComponent<PlayerController>().IsAlive)
            Animate(nextState);
        else
            state = State.Finish;
    }

    // Update is called once per frame
    void Update () {
        if (state.Equals(State.Start) || state.Equals(State.Animation))
        {
            if (players[0].GetComponent<PlayerController>().ready && players[1].GetComponent<PlayerController>().ready)
                StartRound();
        }
        else if (state.Equals(State.Round))
        {
            if(endTime < Time.fixedTime)
            {
                FinishRound();
            }
        }
        else if (state.Equals(State.Finish))
        {
            foreach (GameObject player in players)
            {
                player.GetComponent<PlayerController>().Rpc_Finish();
            }
        }
	}
}
