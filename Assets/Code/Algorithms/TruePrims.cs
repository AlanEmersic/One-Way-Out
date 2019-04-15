using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace MazeAlgorithms
{
    public class TruePrims : MonoBehaviour
    {
        public static Grid CreateMaze(Grid grid, int seed)
        {
            System.Random random = new System.Random(seed);
            Cell start = grid.RandomCell();
            List<Cell> active = new List<Cell>() { start };
            Dictionary<Cell, int> costs = new Dictionary<Cell, int>();

            foreach (var cell in grid.EachCell())
            {
                costs[cell] = random.Next(0, 100);
            }

            while (active.Any())
            {
                Cell cell = active.Aggregate((min, next) => costs[min] < costs[next] ? min : next);
                List<Cell> availableNeighbors = cell.Neighbors.Where(e => e.Links().Count == 0).ToList();

                if (availableNeighbors.Any())
                {
                    Cell neighbor = availableNeighbors.Aggregate((min, next) => costs[min] < costs[next] ? min : next);
                    cell.Link(neighbor);
                    active.Add(neighbor);
                }
                else
                {
                    active.Remove(cell);
                }
            }

            return grid;
        }
    }
}