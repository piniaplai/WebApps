using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyTutorialButton : MonoBehaviour {

	public Image image;
	public GameObject toDoList;
	public GameObject clock;
	public GameObject lobby;
	public GameObject logout;
	private Text text;




	void Awake () {
		image.gameObject.SetActive (false);
		text = gameObject.GetComponentInChildren<Text> ();
	}

	public void FlipImageActive () {
		if (image.gameObject.GetActive ()) {
			image.gameObject.SetActive (false);
			toDoList.gameObject.SetActive (true);
			clock.gameObject.SetActive (true);
			lobby.gameObject.SetActive (true);
			logout.gameObject.SetActive (true);
			text.text = "Lobby Tutorial";
		} else {
			image.gameObject.SetActive (true);
			toDoList.gameObject.SetActive (false);
			clock.gameObject.SetActive (false);
			lobby.gameObject.SetActive (false);
			logout.gameObject.SetActive (false);
			text.text = "Leave";
		}

	}
}
