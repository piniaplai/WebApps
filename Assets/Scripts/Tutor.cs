using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Tutor : MonoBehaviour {

	private Animator animator;
	public GameObject tower;
	public GameObject towerInstance;
	public GameObject potion;
	private bool stay = false;
	private GameObject drink;
	public SpriteRenderer sprite;
	public Button next;
	private Collider2D[] objsInRange;
	private Rigidbody2D rb2d;
	private bool touchingPlayer = false;
	private bool facingRight = false;
	private float speed = 2.0f;
	private Text msg;
	private int msgCounter = 0;

	void Start () {
		animator = transform.Find("Sprite").GetComponent<Animator> ();
		msg = transform.Find ("TutorialCanvas").Find ("Message").Find ("Text").GetComponent<Text> ();
		sprite = transform.Find ("Sprite").GetComponent<SpriteRenderer>();
		rb2d = GetComponent<Rigidbody2D> ();
		towerInstance = PhotonNetwork.InstantiateSceneObject (tower.name, new Vector3 (-1.91f, -8.33f, 0), Quaternion.identity, 0, null);
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

	//This is a very ugly method
	public void switchText() {
		if (msgCounter == 0) {
			msg.text = "I will teach you \n how to play!";
			msgCounter++;
		} else if (msgCounter == 1) {
			msg.text = "Use WASD or Arrow \n keys to move.";
			msgCounter++;
			next.gameObject.SetActive (false);
			StartCoroutine (waitForMovement ());
		} else if (msgCounter == 2) {
			msg.text = "You have 2 weapons: \n a lance and magic.";
			msgCounter++;
		} else if (msgCounter == 3) {
			msg.text = "Press the space bar to \n attack with your lance.";
			msgCounter++;
			next.gameObject.SetActive (false);
			StartCoroutine (waitForSpace ());
		} else if (msgCounter == 4) {
			msg.text = "Now RIGHT click the \n mouse to attack with \n magic.";
			next.gameObject.SetActive (false);
			msgCounter++;
			StartCoroutine (waitForMagic ());
		} else if (msgCounter == 5) {
			msg.text = "Let's move on: the blue \n bar above your head \n shows your MANA.";
			msgCounter++;
		} else if (msgCounter == 6) {
			drink = PhotonNetwork.InstantiateSceneObject
				(potion.name, new Vector2 (-27.8f, -4f), Quaternion.identity, 0, null);
			msg.text = "That's a potion. Use it \n to recover your MANA.";
			next.gameObject.SetActive (false);
			msgCounter++;
			StartCoroutine (waitForPotion ());
		} else if (msgCounter == 7) {
			msg.text = "See that skeleton? \n Try getting closer.";
			next.gameObject.SetActive (false);
			msgCounter++;
			StartCoroutine (skeleton ());
		} else if (msgCounter == 8) { 
			next.gameObject.SetActive (false);
			msgCounter++;
			StartCoroutine (waitForStop ());
		} else if (msgCounter == 9) {
			next.gameObject.SetActive (false);
			msg.text = "Be careful! \n It will defend itself.";
			msgCounter++;
			StartCoroutine (waitForTower ());
		} else if (msgCounter == 10) {
			msg.text = "Now you are ready. \n I have to stay here \n so this is farewell..";
			msgCounter++;
		} else if (msgCounter == 11) {
			msg.text = "Good luck! \n Stay productive!";
            next.GetComponentInChildren<Text> ().text = "Back";
            msgCounter++;
		} else if (msgCounter == 12) {
            SceneManager.LoadSceneAsync ("Lobby", LoadSceneMode.Single);
        }
	}

	IEnumerator waitForTower() {
		while (towerInstance.GetComponent<TowerAI>().owner == null) {
			yield return null;
		}
		msg.text = "Well done! \n You did it!";
		next.gameObject.SetActive (true);
	}

	IEnumerator waitForStop() {
		while (!stay)
			yield return null;
		msg.text = "See that? \n It's a tower!";
		yield return new WaitForSeconds(4);
		msg.text = "That's what you're \n here for! Attack to \n capture the tower.";
		next.gameObject.SetActive (true);
	}

	IEnumerator skeleton() {
		print ("Begin wait()" + Time.time);
		yield return new WaitForSeconds(4);
		msg.text = "Don't worry, it \n can't get out.";
		yield return new WaitForSeconds(4);
		msg.text = "Skeletons will attack \n when they see you.";
		yield return new WaitForSeconds(4);
		msg.text = "Let's move on";
		next.gameObject.SetActive (true);
	}

	IEnumerator waitForPotion () {
		while (drink.gameObject.GetActive ()) {
			yield return null;
		}	
		print ("Consumed");
		msg.text = ("That's better.");
		next.gameObject.SetActive (true);
	}

	IEnumerator waitForMovement() {
		print ("Waiting for magic");
		while (!(Input.GetAxis("Horizontal") > 0)) {
			yield return null; 
		}
		print ("Key pressed");
		msg.text = "That's it!";
		next.gameObject.SetActive (true);
	}

	IEnumerator waitForMagic() {
		print ("Waiting for magic");
		while (!(Input.GetAxis("Mouse") > 0)) {
			yield return null; 
		}
		print ("Key pressed");
		msg.text = "Nice!";
		next.gameObject.SetActive (true);
	}

	IEnumerator waitForSpace() {
		print ("Starting");
		while (!Input.GetButtonDown("Melee")) {
			yield return null; 
		}
		print ("Key pressed");
		msg.text = "Well done!";
		next.gameObject.SetActive (true);
	}

	// Update is called once per frame
	void Update () {

		objsInRange = Physics2D.OverlapCircleAll (transform.position, 100f);
		GameObject nearestPlayer = FindNearestPlayer ();

		if (rb2d.velocity.x > 0 || rb2d.velocity.y > 0) {
			animator.SetTrigger ("Walk");	
		}

		float x = rb2d.velocity.x;
		if (x > 0 && !facingRight || x < 0 && facingRight) {
			// Flip sprite
			sprite.transform.localScale = new Vector3 (sprite.transform.localScale.x * -1,
				sprite.transform.localScale.y,
				sprite.transform.localScale.z);
			facingRight = !facingRight;
		}

		if (nearestPlayer != null && !stay) {
			// Go to player
			rb2d.velocity = touchingPlayer ? Vector3.zero :
				(nearestPlayer.transform.position - transform.position).normalized * speed;
		}
		if (stay) {
			rb2d.velocity = Vector3.zero;
		}

	}

	void OnTriggerStay2D (Collider2D other) {
		if (other.CompareTag ("Player")) {
			touchingPlayer = true;
		} else if (other.CompareTag ("Stop")) {
			stay = true;
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		if (other.CompareTag ("Player")) {
			touchingPlayer = false;
		}
	}
}
