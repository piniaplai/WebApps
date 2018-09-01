using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
	private BoardManager board;

	void Start ()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
		board = GetComponent<BoardManager> ();
		InitGame ();
	}

	void InitGame() {
		board.SetupScene ();
    }

}