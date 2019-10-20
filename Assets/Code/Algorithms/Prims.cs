using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MazeAlgorithms
{
    public class Prims : MonoBehaviour
    {
        public static G CreateMaze<G, T>(G grid, int seed) where G : Grid where T : Cell
        {
            System.Random random = new System.Random(seed);
            T start = grid.RandomCell() as T;
            List<T> active = new List<T>() { start };

            while (active.Any())
            {
                T cell = active[random.Next(0, active.Count)];
                List<T> availableNeighbors = cell.Neighbors.Where(e => e.Links().Count == 0).Cast<T>().ToList();

                if (availableNeighbors.Any())
                {
                    T neighbor = availableNeighbors[random.Next(0, availableNeighbors.Count)];
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