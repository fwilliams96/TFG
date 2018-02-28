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
            if (damageTakenByTarget > 0)
                DamageEffect.CreateDamageEffect(target.transform.position, damageTakenByTarget);
            if (damageTakenByAttacker > 0)
                DamageEffect.CreateDamageEffect(transform.position, damageTakenByAttacker);

            if (targetUniqueID == DatosGenerales.Instance.LowPlayer.PlayerID || targetUniqueID == DatosGenerales.Instance.TopPlayer.PlayerID)
            {
                // target is a player
                //TODO esto sobraria
                target.GetComponent<PlayerPortraitVisual>().HealthText.text = targetHealthAfter.ToString();
            }
            else
                target.GetComponent<OneCreatureManager>().HealthText.text = targetHealthAfter.ToString();

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
