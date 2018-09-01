using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeTrigger : MonoBehaviour {

	public TowerAI towerAI;

	void Awake () {
		towerAI = gameObject.GetComponentInParent<TowerAI> ();
	}

	void OnTriggerStay2D (Collider2D other)
	{
		if (other.CompareTag("Player")) {
			towerAI.target = other.transform;
			towerAI.Attack();
		}
	}
}
