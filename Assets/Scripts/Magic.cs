using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Magic : MonoBehaviour {
	public GameObject player;

	public abstract void Cast (); 
	//Calls DrawCastField with another parameter (if necessary) 
	//Remember to set magic game object to active

	public abstract void DrawCastField(Vector3 other); //Uses ray cast to draw a general field of effect
}
