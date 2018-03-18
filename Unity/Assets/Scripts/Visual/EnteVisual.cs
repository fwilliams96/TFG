using UnityEngine;
using System.Collections;
using DG.Tweening;

public class EnteVisual : MonoBehaviour
{
    protected OneCreatureManager manager;
    protected WhereIsTheCardOrEntity w;

    void Awake()
    {
        manager = GetComponent<OneCreatureManager>();
        w = GetComponent<WhereIsTheCardOrEntity>();
    }

    protected void RotarObjetoEjeY(GameObject objeto, int grados, float tiempoTransicion)
    {
        float x = objeto.transform.eulerAngles.x;
        float y = grados;
        float z = objeto.transform.eulerAngles.z;
        objeto.transform.DORotate(new Vector3(x, y, z), tiempoTransicion);
    }

    protected void RotarObjetoEjeZ(GameObject objeto, int grados, float tiempoTransicion)
    {
        float x = objeto.transform.eulerAngles.x;
        float y = objeto.transform.eulerAngles.y;
        //TODO
        float z = grados;
        objeto.transform.DORotate(new Vector3(x, y, z), tiempoTransicion);
    }

}
