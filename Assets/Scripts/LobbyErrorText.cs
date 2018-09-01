using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyErrorText : MonoBehaviour {

	public static LobbyErrorText instance = null;

	public GameObject errorText;
	public static Text text;

	void Awake () {
		if (instance == null) {
			instance = this;
		} else {
			Destroy (gameObject);
		}
		text = errorText.GetComponent<Text> ();
		text.text = "";
	}

	public static void SetErrorCode (int errorCode) {
		//Adhering to error codes in SessionInfo class
		instance.StartCoroutine (SetText (errorCode));
	}

	static IEnumerator SetText (int errorCode) {
		if (errorCode == SessionInfo.NOTOKENS) {
			text.text = "No tokens!";
		} else if (errorCode == SessionInfo.BADXMLCODE) {
			text.text = "Attempting to import unfamiliar format!";
		} else if (errorCode == SessionInfo.MAXNOTINT) {
			text.text = "Max player input can't be an integer!";
		} else if (errorCode == SessionInfo.GUESTCREATE) {
			text.text = "Guests can't create games, only join!";
		}
		yield return new WaitForSeconds (3);
		text.text = "";
	}
}
