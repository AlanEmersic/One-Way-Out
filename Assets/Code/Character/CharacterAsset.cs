using UnityEngine;

[CreateAssetMenu(fileName = "New Character Asset", menuName = "Character Asset")]
public class CharacterAsset : ScriptableObject
{
    public GameObject cell;
    public GameObject wall;
}