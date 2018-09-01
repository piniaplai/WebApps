using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyManager : Photon.PunBehaviour {

    private string VERSION = "1";

    private bool tutorial;

    public GameObject roomPrefab;
    public InputField roomNameInput;
    public InputField maxPlayerInput;
    public Transform gridTransform;
    public GameObject lobbyPanel;
    public GameObject inRoomPanel;
    private List<GameObject> roomPrefabList;

    void Start () {
        PhotonNetwork.ConnectUsingSettings (VERSION);
        roomPrefabList = new List<GameObject> ();
        PhotonNetwork.automaticallySyncScene = true;
        tutorial = false;
    }

    public void createRoomFromInput () {
		SessionInfo sessionInfo = SessionInfo.GetInstance ();
		if (sessionInfo.IsGuest ()) {
			LobbyErrorText.SetErrorCode (SessionInfo.GUESTCREATE);
			return;
		}
		if (sessionInfo.CanPlayGame ()) {
			string roomName = roomNameInput.text;
            int maxPlayer = 0;
            if (Int32.TryParse (maxPlayerInput.text, out maxPlayer)) {
                Debug.Log ("Max player input is " + maxPlayer);
                PhotonNetwork.CreateRoom (roomName, new RoomOptions () { MaxPlayers = (byte) maxPlayer }, TypedLobby.Default);
            } else {
				LobbyErrorText.SetErrorCode (SessionInfo.MAXNOTINT);
            }
		} else {
			LobbyErrorText.SetErrorCode (SessionInfo.NOTOKENS);
		}
    }

    public void joinRoomFromList (string room) {
        if (!PhotonNetwork.JoinRoom (room)) {

        }
    }

    public void refreshRooms () {
        Debug.Log ("Refreshing rooms");

        for (int i = 0; i < roomPrefabList.Count; i++) {
            Destroy (roomPrefabList[i]);
        }
        roomPrefabList.Clear ();

        foreach (RoomInfo game in PhotonNetwork.GetRoomList ()) {
            GameObject newRoom = Instantiate (roomPrefab);
            newRoom.transform.SetParent (gridTransform);
            newRoom.GetComponentInChildren<Text> ().text = game.Name + "\t\t\t" + game.PlayerCount + "/" + game.MaxPlayers;
            newRoom.GetComponentInChildren<Button> ().onClick.AddListener (() => joinRoomFromList (game.Name));
            if (game.PlayerCount == game.MaxPlayers) {
                newRoom.GetComponentInChildren<Button> ().interactable = false;
            }
            roomPrefabList.Add (newRoom);
        }
    }

    public void joinRandomRoom () {
        PhotonNetwork.JoinRandomRoom ();
    }

    public override void OnJoinedLobby () {
        Debug.Log ("Joined Lobby");
        Invoke ("refreshRooms", 0.1f);
		PhotonNetwork.player.NickName = SessionInfo.GetInstance().GetSessionOwner();
    }

    public override void OnJoinedRoom () {
        Debug.Log ("Joined room");
        if (tutorial) {
            SceneManager.LoadSceneAsync ("Tutorial", LoadSceneMode.Single);
        } else {
            inRoomPanel.SetActive (true);
            lobbyPanel.SetActive (false);
        }
    }

    public override void OnPhotonRandomJoinFailed (object[] codeAndMsg) {
        Debug.Log ("Failed to join room");
        PhotonNetwork.CreateRoom (PhotonNetwork.player.NickName + "'s room",
                new RoomOptions () { MaxPlayers = 4 }, TypedLobby.Default);
    }
    public void startGame () {
        // PhotonNetwork.LoadLevel ("Main");
		SessionInfo.GetInstance ().DecTokenNumber ();
        SceneManager.LoadSceneAsync ("Tutorial", LoadSceneMode.Single);
    }

    public void launchTutorial () {
        PhotonNetwork.CreateRoom (null, new RoomOptions () { MaxPlayers = 1 }, TypedLobby.Default);
        tutorial = true;
        //SceneManager.LoadSceneAsync ("Tutorial", LoadSceneMode.Single);
        Debug.Log ("Created tutorial room");
    }

}
