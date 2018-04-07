using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CardDesk : ScriptableObject {



    public List<Card> cardDesk;
	
    
    // Use this for initialization
	void Start () {
        var cards = Resources.LoadAll("Cards", typeof(Card)).Cast<Card>().ToArray();

        for(int i = 0; i < cards.Length; i++)
        {
            cardDesk.Add(cards[i]);
        }
	}


	
	
}
