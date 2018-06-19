using UnityEngine;
using System.Collections;
using DG.Tweening;

public class DelayCommand : Comanda 
{
    float delay;

    public DelayCommand(float timeToWait)
    {
        delay = timeToWait;    
    }

    public override void EmpezarEjecucionComanda()
    {
        Sequence s = DOTween.Sequence();
        s.PrependInterval(delay);
        s.OnComplete(comandas.CompletarEjecucionComanda);
    }
}
