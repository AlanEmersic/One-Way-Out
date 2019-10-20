using System.Collections.Generic;
using UnityEngine;

namespace MazeAlgorithms
{
    public class BinaryTree : MonoBehaviour
    {
        enum Direction
        {
            NorthWest, NorthEast
        }

        public static Grid CreateMaze(Grid grid, int seed)
        {
            System.Random random = new System.Random(seed);
            Direction direction = (Direction)random.Next(System.Enum.GetNames(typeof(Direction)).Length);

            foreach (var cell in grid.EachCell())
            {
                List<Cell> neighbors = new List<Cell>();

                switch (direction)
                {
                    case Direction.NorthWest:
                        if (cell.North != null)
                            neighbors.Add(cell.North);
                        if (cell.West != null)
                            neighbors.Add(cell.West);
                        break;
                    case Direction.NorthEast:
                        if (cell.North != null)
                            neighbors.Add(cell.North);
                        if (cell.East != null)
                            neighbors.Add(cell.East);
                        break;
                }               

                int index = random.Next(0, neighbors.Count);

                if (index < neighbors.Count)
                {
                    Cell neighbor = neighbors[index];
                    cell.Link(neighbor);
                }
            }

            return grid;
        }
    }
}