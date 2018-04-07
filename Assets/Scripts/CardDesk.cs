using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CardDesk : MonoBehaviour {



    public Card[] cardDesk;
    public uint totalSum;
	
    
    // Use this for initialization
	void Start () {
        cardDesk = Resources.LoadAll("Cards", typeof(Card)).Cast<Card>().ToArray();

        totalSum = 0;
        foreach (Card card in cardDesk)
        {
            totalSum += card._value;
        }
    }


	
	
}
