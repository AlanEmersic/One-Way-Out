using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using MazeAlgorithms;

[RequireComponent(typeof(Grid))]
public class DeadEndCounts : MonoBehaviour
{
    enum Algorithm
    {
        AldousBroder, BinaryTree, HuntAndKill, RecursiveBacktracker, Sidewinder,
        Wilsons, Prims, TruePrims, Kruskals, GrowingTree, RecursiveDivision, Ellers
    }

    [SerializeField] int tries = 500;
    [SerializeField] int size = 10;
    [SerializeField] int seed = 100;

    public void GenerateMazes()
    {
        int algorithmCount = System.Enum.GetNames(typeof(Algorithm)).Length;
        Dictionary<Algorithm, int> averages = new Dictionary<Algorithm, int>();
        Grid grid = gameObject.GetComponent<Grid>();
        string file = @"C:\Alan\Unity 2018\One Way Out\Assets\DeadEnds.txt";
        StreamWriter sw = File.CreateText(file);
        sw.WriteLine("Dead - ends");
        print(file);
        print("Started");
        foreach (Algorithm algorithm in System.Enum.GetValues(typeof(Algorithm)))
        {
            List<int> deadEndCounts = new List<int>();

            for (int i = 0; i < tries; i++)
            {
                grid.Initialize(size, size, seed);
                grid = GetAlgorithm(grid, algorithm);
                deadEndCounts.Add(grid.DeadEnds().Count);
            }

            int totalDeadEnds = 0;
            for (int i = 0; i < deadEndCounts.Count; i++)
                totalDeadEnds += deadEndCounts[i];
            averages[algorithm] = totalDeadEnds / deadEndCounts.Count;
        }

        int totalCells = size * size;
        sw.WriteLine($"Average dead-ends per {size}x{size} maze {totalCells} cells, tries: {tries}");
        var sortedAlgorithms = averages.OrderBy(x => averages[x.Key]).ToList();

        foreach (var algorithm in sortedAlgorithms)
        {
            float precentage = averages[algorithm.Key] * 100.0f / (size * size);
            sw.WriteLine($"{algorithm.Key} : {averages[algorithm.Key]}/{totalCells} ({precentage}%)");
        }

        sw.Close();
        print("Finished");
    }

    Grid GetAlgorithm(Grid grid, Algorithm algorithm)
    {
        System.Random random = new System.Random();
        int seed = random.Next();

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