using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour {
    public InputField email;
    public InputField password;
	public Button login;
	public Button crearCuenta;

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
		ProgressBar.Instance.MostrarBarraProgreso();
		login.interactable = false;
		crearCuenta.interactable = false;
        SesionUsuario.Instance.Login(email.text, password.text, Callback);
    }

	public void Callback(string message)
    {
		if("".Equals(message)){
    		SceneManager.LoadSceneAsync("Menu");
		}else{
			ProgressBar.Instance.OcultarBarraProgreso ();
			login.interactable = true;
			crearCuenta.interactable = true;
			MessageManager.Instance.ShowMessage(message,1.5f);
		}

    }

}
