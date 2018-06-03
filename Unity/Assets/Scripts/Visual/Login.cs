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
		/*TODO email.text = "fwmcomputer@gmail.com";
		password.text = "monardez12";
		Logearse ();*/
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Logearse()
    {
        try
        {
            SesionUsuario.Instance.Login(email.text, password.text, LogeadoCompleto);
        }
        catch(System.Exception e)
        {
            Debug.Log(e.Message);
        }
        
    }

    public void LogeadoCompleto()
    {
        SceneManager.LoadSceneAsync("Menu");

    }

}
