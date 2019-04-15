using System.Collections.Generic;
using UnityEngine;

namespace MazeAlgorithms
{
    public class Sidewinder : MonoBehaviour
    {
        public static Grid CreateMaze(Grid grid, int seed)
        {
            System.Random random = new System.Random(seed);

            foreach (var eachRow in grid.EachRow())
            {
                List<Cell> run = new List<Cell>();

                foreach (var cell in eachRow)
                {
                    run.Add(cell);

                    bool atEasternBoundary = cell.East == null;
                    bool atNorthernBoundary = cell.North == null;
                    bool shouldCloseOut = atEasternBoundary || (!atNorthernBoundary && random.Next(0, 2) == 0);

                    if (shouldCloseOut)
                    {
                        Cell member = run[random.Next(0, run.Count)];
                        if (member.North != null)
                            member.Link(member.North);
                        run.Clear();
                    }
                    else
                    {
                        cell.Link(cell.East);
                    }
                }
            }

            return grid;
        }
    }
}