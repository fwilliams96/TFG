﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneReloader: MonoBehaviour {

	public static SceneReloader Instance;

	void Awake(){
		Instance = this;
	}

    public void ReloadScene()
    {
        // Command has some static members, so let`s make sure that there are no commands in the Queue
        Debug.Log("Scene reloaded");
        // reset all card and creature IDs
        IDFactory.ResetIDs();
        IDHolder.ClearIDHoldersList();
        Comandas.Instance.Clear();
        Comandas.Instance.CompletarEjecucionComanda();
		Controlador.Instance.Clear();
		SceneManager.LoadSceneAsync("Loading");
    }
}
