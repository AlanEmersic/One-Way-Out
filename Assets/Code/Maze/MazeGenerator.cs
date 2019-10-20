using MazeAlgorithms;
using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;

[RequireComponent(typeof(Grid))]
public class MazeGenerator : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI algorithmText;
    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] TaskGenerator taskGenerator;
    [SerializeField] TrapGenerator trapGenerator;
    [SerializeField] ColorGenerator colorGenerator;

    int gridSize = 5;
    Algorithm algorithm;
    MazeType mazeType;
    int seed;

    enum Algorithm
    {
        AldousBroder, BinaryTree, HuntAndKill, RecursiveBacktracker, Sidewinder,
        Wilsons, Houstons, Prims, TruePrims, Kruskals, GrowingTree, RecursiveDivision, Ellers
    }

    enum MazeType
    {
        Grid//, Polar, Hex, Triangle
    }

    public void GenerateMaze()
    {
        System.Random random = new System.Random(seed);
        gridSize = random.Next(5, 11);
        seed = random.Next(int.MinValue, int.MaxValue);

        //seed = (int)System.DateTime.Now.Ticks;
        int algorithmCount = System.Enum.GetNames(typeof(Algorithm)).Length;
        algorithm = (Algorithm)random.Next(algorithmCount);
        mazeType = (MazeType)random.Next(System.Enum.GetNames(typeof(MazeType)).Length);
        Stopwatch stopwatch = new Stopwatch();
        algorithmText.text = algorithm.ToString();
        Grid grid = gameObject.GetComponent<Grid>();
        PolarGrid polarGrid = gameObject.GetComponent<PolarGrid>();

        if (algorithm != Algorithm.BinaryTree && algorithm != Algorithm.Ellers
            && algorithm != Algorithm.Kruskals && algorithm != Algorithm.RecursiveDivision
            && algorithm != Algorithm.Sidewinder)
            switch (mazeType)
            {
                case MazeType.Grid:
                    grid.Initialize(gridSize, gridSize, seed);
                    RandomAlgorithm<Grid, Cell>(grid, algorithm);
                    grid.GenerateMaze();
                    break;
                //case MazeType.Polar:
                //    polarGrid.Initialize(gridSize, 1, seed);
                //    RandomAlgorithm<PolarGrid, PolarCell>(polarGrid, algorithm);
                //    polarGrid.GenerateMaze();
                //    break;
                    //case MazeType.Hex:
                    //    break;
                    //case MazeType.Triangle:
                    //    break;
            }
        else
        {
            grid.Initialize(gridSize, gridSize, seed);
            RandomAlgorithm<Grid, Cell>(grid, algorithm);
            grid.GenerateMaze();
        }

        stopwatch.Start();
        //maze.Braid();
        //LongestPathInMaze(maze);        

        //taskGenerator.CreateTasks(maze, seed);
        //trapGenerator.CreateTraps(maze, seed);
        //colorGenerator.Initialize(seed);

        Camera.main.transform.position = new Vector3(gridSize * 10, gridSize * 5, Camera.main.transform.position.z);
        Camera.main.orthographicSize = gridSize * 5;

        stopwatch.Stop();
        timer.text = stopwatch.ElapsedMilliseconds.ToString() + "ms";
    }

    public void Dijkstra(Grid maze)
    {
        Cell startCell = new Cell(Random.Range(0, gridSize), Random.Range(0, gridSize));
        Cell endCell = new Cell(Random.Range(0, gridSize), Random.Range(0, gridSize));

        while (startCell.Row == endCell.Row && startCell.Column == endCell.Column)
            endCell = new Cell(Random.Range(0, gridSize), Random.Range(0, gridSize));

        maze.Start = maze.GetCell(startCell.Row, startCell.Column);
        maze.End = maze.GetCell(endCell.Row, endCell.Column);
        Distances distances = maze.Start.Distances();
        maze.Distances = distances.PathTo(maze.End);
        maze.GenerateMaze();
    }

    public void LongestPathInMaze(Grid maze)
    {
        maze.Start = maze.GetCell(0, 0);
        Distances distances = maze.Start.Distances();

        maze.Distances = distances;
        KeyValuePair<Cell, int> newStart = distances.Maximum();

        Distances newDistances = newStart.Key.Distances();
        maze.Start = newStart.Key;
        KeyValuePair<Cell, int> goal = newDistances.Maximum();
        maze.End = goal.Key;
        maze.Distances = newDistances.PathTo(maze.End);
    }

    void RandomAlgorithm<G, T>(Grid grid, Algorithm algorithm) where G : Grid where T : Cell
    {
        switch (algorithm)
        {
            case Algorithm.AldousBroder: AldousBroder.CreateMaze<G, T>(grid as G, seed); break;

            case Algorithm.BinaryTree: BinaryTree.CreateMaze(grid, seed); break;

            case Algorithm.HuntAndKill: HuntAndKill.CreateMaze<G, T>(grid as G, seed); break;

            case Algorithm.RecursiveBacktracker: RecursiveBacktracker.CreateMaze<G, T>(grid as G, seed); break;

            case Algorithm.Sidewinder: Sidewinder.CreateMaze(grid, seed); break;

            case Algorithm.Wilsons: Wilsons.CreateMaze<G, T>(grid as G, seed); break;

            case Algorithm.Kruskals: Kruskals.CreateMaze(grid, seed); break;

            case Algorithm.Prims: Prims.CreateMaze<G, T>(grid as G, seed); break;

            case Algorithm.TruePrims: TruePrims.CreateMaze<G, T>(grid as G, seed); break;

            case Algorithm.GrowingTree: GrowingTree.CreateMaze<G, T>(grid as G, seed); break;

            case Algorithm.RecursiveDivision: RecursiveDivision.CreateMaze(grid, seed); break;

            case Algorithm.Ellers: Ellers.CreateMaze(grid, seed); break;

            case Algorithm.Houstons: Houstons.CreateMaze<G, T>(grid as G, seed); break;
        }
    }
}