using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotSpawn : MonoBehaviour {

	public GameObject magic;
	public GameObject player;

	// Casts magic assigned to left click
	public void LeftClickMagic () {
		GameObject magicInstantiation = PhotonNetwork.Instantiate (magic.name, transform.position, Quaternion.identity, 0);
		Magic m = magicInstantiation.GetComponent<Magic> ();
		m.player = player;
		m.Cast ();
		StartCoroutine (Utils.NetworkDestroy (magicInstantiation, 1.0f));
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
