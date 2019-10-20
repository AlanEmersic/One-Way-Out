﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//[DisallowMultipleComponent]
public class Grid : MonoBehaviour
{
    public int Rows { get; protected set; }
    public int Columns { get; protected set; }
    public int Size { get; protected set; }
    public Cell[][] Cells { get; protected set; }
    public Cell Start { get; set; }
    public Cell End { get; set; }
    public Distances Distances { get; set; }
    public Dictionary<Cell, Transform> CellTransform { get; protected set; }

    [SerializeField] protected List<GameObject> wallPrefabs;
    [SerializeField] protected GameObject cellPrefab;
    [SerializeField] protected MazeColors[] mazeColorsList;
    protected MazeColors mazeColors;

    protected System.Random random;
    protected int cellSize;

    public virtual void Initialize(int rows, int columns, int seed)
    {
        Rows = rows;
        Columns = columns;
        CellTransform = new Dictionary<Cell, Transform>();
        mazeColors = mazeColorsList[Random.Range(0, mazeColorsList.Length)];
        Camera.main.backgroundColor = mazeColors.background;
        cellSize = (int)(cellPrefab.GetComponent<Renderer>().bounds.size.x);
        random = new System.Random(seed);

        PrepareGrid();
        ConfigureCells();
    }

    public virtual Cell this[int row, int col]
    {
        get
        {
            if (row < 0 || row >= Rows) return null;
            if (col < 0 || col >= Columns) return null;
            return Cells[row][col];
        }
    }

    protected virtual void PrepareGrid()
    {
        Size = Rows * Columns;
        Cells = new Cell[Rows][];

        for (int i = 0; i < Rows; i++)
        {
            Cells[i] = new Cell[Columns];
            for (int j = 0; j < Columns; j++)
                Cells[i][j] = new Cell(i, j);
        }
    }

    protected virtual void ConfigureCells()
    {
        foreach (var cellRow in Cells)
        {
            foreach (var cell in cellRow)
            {
                int row = cell.Row;
                int col = cell.Column;

                cell.North = this[row - 1, col];
                cell.South = this[row + 1, col];
                cell.West = this[row, col - 1];
                cell.East = this[row, col + 1];
            }
        }
    }

    public virtual Cell RandomCell()
    {
        int row = random.Next(0, Rows);
        int col = random.Next(0, Columns);

        return Cells[row][col];
    }

    public IEnumerable<Cell[]> EachRow()
    {
        for (int i = 0; i < Rows; i++)
            yield return Cells[i];
    }

    public IEnumerable<Cell> EachCell()
    {
        foreach (var cellRow in Cells)
            foreach (var cell in cellRow)
                yield return cell;
    }

    public Cell GetCell(int row, int column) => Cells[row][column];

    public virtual void GenerateMaze()
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

        //Cells
        for (int x = 0; x < Rows; x++)
        {
            for (int y = 0; y < Columns; y++)
            {
                GameObject obj = Instantiate(cellPrefab, new Vector3(x, 0, y) * cellSize, Quaternion.identity, cellsHolder);
                obj.name = obj.transform.position.ToString();
                CellTransform.Add(Cells[x][y], obj.transform);

                if (Cells[x][y] == Start)
                    obj.GetComponent<Renderer>().material.color = mazeColors.start;
                else if (Cells[x][y] == End)
                    obj.GetComponent<Renderer>().material.color = mazeColors.end;
                else
                    obj.GetComponent<Renderer>().material.color = mazeColors.cell;
            }
        }

        int wallX = (int)(cellPrefab.GetComponent<Renderer>().bounds.size.x / 2);
        int wallZ = (int)(cellPrefab.GetComponent<Renderer>().bounds.size.z / 2);
        int wallIndex = Random.Range(0, wallPrefabs.Count);

        //Walls
        foreach (Cell cell in EachCell())
        {
            if (cell.North == null)
            {
                Vector3 wallPosition = new Vector3(CellTransform[cell].position.x, 0, CellTransform[cell].position.z) + new Vector3(-wallX, 0, 0);
                GameObject obj = Instantiate(wallPrefabs[wallIndex], wallPosition, Quaternion.Euler(0, 90, 0), wallsHolder);
                obj.name = "North";
                obj.GetComponent<Renderer>().material.color = mazeColors.wall;
            }
            if (cell.West == null)
            {
                Vector3 wallPosition = new Vector3(CellTransform[cell].position.x, 0, CellTransform[cell].position.z) + new Vector3(0, 0, -wallZ);
                GameObject obj = Instantiate(wallPrefabs[wallIndex], wallPosition, Quaternion.identity, wallsHolder);
                obj.name = "West";
                obj.GetComponent<Renderer>().material.color = mazeColors.wall;
            }
            if (!cell.IsLinked(cell.East))
            {
                Vector3 wallPosition = new Vector3(CellTransform[cell].position.x, 0, CellTransform[cell].position.z) + new Vector3(0, 0, wallZ);
                GameObject obj = Instantiate(wallPrefabs[wallIndex], wallPosition, Quaternion.identity, wallsHolder);
                obj.name = "East";
                obj.GetComponent<Renderer>().material.color = mazeColors.wall;
            }
            if (!cell.IsLinked(cell.South))
            {
                Vector3 wallPosition = new Vector3(CellTransform[cell].position.x, 0, CellTransform[cell].position.z) + new Vector3(wallX, 0, 0);
                GameObject obj = Instantiate(wallPrefabs[wallIndex], wallPosition, Quaternion.Euler(0, 90, 0), wallsHolder);
                obj.name = "South";
                obj.GetComponent<Renderer>().material.color = mazeColors.wall;
            }
        }
    }

    public List<Cell> DeadEnds()
    {
        List<Cell> list = new List<Cell>();

        foreach (Cell cell in EachCell())
            if (cell.Links().Count == 1)
                list.Add(cell);

        return list;
    }

    public void Braid(float p = 1.0f)
    {
        List<Cell> deadends = DeadEnds().OrderBy(x => random.Next()).ToList();
        foreach (Cell cell in deadends)
        {
            if (cell.Links().Count != 1 || Random.Range(0, p) > p)
                continue;

            List<Cell> neighbors = cell.Neighbors.FindAll((Cell n) => !cell.IsLinked(n)).ToList();
            List<Cell> best = neighbors.Where(n => n.Links().Count == 1).ToList();

            if (!best.Any())
                best = neighbors;

            Cell neighbor = best[random.Next(0, best.Count)];
            cell.Link(neighbor);
        }
    }
}