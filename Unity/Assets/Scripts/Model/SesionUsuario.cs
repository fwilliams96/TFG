using UnityEngine;
using Firebase.Auth;
using System;

public class SesionUsuario
{
    private static SesionUsuario instance;
    private Firebase.Auth.FirebaseAuth auth;
    private Firebase.Auth.FirebaseUser user;
    private bool registro;
    public delegate void CallBack();
    //private BaseDatos baseDatos;

    private SesionUsuario()
    {
        InitializeFirebase();
    }

    public static SesionUsuario Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SesionUsuario();
            }
            return instance;

        }
    }

    void InitializeFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		user = auth.CurrentUser;
        //auth.StateChanged += AuthStateChanged;
        //AuthStateChanged(this, null);
    }

    // Track state changes of the auth object.
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                if (!registro)
                {
                    Debug.Log("Signed in " + user.UserId);
                    //BaseDatos.Instance.InicializarJugador(user.UserId);
                }
               
            }
        }
    }

    void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }

    public bool ExisteSesion()
    {
        return user != null;
    }

    public void Login(string email, string password, CallBack callback)
    {
        registro = false;
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
				Debug.Log("Excepcion: "+task.Exception);
				MessageManager.Instance.ShowMessage(GetErrorMessage(task.Exception),1.5f);
				//MessageManager.Instance.ShowMessage("Ha habido algún error con el inicio de sesión",1.5f);
				Debug.Log("SignInWithEmailAndPasswordAsync was canceled." +task.Exception);
                return;
            }
            if (task.IsFaulted)
            {
				Debug.Log("Excepcion: "+task.Exception);
				MessageManager.Instance.ShowMessage(GetErrorMessage(task.Exception),1.5f);
				//MessageManager.Instance.ShowMessage("El usuario o la contraseña son incorrectos",1.5f);
                Debug.Log("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            user = task.Result;
            BaseDatos.Instance.RecogerJugador(user.UserId, callback);
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                user.DisplayName, user.UserId);

        });
    }
    
    public void Registro(string email, string password, CallBack callBack)
    {
        registro = true;
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
				Debug.Log("Excepcion: "+task.Exception);
				MessageManager.Instance.ShowMessage(GetErrorMessage(task.Exception),1.5f);
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
				Debug.Log("Excepcion: "+task.Exception);
				MessageManager.Instance.ShowMessage(GetErrorMessage(task.Exception),1.5f);
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }
            // Firebase user has been created.
            user = task.Result;
            registro = false;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
               user.DisplayName, user.UserId);
            BaseDatos.Instance.CrearJugador(user.UserId, callBack);
           
        });
    }

	public static string GetErrorMessage(Exception exception)
	{
		Debug.Log(exception.ToString());
		Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
		AggregateException ex = exception as AggregateException;
		if (ex != null) {
			Firebase.FirebaseException inner = ex.InnerExceptions[0] as Firebase.FirebaseException;
			return GetCodeErrorMessage((AuthError)inner.ErrorCode);
		}
		return exception.Message.ToString();
	}

	private static string GetCodeErrorMessage(AuthError errorCode)
	{
		var message = "";
		switch (errorCode)
		{
		case AuthError.AccountExistsWithDifferentCredentials:
			message = "Ya existe la cuenta con credenciales diferentes";
			break;
		case AuthError.MissingPassword:
			message = "Es necesario la contraseña";
			break;
		case AuthError.WeakPassword:
			message = "La contraseña es débil";
			break;
		case AuthError.WrongPassword:
			message = "La contraseña es incorrecta";
			break;
		case AuthError.EmailAlreadyInUse:
			message = "Ya existe una cuenta con ese correo electrónico";
			break;
		case AuthError.InvalidEmail:
			message = "Correo electrónico inválido";
			break;
		case AuthError.MissingEmail:
			message = "Hace falta el correo electrónico";
			break;
		default:
			message = "Ocurrió un error";
			break;
		}
		return message;
	}

	public Firebase.Auth.FirebaseUser User
    {
        get
        {
			return user;
        }
    }

}
