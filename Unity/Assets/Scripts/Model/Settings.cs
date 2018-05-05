using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour {

	public static Settings Instance = null;
	//private static bool created = false;

	public enum TIPO_NUMERO
	{
		ENTERO,
		RACIONAL
	}

	public TIPO_NUMERO Batalla;
	public TIPO_NUMERO Items;

	private Settings(){	}

	void Awake(){
		if (Instance == null) {
			DontDestroyOnLoad(this.gameObject);
			Instance = this;
		}
			
	}

	/*public static Settings Instance{
		get{
			if (instance == null)
				instance = new Settings ();
			return instance;
		}
	}*/

	public static string ObtenerFraccion(int numerador, int denominador){
		int gcd = MayorDenominadorComun (numerador, denominador);
		return (numerador / gcd).ToString () + "/" + (denominador / gcd).ToString ();
	}

	private static int MayorDenominadorComun (int a, int b) { 
		while (b != 0) { 
			int t = b; 
			b = a % b; 
			a = t; 
		} 
		return a; 
	}


}
