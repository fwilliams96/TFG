using UnityEngine;
using System.Collections;

public class UpdateManaCrystalsCommand : Comanda {

	private JugadorPartida p;
    private int TotalMana;
    private int AvailableMana;

    public UpdateManaCrystalsCommand(JugadorPartida p, int TotalMana, int AvailableMana)
    {
        this.p = p;
        this.TotalMana = TotalMana;
        this.AvailableMana = AvailableMana;
    }

	/// <summary>
	/// Función que actualiza de forma visual el mana del jugador usuario.
	/// </summary>
    public override void EmpezarEjecucionComanda()
    {
        PlayerArea areaJugador = Controlador.Instance.AreaJugador(p);
        areaJugador.ManaBar.TotalCrystals = TotalMana;
        areaJugador.ManaBar.AvailableCrystals = AvailableMana;
        comandas.CompletarEjecucionComanda();
    }
}
