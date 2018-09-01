using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToDoListItem {

	public bool complete;
	public string description;

	public ToDoListItem (string description) {
		this.complete = false;
		this.description = description;
	}
}
