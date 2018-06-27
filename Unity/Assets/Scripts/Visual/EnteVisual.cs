using UnityEngine;
using System.Collections;
using DG.Tweening;

public class EnteVisual : MonoBehaviour
{
    protected OneCreatureManager manager;
    protected WhereIsTheCardOrEntity w;

    protected void Awake()
    {
        manager = GetComponent<OneCreatureManager>();
        w = GetComponent<WhereIsTheCardOrEntity>();
    }

	/// <summary>
	/// Rota el ente en el eje Y (boca abajo o boca arriba)
	/// </summary>
	/// <param name="objeto">Objeto.</param>
	/// <param name="grados">Grados.</param>
	/// <param name="tiempoTransicion">Tiempo transicion.</param>
    protected void RotarObjetoEjeY(GameObject objeto, int grados, float tiempoTransicion)
    {
        float x = objeto.transform.eulerAngles.x;
        float y = grados;
        float z = objeto.transform.eulerAngles.z;
        objeto.transform.DORotate(new Vector3(x, y, z), tiempoTransicion);
    }

	/// <summary>
	/// Rota el ente en el eje Z (defensa o ataque)
	/// </summary>
	/// <param name="objeto">Objeto.</param>
	/// <param name="grados">Grados.</param>
	/// <param name="tiempoTransicion">Tiempo transicion.</param>
    protected void RotarObjetoEjeZ(GameObject objeto, int grados, float tiempoTransicion)
    {
        float x = objeto.transform.eulerAngles.x;
        float y = objeto.transform.eulerAngles.y;
        float z = grados;
        objeto.transform.DORotate(new Vector3(x, y, z), tiempoTransicion);
    }

}
