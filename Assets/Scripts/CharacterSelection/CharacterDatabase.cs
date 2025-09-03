using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class CharacterDatabase : ScriptableObject
{
    public CharacterC[] character;

    public int CharacterCount
    {
        get
        {
            return character.Length;
        }

    }

    public CharacterC GetCharacter(int index)
    {
        return character[index];
    }
    
}
