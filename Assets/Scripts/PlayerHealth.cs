using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Photon.MonoBehaviour {

	private Vector3 spawn;
	private int maximum = 100;
	public int current = 0;
	public int currScore;

	private Image healthBarFloat;
	private float healthBarLength;
	private Animator animator;

	void Awake() {
		animator = transform.Find("Sprite").GetComponent<Animator> ();
	}

	// Use this for initialization
	void Start () {
		healthBarLength = Screen.width / 2;
		current = maximum;
		healthBarFloat = transform.Find ("PlayerCanvas").Find ("HealthBarBG").Find ("Health").GetComponent<Image> ();
	}

	void Update () {
		healthBarLength = (Screen.width / 2) * (current / (float) maximum);
		healthBarFloat.fillAmount = (float) current / maximum;
	}

	public void Damage (int damage) {
		photonView.RPC ("UpdateHealth", PhotonTargets.All, -damage);
		photonView.RPC ("SetAnimation", PhotonTargets.All, "Hit");
	}

	[PunRPC]
	public void UpdateHealth(int update)  {
		current += update;
		if (current < 0) {
			current = 0;
		} else if (current > maximum) {
			current = maximum;
		}

		if (maximum < 1) {
			maximum = 1;
		}
		
		if (current <= 0) {
			photonView.RPC ("SetAnimation", PhotonTargets.All, "Dying");
			Respawn ();
		}
	}

	public void UpdateScore(int update) {
		currScore += update;
		if (currScore <= 0) {
			currScore = 0;
		}

        PhotonNetwork.player.SetScore (currScore);
	}

	void Respawn() {
        UpdateScore (-100);
		current = maximum;
		transform.position = spawn;
		UpdateHealth (0);
	}

	void OnGUI() {
        if (photonView.isMine) {
            GUI.Box (new Rect (Screen.width - (Screen.width / 5) - 5, 10, Screen.width / 5, 20), "Score: " + currScore);
        }
	}

	public Vector3 GetSpawn () {
		return spawn;
	}

	public void SetSpawn (Vector3 spawn) {
		this.spawn = spawn;
	}
}

