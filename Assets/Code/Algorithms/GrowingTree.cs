using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MazeAlgorithms
{
    public class GrowingTree : MonoBehaviour
    {
        public static G CreateMaze<G, T>(G grid, int seed) where G : Grid where T : Cell
        {
            System.Random random = new System.Random(seed);

            T start = grid.RandomCell() as T;
            List<T> active = new List<T>() { start };

            while (active.Any())
            {
                T cell = random.Next(0, 2) == 0 ? active[active.Count - 1] : active[random.Next(0, active.Count)]; //recursive backtracker/prims 50/50 split
                List<T> availableNeighbors = cell.Neighbors.Where(e => e.Links().Count == 0).Cast<T>().ToList();

                if (availableNeighbors.Any())
                {
                    T neighbor = availableNeighbors[random.Next(0, availableNeighbors.Count)];
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