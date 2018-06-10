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
		/*ProgressBar.Instance.MostrarBarraProgreso ();
		if (!BaseDatos.Instance.BaseDatosInicializada)
			BaseDatos.Instance.InicializarBase (Callback);
		//Recursos.InicializarCartas();*/
		ProgressBar.Instance.MostrarBarraProgreso ();
		StartCoroutine (CheckForConnection ());
	}

	private IEnumerator CheckForConnection() {
		float startTime = Time.time;
		while (!ExistsConnection() && Time.time < startTime + 6.0f) {
			yield return new WaitForSeconds(0.1f);
		}
		if (existsConnection) {
			//MessageManager.Instance.ShowMessage ("Hay conexion",1.5f);
			if (!BaseDatos.Instance.BaseDatosInicializada)
				BaseDatos.Instance.InicializarBase (Callback);
			//Recursos.InicializarCartas();
		} else {
			ProgressBar.Instance.OcultarBarraProgreso ();
			MessageManager.Instance.ShowMessage ("No se ha podido establecer conexión. Vuelva a intentarlo más tarde",4f);
		}
	}

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

	/*private IEnumerator CheckConnectionToMasterServer() {
		Ping pingMasterServer = new Ping("67.225.180.24");
		Debug.Log(pingMasterServer.ip);
		float startTime = Time.time;
		while (!pingMasterServer.isDone && Time.time < startTime + 5.0f) {
			yield return new WaitForSeconds(0.1f);
		}
		if(pingMasterServer.isDone) {
			existsConnection = true;
		}
	}*/

	/*public static string GetHtmlFromUri(string resource)
	{
		string html = string.Empty;
		HttpWebRequest req = (HttpWebRequest)WebRequest.Create(resource);
		using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
		{
			bool isSuccess = (int)resp.StatusCode < 299 && (int)resp.StatusCode >= 200;
			if (isSuccess)
			{
				using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
				{
					html = reader.ReadToEnd();
				}
			}
		}
		return html;
	}*/



	public void Callback(string message)
    {
		if ("".Equals (message)) {
			if (SesionUsuario.Instance.ExisteSesion ()) {
				BaseDatos.Instance.RecogerJugador (SesionUsuario.Instance.User.UserId, CargaJugador);
			} else {
				CargarEscenaLogin ();
			}
		} else {
			ProgressBar.Instance.OcultarBarraProgreso ();
			MessageManager.Instance.ShowMessage (message,1.5f);
		}
        
    }

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
