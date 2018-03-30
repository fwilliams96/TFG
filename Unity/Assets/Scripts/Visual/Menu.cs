using Firebase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Unity.Editor;

public class Menu : MonoBehaviour {

    public GameObject inventario;
    public GameObject batalla;
    public GameObject perfil;

    void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    // Use this for initialization
    void Start () {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://battle-galaxy-cda70.firebaseio.com/");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Inventario()
    {
        //SceneManager.LoadScene("Inventario");
        inventario.SetActive(true);
        batalla.SetActive(false);
    }

    public void Batalla()
    {
        //SceneManager.LoadScene("Batalla");
        batalla.SetActive(true);
        inventario.SetActive(false);
    }
}
