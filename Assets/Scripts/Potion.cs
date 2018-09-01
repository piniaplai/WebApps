using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour {

	public int healAmount;
	private Animator animator;

	// Use this for initialization
	void Awake () {
		healAmount = 30;	
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		animator.Play ("YmirIdle");
	}

}
