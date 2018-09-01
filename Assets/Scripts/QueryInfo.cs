using System;
using System.Net.Mail;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class QueryInfo : MonoBehaviour {

	// Different login codes
	public static int SUCCESSLOGIN = 0;
	public static int FAILPASSWORD = 1;
	public static int FAILUSERNAME = 2;
	public static int USEDUSERNAME = 3;
	public static int SUCCESSINSERT = 4;
	public static int EMPTY = 5;
	public static int BADEMAIL = 6;

	// Different query codes
	public static int NEWUSER = 0;
	public static int CHECKLOGIN = 1;
	public static int GETINFO = 2;
	public static int SETTOKENS = 3;
	public static int SETLASTLOGIN = 4;
	public static int SETXML = 5;

	private static char phpDelimiter = '~';

	public string inputUsername;
	public InputField usernameField;
	public string inputPassword;
	public InputField passwordField;
	public string inputEmail;
	public InputField emailField;
	public Button button;
	private LoginText loginText;

	private static bool loginSuccessful = false;
	private static string url = "https://tower-watch-deploy.herokuapp.com/query.php";
	private int inputQueryCode = CHECKLOGIN;
	private string[] split;


	void Awake () {
		loginText = gameObject.GetComponent<LoginText> ();
	}

	public static bool GetLoginSuccess () {
		return loginSuccessful;
	}

	public static string GetCurrentDate () {
		return DateTime.Now.Year.ToString () + "-" 
			+ DateTime.Now.Month.ToString ("d2") + "-" 
			+ DateTime.Now.Day.ToString ();
	}

	public void SetUsername () {
		inputUsername = usernameField.text;
	}

	public void SetPassword () {
		inputPassword = passwordField.text;
	}

	public void SetEmail () {
		inputEmail = emailField.text;
	}

	public void SetQueryCode (int queryCode) {
		inputQueryCode = queryCode;
	}

	public int GetQueryCode () {
		return inputQueryCode;
	}

	private Regex GetEmailRegex () {
		string emailRegex = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"             + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"             + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
		return new Regex (emailRegex,RegexOptions.IgnoreCase);
	}

	private bool CheckEmail (string email) {
		return GetEmailRegex ().IsMatch (email);
	}

	//To be called by pressing the start button in the login screen UI
	IEnumerator StartButton () {
		if (inputQueryCode == NEWUSER) {
			if (!CheckEmail (inputEmail)) {
				loginText.SetLoginCode (BADEMAIL);
				button.gameObject.SetActive (true);
				yield break;
			}
			yield return StartCoroutine (NewUser (inputUsername, inputPassword));
		} else if (inputQueryCode == CHECKLOGIN) {
			yield return StartCoroutine (UserLogin (inputUsername, inputPassword));
		}
		if (loginSuccessful) {
			yield return StartCoroutine (GetInfo ());
			SceneManager.LoadSceneAsync ("Lobby", LoadSceneMode.Single);
		}
		button.gameObject.SetActive (true);
	}

	//Get information from database like last login and token number
	//TODO: FIX MAGIC NUMBERS
	IEnumerator GetInfo () {
		WWW www = FieldCompletion (inputUsername,inputPassword, GETINFO);
		yield return www;
		//print (www.text);
		string printLine = www.text;
		split = printLine.Split (phpDelimiter);
		string currDate;
		currDate = GetCurrentDate ();
		if (split [1] != currDate) {
			//reset
			PlayerPrefs.SetInt("tokenNumber", 3);
			CallSetTokens (this);
			CallSetLastLogin (this);
		} else {
			//don't reset
			PlayerPrefs.SetInt("tokenNumber", int.Parse(split [2]));
		}
		PlayerPrefs.SetString ("todoList", split [3]);
	}

	public static void CallSetTokens (MonoBehaviour instance) {
		if (PlayerPrefs.HasKey ("username")) {
			instance.StartCoroutine (SetTokens (PlayerPrefs.GetString ("username")));
		}
	}

	public static void CallSetLastLogin (MonoBehaviour instance) {
		if (PlayerPrefs.HasKey ("username")) {
			instance.StartCoroutine (SetLastLogin (PlayerPrefs.GetString ("username")));
		}
	}

	public static void CallSetXML (MonoBehaviour instance) {
		if (PlayerPrefs.HasKey ("username")) {
			instance.StartCoroutine (SetXML (PlayerPrefs.GetString ("username")));
		}
	}

	static IEnumerator SetTokens (string username) {
		WWW www = FieldCompletion (username,"", SETTOKENS);
		yield return www;
	}

	static IEnumerator SetLastLogin (string username) {
		WWW www = FieldCompletion (username,"", SETLASTLOGIN);
		yield return www;
	}

	static IEnumerator SetXML (string username) {
		WWW www = FieldCompletion (username,"", SETXML);
		yield return www;
	}

	//Execute query to create a new user
	IEnumerator NewUser (string username, string password) {
		WWW www = FieldCompletion (inputUsername,inputPassword, inputEmail);
		yield return www;
		//print (www.text);
		string printLine = www.text;
		split = printLine.Split (phpDelimiter);
		if (split [1] == "Error") {
			loginText.SetLoginCode (USEDUSERNAME);
		} else {
			loginText.SetLoginCode (SUCCESSINSERT);
		}
	}

	//Execute query to check login information of a user
	IEnumerator UserLogin (string username, string password) {
		WWW www = FieldCompletion (inputUsername,inputPassword, CHECKLOGIN);
		yield return www;
		//print (www.text);
		string printLine = www.text;
		split = printLine.Split (phpDelimiter);
		switch (split [1]) {
		case "Success":
			PlayerPrefs.SetString ("username", inputUsername);
			loginSuccessful = true;
			loginText.SetLoginCode (SUCCESSLOGIN);
			break;
		case "BadPassword":
			loginText.SetLoginCode (FAILPASSWORD);
			break;
		case "BadUsername":
			loginText.SetLoginCode (FAILUSERNAME);
			break;
		}
	}

	// Abstracted block of code to create WWWform for query
	private static WWW FieldCompletion (string username, string password, int queryCode) {
		WWWForm wwwform = new WWWForm ();
		wwwform.AddField ("usernameP", username);
		wwwform.AddField ("passwordP", password);
		wwwform.AddField ("queryCodeP", queryCode);
		wwwform.AddField ("lastLoginP", GetCurrentDate ());
		if (PlayerPrefs.HasKey ("tokenNumber")) {
			wwwform.AddField ("tokensP", PlayerPrefs.GetInt ("tokenNumber"));
		}
		if (PlayerPrefs.HasKey ("todoList")) {
			wwwform.AddField ("todoListP", PlayerPrefs.GetString ("todoList"));
		}
		return new WWW (url, wwwform);
	}

	private static WWW FieldCompletion (string username, string password, string email) {
		WWWForm wwwform = new WWWForm ();
		wwwform.AddField ("usernameP", username);
		wwwform.AddField ("passwordP", password);
		wwwform.AddField ("emailP", email);
		wwwform.AddField ("queryCodeP", NEWUSER);
		return new WWW (url, wwwform);
	}

	//Called to load in hub screen when start button is clicked
	//Not an IEnumerator so can be called by UI
	public void LoadGame () {
		button.gameObject.SetActive (false);
		StartCoroutine (StartButton ());
	}

	public void GuestGame () {
		SceneManager.LoadSceneAsync ("Lobby", LoadSceneMode.Single);
	}

}
