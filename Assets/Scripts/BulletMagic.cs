using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMagic : Magic {

	private float speed = 3f;
	private Rigidbody2D rb2d;
	private int damage = 10;

	//Called in shotspawn, shoots out magic
	public override void Cast () {
		//DrawCastField ();
		Vector2 mousePos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y); // Get mouse position
		mousePos = Camera.main.ScreenToWorldPoint (mousePos);
		rb2d = GetComponent<Rigidbody2D> ();
		rb2d.velocity = (mousePos - (Vector2) player.transform.position).normalized * speed; // Shoots out with velocity
		if (rb2d.velocity.x < 0) {
			transform.localScale = new Vector3 (transform.localScale.x * -1,
				transform.localScale.y,
				transform.localScale.z);

		}
	}
		
	public override void DrawCastField (Vector3 other)
	{
		throw new System.NotImplementedException ();
	}

	void OnTriggerEnter2D (Collider2D collision) {

		// If touching an enemy player or monster, damage them
		if (collision.gameObject.CompareTag ("Player") && !collision.gameObject.Equals (player)) {
			if (collision.GetComponent<PlayerHealth> ().current < damage * 2) {
				PlayerHealth stats = player.GetComponent<PlayerHealth> ();
				stats.UpdateScore (50);
			} 
		}

		if (collision.gameObject.CompareTag ("Monster")) {
			if (collision.GetComponent<Enemy> ().currHealth < damage * 2) {
				PlayerHealth stats = player.GetComponent<PlayerHealth> ();
				stats.UpdateScore (10);
			} 
			collision.GetComponent<Collider2D> ().SendMessageUpwards ("Damage", damage);
		}
	}
	

}