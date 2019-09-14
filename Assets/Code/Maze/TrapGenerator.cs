using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class TrapGenerator : MonoBehaviour
{
    public static List<Cell> TrapCells { get; private set; }
    public static bool IsTasksOn { get; set; }
    Transform trapHolder;

    public void CreateTraps(Grid maze, int seed)
    {
        System.Random random = new System.Random(seed);
        TrapCells = new List<Cell>();

        int trapCount = maze.Rows;

        string trapName = "Traps";

        if (transform.Find(trapName))
            DestroyImmediate(transform.Find(trapName).gameObject);

        trapHolder = new GameObject(trapName).transform;
        trapHolder.parent = transform;

        IsTasksOn = random.Next(2) == 1;
        if (IsTasksOn)
            TrapsOnTaskPath(maze, TaskGenerator.TaskCount);

        TrapsOnRandomPath(maze, trapCount);        
    }

    bool IsNeighborTrap(Cell cell)
    {
        foreach (Cell neighbor in cell.Neighbors)
            if (TrapCells.Contains(neighbor))
                return true;

        return false;
    }

    bool IsCellAvailable(Cell cell)
    {
        return IsTasksOn ?
            !TrapCells.Contains(cell) && !TaskGenerator.TaskCells.Contains(cell) && !IsNeighborTrap(cell) && cell.Links().Count != 1
            : !TrapCells.Contains(cell) && !IsNeighborTrap(cell) && cell.Links().Count != 1;
    }

    void TrapsOnTaskPath(Grid maze, int taskCount)
    {
        Distances distances = maze.Start.Distances().PathTo(maze.End);
        Cell mid = distances.FirstOrDefault(x => x.Value == distances.Maximum().Value / 2).Key;

        for (int i = 0; i < taskCount; i++)
        {
            Distances newDistances = mid.Distances().PathTo(TaskGenerator.TaskCells[i]);
            Cell cell = newDistances.FirstOrDefault(x => x.Value == newDistances.Maximum().Value / 2).Key;

            int depth = 0;
            while (!IsCellAvailable(cell))
            {
                if (depth > 5) break;
                int value = Random.Range(1, newDistances.Maximum().Value) / 2;
                cell = newDistances.FirstOrDefault(x => x.Value == value).Key;
                depth++;
            }
            if (depth > 5) continue;

            TrapCells.Add(cell);
            //int index = Random.Range(0, trapPrefabs.Count);
            //int index = 1;
            //Vector3 trapHeight = new Vector3(0, trapPrefabs[index].GetComponent<Renderer>().bounds.size.y, 0);
            //GameObject obj = Instantiate(trapPrefabs[index], maze.CellTransform[cell].position + trapHeight, Quaternion.identity);
            //obj.transform.SetParent(trapHolder);

            //maze.CellTransform[cell].GetComponent<Renderer>().material.color = Color.yellow;
        }
    }

    void TrapsOnRandomPath(Grid maze, int trapCount)
    {
        for (int i = 0; i < trapCount; i++)
        {
            Cell cell = maze.RandomCell();

            int depth = 0;
            while (!IsCellAvailable(cell))
            {
                if (depth > 5) break;
                cell = maze.Cells[Random.Range(0, maze.Rows)][Random.Range(0, maze.Columns)];
                depth++;
            }
            if (depth > 5) continue;

            TrapCells.Add(cell);
            //int index = 0;
            //Vector3 trapHeight = new Vector3(0, trapPrefabs[index].GetComponent<Renderer>().bounds.size.y, 0);
            //GameObject obj = Instantiate(trapPrefabs[index], maze.CellTransform[cell].position + trapHeight, Quaternion.identity);
            //obj.transform.SetParent(trapHolder);

            maze.CellTransform[cell].GetComponent<Renderer>().material.color = new Color(224 / 256f, 148 / 256f, 93 / 256f);
        }
    }
}
