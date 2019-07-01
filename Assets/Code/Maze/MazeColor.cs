using UnityEngine;

[CreateAssetMenu(fileName = "Color", menuName = "Maze Color")]
public class MazeColor : ScriptableObject
{
    public Color cell;
    public Color wall;
    public Color start;
    public Color end;
    public Color background;
}

