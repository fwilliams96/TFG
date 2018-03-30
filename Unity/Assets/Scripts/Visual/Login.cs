using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour {
    private Firebase.Auth.FirebaseAuth auth;
    public InputField email;
    public InputField password;

    // Use this for initialization
    void Start () {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Logearse()
    {
        auth.SignInWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
            SceneManager.LoadScene("Menu");
        });
       
    }
}
