using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    
	// Use this for initialization
	void Start () {
		
	}


    private bool m_ShotAxis = false;

    // Update is called once per frame
    void Update () {
        if (Input.GetButtonDown("Socket1")){
            Debug.Log("A");
        }
        if (Input.GetButtonDown("Socket2"))
        {
            Debug.Log("X");
        }
        if (Input.GetButtonDown("Socket3") )
        {
            Debug.Log("Y");
        }
        
        if(Input.GetButtonDown("Socket4"))
        {
            Debug.Log("B");
        }

      




        //Gamepad Shot
        if (Input.GetAxisRaw("Shot") != 0)
        {
            if (m_ShotAxis == false)
            {
                Debug.Log("RT");
                // Call your event function here.
                m_ShotAxis = true;
            }
        }
        if (Input.GetAxisRaw("Shot") == 0)
        {
            m_ShotAxis = false;
        }
    }
}
