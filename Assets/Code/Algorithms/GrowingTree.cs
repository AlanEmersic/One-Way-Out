using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MazeAlgorithms
{
    public class GrowingTree : MonoBehaviour
    {
        public static Grid CreateMaze(Grid grid, int seed)
        {
            System.Random random = new System.Random(seed);

            Cell start = grid.RandomCell();
            List<Cell> active = new List<Cell>() { start };

            while (active.Any())
            {
                Cell cell = random.Next(0, 2) == 0 ? active[active.Count - 1] : active[random.Next(0, active.Count)]; //recursive backtracker/prims 50/50 split
                List<Cell> availableNeighbors = cell.Neighbors.Where(e => e.Links().Count == 0).ToList();

                if (availableNeighbors.Any())
                {
                    Cell neighbor = availableNeighbors[random.Next(0, availableNeighbors.Count)];
                    cell.Link(neighbor);
                    active.Add(neighbor);
                }
                else
                    active.Remove(cell);
            }
            return grid;
        }
    }
}