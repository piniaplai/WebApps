using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SessionInfo : MonoBehaviour {

	//Lobby Error Codes
	public static int NOTOKENS = 0;
	public static int BADXMLCODE = 1;
	public static int MAXNOTINT = 2;
	public static int GUESTCREATE = 3;

	public static SessionInfo instance = null;
	private string sessionOwner;
	private bool guest = false;
	private int tokens;

	void Awake () {
		if (instance == null) {
			instance = this;
		} else {
			Destroy (gameObject);
		}

		if (QueryInfo.GetLoginSuccess ()) {
			if (PlayerPrefs.HasKey ("username")) {
				sessionOwner = PlayerPrefs.GetString ("username");
			} else {
				print ("Error");
			}
			if (PlayerPrefs.HasKey ("tokenNumber")) {
				tokens = PlayerPrefs.GetInt ("tokenNumber");
			} else {
				print ("Error");
			}
		} else {
			guest = true;
			sessionOwner = "guest";
		}
	}

	void Update () {
		if (tokens < 0) {
			tokens = 0;
			if (PlayerPrefs.HasKey ("tokenNumber")) {
				PlayerPrefs.SetInt ("tokenNumber",tokens);
			} else {
				print ("Error");
			}
			QueryInfo.CallSetTokens (this);
		}
	}

	public static SessionInfo GetInstance () {
		return instance;
	}

	public string GetSessionOwner () {
		return sessionOwner;
	}

	public int GetTokenNumber () {
		return tokens;
	}

	public void SetTokenNumber (int tokens) {
		this.tokens = tokens;
	}

	public void IncTokenNumber () {
		tokens++;
		if (PlayerPrefs.HasKey ("tokenNumber")) {
			PlayerPrefs.SetInt ("tokenNumber",tokens);
		} else {
			print ("Error");
		}
		QueryInfo.CallSetTokens (this);
	}

	public void DecTokenNumber () {
		tokens--;
		if (PlayerPrefs.HasKey ("tokenNumber")) {
			PlayerPrefs.SetInt ("tokenNumber",tokens);
		} else {
			print ("Error");
		}
		QueryInfo.CallSetTokens (this);
	}

	public bool CanPlayGame () {
		return tokens > 0;
	}

	public bool IsGuest () {
		return guest;
	}

	public void LogOut () {
		SceneManager.LoadSceneAsync ("UI", LoadSceneMode.Single);
	}
}
