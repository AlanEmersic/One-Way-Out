using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace MazeAlgorithms
{
    public class HuntAndKill : MonoBehaviour
    {
        public static G CreateMaze<G, T>(G grid, int seed) where G : Grid where T : Cell
        {
            System.Random random = new System.Random(seed);
            T current = grid.RandomCell() as T;

            while (current != null)
            {
                List<T> unvistedNeighbors = current.Neighbors.Where(e => e.Links().Count == 0).Cast<T>().ToList();

                if (unvistedNeighbors.Any())
                {
                    T neighbor = unvistedNeighbors[random.Next(0, unvistedNeighbors.Count)];
                    current.Link(neighbor);
                    current = neighbor;
                }
                else
                {
                    current = null;

                    foreach (T cell in grid.EachCell())
                    {
                        List<T> vistedNeighbors = cell.Neighbors.Where(e => e.Links().Any()).Cast<T>().ToList();

                        if (cell.Links().Count == 0 && vistedNeighbors.Any())
                        {
                            current = cell;
                            T neighbor = vistedNeighbors[random.Next(0, vistedNeighbors.Count)];
                            current.Link(neighbor);
                            break;
                        }
                    }
                }
            }

            return grid;
        }
    }
}