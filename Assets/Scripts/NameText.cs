using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameText : MonoBehaviour {

	public string preText;
	public string postText;
	public Text text;
	private SessionInfo sessionInfo;


	void Start () {
		sessionInfo = SessionInfo.GetInstance ();
		text.text = preText + sessionInfo.GetSessionOwner () + postText;
	}

}
