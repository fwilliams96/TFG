using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ConfiguracionUsuario : MonoBehaviour {

	public static ConfiguracionUsuario Instance = null;
	//private static bool created = false;

	public enum TIPO_CONFIGURACION
	{
		ENTERO,
		FRACCION,
		PORCENTAJE
	}

	[Header("Colors")]
	public Color32 CardBodyStandardColor;
	public Color32 CardRibbonsStandardColor;
	public Color32 CardGlowColor;
	[Header("Numbers and Values")]
	public float CardPreviewTime = 1f;
	public float CardTransitionTime= 1f;
	public float CardPreviewTimeFast = 0.2f;
	public float CardTransitionTimeFast = 0.5f;
	public int NumMaximoCriaturasMesa = 7;
	[Header("Opciones parametrizables")]
	public TIPO_CONFIGURACION ConfiguracionBatalla;
	public TIPO_CONFIGURACION ConfiguracionItems;
	[Header("Configuración juego")]
	public bool Musica;

	private ConfiguracionUsuario(){	}

	void Awake(){
		if (Instance == null) {
			DontDestroyOnLoad(this.gameObject);
			Instance = this;
		}
		Musica = true;
		ConfiguracionBatalla = TIPO_CONFIGURACION.ENTERO;
		ConfiguracionItems = TIPO_CONFIGURACION.ENTERO;	
	}

	/// <summary>
	/// Devuelve el valor en unidades.
	/// </summary>
	/// <returns>The entero.</returns>
	/// <param name="numero">Numero.</param>
	public static string ObtenerEntero(int numero){
		return numero.ToString () + "u";
	}

	/// <summary>
	/// Devuelve un valor en un factor de porcentaje.
	/// </summary>
	/// <returns>The porcentaje.</returns>
	/// <param name="numerador">Numerador.</param>
	/// <param name="denominador">Denominador.</param>
	public static string ObtenerPorcentaje(int numerador, int denominador){
		Decimal division = Convert.ToDecimal((double)numerador / (double)denominador);
		division = Decimal.Round (division, 2);
		return (Convert.ToInt32 (division*100)).ToString()+"%";
	}

	/// <summary>
	/// Devuelve una fraccion dado el numerador y denominador.
	/// </summary>
	/// <returns>The fraccion.</returns>
	/// <param name="numerador">Numerador.</param>
	/// <param name="denominador">Denominador.</param>
	public static string ObtenerFraccion(int numerador, int denominador){
		int gcd = MayorDenominadorComun (numerador, denominador);
		numerador /= gcd;
		denominador /= gcd;
		if (denominador == 1)
			return numerador.ToString ();
		return numerador.ToString () + "/" + denominador.ToString ();
	}

	/// <summary>
	/// Calcula el maximo denonimador comun.
	/// </summary>
	/// <returns>The denominador comun.</returns>
	/// <param name="a">The alpha component.</param>
	/// <param name="b">The blue component.</param>
	private static int MayorDenominadorComun (int a, int b) { 
		while (b != 0) { 
			int t = b; 
			b = a % b; 
			a = t; 
		} 
		return a; 
	}


}
