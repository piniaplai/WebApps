using UnityEngine;
using System;
using UnityEngine.UI;

public class LobbyClock : Photon.MonoBehaviour {

    private float time;
    private bool running;
    private Text clockText;

    public GameObject clockTextObject;
    public GameObject clockInputObject;
    public SessionInfo sessInfo;

    void Start () {
        running = false;
        clockText = clockTextObject.GetComponent<Text> ();
    }

    void Update () {
        if (running) {
            clockText.text = Utils.toTime (time);
            time -= Time.deltaTime;

            if (time <= 0) {
                stopTimer ();
                sessInfo.IncTokenNumber ();
            }
        }
    }

    public void startTimer () {
        int minute;
        Int32.TryParse (clockInputObject.GetComponent<InputField> ().text, out minute);
        time = minute * 60 + 1;
        running = true;
        clockInputObject.SetActive (false);
        clockTextObject.SetActive (true);
    }

    public void stopTimer () {
        running = false;
        clockInputObject.SetActive (true);
        clockTextObject.SetActive (false);
    }

}
