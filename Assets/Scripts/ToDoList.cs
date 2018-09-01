using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ToDoList : MonoBehaviour {

	public List<ToDoToggle> toDos;
    public Transform listPanel;
    public InputField inputField;
    public Toggle tog;
	public Canvas exportTextCanvas;
	public Canvas importTextCanvas;
	public Text importText;
	public string importString;

	// Use this for initialization
	void Start () {
		toDos = new List<ToDoToggle> ();
		if (PlayerPrefs.HasKey ("todoList")) {
			importTextCanvas.gameObject.GetComponentInChildren<InputField> ().text 
				= PlayerPrefs.GetString ("todoList");
		}
		LeaveCanvas ();
		ImportTextSubmit ();
	}

	public int Size () {
		return toDos.Count;
	}

	public void RemoveAll () {
		foreach (ToDoToggle tdt in toDos) {
			Destroy (tdt.GetToggle ().gameObject);
			Destroy (tdt.gameObject);
		}
		toDos.Clear ();
	}

	public void RemoveToDo (ToDoToggle rm) {
		toDos.Remove (rm);
		Destroy (rm.GetToggle ().gameObject);
		Destroy (rm.gameObject);
	}

    public void AddItemFromUI() {
        string description = inputField.text;
		AddItem (description);
    }

	void AddItem (string description) {
		Toggle newToggle = Instantiate (tog);
		ToDoToggle tdt = newToggle.GetComponent<ToDoToggle> ();
		Toggle.ToggleEvent toggleEvent = newToggle.onValueChanged;
		newToggle.onValueChanged = new Toggle.ToggleEvent ();
		newToggle.isOn = false;
		newToggle.onValueChanged = toggleEvent;
		newToggle.GetComponentInChildren<Text> ().text = description;
		newToggle.transform.SetParent (listPanel);
		tdt.SetToggle (newToggle);
		tdt.SetDescription (description);
		toDos.Add (tdt);
	}

	public void ExportButton () {
		ExportXML ();
		importTextCanvas.gameObject.SetActive (false);
		exportTextCanvas.gameObject.SetActive(true);
	}

	public void ExportXML () {
		XElement xElement = new XElement ("items", toDos.Select(item => 
			new XElement("todo", item.description)));
		exportTextCanvas.gameObject.GetComponentInChildren<InputField> ().text 
		= xElement.ToString ();
	}


	public void ImportButton () {
		importTextCanvas.gameObject.SetActive(true);
		exportTextCanvas.gameObject.SetActive (false);
	}

	public void ImportTextSubmit () {
		importString = importText.text;
		ImportXML ();
	}

	public void ImportXML () {
		RemoveAll ();
		XDocument xDocument = new XDocument();
		if (importString != "") {
			try {
				xDocument = XDocument.Parse(importString);
				List<string> strings = (from str in xDocument.Descendants("todo") select str.Value).ToList ();
				toDos = new List<ToDoToggle> ();
				foreach (string str in strings){
					AddItem (str);
				}
			} catch (XmlException e) {
				LobbyErrorText.SetErrorCode (SessionInfo.BADXMLCODE);
			}
		}
	}

	public void SaveButton () {
		ExportXML ();
		string savedString = exportTextCanvas.gameObject.GetComponentInChildren<InputField> ().text;
		if (savedString.Contains ("'")) {
			savedString.Replace("'","''");
		}
		if (PlayerPrefs.HasKey ("todoList")) {
			PlayerPrefs.SetString ("todoList", savedString);
		}
		QueryInfo.CallSetXML (this);
	}

	public void LeaveCanvas () {
		importTextCanvas.gameObject.SetActive (false);
		exportTextCanvas.gameObject.SetActive (false);
	}
}
