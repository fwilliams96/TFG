using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBatalla : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Jugar()
    {
		//TODO revisar que esto funcione bien
		IDHolder.ClearIDHoldersList ();
        SceneManager.LoadScene("BattleGalaxy");
    }
}
