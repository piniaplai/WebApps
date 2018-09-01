using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerHealth : MonoBehaviour {

	public int maximum = 100;
	public int current = 100;

	private Image healthBar;

	// Use this for initialization
	void Start () {
		healthBar = transform.Find ("TowerCanvas").Find ("HealthBarBG").Find ("Health").GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		healthBar.fillAmount = (float) current / maximum;
	}

	public void Loyalty() {
		healthBar.color = Color.green;
	}

	[PunRPC]
	public void UpdateHealth (int update) {
		current += update;
		if (current < 0) {
			current = 0;
		} else if (current > maximum) {
			current = maximum;
		}

		if (maximum < 1) {
			maximum = 1;
		}
	}

	public void Damage (int damage) {
		GetComponentInParent<TowerAI> ().photonView.RPC ("UpdateHealth", PhotonTargets.All, -damage);
	}

}