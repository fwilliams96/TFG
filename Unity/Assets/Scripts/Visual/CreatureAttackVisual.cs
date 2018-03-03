using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CreatureAttackVisual : MonoBehaviour
{
    private OneCreatureManager manager;
    private WhereIsTheCardOrEntity w;

    void Awake()
    {
        manager = GetComponent<OneCreatureManager>();
        w = GetComponent<WhereIsTheCardOrEntity>();
    }

    public void AttackTarget(int targetUniqueID, int damageTakenByTarget, int damageTakenByAttacker, int attackerHealthAfter, int targetHealthAfter)
    {
        Debug.Log(targetUniqueID);
        manager.PuedeAtacar = false;
        GameObject target = IDHolder.GetGameObjectWithID(targetUniqueID);

        // bring this creature to front sorting-wise.
        w.TraerAlFrente();
        VisualStates tempState = w.EstadoVisual;
        w.EstadoVisual = VisualStates.Transicion;

        transform.DOMove(target.transform.position, 0.5f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InCubic).OnComplete(() =>
        {
            //Solo se atacan entes no jugadores
            target.GetComponent<OneCreatureManager>().HacerDaño(damageTakenByTarget, targetHealthAfter);

            w.SetearOrdenCriatura();
            w.EstadoVisual = tempState;

            manager.HealthText.text = attackerHealthAfter.ToString();
            Sequence s = DOTween.Sequence();
            s.AppendInterval(1f);
            s.OnComplete(Comandas.Instance.CompletarEjecucionComanda);
            //Command.CommandExecutionComplete();
        });
    }

}
