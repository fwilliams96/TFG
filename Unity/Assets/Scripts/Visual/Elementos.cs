using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elementos : MonoBehaviour {
    // Drag & Drop the vertical layout group here
    public UnityEngine.UI.GridLayoutGroup gridLayoutGroup;
    public List<GameObject> ListaElementos;

    void function()
    {
        // ... In your function
        ListaElementos = new List<GameObject>();
        GameObject c1 = Instantiate(DatosGenerales.Instance.CardPrefab, transform.position, Quaternion.identity) as GameObject;
        //GameObject c2 = GameObject.Instantiate(DatosGenerales.Instance.CardPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        //GameObject c3 = GameObject.Instantiate(DatosGenerales.Instance.CardPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        ListaElementos.Add(c1);
        //ListaElementos.Add(c2);
        //ListaElementos.Add(c3);
        RectTransform parent = gridLayoutGroup.GetComponent<RectTransform>();
        for (int index = 0; index < ListaElementos.Count; ++index)
        {
            GameObject elemento = ListaElementos[index];
            //OneCardManager manager = elemento.GetComponent<OneCardManager>();
            elemento.transform.SetParent(parent);
            //UnityEngine.UI.Text t = g.AddComponent<UnityEngine.UI.Text>();
            //t.AddComponent<RectTransform>().setParent(parent);
            //t.text = stringList[index];
        }
        for (int i = 1; i < ListaElementos.Count; i++)
        {
            ListaElementos[i].transform.position = ListaElementos[i - 1].transform.position + new Vector3(0,4,0);
        }
    }
    // Use this for initialization
    void Start () {
        ListaElementos = new List<GameObject>();
        //GameObject c1 = Instantiate(DatosGenerales.Instance.CardPrefab, transform);
        GameObject elemento;
        for (int i=0; i < 20; i++)
        {
            //elemento = Instantiate(DatosGenerales.Instance.CardInventario, transform.position, Quaternion.identity) as GameObject;
            elemento = Instantiate(DatosGenerales.Instance.CardInventario, transform);
            ListaElementos.Add(elemento);
            //elemento.transform.parent = gridLayoutGroup.gameObject.transform;
        }
        
        //c1.transform.SetParent(transform);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
