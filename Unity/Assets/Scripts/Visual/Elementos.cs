using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elementos : MonoBehaviour {
    // Drag & Drop the vertical layout group here
    public UnityEngine.UI.GridLayoutGroup gridLayoutGroup;
    public List<GameObject> ListaElementos;
    // Use this for initialization
    void Start () {
        List<System.Object> listaCartas = BaseDatos.Instance.Local.Cartas();
        GameObject elemento;
        foreach(Carta carta in listaCartas)
        {
            //elemento = Instantiate(DatosGenerales.Instance.CardInventario, transform.position, Quaternion.identity) as GameObject;
            CartaAsset asset = carta.assetCarta;
            float progresoTrebol = carta.Progreso.Material;
            float progresoPocion = carta.Progreso.Pocion;
            elemento = Instantiate(DatosGenerales.Instance.CardInventario, transform);
            OneCardManager manager = elemento.GetComponent<OneCardManager>();
            manager.CartaAsset = asset;
            manager.PorcentajeProgresoTrebol = progresoTrebol;
            manager.PorcentajeProgresoPocion = progresoPocion;
            manager.LeerDatos();

            ListaElementos.Add(elemento);
            elemento.transform.parent = gridLayoutGroup.gameObject.transform;
        }
        
        //c1.transform.SetParent(transform);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
