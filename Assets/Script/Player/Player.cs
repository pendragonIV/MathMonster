using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region Movement variables
    private Vector2 movementDirection;
    private bool isMoving = false;
    #endregion

    #region Push variables
    [SerializeField]
    private List<GameObject> pushing;
    #endregion

    private void Update()
    {
        MovementController();
    }

    private void MovementController()
    {
        if (movementDirection != Vector2.zero && !GameManager.instance.IsGameLose() && !GameManager.instance.IsGameWin() && !isMoving)
        {
            Vector3Int cellPos = GridCellManager.instance.GetObjCell(transform.position);
            Vector3Int nextCellPos = cellPos + new Vector3Int((int)movementDirection.x, (int)movementDirection.y, 0);
            if (GridCellManager.instance.IsPlaceableArea(nextCellPos))
            {
                if(GridCellManager.instance.IsPlacedCell(nextCellPos))
                {
                    GameObject check = GridCellManager.instance.GetPlacedObj(nextCellPos);
                    if (check.CompareTag("Result"))
                    {
                        movementDirection = Vector2.zero;
                        return;
                    }
                }
                GetNextNumbers(nextCellPos, movementDirection);
                if (pushing.Count > 0)
                {
                    if (!IsCanPush(pushing[pushing.Count - 1], movementDirection))
                    {
                        movementDirection = Vector2.zero;
                        return;
                    }
                    else
                    {
                        PushingNumbers(pushing, movementDirection);
                    }
                }
                MoveObj(this.gameObject, movementDirection);
                isMoving = true;
            }
            movementDirection = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Goal"))
        {
            GameManager.instance.Win();
        }
    }

    #region Input System
    public void OnMove(InputValue value)
    {
        if(GameManager.instance.IsGameLose() || GameManager.instance.IsGameWin())
        {
            return;
        }
        movementDirection = value.Get<Vector2>();
    }
    #endregion

    #region Pushing

    public void PushingNumbers(List<GameObject> numbers, Vector2 dir)
    {
        Vector3Int pos = GridCellManager.instance.GetObjCell(numbers[0].transform.position);
        Vector3Int next = pos + Vector3Int.FloorToInt(dir);

        int totalNumber = numbers.Count;
        for(int i  = totalNumber - 1; i >= 0; i--)
        {
            Vector3Int nextCellPos = MoveObj(numbers[i], dir);
        }
        Calculator.instance.GetNearbyNumbers(null, pos);
        Calculator.instance.GetNearbyNumbers(numbers[0], next);
    }

    #endregion

    #region Moving

    public Vector3Int MoveObj(GameObject objectToMove, Vector2 dir)
    {
        Vector3Int cellPos = GridCellManager.instance.GetObjCell(objectToMove.transform.position);
        Vector3Int nextCellPos = cellPos + Vector3Int.FloorToInt(dir);
        GridCellManager.instance.RemovePlacedCell(cellPos);
        objectToMove.transform.DOMove(GridCellManager.instance.PositonToMove(nextCellPos), 0.3f).OnComplete(() =>
        {
            isMoving = false;
        });
        GridCellManager.instance.SetPlacedCell(nextCellPos, objectToMove);

        return nextCellPos;
    }

    #endregion

    #region Checking

    public void GetNextNumbers(Vector3Int nextPos, Vector2 dir)
    {
        pushing.Clear();
        Vector3Int nextCellPos = nextPos;
        while(GridCellManager.instance.IsPlaceableArea(nextCellPos))
        {
            Collider2D next = Physics2D.OverlapPoint(GridCellManager.instance.PositonToMove(nextCellPos));
            if (next && next.CompareTag("Pushable"))
            {
                pushing.Add(next.gameObject); 
            }
            else
            {
                break;
            }
            nextCellPos += Vector3Int.FloorToInt(dir);
        }
    }

    public bool IsCanPush(GameObject numberBlock, Vector2 dir)
    {
        Vector3Int cellPos = GridCellManager.instance.GetObjCell(numberBlock.transform.position);
        Vector3Int nextCellPos = cellPos + Vector3Int.FloorToInt(dir);

        if(GridCellManager.instance.IsPlaceableArea(nextCellPos) && !GridCellManager.instance.IsPlacedCell(nextCellPos))
        {
            return true;
        }
        return false;
    }

    #endregion
}
