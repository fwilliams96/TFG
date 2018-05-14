using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CreatureAttackVisual : EnteVisual
{

    //void Awake() { base.Awake(); }

    public void AttackTarget(int targetUniqueID, int damageTaken,int targetHealthAfter)
    {
        manager.PuedeAtacar = false;
        GameObject target = IDHolder.GetGameObjectWithID(targetUniqueID);

        // bring this creature to front sorting-wise.
        w.TraerAlFrente();
        /*VisualStates tempState = w.EstadoVisual;
        w.EstadoVisual = VisualStates.Transicion;*/

        transform.DOMove(target.transform.position, 0.5f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InCubic).OnComplete(() =>
        {

			if (targetUniqueID == Controlador.Instance.Local.ID || targetUniqueID == Controlador.Instance.Enemigo.ID)
			{
				target.GetComponent<PlayerPortraitVisual>().HacerDaño(damageTaken,targetHealthAfter);
			}else{
				if(target.tag.Contains("Criatura"))
					target.GetComponent<OneCreatureManager>().HacerDaño(damageTaken, targetHealthAfter);
				else
					target.GetComponent<OneMagicaManager>().HacerDaño();
			}
            /*w.SetearOrdenCriatura();
            w.EstadoVisual = tempState;*/

            //manager.HealthText.text = attackerHealthAfter.ToString();
            Sequence s = DOTween.Sequence();
            s.AppendInterval(1f);
            s.OnComplete(Comandas.Instance.CompletarEjecucionComanda);
            //Command.CommandExecutionComplete();
        });
    }

    public void ChangePosition(PosicionCriatura pos) //OPTIONAL 0 para ataque, 1 para defensa
    {
        if(pos.Equals(PosicionCriatura.ATAQUE))
        {
            ColocarCriaturaEnAtaque(Settings.Instance.CardTransitionTimeFast);
        }
        else
        {
			ColocarCriaturaEnDefensa(Settings.Instance.CardTransitionTimeFast);
        }
        Comandas.Instance.CompletarEjecucionComanda();
    }

    public void ColocarCriaturaEnAtaque(float tiempo)
    {
        RotarObjetoEjeZ(this.gameObject.transform.Find("Canvas").gameObject, 0, tiempo);
    }

    public void ColocarCriaturaEnDefensa(float tiempo)
    {
        RotarObjetoEjeZ(this.gameObject.transform.Find("Canvas").gameObject, 90, tiempo);
    }

}
