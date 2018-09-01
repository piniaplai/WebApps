using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOrganisation : MonoBehaviour {

	public InputField emailInputField;

	public Text loginButtonText;
	private QueryInfo qi;

	void Start () {
		qi = GetComponent<QueryInfo> ();
	}

	void Update () {
		if (qi.GetQueryCode () == 0) {
			loginButtonText.text = "Create account";
			emailInputField.gameObject.SetActive (true);
		} else {
			loginButtonText.text = "Login";
			emailInputField.gameObject.SetActive (false);
		}
	}
}
