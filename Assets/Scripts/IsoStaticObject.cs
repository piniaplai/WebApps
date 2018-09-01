
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]	
public class IsoStaticObject : IsoObject {

	private bool isSorted = false;

	// Update the position in the Z axis:
	void Update()
	{
		// Run in editor mode for scenes constructed manually
		if (Application.isEditor) {
			base.UpdatePosition ();
		}

		// Only run once in game mode for scenes that are script generated
		if (!isSorted) {
			base.UpdatePosition ();
			isSorted = true;
		}

	}
}