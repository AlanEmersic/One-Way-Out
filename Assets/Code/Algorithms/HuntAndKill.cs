using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace MazeAlgorithms
{
    public class HuntAndKill : MonoBehaviour
    {
        public static Grid CreateMaze(Grid grid, int seed)
        {
            System.Random random = new System.Random(seed);
            Cell current = grid.RandomCell();

            while (current != null)
            {
                List<Cell> unvistedNeighbors = current.Neighbors.Where(e => e.Links().Count == 0).ToList();

                if (unvistedNeighbors.Any())
                {
                    Cell neighbor = unvistedNeighbors[random.Next(0, unvistedNeighbors.Count)];
                    current.Link(neighbor);
                    current = neighbor;
                }
                else
                {
                    current = null;

                    foreach (var cell in grid.EachCell())
                    {
                        List<Cell> vistedNeighbors = cell.Neighbors.Where(e => e.Links().Any()).ToList();

                        if (cell.Links().Count == 0 && vistedNeighbors.Any())
                        {
                            current = cell;
                            Cell neighbor = vistedNeighbors[random.Next(0, vistedNeighbors.Count)];
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