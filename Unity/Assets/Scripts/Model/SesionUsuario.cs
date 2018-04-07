using UnityEngine;

public class SesionUsuario
{
    private static SesionUsuario instance;
    private Firebase.Auth.FirebaseAuth auth;
    private Firebase.Auth.FirebaseUser user;
    private bool registro;
    //private BaseDatos baseDatos;

    private SesionUsuario()
    {
        InitializeFirebase();
    }

    /*public BaseDatos BaseDatos
    {
        get
        {
            return baseDatos;
        }
    }*/

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
        Debug.Log("Setting up Firebase Auth");
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
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
                    BaseDatos.Instance.InicializarJugador(user.UserId);
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
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        return user != null;
    }

    public void Login(string email, string password)
    {
        registro = false;
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
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

        });
    }
    
    public void Registro(string email, string password)
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
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            // Firebase user has been created.
            Firebase.Auth.FirebaseUser newUser = task.Result;
            registro = false;
            BaseDatos.Instance.crearJugador = true;
            BaseDatos.Instance.InicializarJugador(newUser.UserId);
            //BaseDatos.Instance.CrearJugador(newUser.UserId);
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });
    }

    public string UserID
    {
        get
        {
            if(user != null)
                return user.UserId;
            //TODO
            return "0";
        }
    }

}
