using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoadOut : MonoBehaviour {

	public string inputUsername;
	public string inputPassword;

	void Update () {
		if (Input.GetKeyDown (KeyCode.O)) {
			NewUser (inputUsername, inputPassword);
		}
	}

	public void NewUser (string username, string password) {
		WWWForm wwwform = new WWWForm ();
		wwwform.AddField ("usernameP", username);
		wwwform.AddField ("passwordP", password);
		new WWW ("http://towerwatchdb.96.lt/CreateUser.php", wwwform);
	}
}
