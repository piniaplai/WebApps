using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialGameManager : MonoBehaviour {

    public static TutorialGameManager instance = null;
	private TutorialBoardManager board;

	void Start ()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
		board = GetComponent<TutorialBoardManager> ();
		InitGame ();
	}

	void InitGame() {
    }

}