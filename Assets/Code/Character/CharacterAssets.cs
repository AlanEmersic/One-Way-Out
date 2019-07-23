using UnityEngine;

[CreateAssetMenu(menuName = "Character Asset")]
public class CharacterAssets : ScriptableObject
{
    public MazeColors mazeColors;
    public GameObject[] trapPrefabs;
    public GameObject[] taskPrefabs;
}

