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
    Rigidbody rb;    

    enum Direction
    {
        Left, Right, Up, Down
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();

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

