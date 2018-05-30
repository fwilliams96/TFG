using UnityEngine;

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
                Debug.Log("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
				MessageManager.Instance.ShowMessage("El usuario o la contraseña son incorrectos",1.5f);
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
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
				MessageManager.Instance.ShowMessage("El usuario ya se encuentra registrado",1.5f);
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

	public Firebase.Auth.FirebaseUser User
    {
        get
        {
			return user;
        }
    }

}
