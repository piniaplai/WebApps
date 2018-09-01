using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBoardManager : MonoBehaviour {

	public GameObject potion;
	public Canvas tutorial;
	public GameObject towers; // Player-ownable towers
	public GameObject enemy; // Other towers/buildings
	private Board board; // Used to track objects placed on board

	private float tileWidth;
	private float tileHeight;
	private float renderFixHeight;

	void Awake(){
		
	}

}