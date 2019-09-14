using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class CharacterMovement : MonoBehaviour
{
    [SerializeField] Button up, down, left, right;
    [SerializeField] Grid grid;

    Cell currentCell;
    float speed = 20f;
    IEnumerator currentMoveCoroutine;
    Direction previousDirection;

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
        RotateCharacterOnStart(currentCell);
    }

    void RotateCharacterOnStart(Cell cell)
    {
        if (cell.IsLinked(cell.North))
        {
            transform.rotation = Quaternion.Euler(0, -90, 0);
            previousDirection = Direction.Up;
        }
        else if (cell.IsLinked(cell.South))
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
            previousDirection = Direction.Down;
        }
        else if (cell.IsLinked(cell.West))
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            previousDirection = Direction.Left;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            previousDirection = Direction.Right;
        }
    }

    void RotateCharacter(Direction direction)
    {
        if (previousDirection == direction)
            return;
        else if (direction == Direction.Left)
        {
            previousDirection = Direction.Left;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (direction == Direction.Right)
        {
            previousDirection = Direction.Right;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (direction == Direction.Up)
        {
            previousDirection = Direction.Up;
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        else if (direction == Direction.Down)
        {
            previousDirection = Direction.Down;
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
    }

    void MovementDirection(Direction direction)
    {
        RotateCharacter(direction);

        switch (direction)
        {
            case Direction.Left: MoveToCell(currentCell.West); break;
            case Direction.Right: MoveToCell(currentCell.East); break;
            case Direction.Up: MoveToCell(currentCell.North); break;
            case Direction.Down: MoveToCell(currentCell.South); break;
        }
    }

    void MoveToCell(Cell cell)
    {
        if (!currentCell.IsLinked(cell))
            return;

        if (currentMoveCoroutine != null)
            StopCoroutine(currentMoveCoroutine);

        currentMoveCoroutine = Move(grid.CellTransform[cell].position, speed);
        StartCoroutine(currentMoveCoroutine);
        currentCell = cell;
    }

    IEnumerator Move(Vector3 destination, float speed)
    {
        while (transform.position != destination)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
            yield return null;
        }
    }
}