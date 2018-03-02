﻿using UnityEngine;
using System.Collections;

public class UpdateManaCrystalsCommand : Comanda {

    private Jugador p;
    private int TotalMana;
    private int AvailableMana;

    public UpdateManaCrystalsCommand(Jugador p, int TotalMana, int AvailableMana)
    {
        this.p = p;
        this.TotalMana = TotalMana;
        this.AvailableMana = AvailableMana;
    }

    public override void EmpezarEjecucionComanda()
    {
        PlayerArea areaJugador = Controlador.Instance.AreaJugador(p);
        areaJugador.ManaBar.TotalCrystals = TotalMana;
        areaJugador.ManaBar.AvailableCrystals = AvailableMana;
        comandas.CompletarEjecucionComanda();
    }
}
