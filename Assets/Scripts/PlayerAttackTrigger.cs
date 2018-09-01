using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackTrigger : MonoBehaviour {

	public int damage = 10;

	void OnTriggerEnter2D (Collider2D other) {
		// Attacking other player
		if (other.CompareTag ("Player")) {
			if (other.GetComponent<PlayerHealth> ().current < damage * 2) {
				PlayerHealth stats = GetComponentInParent<PlayerHealth> ();
				stats.UpdateScore (50);
			}
			other.GetComponent<PlayerHealth> ().Damage (damage);
		}

		// Attacking neutral/enemy monster
		if (other.CompareTag ("Monster") && !other.GetComponent<Enemy> ().IsOwner (transform.parent.gameObject)) {
			if (other.GetComponent<Enemy> ().currHealth < damage * 2) {
				PlayerHealth stats = GetComponentInParent<PlayerHealth> ();
				stats.UpdateScore (10);
			}
			other.GetComponent<Enemy> ().Damage (damage * 2);
		}

		// If we are attacking a neutral/enemy tower, damage it
		if (other.CompareTag ("Tower") && !other.GetComponent<TowerAI> ().IsOwner (transform.parent.gameObject)) {
			other.GetComponent<TowerHealth> ().Damage (damage);
		}
	}
}
