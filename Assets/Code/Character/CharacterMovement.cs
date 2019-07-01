using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] Button up, down, left, right;
    [SerializeField] Grid grid;
    Cell currentCell;

    public static CharacterMovement Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    enum Direction
    {
        Left, Right, Up, Down
    }

    void Start()
    {
        up.onClick.AddListener(() => MovementDirection(Direction.Up));
        down.onClick.AddListener(() => MovementDirection(Direction.Down));
        left.onClick.AddListener(() => MovementDirection(Direction.Left));
        right.onClick.AddListener(() => MovementDirection(Direction.Right));
    }    

    public void SpawnPlayer()
    {
        currentCell = grid.Start;
        gameObject.transform.position = grid.CellTransform[currentCell].position;
    }

    void MovementDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Left: MoveToCell(currentCell.North); break;
            case Direction.Right: MoveToCell(currentCell.South); break;
            case Direction.Up: MoveToCell(currentCell.East); break;
            case Direction.Down: MoveToCell(currentCell.West); break;
        }
    }

    void MoveToCell(Cell cell)
    {
        if (!currentCell.IsLinked(cell))
            return;
        transform.position = grid.CellTransform[cell].position;
        currentCell = cell;
    }
}

