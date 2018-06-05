using UnityEngine;
using System.Collections;

public enum CharClass{ Monje, Japones, Vikingo, Tuareg}

public class CharacterAsset : ScriptableObject 
{
	public CharClass Class;
	public int MaxHealth = 3000;
	public Sprite AvatarImage;
    public Sprite AvatarBGImage;
    public Color32 AvatarBGTint;
}
