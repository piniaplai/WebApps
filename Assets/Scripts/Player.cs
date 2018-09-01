using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;
using UnityEngine.UI;

public class Player : Photon.MonoBehaviour {

	private Rigidbody2D rb2d;
	private IsoDynamicObject isoSorting;
	private ShotSpawn shotSpawn;
	private Animator animator;
	private bool facingRight = false;
	public SpriteRenderer sprite;

	private Image manaBarFloat;
	public Camera playerCam;
	private string playerName;
	public int speed = 1;
	public int unitDamage = 1;
	public float restartDelay = 1f;


	// Mana variables
	private int currMana;
	private int maxMana = 100;
	private int magicCost = 10;
	private float manaTime = 0;
	private int manaChargeRate = 5; // 5 mana per second
	private float manaBarLength;

	// Attack variables
	private bool attacking = false;
	private float attackTimer = 0;
	private float attackCd = 0.3f;
	public Collider2D attackTrigger;

	//On start up
	void Awake () {
		animator = transform.Find ("Sprite").GetComponent<Animator> ();
		rb2d = GetComponent<Rigidbody2D> ();
		shotSpawn = GetComponentInChildren<ShotSpawn> ();
		shotSpawn.player = gameObject;
		isoSorting = transform.Find ("Sprite").GetComponent<IsoDynamicObject> ();
		sprite = transform.Find ("Sprite").GetComponent<SpriteRenderer> ();
		playerName = SessionInfo.GetInstance ().GetSessionOwner ();
		attackTrigger.enabled = false;
	}

	internal string GetName () {
		return playerName;
	}

	void Start () {
		// Ensures only 1 camera in client's scene, which is
		// focussed on their player
		if (!photonView.isMine) {
			return;
		}

		Camera cam = Instantiate (playerCam);
		// Set the camera to follow player
		cam.transform.parent = gameObject.transform;
		// Set camera to player's position
		cam.transform.position = new Vector3 (transform.position.x, transform.position.y, -20);

		currMana = maxMana;
		manaBarLength = Screen.width / 2;
		manaBarFloat = transform.Find ("PlayerCanvas").Find ("ManaBarBG").Find ("Mana").GetComponent<Image> ();
	}

	void Update () {
		if (!photonView.isMine && PhotonNetwork.connected) {
			return;
		}

		if (Input.GetMouseButtonDown (1) && currMana > magicCost) {
			currMana -= magicCost;
			shotSpawn.LeftClickMagic ();
		}

		if (Input.GetButtonDown ("Melee")) {
			photonView.RPC ("SetAnimation", PhotonTargets.All, "Attack");
			attacking = true;
			attackTimer = attackCd;
			attackTrigger.enabled = true;
		}

		if (attacking) {
			if (attackTimer > 0) {
				attackTimer -= Time.deltaTime;
			} else {
				attacking = false;
				attackTrigger.enabled = false;
			}
		}

		manaBarLength = (Screen.width / 2) * (currMana / (float) maxMana);
		ChargeMana ();
	}

	//Every "physics" frame
	void FixedUpdate () {
		if (!photonView.isMine && PhotonNetwork.connected) {
			return;
		}
		bool isWalkingUp = false;
		ReadMovementInput (out isWalkingUp);
		if (rb2d.velocity.magnitude > 0) {
			if (isWalkingUp)
				photonView.RPC ("SetAnimation", PhotonTargets.All, "WalkUp");
			else
				photonView.RPC ("SetAnimation", PhotonTargets.All, "Walk");
		}
	}

	private void ChargeMana () {
		if (currMana >= maxMana) {
			currMana = maxMana;
			return;
		}
		manaTime += Time.deltaTime;
		if (manaTime > 1) {
			currMana += manaChargeRate;
			manaTime -= Mathf.Floor (manaTime);
		}
		manaBarFloat.fillAmount = (float) currMana / maxMana;
	}

	void ReadMovementInput (out bool isWalkingUp) {
		isWalkingUp = false;
		float y = Input.GetAxis ("Vertical"); // On 'a' or left arrow // 'd' or right arrow
		if (y > 0)
			isWalkingUp = true;
		float x = Input.GetAxis ("Horizontal"); // On 'w' or up arrow // 's' or down arrow
		isoSorting.Sort ();
		Vector3 direction = new Vector2 (x, y);

		// Only flips the player's direction if they start moving
		// i.e. doesn't flip back when coming to a rest
		if (x > 0 && !facingRight || x < 0 && facingRight) {
			// Flip sprite
			photonView.RPC ("FlipSprite", PhotonTargets.All, null);
			facingRight = !facingRight;
		}
		rb2d.velocity = direction * speed;
	}

	[PunRPC]
	private void FlipSprite () {
		sprite.transform.localScale = new Vector3 (sprite.transform.localScale.x * -1,
			sprite.transform.localScale.y,
			sprite.transform.localScale.z);
	}

	private void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "ManaPotion") {
			currMana += other.GetComponent<Potion> ().healAmount;
			other.gameObject.SetActive (false);
		}
	}

	[PunRPC]
	internal void SetAnimation (String animName) {
		animator.SetTrigger (animName);
	}
}
