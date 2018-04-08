using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerBehaviour : NetworkBehaviour
{
    public const double roundTime = 10;
    public double endTime;
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

    public void Animate(PlayerState[] nextStates)
    {
        state = State.Animation;
        for(int i = 0; i < players.Count; i++)
        {
            //player.GetComponent<PlayerController>().ready = false;
            players[i].GetComponent<PlayerController>().Rpc_Animate(nextStates[i]);
        }
    }

    public PlayerState[] ApplyActions()
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
        PlayerState[] playerStates = new PlayerState[2];
        for (int j = 0; j < 2; j++)
        {
            int playerHP = players[j].GetComponent<PlayerController>().Health -
            Mathf.Max(0, playerActions[j].damage - playerActions[j].evade) +
            playerActions[j].heal;
            playerHP = Mathf.Min(players[j].GetComponent<PlayerController>()._maxHealth, playerHP);
            if (playerHP > players[j].GetComponent<PlayerController>().Health)
                playerStates[j] = PlayerState.Healed;
            else if (playerHP < players[j].GetComponent<PlayerController>().Health)
                playerStates[j] = PlayerState.Damaged;
            else
                playerStates[j] = PlayerState.NoDamaged;
            players[j].GetComponent<PlayerController>().Health = playerHP;
        }
        return playerStates;
    }

    public void FinishRound()
    {
        if (state.Equals(State.Round))
        {
            PlayerState[] nextStates = ApplyActions();
            if (players[0].GetComponent<PlayerController>().IsAlive && players[1].GetComponent<PlayerController>().IsAlive)
                Animate(nextStates);
            else
                state = State.Finish;
        }
    }



    bool isFinished = false;
    // Update is called once per frame
    void Update () {
        if (isServer)
        {
            if (state.Equals(State.Start) || state.Equals(State.Animation))
            {
                bool pl2ready = players[1].GetComponent<PlayerController>().ready;
                bool pl1ready = players[0].GetComponent<PlayerController>().ready;
                if (pl1ready && pl2ready)
                {
                    Debug.Log("Both true");
                    StartRound();
                }
            }
            else if (state.Equals(State.Round))
            {
                if (endTime < Time.fixedTime)
                {
                    FinishRound();
                }
            }
            else if (state.Equals(State.Finish) && !isFinished)
            {
                isFinished = true;
                foreach (GameObject player in players)
                {
                    player.GetComponent<PlayerController>().Rpc_Finish();
                }
            }
        }
	}
}
