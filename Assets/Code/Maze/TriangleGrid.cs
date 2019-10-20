using UnityEngine;

public class TriangleGrid : Grid
{
    public override void Initialize(int rows, int columns, int seed)
    {
        base.Initialize(rows, columns, seed);
    }

    protected override void PrepareGrid()
    {
        Size = Rows * Columns;
        Cells = new TriangleCell[Rows][];

        for (int i = 0; i < Rows; i++)
        {
            Cells[i] = new TriangleCell[Columns];
            for (int j = 0; j < Columns; j++)
                Cells[i][j] = new TriangleCell(i, j);
        }
    }

    protected override void ConfigureCells()
    {
        foreach (TriangleCell cell in EachCell())
        {
            int row = cell.Row;
            int col = cell.Column;

            cell.West = this[row, col - 1];
            cell.East = this[row, col + 1];

            if (cell.IsUpright())
                cell.South = this[row + 1, col];
            else
                cell.North = this[row - 1, col];
        }
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

        float height = (float)(cellSize * System.Math.Sqrt(3) / 2.0f);
        float halfWidth = cellSize / 2.0f;
        float halfHeight = height / 2.0f;

        //int imgWidth = (int)(cellSize * (Columns + 1) / 2.0f);
        //int imgHeight = (int)(height * Rows);

        int wallIndex = Random.Range(0, wallPrefabs.Count);

        foreach (TriangleCell cell in EachCell())
        {
            float cx = halfWidth + cell.Column * halfWidth;
            float cy = halfHeight + cell.Row * height;

            int westX = (int)(cx - halfWidth);
            int midX = (int)cx;
            int eastX = (int)(cx + halfWidth);

            int apexY, baseY;

            if (cell.IsUpright())
            {
                apexY = (int)(cy - halfHeight);
                baseY = (int)(cy + halfHeight);
            }
            else
            {
                apexY = (int)(cy + halfHeight);
                baseY = (int)(cy - halfHeight);
            }

            //if (includeBackgrounds)
            //{
            //    Color color = BackgroundColor(cell);

            //    if (cell == Start)
            //        color = CellStartEndColor(cell);
            //    else if (cell == End)
            //        color = CellStartEndColor(cell);

            //    //graphics.FillRectangle(brush, x1, y1, x2 - x1, y2 - y1);
            //}

            if (cell.West == null)
            {
                //graphics.DrawLine(wall, westX, baseY, midX, apexY);
                Vector3 a = new Vector3(westX, 0, baseY);
                Vector3 b = new Vector3(midX, 0, apexY);

                Vector3 wallPosition = (a + b) / 2;
                Vector3 direction = (b - a) / wallPosition.magnitude;
                Quaternion wallRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 90, 0);
                GameObject obj = Instantiate(wallPrefabs[wallIndex], wallPosition, wallRotation, wallsHolder);
                obj.name = "West";
                obj.GetComponent<Renderer>().material.color = mazeColors.wall;
            }

            if (!cell.IsLinked(cell.East))
            {
                //graphics.DrawLine(wall, eastX, baseY, midX, apexY);
                Vector3 a = new Vector3(eastX, 0, baseY);
                Vector3 b = new Vector3(midX, 0, apexY);

                Vector3 wallPosition = (a + b) / 2;
                Vector3 direction = (b - a) / wallPosition.magnitude;
                Quaternion wallRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 90, 0);
                GameObject obj = Instantiate(wallPrefabs[wallIndex], wallPosition, wallRotation, wallsHolder);
                obj.name = "East";
                obj.GetComponent<Renderer>().material.color = mazeColors.wall;
            }

            bool noSouth = cell.IsUpright() && cell.South == null;
            bool notLinked = !cell.IsUpright() && !cell.IsLinked(cell.North);

            if (noSouth || notLinked)
            {
                //graphics.DrawLine(wall, eastX, baseY, westX, baseY);
                Vector3 a = new Vector3(eastX, 0, baseY);
                Vector3 b = new Vector3(westX, 0, baseY);

                Vector3 wallPosition = (a + b) / 2;
                Vector3 direction = (b - a) / wallPosition.magnitude;
                Quaternion wallRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 90, 0);
                GameObject obj = Instantiate(wallPrefabs[wallIndex], wallPosition, wallRotation, wallsHolder);
                obj.name = "South";
                obj.GetComponent<Renderer>().material.color = mazeColors.wall;
            }
        }
    }
}