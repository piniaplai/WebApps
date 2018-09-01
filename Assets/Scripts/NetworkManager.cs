using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : Photon.PunBehaviour {

    public GameObject playerPrefab;
	private List<GameObject> players;

    void Awake () {
		players = new List<GameObject> ();
    }

    void Start () {
		GameObject newPlayer = PhotonNetwork.Instantiate (playerPrefab.name, Vector3.zero, Quaternion.identity, 0);
		Vector2 spawnPoint = GetComponent<BoardManager> ().NewPlayerSpawn (newPlayer);
		newPlayer.transform.position = spawnPoint;
        players.Add (newPlayer);
		newPlayer.GetComponent<PlayerHealth> ().SetSpawn (spawnPoint);
    }

    /*public override void OnJoinedLobby () {
        Debug.Log ("Joining random room...");
        PhotonNetwork.JoinRandomRoom ();
    }

    public override void OnPhotonRandomJoinFailed (object[] codeAndMsg) {
        Debug.Log ("Failed to join room");
        PhotonNetwork.CreateRoom (null, new RoomOptions () { MaxPlayers = 4 }, null);
    }

    public override void OnJoinedRoom () {
        Debug.Log ("Client in room");
    }*/

	public List<GameObject> GetPlayers () {
		return players;
	}

}
