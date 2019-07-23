using UnityEngine;

[CreateAssetMenu(fileName = "Colors", menuName = "Maze Colors")]
public class MazeColors : ScriptableObject
{
    public Color cell;
    public Color wall;
    public Color start;
    public Color end;
    public Color background;
}

