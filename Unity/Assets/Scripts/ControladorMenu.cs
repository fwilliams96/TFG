using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorMenu : MonoBehaviour {

	public static ControladorMenu Instance;

	void Awake(){
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AgregarItemCarta(int idCarta, int idItem){

		Carta carta = BuscarCarta (idCarta);
		Item item = BuscarItem (idItem);
		if (item.Tipo.Equals (TipoItem.Material))
			carta.AñadirMaterial (item.Cantidad);
		//else if(item.Tipo.Equals (TipoItem.Pocion))
		else
			carta.AñadirPocion (item.Cantidad);
		new AñadirItemCartaPrevisualizadaCommand (item).AñadirAlaCola ();
		new AñadirItemCartaCommand (carta,item).AñadirAlaCola ();
		BaseDatos.Instance.ActualizarItemCarta (carta,item);
	}

	private Carta BuscarCarta(int idCarta){
		bool trobat = false;
		int i = 0;
		Carta carta = null;
		List<System.Object> cartasJugador = BaseDatos.Instance.Local.Cartas ();
		while (i < cartasJugador.Count && !trobat) {
			carta = (Carta)cartasJugador [i];
			if (carta.ID == idCarta)
				trobat = true;
			else
				i += 1;
		}
		return carta;
	}

	private Item BuscarItem(int idItem){
		bool trobat = false;
		int i = 0;
		Item item = null;
		List<System.Object> itemsJugador = BaseDatos.Instance.Local.Items ();
		while (i < itemsJugador.Count && !trobat) {
			item = (Item)itemsJugador [i];
			if (item.ID == idItem)
				trobat = true;
			else
				i += 1;
		}
		return item;
	}

	public bool SePuedeEvolucionar(int idCarta){
		Carta carta = BuscarCarta (idCarta);
		return ExisteEvolucion(carta) && carta.Progreso.Material >= 100f && carta.Progreso.Pocion >= 100f;
	}

	private bool ExisteEvolucion(Carta carta){
		Familia familia = carta.AssetCarta.Familia;
		int evolucionActual = carta.AssetCarta.Evolucion;
		KeyValuePair<string,CartaAsset> evolucion = BuscarEvolucion (familia, evolucionActual);
		return !"".Equals(evolucion.Key)  && null != evolucion.Value;
	}

	public KeyValuePair<string,CartaAsset> BuscarEvolucion(Familia familia, int evolucionActual){
		return BaseDatos.Instance.BuscarEvolucion (familia, evolucionActual);
	}

	public void EvolucionarCarta(GameObject cartaG){
		Carta carta = BuscarCarta (cartaG.GetComponent<IDHolder>().UniqueID);
		//Buscamos la evolución
		KeyValuePair<string,CartaAsset> evolucion = BuscarEvolucion (carta.AssetCarta.Familia, carta.AssetCarta.Evolucion);
		//modificamos el progreso de la carta restando las 100 unidades necesarias para evolucionar
		carta.Progreso.Material -= 100;
		carta.Progreso.Pocion -= 100;
		carta.IdAsset = evolucion.Key;
		carta.AssetCarta = evolucion.Value;
		OneCardManager manager = cartaG.GetComponent<OneCardManager>();
		//modificamos el asset de la carta seleccionada actual y lo cambiamos por la evolución
		manager.CartaAsset = evolucion.Value;
		//actualizamos en la carta visual el progreso
		manager.PorcentajeProgresoTrebol = carta.Progreso.Material > 100 ? 100: carta.Progreso.Material;
		manager.PorcentajeProgresoPocion = carta.Progreso.Pocion > 100 ? 100: carta.Progreso.Pocion;
		manager.LeerDatos();
		BaseDatos.Instance.ActualizarCartaBaseDatos (carta);
	}
}
