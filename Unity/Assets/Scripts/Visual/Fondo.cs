using UnityEngine;
using System.Collections;

public class Fondo : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        Debug.Log("Fondo script");
        if (OpcionesObjeto.PrevisualizandoAlgunaCarta())
            OpcionesObjeto.PararTodasPrevisualizaciones();
    }
}
