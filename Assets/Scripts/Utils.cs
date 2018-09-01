using UnityEngine;
using System.Collections;
using System;

public class Utils : MonoBehaviour

{
	private static float TILE_WIDTH = 6f;
	private static float TILE_HEIGHT = 3.11f;
	/* 
	 * TRANSLATE A 2D X COORDINATE INT0 AN ISO X COORDINATE
	 * Takes width, height of the sprite (floor tile sprite?)
	 */

	public static float isoX(float W, float H, float i, float j) {
		return (j * W / 2) + (i * W / 2);	
	}


	/*
	 * TRANSLATE A 2D Y COORDINATE INT0 AN ISO Y COORDINATE
	 * Takes width, height of the sprite (floor tile sprite?)
	 */

	public static float isoY(float W, float H, float i, float j) {
		return (i * H / 2) - (j * H / 2);
	}

	public static Vector3 isoVector3(Vector3 vector) {
		Vector3 isoVector;
		isoVector.x = isoX (TILE_WIDTH, TILE_HEIGHT, vector.x, vector.y);
		isoVector.y = isoY (TILE_WIDTH, TILE_HEIGHT, vector.x, vector.y);
		isoVector.z = vector.z;
		return isoVector;
	}

	private static float cos_30 = Mathf.Cos(Mathf.PI/6);
	private static float sin_30 = Mathf.Sin(Mathf.PI/6);

	public static Vector3 isoProjectile(Vector3 v) {
		Vector3 p;
		p.x = v.x * cos_30 + v.y * sin_30;
		p.y = v.y * cos_30 + v.x * sin_30;
		p.z = v.z;
		return p;
	}

	public static void NetworkDestroy (GameObject gameObj) {
		PhotonNetwork.Destroy (gameObj);
	}

	public static IEnumerator NetworkDestroy (GameObject gameObj, float timeToWait) {
		yield return new WaitForSeconds (timeToWait);
		PhotonNetwork.Destroy (gameObj);
	}

    public static string toTime (float time) {
        return string.Format ("{0}:{1:00}", (int) time / 60, (int) time % 60);
    }

}

