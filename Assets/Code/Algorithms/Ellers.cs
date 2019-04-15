using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace MazeAlgorithms
{
    public class Ellers : MonoBehaviour
    {
        private class RowState
        {
            Dictionary<int, List<Cell>> cellsInSet;
            Dictionary<int, int> setForCell;
            int nextSet;

            public RowState(int startingSet = 0)
            {
                cellsInSet = new Dictionary<int, List<Cell>>();
                setForCell = new Dictionary<int, int>();
                nextSet = startingSet;
            }

            public void Record(int set, Cell cell)
            {
                setForCell[cell.Column] = set;
                if (!cellsInSet.ContainsKey(set))
                    cellsInSet[set] = new List<Cell>();
                cellsInSet[set].Add(cell);
            }

            public int SetFor(Cell cell)
            {
                if (!setForCell.ContainsKey(cell.Column))
                {
                    Record(nextSet, cell);
                    nextSet++;
                }
                return setForCell[cell.Column];
            }

            public void Merge(int winner, int loser)
            {
                foreach (Cell cell in cellsInSet[loser])
                {
                    setForCell[cell.Column] = winner;
                    cellsInSet[winner].Add(cell);
                }
                cellsInSet.Remove(loser);
            }

            public RowState Next() => new RowState(nextSet);

            public IEnumerable<KeyValuePair<int, List<Cell>>> EachSet()
            {
                foreach (var set in cellsInSet)
                    yield return set;
            }
        }

        public static Grid CreateMaze(Grid grid, int seed)
        {
            RowState rowState = new RowState();
            System.Random random = new System.Random(seed);

            foreach (Cell[] row in grid.EachRow())
            {
                foreach (Cell cell in row)
                {
                    if (cell.West == null)
                        continue;

                    int set = rowState.SetFor(cell);
                    int priorSet = rowState.SetFor(cell.West);
                    bool shouldLink = set != priorSet && (cell.South == null || random.Next(2) == 0);

                    if (shouldLink)
                    {
                        cell.Link(cell.West);
                        rowState.Merge(priorSet, set);
                    }
                }
                if (row[0].South != null)
                {
                    RowState nextRow = rowState.Next();

                    foreach (var set in rowState.EachSet())
                    {
                        var list = set.Value.OrderBy(x => random.Next()).ToList();
                        int index = 0;
                        Cell cell = list[index];
                        if (index == 0 || random.Next(4) == 0) //random chance 25%
                        {
                            cell.Link(cell.South);
                            nextRow.Record(rowState.SetFor(cell), cell.South);
                        }
                    }
                    rowState = nextRow;
                }
            }
            return grid;
        }
    }
}