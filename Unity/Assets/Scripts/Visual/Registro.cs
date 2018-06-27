using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Registro : MonoBehaviour {

    public InputField email;
    public InputField password;
    public InputField password2;
	public Button crear;
	public Button back;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// Permite registrarse en la base de datos.
	/// </summary>
    public void Registrarse()
    {
		if (!camposVacios () && password.text.Equals (password2.text)) {
			SesionUsuario.Instance.Registro (email.text, password.text, Callback);
			ProgressBar.Instance.MostrarBarraProgreso();
			crear.interactable = false;
			back.interactable = false;
            
		} else if (camposVacios ()) {
			MessageManager.Instance.ShowMessage ("No pueden haber campos vacíos", 1.5f);
			Debug.Log ("Error de campos");
		} else {
			MessageManager.Instance.ShowMessage ("Ambas contraseñas deben coincidir", 1.5f);
			Debug.Log ("Error de contraseñas");
		}
        
        
    }

	/// <summary>
	/// Carga la escena de menu o no.
	/// </summary>
	/// <param name="message">Message.</param>
	public void Callback(string message)
    {
		if ("".Equals (message)) {
			SceneManager.LoadSceneAsync ("Menu");
		} else {
			ProgressBar.Instance.OcultarBarraProgreso ();
			crear.interactable = true;
			back.interactable = true;
			MessageManager.Instance.ShowMessage (message, 1.5f);
		}
        
    }

    public bool camposVacios()
    {
        return "".Equals(email.text) || "".Equals(password.text) || "".Equals(password2.text);
    }
}
