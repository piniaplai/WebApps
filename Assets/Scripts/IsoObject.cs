using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IsoObject : MonoBehaviour {

	[SerializeField] private float floorHeight;
	private float                  spriteBottom;
	private float                  spriteMidW;
	private float         tan30 = Mathf.Tan(Mathf.PI / 5);

	void Awake()
	{
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
		spriteBottom  = spriteRenderer.bounds.size.y * 0.5f;
		spriteMidW   = spriteRenderer.bounds.size.x * 0.5f;
	}

	protected void UpdatePosition () {
		transform.position = new Vector3 (
			transform.position.x,
			transform.position.y,
			(transform.position.y - spriteBottom + floorHeight) * tan30
		);
	} 

	public float GetFloorHeight () {
		return floorHeight;
	}

	// Draws a line in editor to help set up the floor variable visually
	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;

		Vector3 floorHeightPos = new Vector3
			(
				transform.position.x,
				transform.position.y - spriteBottom + floorHeight,
				transform.position.z
			);

		Gizmos.DrawLine(floorHeightPos + Vector3.left * spriteMidW, floorHeightPos + Vector3.right * spriteMidW);
	}

	/*
	 * Alternative mechanism of updating sortingOrder instead of the Z axis
   	void Update()
   	{
		Renderer renderer = GetComponent<Renderer>();
       	renderer.sortingOrder = -(int)(transform.position.y * IsometricRangePerYUnit);
   	}
   	*/
}
