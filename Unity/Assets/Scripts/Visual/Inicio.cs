using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Inicio : MonoBehaviour {

    public GameObject menuLogin;
    public GameObject menuRegistrarse;

    void Awake()
    {
    }

    // Use this for initialization
    void Start () {
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
