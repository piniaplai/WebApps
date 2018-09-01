using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board {

	private BoardObject[,] boardMatrix;
	private int rows;
	private int cols;


	public Board (int rows, int cols) {
		this.rows = rows;
		this.cols = cols;
		boardMatrix = new BoardObject[rows, cols];
	}

	public void AddBoardObject (int row, int col, GameObject gameObj) {
		boardMatrix[row, col] = new BoardObject (gameObj);
	}

	public bool SquareInUse (int row, int col) {
		return boardMatrix[row, col] != null;
	}

	public bool SquareHasTag (int row, int col, string tag) {
		if (!SquareInUse(row, col)) {
			return false;
		}
		return boardMatrix[row, col].HasTag (tag);
	}

	// Checks whether there is an object with tag objectTag in the
	// surrounding tiles with row, col at the centre, with dist
	// being the distance to check between objects

	public bool HasObjectInRange (int row, int col, string objectTag, int minDist) {
		for (int i = -minDist; i <= minDist; i++) {
			for (int j = -minDist; j <= minDist; j++) {

				// Ensure no array access violations via short circuiting
				if (0 <= row + i && row + i < rows
					&& 0 <= col + j && col + j < cols
					&& SquareHasTag (row + i, col + j, objectTag)) {
					return true;
				}
			}
		}
		return false;
	}

	private class BoardObject {
		int row;
		int col;
		GameObject gameObj;

		public BoardObject (GameObject gameObj) {
			this.gameObj = gameObj;
		}

		public bool HasTag (string tag) {
			return gameObj.CompareTag (tag);
		}
	}
}
