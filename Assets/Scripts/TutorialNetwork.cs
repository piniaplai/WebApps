using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialNetwork : Photon.PunBehaviour {

    public GameObject playerPrefab;
    public GameObject magePrefab;
	public GameObject enemyPrefab;
    private List<GameObject> players;

    void Awake () {
        players = new List<GameObject> ();
    }

    void Start () {
        GameObject newPlayer = PhotonNetwork.Instantiate (playerPrefab.name, Vector3.zero, Quaternion.identity, 0);
        Vector2 playerSpawnPoint = new Vector2 (-27.8f, -4);
        newPlayer.transform.position = playerSpawnPoint;
        players.Add (newPlayer);
        newPlayer.GetComponent<PlayerHealth> ().SetSpawn (playerSpawnPoint);

        GameObject mage = PhotonNetwork.Instantiate (magePrefab.name, Vector3.zero, Quaternion.identity, 0);
        Vector2 mageSpawnPoint = new Vector2 (-13.7f, 2.12f);
        mage.transform.position = mageSpawnPoint;

        GameObject enemy = PhotonNetwork.Instantiate (enemyPrefab.name, Vector3.zero, Quaternion.identity, 0);
        Vector2 enemySpawnPoint = new Vector2 (-18.92f, -5.93f);
		enemy.transform.position = enemySpawnPoint;
    }

    public List<GameObject> GetPlayers () {
        return players;
    }

}
