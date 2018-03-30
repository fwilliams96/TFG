using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Inicio : MonoBehaviour {

    public GameObject menuLogin;
    public GameObject menuRegistrarse;
    private Firebase.Auth.FirebaseAuth auth;

    void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            SceneManager.LoadScene("Menu");
        }
    }

    // Use this for initialization
    void Start () {
        //Screen.orientation = ScreenOrientation.LandscapeLeft;
        //auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void NuevaCuenta()
    {
        menuLogin.SetActive(false);
        menuRegistrarse.SetActive(true);
    }

    public void Back()
    {
        menuLogin.SetActive(true);
        menuRegistrarse.SetActive(false);
    }
}
