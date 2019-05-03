using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace MazeProperties
{
    public class TrapGenerator : MonoBehaviour
    {
        [SerializeField] List<GameObject> trapPrefabs = new List<GameObject>();
        List<Cell> trapCells;
        Transform trapHolder;

        public void CreateTraps(Grid maze, int seed)
        {
            System.Random random = new System.Random(seed);
            trapCells = new List<Cell>();

            int min = maze.Rows / 2;
            int max = (maze.Rows + maze.Columns) / 2;
            int trapCount = random.Next(min, max);
            //print($"traps:{trapCount}");
            string trapName = "Traps";

            if (transform.Find(trapName))
                DestroyImmediate(transform.Find(trapName).gameObject);

            trapHolder = new GameObject(trapName).transform;
            trapHolder.parent = transform;

            TrapsOnTaskPath(maze, TaskGenerator.Instance.TaskCount);
            TrapsOnRandomPath(maze, trapCount);
        }

        bool IsNeighborTrap(Cell cell)
        {
            foreach (Cell neighbor in cell.Neighbors)
                if (trapCells.Contains(neighbor))
                    return true;

            return false;
        }

        bool IsCellAvailable(Cell cell)
        {
            return !trapCells.Contains(cell) && !TaskGenerator.Instance.TaskCells.Contains(cell) && !IsNeighborTrap(cell) && cell.Links().Count != 1;
        }

        void TrapsOnTaskPath(Grid maze, int taskCount)
        {
            Distances distances = maze.Start.Distances().PathTo(maze.End);
            Cell mid = distances.FirstOrDefault(x => x.Value == distances.Maximum().Value / 2).Key;

            for (int i = 0; i < taskCount; i++)
            {
                Distances newDistances = mid.Distances().PathTo(TaskGenerator.Instance.TaskCells[i]);
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

                trapCells.Add(cell);
                //int index = Random.Range(0, trapPrefabs.Count);
                //int index = 1;
                //Vector3 trapHeight = new Vector3(0, trapPrefabs[index].GetComponent<Renderer>().bounds.size.y, 0);
                //GameObject obj = Instantiate(trapPrefabs[index], maze.CellTransform[cell].position + trapHeight, Quaternion.identity);
                //obj.transform.SetParent(trapHolder);
                maze.CellTransform[cell].GetComponent<Renderer>().material.color = Color.yellow;
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

                trapCells.Add(cell);
                //int index = 0;
                //Vector3 trapHeight = new Vector3(0, trapPrefabs[index].GetComponent<Renderer>().bounds.size.y, 0);
                //GameObject obj = Instantiate(trapPrefabs[index], maze.CellTransform[cell].position + trapHeight, Quaternion.identity);
                //obj.transform.SetParent(trapHolder);
                maze.CellTransform[cell].GetComponent<Renderer>().material.color = Color.red;
            }
        }
    }
}