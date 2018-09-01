using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerAI : Photon.MonoBehaviour {

	public TowerHealth health;
	private float bulletSpeed;
	private float bulletLifetime;
	private float bulletCooldown;
	private float bulletCastTime;

	public GameObject monster;
	private int maxMonsterSpawn = 1;
	private int numMonsters = 0;

	public GameObject bullet;
	public Transform target;
	public Animator animator;
	public Transform shootPointE;
	public Transform shootPointW;
	public Transform spawnPoint;

	public GameObject owner;

	private Text ownerName;

	void Awake () {
		animator = gameObject.GetComponent<Animator> ();
		health = GetComponent<TowerHealth> ();
		ownerName = transform.Find ("TowerCanvas").Find ("OwnerName").GetComponent<Text> ();
		PhotonView.Instantiate (ownerName);
		bulletSpeed = 3.0f;
		bulletLifetime = 3.0f;
		bulletCastTime = 4.0f;
	}

	void Update () {
		if (health.current <= 0) {
			photonView.RPC ("TowerCapture", PhotonTargets.All, null);
		}
	}

	[PunRPC]
	private void TowerCapture () {
		owner = target.gameObject;
		owner.GetComponent<PlayerHealth> ().UpdateScore (100);
		health.current = health.maximum;
		if (photonView.isMine) {
			health.Loyalty ();
		}
		ownerName.text = "Tower [" + owner.GetComponent<Player> ().GetName () + "]";
	}

	public void Attack () {
		if (!PhotonNetwork.isMasterClient) {
			return;
		}
		if (numMonsters < maxMonsterSpawn) {
			GameObject newMonster = PhotonNetwork.InstantiateSceneObject (monster.name, spawnPoint.position, Quaternion.identity, 0, null);
			newMonster.GetComponent<Enemy> ().SetTowerParent (gameObject);
			numMonsters++;
		}

		bulletCooldown += Time.deltaTime;

		// If cooldown has passed and target is not tower's owner then attack
		if (bulletCooldown >= bulletCastTime && !IsOwner (target.gameObject)) {
			//Vector3 direction = Utils.isoProjectile(Utils.isoVector3(target.transform.position - shootPointSE.position));
			Vector3 direction = target.transform.position - shootPointE.position;
			direction.Normalize ();
			GameObject bulletClone = PhotonNetwork.InstantiateSceneObject (bullet.name,
									shootPointE.transform.position,
									Quaternion.Euler(direction), 0, null);
			bulletClone.transform.position = new Vector3 (shootPointE.transform.position.x,
											shootPointE.transform.position.y,
											target.transform.position.z);
			bulletClone.GetComponent<Rigidbody2D> ().velocity = direction * bulletSpeed;
			bulletCooldown = 0;
			StartCoroutine (Utils.NetworkDestroy (bulletClone, bulletLifetime));
		}
	}

	public bool IsOwner (GameObject player) {
		return player.gameObject.Equals (owner);
	}

	public void DecrementMonsterCount () {
		numMonsters--;
	}
}
