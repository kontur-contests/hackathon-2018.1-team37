using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    private bool m_ShotAxis = false;



    public CardDesk cardDesk;
    public int[] AvailableCards;

    


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
    }

    [ClientRpc]
    public void Rpc_Animate()
    {

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

       


    }




    // Update is called once per frame
    void Update () {

        GetPlayerInput();
        


    }
    



    private void GetPlayerInput()
    {

        if (Input.GetButtonDown("Socket1"))
        {
            
            Debug.Log("A");
        }
        if (Input.GetButtonDown("Socket2"))
        {
            Debug.Log("X");
        }
        if (Input.GetButtonDown("Socket3"))
        {
            Debug.Log("Y");
        }

        if (Input.GetButtonDown("Socket4"))
        {
            Debug.Log("B");
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


