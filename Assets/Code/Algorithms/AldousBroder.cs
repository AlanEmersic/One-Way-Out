using UnityEngine;

namespace MazeAlgorithms
{
    public class AldousBroder : MonoBehaviour
    {
        public static G CreateMaze<G, T>(G grid, int seed) where G : Grid where T : Cell
        {
            System.Random random = new System.Random(seed);

            T cell = grid.RandomCell() as T;
            int unvisited = grid.Size - 1;

            while (unvisited > 0)
            {
                T neighbor = cell.Neighbors[random.Next(0, cell.Neighbors.Count)] as T;

                if (neighbor.Links().Count == 0)
                {
                    cell.Link(neighbor);
                    unvisited--;
                }
                cell = neighbor;
            }

            return grid;
        }
    }
}