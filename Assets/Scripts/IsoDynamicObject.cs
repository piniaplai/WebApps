using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]	
public class IsoDynamicObject : IsoObject {
	
	public void Sort() 
	{
		base.UpdatePosition ();
	}

	void Update()
	{
		// Run in editor mode for scenes constructed manually
		if (Application.isEditor) {
			base.UpdatePosition ();
		}
	}

}