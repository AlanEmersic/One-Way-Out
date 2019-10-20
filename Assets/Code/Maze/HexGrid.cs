using UnityEngine;

public class HexGrid : Grid
{
    public override void Initialize(int rows, int columns, int seed)
    {
        base.Initialize(rows, columns, seed);
    }

    protected override void PrepareGrid()
    {
        Size = Rows * Columns;
        Cells = new HexCell[Rows][];

        for (int i = 0; i < Rows; i++)
        {
            Cells[i] = new HexCell[Columns];
            for (int j = 0; j < Columns; j++)
                Cells[i][j] = new HexCell(i, j);
        }
    }

    protected override void ConfigureCells()
    {
        foreach (HexCell cell in EachCell())
        {
            int row = cell.Row;
            int col = cell.Column;
            int northDiagonal, southDiagonal;

            if (col % 2 == 0)
            {
                northDiagonal = row - 1;
                southDiagonal = row;
            }
            else
            {
                northDiagonal = row;
                southDiagonal = row + 1;
            }

            cell.NorthWest = this[northDiagonal, col - 1] as HexCell;
            cell.North = this[row - 1, col] as HexCell;
            cell.NorthEast = this[northDiagonal, col + 1] as HexCell;
            cell.SouthWest = this[southDiagonal, col - 1] as HexCell;
            cell.South = this[row + 1, col] as HexCell;
            cell.SouthEast = this[southDiagonal, col + 1] as HexCell;
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

        float aSize = cellSize / 2.0f;
        float bSize = (float)(cellSize * System.Math.Sqrt(3) / 2.0f);
        float height = bSize * 2;
        //includeBackgrounds = true;        
        int wallIndex = Random.Range(0, wallPrefabs.Count);

        foreach (HexCell cell in EachCell())
        {
            float cx = cellSize + 3 * cell.Column * aSize;
            float cy = bSize + cell.Row * height;
            if (cell.Column % 2 != 0)
                cy += bSize;

            int xFW = (int)(cx - cellSize);
            int xNW = (int)(cx - aSize);
            int xNE = (int)(cx + aSize);
            int xFE = (int)(cx + cellSize);

            int yN = (int)(cy - bSize);
            int yM = (int)cy;
            int yS = (int)(cy + bSize);

            #region BG
            //if (includeBackgrounds)
            //{
            //    Color color = BackgroundColor(cell);

            //    PointF point1 = new PointF(xFW, yM);
            //    PointF point2 = new PointF(xNW, yN);
            //    PointF point3 = new PointF(xNE, yN);
            //    PointF point4 = new PointF(xFE, yM);
            //    PointF point5 = new PointF(xNE, yS);
            //    PointF point6 = new PointF(xNW, yS);

            //    PointF[] points = new PointF[] { point1, point2, point3, point4, point5, point6 };

            //    //if (cell == Start)
            //    //    color = CellStartEndColor(cell);
            //    //else if (cell == End)
            //    //    color = CellStartEndColor(cell);

            //    Brush brush = new SolidBrush(color);
            //    graphics.DrawPolygon(wall, points);
            //}
            #endregion

            if (cell.SouthWest == null)
            {
                //graphics.DrawLine(wall, xFW, yM, xNW, yS);
                Vector3 a = new Vector3(xFW, 0, yM);
                Vector3 b = new Vector3(xNW, 0, yS);

                Vector3 wallPosition = (a + b) / 2;
                Vector3 direction = (b - a) / wallPosition.magnitude;
                Quaternion wallRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 90, 0);
                GameObject obj = Instantiate(wallPrefabs[wallIndex], wallPosition, wallRotation, wallsHolder);
                obj.name = "SouthWest";
                obj.GetComponent<Renderer>().material.color = mazeColors.wall;
            }
            if (cell.NorthWest == null)
            {
                //graphics.DrawLine(wall, xFW, yM, xNW, yN);
                Vector3 a = new Vector3(xFW, 0, yM);
                Vector3 b = new Vector3(xNW, 0, yN);

                Vector3 wallPosition = (a + b) / 2;
                Vector3 direction = (b - a) / wallPosition.magnitude;
                Quaternion wallRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 90, 0);
                GameObject obj = Instantiate(wallPrefabs[wallIndex], wallPosition, wallRotation, wallsHolder);
                obj.name = "NorthWest";
                obj.GetComponent<Renderer>().material.color = mazeColors.wall;
            }
            if (cell.North == null)
            {
                //graphics.DrawLine(wall, xNW, yN, xNE, yN);
                Vector3 a = new Vector3(xNW, 0, yN);
                Vector3 b = new Vector3(xNE, 0, yN);

                Vector3 wallPosition = (a + b) / 2;
                Vector3 direction = (b - a) / wallPosition.magnitude;
                Quaternion wallRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 90, 0);
                GameObject obj = Instantiate(wallPrefabs[wallIndex], wallPosition, wallRotation, wallsHolder);
                obj.name = "North";
                obj.GetComponent<Renderer>().material.color = mazeColors.wall;
            }

            if (!cell.IsLinked(cell.NorthEast))
            {
                //graphics.DrawLine(wall, xNE, yN, xFE, yM);
                Vector3 a = new Vector3(xNE, 0, yN);
                Vector3 b = new Vector3(xFE, 0, yM);

                Vector3 wallPosition = (a + b) / 2;
                Vector3 direction = (b - a) / wallPosition.magnitude;
                Quaternion wallRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 90, 0);
                GameObject obj = Instantiate(wallPrefabs[wallIndex], wallPosition, wallRotation, wallsHolder);
                obj.name = "NorthEast";
                obj.GetComponent<Renderer>().material.color = mazeColors.wall;
            }
            if (!cell.IsLinked(cell.SouthEast))
            {
                //graphics.DrawLine(wall, xFE, yM, xNE, yS);
                Vector3 a = new Vector3(xFE, 0, yM);
                Vector3 b = new Vector3(xNE, 0, yS);

                Vector3 wallPosition = (a + b) / 2;
                Vector3 direction = (b - a) / wallPosition.magnitude;
                Quaternion wallRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 90, 0);
                GameObject obj = Instantiate(wallPrefabs[wallIndex], wallPosition, wallRotation, wallsHolder);
                obj.name = "SouthEast";
                obj.GetComponent<Renderer>().material.color = mazeColors.wall;
            }
            if (!cell.IsLinked(cell.South))
            {
                //graphics.DrawLine(wall, xNE, yS, xNW, yS);
                Vector3 a = new Vector3(xNE, 0, yS);
                Vector3 b = new Vector3(xNW, 0, yS);

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
