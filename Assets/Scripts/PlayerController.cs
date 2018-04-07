using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    private bool m_ShotAxis = false;

    public GameObject[] cardSockets;
    
    public CardDesk cardDesk;
    public int[] AvailableCards;

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
           

        for(int i=0; i<4; i++)
        {
           
            int temp = Random.Range(0, (int)cardDesk.totalSum);
            int index = 0;
            uint sum = cardDesk.cardDesk[index]._chanceCoefficient;
            
            while (sum < temp)
            {
                index++;
                sum+= cardDesk.cardDesk[index]._chanceCoefficient;
            }
            AvailableCards[i] = index;
        }
        for (int i = 0; i < 4; i++)
            SelectedCards[i] = -1;    
        for (int i=0; i<4; i++)
        {
            cardSockets[i].GetComponent<SpriteRenderer>().sprite = cardDesk.cardDesk[AvailableCards[i]]._NotSelectedImage;
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

    }

    public int ID {
        get;
        set;
    }

    
    public int[] SelectedCards;
    
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

  


    // Use this for initialization
    void Start () {

        cardDesk = GameObject.Find("GameManager").GetComponent<CardDesk>();
        SelectedCards = new int[4];
        for (int i = 0; i < 4; i++)
            SelectedCards[i] = -1;
        AvailableCards = new int[4];
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
            SelectedCards[socketIndex] = AvailableCards[socketIndex];
            cardSockets[socketIndex].GetComponent<SpriteRenderer>().sprite = cardDesk.cardDesk[AvailableCards[socketIndex]]._SelectedImage;
        }
        else
        {
            SelectedCards[socketIndex] = -1;
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
    }

    [Command]
    public void Cmd_SetReady(bool status)
    {
        ready = status;
    }

}


