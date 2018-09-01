using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RoomManager : Photon.PunBehaviour {

    public GameObject playerNamePrefab;
    public Transform gridTransform;
    public GameObject lobbyPanel;
    public GameObject inRoomPanel;
    public Button startButton;
    private List<GameObject> playerNameList;

	void Start () {
        PhotonNetwork.automaticallySyncScene = true;

        playerNameList = new List<GameObject> ();
        refreshPlayerList ();

        if (!PhotonNetwork.isMasterClient) {
            // gameObject.GetComponentInParent<Button> ().gameObject.SetActive (false);
            startButton.gameObject.SetActive (false);
        }
	}
	
	void Update () {
        refreshPlayerList ();
        startButton.interactable = (playerNameList.Count == PhotonNetwork.room.MaxPlayers);
	}

    public void leaveRoomAndGoToLobby () {
        Debug.Log ("Leaving room");
        PhotonNetwork.LeaveRoom ();
        lobbyPanel.SetActive (true);
        inRoomPanel.SetActive (false);
    }

    public void startGame () {
        // PhotonNetwork.LoadLevel ("Main");
		SessionInfo.GetInstance ().DecTokenNumber ();
        SceneManager.LoadSceneAsync ("Main", LoadSceneMode.Single);
    }

    void refreshPlayerList () {
        for (int i = 0; i < playerNameList.Count; i++) {
            Destroy (playerNameList[i]);
        }
        playerNameList.Clear ();

        foreach (PhotonPlayer player in PhotonNetwork.playerList) {
            GameObject newPlayerName = Instantiate (playerNamePrefab);
            newPlayerName.transform.SetParent (gridTransform);
            newPlayerName.GetComponent<Text> ().text = player.NickName;
            playerNameList.Add (newPlayerName);
        }
    }

}
