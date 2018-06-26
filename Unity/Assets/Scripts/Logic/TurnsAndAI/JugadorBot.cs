using UnityEngine;
using System.Collections;

public class JugadorBot: JugadorPartida {

	public static JugadorBot Instance;

	protected override void Awake(){
		Instance = this;
		area = AreaPosition.Top;
		base.Awake ();
	}

	public void EmpezarTurnoBot(){
		StartCoroutine (RealizarTurno ());
	}

	/// <summary>
	/// Realiza el turno del bot.
	/// </summary>
	/// <returns>The turno.</returns>
	public IEnumerator RealizarTurno()
	{
		bool strategyAttackFirst = false;
		if (Random.Range(0, 2) == 0)
			strategyAttackFirst = true;

		while (MakeOneAIMove(strategyAttackFirst))
		{
			yield return null;
		}

		InsertDelay(1f);

		Controlador.Instance.EndTurn();
	}

	/// <summary>
	/// Acaba el turno del bot.
	/// </summary>
	public override void OnTurnEnd ()
	{
		this.StopAllCoroutines ();
		base.OnTurnEnd ();
	}

	/// <summary>
	/// Según si su primera intencion es atacar o sacar una carta, se devuelve un booleano
	/// que corresponde a si ha podido realizar una de las dos acciones o ambas.
	/// </summary>
	/// <returns><c>true</c>, if one AI move was made, <c>false</c> otherwise.</returns>
	/// <param name="attackFirst">If set to <c>true</c> attack first.</param>
	bool MakeOneAIMove(bool attackFirst)
	{
		if (!Controlador.Instance.AreaJugador (this).ControlActivado)
			return false;
		if (Comandas.Instance.ComandasDeDibujoCartaPendientes())
			return true;
		else if (attackFirst)
			return JugarEnte() || JugarCarta();
		else 
			return JugarCarta() || JugarEnte();
	}

	/// <summary>
	/// Permite jugar una carta para el bot.
	/// </summary>
	/// <returns><c>true</c>, if carta was jugared, <c>false</c> otherwise.</returns>
	bool JugarCarta()
	{
		if (NumEntesEnLaMesa () == 5)
			return false;
		foreach (CartaPartida c in CartasEnLaMano())
		{
			if (Controlador.Instance.CartaPuedeUsarse(this,c))
			{
				/* TODO
				 int tablePos = Controlador.Instance.AreaJugador(this).tableVisual.PosicionSlotNuevaEnte(Camera.main.ScreenToWorldPoint(
					new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, transform.position.z - Camera.main.transform.position.z)).x);*/
				
				if (c.AssetCarta.Familia.Equals(Familia.Magica))
				{
					Controlador.Instance.JugarMagicaMano(this,c.ID, 0);
					InsertDelay(1.5f);
					return true;
				}
				else
				{
					int rnd = Random.Range(0,2);
					Controlador.Instance.JugarCartaMano(this,c.ID, 0, rnd == 0);
					InsertDelay(1.5f);
					return true;
				}

			}
			//Debug.Log("Card: " + c.ca.name + " can NOT be played");
		}
		return false;
	}

	/// <summary>
	/// Permite jugar un ente para el bot.
	/// </summary>
	/// <returns><c>true</c>, if ente was jugared, <c>false</c> otherwise.</returns>
	bool JugarEnte()
	{
		bool movimientoHecho = false;
		for(int i=0; i < NumEntesEnLaMesa() && !movimientoHecho;i++)
		{
			Ente cl = EntesEnLaMesa () [i];
			if (Controlador.Instance.EntePuedeUsarse(cl))
			{
				// attack a random target with a creature
				if (cl.GetType () == typeof(Magica)) {
					if (!((Magica)cl).EfectoActivado) {
						if (!Controlador.Instance.EsMagicaTrampa (cl)) {
							Controlador.Instance.ActivarEfectoMagica (cl.ID);
							movimientoHecho = true;
						}
					}
				} else {
					Criatura criatura = (Criatura)cl;
					if (criatura.PosicionCriatura.Equals (PosicionCriatura.ATAQUE)) {
						if (!Controlador.Instance.CriaturaHaAtacado (criatura)) {
							JugadorPartida enemigo = Controlador.Instance.OtroJugador (this);
							if (enemigo.NumEntesEnLaMesa () > 0) {
								int index = Random.Range (0, enemigo.NumEntesEnLaMesa ());
								Ente enteObjetivo = enemigo.EntesEnLaMesa () [index];
								if (enteObjetivo.GetType() == typeof(Criatura) || (enteObjetivo.GetType () == typeof(Magica) && !((Magica)enteObjetivo).EfectoActivado)) {
									Controlador.Instance.AtacarEnte (cl.ID, enteObjetivo.ID);
									movimientoHecho = true;
								}
							} else {
								Controlador.Instance.AtacarJugador (cl.ID, enemigo.ID);
								movimientoHecho = true;
							}
							
						}

					} else {
						Controlador.Instance.CambiarPosicionCriatura (criatura.ID);
						movimientoHecho = true;
					}
						
				}

			}
		}
		if(movimientoHecho)
			InsertDelay(1f);
		return movimientoHecho;
	}

	public void InsertDelay(float delay)
	{
		new DelayCommand (delay).AñadirAlaCola ();
	}
}
