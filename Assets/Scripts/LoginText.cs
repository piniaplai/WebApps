using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginText : MonoBehaviour {
	public Text text;

	public void SetLoginCode (int loginCode) {
		//Adhering to login codes in QueryInfo class
		if (loginCode == QueryInfo.FAILPASSWORD) {
			text.text = "Wrong password!";
		} else if (loginCode == QueryInfo.FAILUSERNAME) {
			text.text = "Username does not exist in database!";
		} else if (loginCode == QueryInfo.USEDUSERNAME) {
			text.text = "Username already exists in database!";
		} else if (loginCode == QueryInfo.SUCCESSINSERT) {
			text.text = "Username available and successfully inserted!";
		} else if (loginCode == QueryInfo.EMPTY) {
			text.text = "";
		} else if (loginCode == QueryInfo.BADEMAIL) {
			text.text = "Email does not exist!";
		}
	}

	public void SetLoginText (string str) {
		this.text.text = str;
	}
}
