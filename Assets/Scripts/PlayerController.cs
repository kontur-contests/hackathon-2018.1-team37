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
           
            var temp = Random.Range(0, cardDesk.totalSum);
            int index = 0;
            int sum = cardDesk.cardDesk[index]._chanceCoefficient;
            
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
            cardSockets[i].GetComponent<SpriteRenderer>().sprite = cardDesk.cardDesk[AvailableCards[i]]._Image;
        }

    }

    [ClientRpc]
    public void Rpc_Animate(ServerBehaviour.PlayerState roundResult)
    {
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

    [SyncVar]
    public SyncListInt SelectedCards;
    
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
        SelectedCards = new SyncListInt();
        for (int i = 0; i < 4; i++)
            SelectedCards.Add(-1);
        AvailableCards = new int[4];
        animator = GetComponent<Animator>();
        animator.SetTrigger("NoDamaged");
       


    }




    // Update is called once per frame
    void Update () {

        GetPlayerInput();
        


    }
    

    private void SetCard(int socketIndex)
    {
        if (SelectedCards[socketIndex] == -1)
        {
            SelectedCards[socketIndex] = AvailableCards[socketIndex];
        }
        else
            SelectedCards[socketIndex] = -1;

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

                // Call your event function here.
                Debug.Log("RT");


                m_ShotAxis = true;
            }
        }

        if (Input.GetAxisRaw("Shot") == 0)
        {
            m_ShotAxis = false;
        }
    }


}


