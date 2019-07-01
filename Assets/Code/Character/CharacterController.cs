using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterListController : MonoBehaviour
{
    List<Character> characters;

    void Awake()
    {
        characters = new List<Character>();        
    }    

    public List<Character> GetCharacters()
    {
        return new List<Character>(characters);
    }
}
