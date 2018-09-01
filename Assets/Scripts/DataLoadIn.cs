using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoadIn : MonoBehaviour {

	public string[] users;

	IEnumerator Start () {
		WWW userData = new WWW ("http://localhost/User_DB/UserData.php");
		yield return userData;
		string userDataStr = userData.text;
		//print (userDataStr);
		users = userDataStr.Split(';');
		print (GetVal (users[0], "Username: "));
	}

	string GetVal (string data, string str) {
		string val = data.Substring (data.IndexOf (str) + str.Length);
		if (val.Contains ("~")){
		  val = val.Remove (val.IndexOf ("~"));
		}
		return val;
	}
}
