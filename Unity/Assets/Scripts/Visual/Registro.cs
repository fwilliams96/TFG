using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Registro : MonoBehaviour {

    private Firebase.Auth.FirebaseAuth auth;
    public InputField email;
    public InputField password;
    public InputField password2;

    // Use this for initialization
    void Start () {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Registrarse()
    {
        if(!camposVacios() && password.text.Equals(password2.text))
        {
            auth.CreateUserWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    return;
                }

                // Firebase user has been created.
                Firebase.Auth.FirebaseUser newUser = task.Result;
                Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                    newUser.DisplayName, newUser.UserId);
                SceneManager.LoadScene("Menu");
            });
        }
        else
        {
            Debug.Log("Error de campos");
        }
        
        
    }

    public bool camposVacios()
    {
        return "".Equals(email.text) || "".Equals(password.text) || "".Equals(password2.text);
    }
}
