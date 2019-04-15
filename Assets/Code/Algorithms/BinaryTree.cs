using System.Collections.Generic;
using UnityEngine;

namespace MazeAlgorithms
{
    public class BinaryTree : MonoBehaviour
    {
        public static Grid CreateMaze(Grid grid, int seed)
        {
            System.Random random = new System.Random(seed);

            foreach (var cell in grid.EachCell())
            {
                List<Cell> neighbors = new List<Cell>();

                if (cell.North != null)
                    neighbors.Add(cell.North);
                if (cell.East != null)
                    neighbors.Add(cell.East);

                int index = random.Next(0, neighbors.Count);

                if (index < neighbors.Count)
                {
                    Cell neighbor = neighbors[index];
                    cell.Link(neighbor);
                }
            }

            return grid;
        }
    }
}