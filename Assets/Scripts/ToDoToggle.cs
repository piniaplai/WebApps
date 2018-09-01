using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToDoToggle : MonoBehaviour {

	private ToDoList toDoList;
	private Toggle toggle;
	public GameObject toDoListGameObject;
	public string description;


	void Start () {
		toDoListGameObject = GameObject.FindGameObjectWithTag ("ToDoList");
		toDoList = toDoListGameObject.GetComponent<ToDoList> ();
	}

	public void SetDescription (string description) {
		this.description = description;
	}

	public void SetToggle (Toggle toggle) {
		this.toggle = toggle;
	}

	public Toggle GetToggle () {
		return toggle;
	}

	public void Toggle () {
		if (toggle.isOn) {
			SessionInfo.GetInstance ().IncTokenNumber ();
		} else {
			toDoList.RemoveToDo (this);
		}
		//print (toDoList.Size ());
	}
}
