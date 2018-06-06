using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {

	public GameObject LoadingBar;
	public int speed = 5;
	private float currentAmount;


	// Use this for initialization
	void Start () {
		this.currentAmount = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (currentAmount < 100f) {
			currentAmount += speed * Time.deltaTime;

		} else {
			currentAmount = 0;
		}
		LoadingBar.GetComponent<Image> ().fillAmount = currentAmount / 100f;
	}
}
