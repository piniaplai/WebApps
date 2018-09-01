using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackTrigger : MonoBehaviour {

	private int damage = 10;
	private float cooldown = 1.0f;
	private bool attacking = false;
	private float attackTimer = 0;

	void OnTriggerStay2D (Collider2D other) {

		if (!attacking) {
			attacking = true;
			attackTimer = cooldown;
			Attack (other);
		}

		if (attacking) {
			if (attackTimer > 0) {
				attackTimer -= Time.deltaTime;
			} else {
				attacking = false;
			}
		}
	}

	private void Attack (Collider2D other) {
		// Attacking player
		if (other.CompareTag ("Player") && !GetComponentInParent<Enemy> ().IsOwner (other.gameObject)) {
			other.GetComponent<Collider2D> ().SendMessageUpwards ("Damage", damage);
		}

	}
}
