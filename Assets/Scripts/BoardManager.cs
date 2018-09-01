using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

	private int columns = 9;
	private int rows = 9;
	int towersToInstantiate = 5;
	int towerMinDist = 2;
	public GameObject potion;
	public GameObject[] playerTowers; // Player-ownable towers
	public GameObject[] worldObjects; // Other towers/buildings

	public GameObject[] floorTiles;
	public GameObject[] fenceBorders;
	public GameObject[] fenceCorners;
	private Board board; // Used to track objects placed on board

	private float tileWidth;
	private float tileHeight;

	private float renderFixHeight;

	private Transform boardHolder;
	private float worldObjectCoverage = 0.25f;
	private int worldObjectsToInstantiate;
	private int maxRelocationTries = 3;
	private float potionSpawnTime = 10; // 10 seconds

	void Awake () {
		board = new Board (rows, columns);
		worldObjectsToInstantiate = (int) (columns * rows * worldObjectCoverage);
		// 2d Coordinates
		tileWidth = floorTiles[0].GetComponent<SpriteRenderer> ().bounds.size.x;
		tileHeight = floorTiles[0].GetComponent<SpriteRenderer> ().bounds.size.y * 0.9f; // Ensure slight overlap
		renderFixHeight = floorTiles[2].GetComponent<SpriteRenderer> ().bounds.size.y * 0.5f;
	}

	void Start () {
		StartCoroutine (SpawnPotions ());
	}

	private IEnumerator SpawnPotions () {
		if (PhotonNetwork.isMasterClient) {
			while (true) {
				// Pick random spawn point for tower
				int row = Random.Range (0, rows);
				int col = Random.Range (0, columns);
				// If there is an object find a new place to spawn potion
				while (board.SquareInUse (row, col)) {
					row = Random.Range (0, rows);
					col = Random.Range (0, columns);
				}
				float x = Utils.isoX (tileWidth, tileHeight, row, col);
				float y = Utils.isoY (tileWidth, tileHeight, row, col) + renderFixHeight;
				PhotonNetwork.InstantiateSceneObject (potion.name, new Vector2 (x, y), Quaternion.identity, 0, null);
				yield return new WaitForSeconds (potionSpawnTime);
			}
		}
	}

	public void SetupScene () {
		GridSetup ();
		InitialisePlayerTowers ();
		InitialiseWorldObjects ();
	}

	private void GridSetup () {
		boardHolder = new GameObject ("Board").transform;
		for (int i = columns - 1; i >= 0; i--) {
			for (int j = 0; j < rows; j++) {

				// 2D -> ISO coordinate tranformation
				float x = Utils.isoX (tileWidth, tileHeight, i, j);
				float y = Utils.isoY (tileWidth, tileHeight, i, j);

				// Instantiate game object
				GameObject instance = Instantiate (floorTiles[2], new Vector3 (x, y, 0f), Quaternion.identity);
				instance.transform.SetParent (boardHolder);
			}
		}
	}

	private void InitialisePlayerTowers () {
		if (!PhotonNetwork.isMasterClient) {
			return;
		}

		for (int i = 0; i < towersToInstantiate; i++) {
			// Pick random spawn point for tower
			int row = Random.Range (1, rows - 1);
			int col = Random.Range (1, columns - 1);

			int tryNumber = 0;
			// If there is a tower already placed within 3x3, try to relocate this one
			while (board.HasObjectInRange (row, col, "Tower", towerMinDist) && tryNumber < maxRelocationTries) {
				row = Random.Range (1, rows - 1);
				col = Random.Range (1, columns - 1);
				tryNumber++;
			}

			// If we failed to successfully relocate the object
			// don't create a new tower
			if (tryNumber == maxRelocationTries) {
				continue;
			}

			InitialiseBoardObject (row, col, playerTowers[Random.Range (0, playerTowers.Length)]);
		}
	}
	private void InitialiseWorldObjects () {
		if (!PhotonNetwork.isMasterClient) {
			return;
		}

		for (int i = 0; i < worldObjectsToInstantiate; i++) {
			// Pick random spawn point for world object
			int row = Random.Range (1, rows - 1);
			int col = Random.Range (1, columns - 1);

			// If there is a tower within 3x3, or this is on
			// another object, try to relocate this object
			int tryNumber = 0;
			while ((board.HasObjectInRange (row, col, "Tower", 1)
				|| board.HasObjectInRange (row, col, "WorldObj", 0))
				&& tryNumber < maxRelocationTries) {
				row = Random.Range (1, rows - 1);
				col = Random.Range (1, columns - 1);
				tryNumber++;
			}

			// If we failed to successfully relocate the object
			// don't create it
			if (tryNumber == maxRelocationTries) {
				continue;
			}

			InitialiseBoardObject (row, col, worldObjects[Random.Range (0, worldObjects.Length)]);
		}
	}

	private void InitialiseBoardObject (int row, int col, GameObject gameObj) {
		float x = Utils.isoX (tileWidth, tileHeight, row, col);
		float y = Utils.isoY (tileWidth, tileHeight, row, col);

		// Move y position up so it renders in the correct place
		y += renderFixHeight;

		PhotonNetwork.InstantiateSceneObject (gameObj.name, new Vector3 (x, y, 0), Quaternion.identity, 0, null);

		board.AddBoardObject (row, col, gameObj);
	}


	internal Vector2 NewPlayerSpawn (GameObject player) {
		int row = Random.Range (1, rows - 1);
		int col = Random.Range (1, columns - 1);

		while (board.SquareInUse (row, col)) { 
			row = Random.Range (1, rows - 1);
			col = Random.Range (1, columns - 1);
		}

		board.AddBoardObject (row, col, player);

		float x = Utils.isoX (tileWidth, tileHeight, row, col);
		float y = Utils.isoY (tileWidth, tileHeight, row, col) + renderFixHeight;

		return new Vector2 (x, y);
	}
}
