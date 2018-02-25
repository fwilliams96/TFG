using UnityEngine;
using System.Collections;

public interface ICharacter: IIdentifiable
{	
    int Vida { get;    set;}

    void Morir();
}

public interface IIdentifiable
{
    int ID { get; }
}
