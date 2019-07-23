using System.Collections.Generic;
using System.Linq;

public class Distances : Dictionary<Cell, int>
{
    Cell root;

    public Distances(Cell root) : base()
    {
        this.root = root;
        this[root] = 0;
    }

    public List<Cell> Cells() => Keys.ToList();    

    public void SetDistance(Cell cell, int distance)
    {
        if (!ContainsKey(cell))
            Add(cell, distance);
        else
            this[cell] = distance;
    }

    public Distances PathTo(Cell goal)
    {
        Cell current = goal;

        Distances breadcrumbs = new Distances(root);
        breadcrumbs[current] = this[current];

        while (current != root)
        {
            foreach (Cell neighbor in current.Links())
            {
                if (this[neighbor] < this[current])
                {
                    if (!breadcrumbs.ContainsKey(neighbor))
                        breadcrumbs.Add(neighbor, this[neighbor]);
                    else
                        breadcrumbs[neighbor] = this[neighbor];

                    current = neighbor;
                    break;
                }
            }
        }

        return breadcrumbs;
    }

    public KeyValuePair<Cell, int> Maximum()
    {
        int maxDistance = 0;
        Cell maxCell = root;

        foreach (Cell cell in Cells())
        {
            if (this[cell] > maxDistance)
            {
                maxCell = cell;
                maxDistance = this[cell];
            }
        }

        return new KeyValuePair<Cell, int>(maxCell, maxDistance);
    }
}