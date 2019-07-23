using UnityEngine;

[CreateAssetMenu(menuName = "Character")]
public class Character : ScriptableObject
{
    public GameObject characterPrefab;
    public string characterName;
    public int characterCost;
    public bool isUnlocked = false;
    public bool loadAssets = false;    
}