using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

	private int maxHealth = 100;
	public int currHealth;
	private GameObject towerParent;
	private Rigidbody2D rb2d;
	private Animator animator;
	public SpriteRenderer sprite;
	private float minDistFromTower = 3.0f;
	private float maxDistFromPlayer = 5.0f;
	private float speed = 2.0f;
	private List<GameObject> players;
	private Collider2D[] objsInRange;
	private bool facingRight = false;
	private bool touchingPlayer = false;

	private Image healthBar;

	void Start () {
		animator = transform.Find("Sprite").GetComponent<Animator> ();
		sprite = transform.Find ("Sprite").GetComponent<SpriteRenderer>();
		currHealth = maxHealth;
		rb2d = GetComponent<Rigidbody2D> ();
		NetworkManager list = FindObjectOfType<GameManager> ().GetComponent<NetworkManager> ();
		if (list == null) {
			players = FindObjectOfType<GameManager> ().GetComponent<TutorialNetwork> ().GetPlayers ();
		} else {
			players = list.GetPlayers();
		}
		healthBar = transform.Find ("EnemyCanvas").Find ("HealthBarBG").Find ("Health").GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!PhotonNetwork.isMasterClient) {
			return;
		}

		if (currHealth <= 0) {
			// Tell parent tower this enemy died and destroy on network
			if (towerParent != null) {
				towerParent.GetComponent<TowerAI> ().DecrementMonsterCount ();
			}
			animator.SetTrigger ("Dying");
			Utils.NetworkDestroy(gameObject);
		}

		// If a player is within range and not touching
		objsInRange = Physics2D.OverlapCircleAll (transform.position, maxDistFromPlayer);
		GameObject nearestPlayer = FindNearestPlayer ();
		if (nearestPlayer != null) {
			// Go to player
			rb2d.velocity = touchingPlayer ? Vector3.zero :
				(nearestPlayer.transform.position - transform.position).normalized * speed;
		}

		// Else go back to tower
		else if (towerParent != null && Vector2.Distance (transform.position, towerParent.transform.position) > minDistFromTower) {
			// Go back to tower
			rb2d.velocity = (towerParent.transform.position - transform.position).normalized * speed;
		} else {
			// Stop moving when close enough to tower
			rb2d.velocity = Vector3.zero;
		}

		float x = rb2d.velocity.x;
		if (x > 0 && !facingRight || x < 0 && facingRight) {
			// Flip sprite
			sprite.transform.localScale = new Vector3 (sprite.transform.localScale.x * -1,
				sprite.transform.localScale.y,
				sprite.transform.localScale.z);
			facingRight = !facingRight;
		}

		// Play the walking animation if the enemy is moving
		// TODO: detect walking 'upwards'
		if (rb2d.velocity.x > 0 || rb2d.velocity.y > 0) {
			animator.SetTrigger ("Walk");	
		}
	}

	public bool IsOwner (GameObject player) {
		if (towerParent == null) {
			return false;
		}
		return towerParent.GetComponent<TowerAI> ().IsOwner (player);
	}

	private GameObject FindNearestPlayer () {
		GameObject nearestPlayer = null;
		float currDist = 0;
		foreach (Collider2D obj in objsInRange) {
			if (!obj.CompareTag("Player")) {
				continue;
			}
			float nextDist = Vector2.Distance (obj.transform.position, transform.position);
			if (nextDist < currDist || nearestPlayer == null) {
				currDist = nextDist;
				nearestPlayer = obj.gameObject;
			}
		}
		return nearestPlayer;
	}

	public void SetTowerParent (GameObject towerParent) {
		this.towerParent = towerParent;
	}

	public void Damage (int damage) {
		currHealth -= damage;
		healthBar.fillAmount = (float) currHealth / maxHealth;
		animator.SetTrigger ("Hit");
	}

	void OnTriggerStay2D (Collider2D other) {
		if (other.CompareTag("Player")) {
			touchingPlayer = true;
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		if (other.CompareTag ("Player")) {
			touchingPlayer = false;
		}
	}
}