using UnityEngine;
using Firebase.Auth;
using System;

public class SesionUsuario
{
    private static SesionUsuario instance;
    private Firebase.Auth.FirebaseAuth auth;
    private Firebase.Auth.FirebaseUser user;
    private bool registro;
	public delegate void CallBack(string message);
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

	public void Login(string email, string password, CallBack callBack)
    {
        registro = false;
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
				Debug.Log("Excepcion: "+task.Exception);
				Debug.Log("SignInWithEmailAndPasswordAsync was canceled." +task.Exception);
				callBack.Invoke(GetErrorMessage(task.Exception));

            }
            if (task.IsFaulted)
            {
				
				Debug.Log("Excepcion: "+task.Exception);
				Debug.Log("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
				callBack.Invoke(GetErrorMessage(task.Exception));               
            }

            user = task.Result;
			BaseDatos.Instance.RecogerJugador(user.UserId, callBack);
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
				Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
				callBack.Invoke(GetErrorMessage(task.Exception));

			}   
            if (task.IsFaulted)
            {
				Debug.Log("Excepcion: "+task.Exception);
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
				callBack.Invoke(GetErrorMessage(task.Exception));
			}

            user = task.Result;
            registro = false;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
               user.DisplayName, user.UserId);
            BaseDatos.Instance.CrearJugador(user.UserId, callBack);
           
        });


    }

	public static string GetErrorMessage(Exception exception)
	{
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
		case AuthError.UserNotFound:
			message = "¡Aún no te encuentras registrado!";
			break;
		default:
			message = "Ocurrió un error";
			break;
		}
		return message;
	}

	public void CerrarSesión(){
		auth.SignOut ();
		user = null;
		BaseDatos.Instance.CerrarSesión ();
	}

	public Firebase.Auth.FirebaseUser User
    {
        get
        {
			return user;
        }
    }

}
