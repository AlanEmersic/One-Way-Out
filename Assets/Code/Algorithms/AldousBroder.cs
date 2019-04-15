using UnityEngine;

namespace MazeAlgorithms
{
    public class AldousBroder : MonoBehaviour
    {
        public static Grid CreateMaze(Grid grid, int seed)
        {
            System.Random random = new System.Random(seed);

            Cell cell = grid.RandomCell();
            int unvisited = grid.Size - 1;

            while (unvisited > 0)
            {
                Cell neighbor = cell.Neighbors[random.Next(0, cell.Neighbors.Count)];

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