using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class Character : ScriptableObject
{
    public GameObject character;
    public string characterName;
    public int characterCost;
    public bool isUnlocked = false;
    public bool loadAssets = false;
    public CharacterAsset mazeAsset = null;
}