using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    private bool m_ShotAxis = false;

    public GameObject[] cardSockets;
    
    public CardDesk cardDesk;
    private int[] AvailableCards;

    private Animator animator;

    public enum PlayerState
    {
        NoDamaged,
        Shooting,
    }

    public PlayerState playerState;

    [ClientRpc]
    public void Rpc_ShaffleCards()
    {
        if (isLocalPlayer)
        {
            Cmd_SetReady(false);

            for (int i = 0; i < 4; i++)
            {

                int temp = Random.Range(0, (int)cardDesk.totalSum);
                int index = 0;
                uint sum = cardDesk.cardDesk[index]._chanceCoefficient;

                while (sum < temp)
                {
                    index++;
                    sum += cardDesk.cardDesk[index]._chanceCoefficient;
                }
                AvailableCards[i] = index;
            }
            Cmd_InitSelectedCards();
            for (int i = 0; i < 4; i++)
            {
                Debug.Log(cardDesk.cardDesk[AvailableCards[i]]._cardName);
                cardSockets[i].GetComponent<SpriteRenderer>().sprite = cardDesk.cardDesk[AvailableCards[i]]._NotSelectedImage;
            }
            animator.SetTrigger("Attack");
            AudioSource audio = GetComponent<AudioSource>();
            audio.Play();
        }
    }


    [ClientRpc]
    public void Rpc_Animate(ServerBehaviour.PlayerState roundResult)
    {

        Cmd_SetReady(false);
        switch (roundResult)
        {
            case ServerBehaviour.PlayerState.Damaged:
                animator.SetTrigger("Damaged");
                break;
            case ServerBehaviour.PlayerState.NoDamaged:
                animator.SetTrigger("NoDamaged");
                break;
            case ServerBehaviour.PlayerState.Healed:
                animator.SetTrigger("Healed");
                break;        
        }

    }

    [ClientRpc]
    public void Rpc_Finish()
    {
        if (!IsAlive)
            animator.SetTrigger("Dead");
        else
            animator.SetTrigger("NoDamaged");
    }

    public int ID {
        get;
        set;
    }

    [SyncVar]
    public SyncListInt SelectedCards = new SyncListInt();
    
    public int _maxHealth = 10;


    [SyncVar]
    public bool ready;

    [SyncVar]
    public int Health;
    
    public bool IsAlive
    {
        get
        {
            return Health > 0;
        }

        set
        {

        }
    }

    [Command]
    private void Cmd_InitSelectedCards()
    {
        SelectedCards.Clear();
        for (int i = 0; i < 4; i++)
            SelectedCards.Add(-1);
    }

    [Command]
    private void Cmd_SetSelectedCard(int index, int value)
    {
        SelectedCards[index] = value;
    }


    // Use this for initialization
    void Start () {

        cardDesk = GameObject.Find("GameManager").GetComponent<CardDesk>();
        cardSockets = new GameObject[4];
        for(int i=0; i<4; i++)
        {
            cardSockets[i] = GameObject.Find("Socket"+(i+1).ToString());
        }
        AvailableCards = new int[4];
        Cmd_InitSelectedCards();
        animator = GetComponent<Animator>();
        animator.SetTrigger("NoDamaged");
        Cmd_SetReady(false);
       


    }




    // Update is called once per frame
    void Update () {

        if(isLocalPlayer)
            GetPlayerInput();
              


    }
    

    private void SetCard(int socketIndex)
    {
        if (SelectedCards[socketIndex] == -1)
        {
            Cmd_SetSelectedCard(socketIndex, AvailableCards[socketIndex]);
            cardSockets[socketIndex].GetComponent<SpriteRenderer>().sprite = cardDesk.cardDesk[AvailableCards[socketIndex]]._SelectedImage;
        }
        else
        {
            Cmd_SetSelectedCard(socketIndex, -1);
            cardSockets[socketIndex].GetComponent<SpriteRenderer>().sprite = cardDesk.cardDesk[AvailableCards[socketIndex]]._NotSelectedImage;
        }

    }

    private void GetPlayerInput()
    {

        if (Input.GetButtonDown("Socket1"))
        {
            SetCard(0);
        }

        if (Input.GetButtonDown("Socket2"))
        {
            SetCard(1);
        }

        if (Input.GetButtonDown("Socket3"))
        {
            SetCard(2);
        }

        if (Input.GetButtonDown("Socket4"))
        {
            SetCard(3);
        }

        //Gamepad Shot
        if (Input.GetAxisRaw("Shot") != 0)
        {
            if (m_ShotAxis == false)
            {

                Cmd_FinishRound();
                Debug.Log("RT");


                m_ShotAxis = true;
            }
        }

        if (Input.GetAxisRaw("Shot") == 0)
        {
            m_ShotAxis = false;
        }

        if (Input.GetButtonDown("Start"))
        {
            Cmd_SetReady(true);
            Debug.Log("Start");
        }
    }

    [Command]
    public void Cmd_FinishRound()
    {
        GameObject.Find("GameServer").GetComponent<ServerBehaviour>().FinishRound();
        Cmd_InitSelectedCards();

    }

    [Command]
    public void Cmd_SetReady(bool status)
    {
        ready = status;
        
    }

}


