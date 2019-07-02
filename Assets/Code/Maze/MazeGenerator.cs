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
    Algorithm algorithm;
    int gridSize = 10;
    int seed;

    enum Algorithm
    {
        AldousBroder, BinaryTree, HuntAndKill, RecursiveBacktracker, Sidewinder,
        Wilsons, Prims, TruePrims, Kruskals, GrowingTree, RecursiveDivision, Ellers
    }    

    public void GenerateMaze()
    {
        seed = Random.Range(int.MinValue, int.MaxValue);
        //seed = (int)System.DateTime.Now.Ticks;
        int algorithmCount = System.Enum.GetNames(typeof(Algorithm)).Length;
        algorithm = (Algorithm)Random.Range(0, algorithmCount);
        Stopwatch stopwatch = new Stopwatch();
        algorithmText.text = algorithm.ToString();

        Grid grid = gameObject.GetComponent<Grid>();

        grid.Initialize(gridSize, gridSize, seed);
        stopwatch.Start();
        Grid maze = RandomAlgorithm(grid, algorithm);

        //maze.Braid();
        //maze.GenerateMaze();
        LongestPathInMaze(maze);

        colorGenerator.Initialize(seed);
        taskGenerator.CreateTasks(maze, seed);        
        trapGenerator.CreateTraps(maze, seed);

        stopwatch.Stop();
        timer.text = stopwatch.ElapsedMilliseconds.ToString() + "ms";
    }

    public void Dijkstra(Grid maze)
    {
        Cell startCell = new Cell(Random.Range(0, gridSize), Random.Range(0, gridSize));
        Cell endCell = new Cell(Random.Range(0, gridSize), Random.Range(0, gridSize));

        if (startCell.Row == endCell.Row && startCell.Column == endCell.Column)
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
        maze.GenerateMaze();
    }

    Grid RandomAlgorithm(Grid grid, Algorithm algorithm)
    {
        switch (algorithm)
        {
            case Algorithm.AldousBroder: return AldousBroder.CreateMaze(grid, seed);

            case Algorithm.BinaryTree: return BinaryTree.CreateMaze(grid, seed);

            case Algorithm.HuntAndKill: return HuntAndKill.CreateMaze(grid, seed);

            case Algorithm.RecursiveBacktracker: return RecursiveBacktracker.CreateMaze(grid, seed);

            case Algorithm.Sidewinder: return Sidewinder.CreateMaze(grid, seed);

            case Algorithm.Wilsons: return Wilsons.CreateMaze(grid, seed);

            case Algorithm.Kruskals: return Kruskals.CreateMaze(grid, seed);

            case Algorithm.Prims: return Prims.CreateMaze(grid, seed);

            case Algorithm.TruePrims: return TruePrims.CreateMaze(grid, seed);

            case Algorithm.GrowingTree: return GrowingTree.CreateMaze(grid, seed);

            case Algorithm.RecursiveDivision: return RecursiveDivision.CreateMaze(grid, seed);

            case Algorithm.Ellers: return Ellers.CreateMaze(grid, seed);
        }
        return null;
    }
}