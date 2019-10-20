using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PolarGrid : Grid
{
    public override void Initialize(int rows, int columns, int seed)
    {
        Rows = rows;
        Columns = 1;
        CellTransform = new Dictionary<Cell, Transform>();
        mazeColors = mazeColorsList[Random.Range(0, mazeColorsList.Length)];
        Camera.main.backgroundColor = mazeColors.background;
        cellSize = (int)(cellPrefab.GetComponent<Renderer>().bounds.size.x);
        random = new System.Random(seed);

        PrepareGrid();
        ConfigureCells();
    }

    public override Cell this[int row, int col]
    {
        get
        {
            if (row < 0 || row >= Rows || col < 0) return null;
            //if (col < 0 || col >= Cells[row].Length) return null;
            //Console.WriteLine($"col:{col} mod:{col % Cells[row].Length}");                
            return Cells[row][col % Cells[row].Length] as PolarCell;
        }
    }

    protected override void PrepareGrid()
    {
        Cells = new PolarCell[Rows][];
        float rowHeight = 1.0f / Rows;
        Cells[0] = new PolarCell[1] { new PolarCell(0, 0) };
        Size = 1;

        for (int row = 1; row < Rows; row++)
        {
            float radius = (float)row / Rows;
            float circumference = (float)(2 * System.Math.PI * radius);
            int previousCount = Cells[row - 1].Length;
            float estimatedCellWidth = circumference / previousCount;
            float ratio = (float)System.Math.Round(estimatedCellWidth / rowHeight);
            int cells = (int)(previousCount * ratio);

            Cells[row] = new PolarCell[cells];
            for (int col = 0; col < cells; col++)
            {
                Cells[row][col] = new PolarCell(row, col);
                Size++;
            }
        }
    }

    protected override void ConfigureCells()
    {
        foreach (PolarCell cell in EachCell())
        {
            int row = cell.Row;
            int col = cell.Column;

            if (row > 0)
            {
                cell.CW = this[row, col + 1] as PolarCell;
                cell.CCW = this[row, col - 1] as PolarCell;

                int ratio = Cells[row].Length / Cells[row - 1].Length;
                PolarCell parent = Cells[row - 1][col / ratio] as PolarCell;
                parent.Outward.Add(cell);
                cell.Inward = parent;
            }
        }
    }

    public override Cell RandomCell()
    {
        int row = random.Next(Rows);
        int col = random.Next(Cells[row].Length);

        return Cells[row][col] as PolarCell;
    }

    public override void GenerateMaze()
    {
        string cellsName = "Cells";

        if (transform.Find(cellsName))
            Destroy(transform.Find(cellsName).gameObject);

        Transform cellsHolder = new GameObject(cellsName).transform;
        cellsHolder.parent = transform;

        string wallsName = "Walls";

        if (transform.Find(wallsName))
            Destroy(transform.Find(wallsName).gameObject);

        Transform wallsHolder = new GameObject(wallsName).transform;
        wallsHolder.parent = transform;

        int groundSize = 2 * Rows * cellSize;
        int center = groundSize / 2;
        cellsHolder.transform.position = new Vector3(center, 0, center);
        //Cells
        for (int x = 0; x < Rows * 2; x++)
        {
            for (int y = 0; y < Rows * 2; y++)
            {
                GameObject obj = Instantiate(cellPrefab, new Vector3(x, 0, y) * cellSize, Quaternion.identity, cellsHolder);
                obj.name = obj.transform.position.ToString();
                //CellTransform.Add(Cells[x][y], obj.transform);

                //if (Cells[x][y] == Start)
                //    obj.GetComponent<Renderer>().material.color = mazeColors.start;
                //else if (Cells[x][y] == End)
                //    obj.GetComponent<Renderer>().material.color = mazeColors.end;
                //else
                //    obj.GetComponent<Renderer>().material.color = mazeColors.cell;
            }
        }
        //int wallX = (int)(cellPrefab.GetComponent<Renderer>().bounds.size.x / 2);
        //int wallZ = (int)(cellPrefab.GetComponent<Renderer>().bounds.size.z / 2);
        int wallIndex = Random.Range(0, wallPrefabs.Count);

        //Walls
        foreach (PolarCell cell in EachCell())
        {
            if (cell.Row == 0) continue;

            float theta = (float)(2 * System.Math.PI / Cells[cell.Row].Length);
            float innerRadius = cell.Row * cellSize;
            float outerRadius = (cell.Row + 1) * cellSize;
            float thetaCCW = cell.Column * theta;
            float thetaCW = (cell.Column + 1) * theta;

            int ax = center + (int)(innerRadius * System.Math.Cos(thetaCCW));
            int ay = center + (int)(innerRadius * System.Math.Sin(thetaCCW));
            int bx = center + (int)(outerRadius * System.Math.Cos(thetaCCW));
            int by = center + (int)(outerRadius * System.Math.Sin(thetaCCW));
            int cx = center + (int)(innerRadius * System.Math.Cos(thetaCW));
            int cy = center + (int)(innerRadius * System.Math.Sin(thetaCW));
            int dx = center + (int)(outerRadius * System.Math.Cos(thetaCW));
            int dy = center + (int)(outerRadius * System.Math.Sin(thetaCW));

            if (!cell.IsLinked(cell.Inward))
            {
                //graphics.DrawLine(wall, ax, ay, cx, cy);
                Vector3 a = new Vector3(ax, 0, ay);
                Vector3 c = new Vector3(cx, 0, cy);
                //Debug.DrawLine(a, c, Color.green, float.MaxValue);
                Vector3 wallPosition = (c + a) / 2;
                Vector3 direction = (c - a) / wallPosition.magnitude;
                Quaternion wallRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 90, 0);
                GameObject obj = Instantiate(wallPrefabs[wallIndex], wallPosition, wallRotation, wallsHolder);
                obj.name = "Inward";
                obj.GetComponent<Renderer>().material.color = mazeColors.wall;
            }
            if (!cell.IsLinked(cell.CW))
            {
                //graphics.DrawLine(wall, cx, cy, dx, dy);
                Vector3 c = new Vector3(cx, 0, cy);
                Vector3 d = new Vector3(dx, 0, dy);
                //Debug.DrawLine(c, d, Color.green, float.MaxValue);
                Vector3 wallPosition = (d + c) / 2;
                Vector3 direction = (d - c) / wallPosition.magnitude;
                Quaternion wallRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 90, 0); ;
                GameObject obj = Instantiate(wallPrefabs[wallIndex], wallPosition, wallRotation, wallsHolder);
                obj.name = "CW";
                obj.GetComponent<Renderer>().material.color = mazeColors.wall;
            }
        }
        //graphics.DrawEllipse(wall, center - Rows * cellSize, center - Rows * cellSize, Rows * 2 * cellSize, Rows * 2 * cellSize);               
        float angle = 360f / Size;        
        float radius = Rows * cellSize;        
        for (int i = 0; i < Size; i++)
        {
            Quaternion rotation = Quaternion.AngleAxis(i * angle, Vector3.up);
            Vector3 direction = rotation * Vector3.forward;
            Vector3 position = new Vector3(center, 0, center) + (direction * radius);
            GameObject obj = Instantiate(wallPrefabs[wallIndex], position, rotation, wallsHolder);
            obj.name = "Circle";
            obj.GetComponent<Renderer>().material.color = mazeColors.wall;
        }
    }
}

