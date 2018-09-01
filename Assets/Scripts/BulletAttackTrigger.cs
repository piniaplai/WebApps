using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAttackTrigger : MonoBehaviour {

	int damage = 5;

	void OnTriggerEnter2D (Collider2D other) {
		if (other.CompareTag ("Player")) {
			other.GetComponent<Collider2D> ().SendMessageUpwards ("Damage", damage);
		}
	}
}
