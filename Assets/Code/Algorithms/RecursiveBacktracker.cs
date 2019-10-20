using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace MazeAlgorithms
{
    public class RecursiveBacktracker : MonoBehaviour
    {
        public static G CreateMaze<G, T>(G grid, int seed) where G : Grid where T : Cell
        {
            System.Random random = new System.Random(seed);
            T start = grid.RandomCell() as T;
            Stack<T> stack = new Stack<T>();
            stack.Push(start);

            while (stack.Any())
            {
                T current = stack.Peek();
                List<T> neighbors = current.Neighbors.Where(e => e.Links().Count == 0).Cast<T>().ToList();

                if (neighbors.Count == 0)
                    stack.Pop();
                else
                {
                    T neighbor = neighbors[random.Next(0, neighbors.Count)];
                    current.Link(neighbor);
                    stack.Push(neighbor);
                }
            }

            return grid;
        }
    }
}