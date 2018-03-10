using UnityEngine;
using System.Collections;
using DG.Tweening;

public class MagicEffectVisual : MonoBehaviour
{
    private OneCreatureManager manager;
    private WhereIsTheCardOrEntity w;

    void Awake()
    {
        manager = GetComponent<OneCreatureManager>();
        w = GetComponent<WhereIsTheCardOrEntity>();
    }

    public void ActivateEffect()
    {
        GameObject canvas = gameObject.transform.Find("Canvas").gameObject;
        float x = canvas.transform.eulerAngles.x;
        //float y = gameObject.transform.eulerAngles.y + 180;
        float y = 180;
        float z = canvas.transform.eulerAngles.z;
        canvas.transform.DORotate(new Vector3(x, y, z), DatosGenerales.Instance.CardTransitionTime);
        Comandas.Instance.CompletarEjecucionComanda();
    }

}
