using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {

	public static GameOver Instance;
	public GameObject GameOverPanel;
	public Text Experiencia;

	void Awake()
	{
		Instance = this;
		GameOverPanel.SetActive(false);
	}

	public void MostrarGameOver(int experiencia){
		GameOverPanel.SetActive(true);
		Experiencia.text = "¡OBTIENES " + experiencia + " PUNTOS DE EXPERIENCIA!";
	}
		
}
