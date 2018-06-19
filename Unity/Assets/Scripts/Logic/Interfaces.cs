using UnityEngine;
using System.Collections;

public interface ICharacter: IIdentifiable
{	
    int Defensa { get;    set;}

    void Morir();
}

public interface IIdentifiable
{
    int ID { get; }
}
