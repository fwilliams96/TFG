using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour {
    public InputField email;
    public InputField password;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Logearse()
    {
        try
        {
            SesionUsuario.Instance.Login(email.text, password.text);
            //TODO mirar si hay excepcion, en ese caso no debo logear
            SceneManager.LoadScene("Menu");
        }catch(System.Exception e)
        {
            Debug.Log(e.Message);
        }
        
    }
}
