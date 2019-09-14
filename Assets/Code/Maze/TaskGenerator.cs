using System.Collections.Generic;
using UnityEngine;

public class TaskGenerator : MonoBehaviour
{
    [SerializeField] TaskColors taskColors;

    public static List<Cell> TaskCells { get; private set; }
    public static int TaskCount { get; private set; }
    public Color[] TaskColorsContainer { get; private set; }

    public void CreateTasks(Grid maze, int seed)
    {
        System.Random random = new System.Random(seed);
        TaskColorsContainer = new Color[5];
        TaskCells = new List<Cell>();
        List<Cell> deadEnds = maze.DeadEnds();

        TaskCount = deadEnds.Count > 5 ? random.Next(2, 5) : random.Next(0, deadEnds.Count - 2);
        //print($"Ends:{deadEnds.Count} tasks:{TaskCount}");
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
                TaskColorsContainer[i] = taskColors.colors[i];
                maze.CellTransform[cell].GetComponent<Renderer>().material.color = taskColors.colors[i];
                i++;
            }
        }
    }

    public void OnTaskCompleted()
    {
        TaskCount--;
    }
}