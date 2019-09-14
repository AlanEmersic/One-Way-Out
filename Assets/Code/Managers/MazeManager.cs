using UnityEngine;

public class MazeManager : MonoBehaviour
{
    [SerializeField] MazeGenerator mazeGenerator;
    [SerializeField] CharacterMovement characterMovement;

    public void Start()
    {
        mazeGenerator.GenerateMaze();
        characterMovement.SpawnPlayer();
        Timer.StartTimer();
    }
}

