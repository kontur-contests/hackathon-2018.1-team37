using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    private bool m_ShotAxis = false;
    


    public int[] AvailableCard;

    


    [ClientRpc]
    public void Rpc_ShaffleCards()
    {
        
    }

    [SyncVar]
    public int[] SelectedCards;
    
    public int _maxHealth = 10;

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
        SelectedCards = new int[4] {-1,-1,-1,-1};
        AvailableCard = new int[4];



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


