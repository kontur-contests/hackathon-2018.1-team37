using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Action
{
	Damage,
	Heal,
	Evade
}


public class Card : ScriptableObject {

	public Action type;
	public int value;

}
