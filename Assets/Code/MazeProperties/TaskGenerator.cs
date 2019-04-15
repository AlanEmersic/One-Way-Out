using System.Collections.Generic;
using UnityEngine;

public class TaskGenerator : MonoBehaviour
{
    public int TaskCount { get; private set; }
    public List<Cell> TaskCells { get; private set; }
    public static TaskGenerator Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void CreateTasks(Grid maze, int seed)
    {
        System.Random random = new System.Random(seed);
        TaskCells = new List<Cell>();
        List<Cell> deadEnds = maze.DeadEnds();

        int min = Mathf.RoundToInt(maze.Rows * (1.0f / 3));
        int max = Mathf.RoundToInt(maze.Rows * (2.0f / 3));
        TaskCount = random.Next(min, max);
        print($"Tasks: {TaskCount}");
        string taskName = "Tasks";

        if (transform.Find(taskName))
            DestroyImmediate(transform.Find(taskName).gameObject);

        Transform tasksHolder = new GameObject(taskName).transform;
        tasksHolder.parent = transform;

        for (int i = 0; i < TaskCount;)
        {
            Cell cell = maze.RandomCell();
            if (cell != maze.Start && cell != maze.End && deadEnds.Contains(cell) && !TaskCells.Contains(cell))
            {
                TaskCells.Add(cell);
                maze.CellTransform[cell].GetComponent<Renderer>().material.color = Color.blue;
                i++;
            }
        }
    }

    public void OnKeyCollected()
    {
        TaskCount--;
        print($"Tasks left: {TaskCount}");
    }
}

