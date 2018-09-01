using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameClock : Photon.MonoBehaviour {

    public float time;
    public GameObject scorePrefab;
    public Transform gridTransform;
    public GameObject resultsPanel;

    private Text clockText;
    private bool gameOver;

	void Start () {
        clockText = GetComponentInChildren<Text> ();
        gameOver = false;
	}
	
	void Update () {
        clockText.text = Utils.toTime (time);
        
        if (!gameOver) {
            time -= Time.deltaTime;
            if (time <= 0) {
                gameOver = true;

                // End game screen
                resultsPanel.SetActive (true);
                foreach (PhotonPlayer player in PhotonNetwork.playerList) {
                    GameObject newPlayerScore = Instantiate (scorePrefab);
                    newPlayerScore.transform.SetParent (gridTransform);

                    GameObject name = newPlayerScore.transform.Find ("Name").gameObject;
                    name.GetComponent<Text> ().text = player.NickName;
                    GameObject score = newPlayerScore.transform.Find ("Score").gameObject;
                    score.GetComponent<Text> ().text = player.GetScore ().ToString ();
                }
            }
        }
    }

    public void backToLobby () {
        PhotonNetwork.LeaveRoom ();
        SceneManager.LoadSceneAsync ("Lobby", LoadSceneMode.Single);
    }

}
