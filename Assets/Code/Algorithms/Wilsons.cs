using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace MazeAlgorithms
{
    public class Wilsons : MonoBehaviour
    {
        public static Grid CreateMaze(Grid grid, int seed)
        {
            System.Random random = new System.Random(seed);

            List<Cell> unvisted = new List<Cell>();

            foreach (var cell in grid.EachCell())
                unvisted.Add(cell);

            Cell first = unvisted[random.Next(0, unvisted.Count)];
            unvisted.Remove(first);

            while (unvisted.Any())
            {
                Cell cell = unvisted[random.Next(0, unvisted.Count)];
                List<Cell> path = new List<Cell> { cell };

                while (unvisted.Contains(cell))
                {
                    cell = cell.Neighbors[random.Next(0, cell.Neighbors.Count)];
                    int position = path.IndexOf(cell);

                    if (position >= 0)                    
                        path = path.Take(position + 1).ToList();                    
                    else                    
                        path.Add(cell);                    
                }

                for (int index = 0; index < path.Count - 1; index++)
                {
                    path[index].Link(path[index + 1]);
                    unvisted.Remove(path[index]);
                }
            }

            return grid;
        }
    }
}