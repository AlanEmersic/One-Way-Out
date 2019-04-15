using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace MazeAlgorithms
{
    public class RecursiveBacktracker : MonoBehaviour
    {
        public static Grid CreateMaze(Grid grid, int seed)
        {
            System.Random random = new System.Random(seed);
            Cell start = grid.RandomCell();
            Stack<Cell> stack = new Stack<Cell>();
            stack.Push(start);

            while (stack.Any())
            {
                Cell current = stack.Peek();
                List<Cell> neighbors = current.Neighbors.Where(e => e.Links().Count == 0).ToList();

                if (neighbors.Count == 0)
                    stack.Pop();
                else
                {
                    Cell neighbor = neighbors[random.Next(0, neighbors.Count)];
                    current.Link(neighbor);
                    stack.Push(neighbor);
                }
            }

            return grid;
        }
    }
}