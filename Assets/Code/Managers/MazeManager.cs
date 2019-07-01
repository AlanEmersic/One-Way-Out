using UnityEngine;

public class MazeManager : MonoBehaviour
{
    [SerializeField] MazeGenerator mazeGenerator;

    void Start()
    {
        mazeGenerator.GenerateMaze();
    }
}

