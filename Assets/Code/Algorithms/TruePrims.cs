using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace MazeAlgorithms
{
    public class TruePrims : MonoBehaviour
    {
        public static G CreateMaze<G, T>(G grid, int seed) where G : Grid where T : Cell
        {
            System.Random random = new System.Random(seed);
            T start = grid.RandomCell() as T;
            List<T> active = new List<T>() { start };
            Dictionary<T, int> costs = new Dictionary<T, int>();

            foreach (T cell in grid.EachCell())
            {
                costs[cell] = random.Next(0, 100);
            }

            while (active.Any())
            {
                T cell = active.Aggregate((min, next) => costs[min] < costs[next] ? min : next);
                List<T> availableNeighbors = cell.Neighbors.Where(e => e.Links().Count == 0).Cast<T>().ToList();

                if (availableNeighbors.Any())
                {
                    T neighbor = availableNeighbors.Aggregate((min, next) => costs[min] < costs[next] ? min : next);
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