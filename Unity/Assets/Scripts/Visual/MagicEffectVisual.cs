using UnityEngine;
using System.Collections;
using DG.Tweening;

public class MagicEffectVisual : EnteVisual
{

    public void ActivateEffect()
    {
        RotarObjetoEjeY(gameObject.transform.Find("Cuerpo").gameObject, 0, DatosGenerales.Instance.CardTransitionTime);
        /*GameObject canvas = gameObject.transform.Find("Cuerpo").gameObject;
        float x = canvas.transform.eulerAngles.x;
        //float y = gameObject.transform.eulerAngles.y + 180;
        float y = 0;
        float z = canvas.transform.eulerAngles.z;
        canvas.transform.DORotate(new Vector3(x, y, z), DatosGenerales.Instance.CardTransitionTime, RotateMode.WorldAxisAdd);*/
        Comandas.Instance.CompletarEjecucionComanda();
    }

    public void ColocarMagicaBocaAbajo(float tiempo)
    {
        RotarObjetoEjeY(this.gameObject.transform.Find("Cuerpo").gameObject, 180, tiempo);
    }

}
