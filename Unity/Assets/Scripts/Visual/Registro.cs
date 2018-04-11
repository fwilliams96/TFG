using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Registro : MonoBehaviour {

    public InputField email;
    public InputField password;
    public InputField password2;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Registrarse()
    {
        if(!camposVacios() && password.text.Equals(password2.text))
        {
            try
            {
                SesionUsuario.Instance.Registro(email.text, password.text, RegistroCompleto);
            }catch(System.Exception e)
            {
                Debug.Log(e.Message);
            }
            
        }
        else
        {
            Debug.Log("Error de campos");
        }
        
        
    }

    public void RegistroCompleto()
    {
        SceneManager.LoadSceneAsync("Menu");
    }

    public bool camposVacios()
    {
        return "".Equals(email.text) || "".Equals(password.text) || "".Equals(password2.text);
    }
}
