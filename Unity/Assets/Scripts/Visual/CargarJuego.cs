using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Net;

public class CargarJuego : MonoBehaviour {

	private bool existsConnection;

    void Awake()
    {
    }

	void Start()
	{
		ProgressBar.Instance.MostrarBarraProgreso ();
		StartCoroutine (CheckForConnection ());
	}

	/// <summary>
	/// Carga el juego o no según haya internet.
	/// </summary>
	/// <returns>The for connection.</returns>
	private IEnumerator CheckForConnection() {
		float startTime = Time.time;
		while (!ExistsConnection() && Time.time < startTime + 6.0f) {
			yield return new WaitForSeconds(0.1f);
		}
		if (existsConnection) {
			if (!BaseDatos.Instance.BaseDatosInicializada)
				BaseDatos.Instance.InicializarBase (Callback);
		} else {
			ProgressBar.Instance.OcultarBarraProgreso ();
			MessageManager.Instance.ShowMessage ("No se ha podido establecer conexión. Vuelva a intentarlo más tarde",4f);
		}
	}

	/// <summary>
	/// Mira si existe conexión a internet.
	/// </summary>
	/// <returns><c>true</c>, if connection was existsed, <c>false</c> otherwise.</returns>
	public bool ExistsConnection()
	{
		string html = string.Empty;
		HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://google.com");
		bool isSuccess = false;
		try{
			using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
			{
				isSuccess = (int)resp.StatusCode < 299 && (int)resp.StatusCode >= 200;

			}
		}catch{
			
		}
		existsConnection = isSuccess;
		return isSuccess;
	}

	/// <summary>
	/// Funcion de callback una vez el juego ha descargado la informacion completamente o no.
	/// </summary>
	/// <param name="message">Message.</param>
	public void Callback(string message)
    {
		if ("".Equals (message)) {
			//Descomentar esta linea y comentar las siguientes para inicializar Firebase con las cartas nuevas 
			//Recursos.AñadirCartasAFirebase();
			//Descomentar esta linea y comentar las demas para subir solo una carta a Firebase, cambiar familia y nombre del XML
			//Recursos.AñadirCartaAFirebase("Magica","Destructor");
			//Descomentar para no iniciar el juego y añadir a Firebase las nuevas cartas
			IniciarJuego();
		} else {
			ProgressBar.Instance.OcultarBarraProgreso ();
			MessageManager.Instance.ShowMessage (message,1.5f);
		}
        
    }

	/// <summary>
	/// Inicia el juego recogiendo los datos del jugador.
	/// </summary>
	private void IniciarJuego(){
		if (SesionUsuario.Instance.ExisteSesion ()) {
			BaseDatos.Instance.RecogerJugador (SesionUsuario.Instance.User.UserId, CargaJugador);
		} else {
			CargarEscenaLogin ();
		}
	}

	/// <summary>
	/// Carga la escena menú.
	/// </summary>
	/// <param name="message">Message.</param>
	public void CargaJugador(string message){
		if ("".Equals (message))
			CargarEscenaMenu ();
		else {
			ProgressBar.Instance.OcultarBarraProgreso ();
			MessageManager.Instance.ShowMessage(message,1.5f);
		}
	}

    public void CargarEscenaMenu()
    {
        SceneManager.LoadSceneAsync("Menu");
    }

    public void CargarEscenaLogin()
    {
        SceneManager.LoadSceneAsync("Login");
    }
		
	// Update is called once per frame
	void Update () {
		
	}
}
