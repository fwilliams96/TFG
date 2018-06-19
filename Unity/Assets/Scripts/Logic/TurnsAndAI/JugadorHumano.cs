using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JugadorHumano : JugadorPartida
{
	public static JugadorHumano Instance;

	protected override void Awake(){
		Instance = this;
		area = AreaPosition.Low;
		base.Awake ();
	}
}
