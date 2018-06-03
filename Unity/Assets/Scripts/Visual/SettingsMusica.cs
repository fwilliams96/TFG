using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMusica : MonoBehaviour {

	public Toggle toggleOn;
	public Toggle toggleOff;

	// Use this for initialization
	void Start () {
		if (Settings.Instance.Musica) {
			toggleOn.isOn = true;
			toggleOff.isOn = false;
		} else {
			toggleOn.isOn = false;
			toggleOff.isOn = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
