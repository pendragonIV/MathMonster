using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OperatorType
{
    None,
    Plus,
    Minus,
    Multiply,
    Divide
}

public enum BlockType
{
    Number,
    Operator
}

public class Calculator : MonoBehaviour
{
    public static Calculator instance;

    [SerializeField]
    private Sprite[] operatorSprites;
    [SerializeField]
    private Sprite[] numberSprites;

    [SerializeField]
    private List<GameObject> verticalNumber = new List<GameObject>();
    [SerializeField]
    private List<GameObject> horizontalNumber = new List<GameObject>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void GetNearbyNumbers(GameObject objToCheck , Vector3Int positionToCheck)
    {
        verticalNumber.Clear();
        horizontalNumber.Clear();

        horizontalNumber.Add(objToCheck);
        verticalNumber.Add(objToCheck);

        Vector3Int nextLeft = positionToCheck + Vector3Int.left;
        Vector3Int nextRight = positionToCheck + Vector3Int.right;

        Vector3Int nextUp = positionToCheck + Vector3Int.up;
        Vector3Int nextDown = positionToCheck + Vector3Int.down;

        while (GridCellManager.instance.IsPlacedCell(nextRight))
        {
            horizontalNumber.Add(GridCellManager.instance.GetPlacedObj(nextRight));
            nextRight += Vector3Int.right;
        }

        while (GridCellManager.instance.IsPlacedCell(nextLeft))
        {
            horizontalNumber.Insert(0,GridCellManager.instance.GetPlacedObj(nextLeft));
            nextLeft += Vector3Int.left;
        }

        while (GridCellManager.instance.IsPlacedCell(nextUp))
        {
            verticalNumber.Add(GridCellManager.instance.GetPlacedObj(nextUp));
            nextUp += Vector3Int.up;
        }

        while (GridCellManager.instance.IsPlacedCell(nextDown))
        {
            verticalNumber.Insert(0,GridCellManager.instance.GetPlacedObj(nextDown));
            nextDown += Vector3Int.down;
        }


        //Test
        if (horizontalNumber.Count > 1)
        {
            string number = "";
            int totalNumber = 0;
            List<int> numbs = new List<int>();
            numbs.Add(0);
            List<OperatorType> operators = new List<OperatorType>();
            
            for (int i = 0; i < horizontalNumber.Count; i++)
            {
                if (horizontalNumber[i].GetComponent<Operation>().GetBlockType() == BlockType.Operator)
                {
                    operators.Add(horizontalNumber[i].GetComponent<Operation>().GetOperatorType());
                    totalNumber++;
                    number = "";
                    numbs.Add(0);
                }
                else
                {
                    number += horizontalNumber[i].GetComponent<Operation>().GetNumberValue().ToString();
                    numbs[totalNumber] = int.Parse(number);
                }
            }

            Calculate(numbs, operators);
        }
    }

    private void Calculate(List<int> numbs, List<OperatorType> operators)
    {
        double result = numbs[0];

        for(int i = 0; i < operators.Count; i++)
        {
            if (operators[i] == OperatorType.Plus)
            {
                if(numbs.Count > i + 1)
                {
                    result += numbs[i + 1];
                }
            }
            else if (operators[i] == OperatorType.Minus)
            {
                if (numbs.Count > i + 1)
                {
                    result -= numbs[i + 1];
                }
            }
            else if (operators[i] == OperatorType.Multiply)
            {
                if (numbs.Count > i + 1)
                {
                    result *= numbs[i + 1];
                }
            }
            else if (operators[i] == OperatorType.Divide)
            {
                if (numbs.Count > i + 1)
                {
                    result /= numbs[i + 1];
                }
            }
        }

        Debug.Log(result);
    }

    #region Get

    public Sprite GetOperatorSprite(OperatorType operatorType)
    {
        return operatorSprites[(int)operatorType - 1];
    }

    public Sprite GetNumberSprite(int number)
    {
        return numberSprites[number - 1];
    }

    #endregion
}
