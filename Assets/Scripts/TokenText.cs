using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TokenText : MonoBehaviour {

	public string preText;
	public string postText;
	public Text text;
	private SessionInfo sessionInfo;


	void Start () {
		sessionInfo = SessionInfo.GetInstance ();
	}

	void Update () {
		text.text = preText + sessionInfo.GetTokenNumber() + postText;
	}
}
