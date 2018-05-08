using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour {

	public static GameOver Instance;
	public GameObject GameOverPanel;

	void Awake()
	{
		Instance = this;
		GameOverPanel.SetActive(false);
	}

	public void MostrarGameOver(){
		GameOverPanel.SetActive(true);
	}
		
}
