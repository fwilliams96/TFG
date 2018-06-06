using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour {

    void Awake()
    {
    }

	void Start(){
		CargarEscenaMenu ();
	}
		
    public void CargarEscenaMenu()
    {
        SceneManager.LoadSceneAsync("Menu");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
