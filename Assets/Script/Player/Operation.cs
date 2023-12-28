using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Operation : MonoBehaviour
{
    [SerializeField]
    private BlockType blockType;
    [SerializeField]
    private OperatorType operatorType;
    [SerializeField]
    private int numberValue;


    private void Awake()
    {
        if(blockType == BlockType.Number)
        {
            operatorType = OperatorType.None;
        }
        else if(blockType == BlockType.Operator)
        {
            numberValue = 0;
        }
    }

    private void Start()
    {
        if(blockType == BlockType.Number && numberValue > 0 && numberValue < 10)
        {
            GetComponent<SpriteRenderer>().sprite = Calculator.instance.GetNumberSprite(numberValue);
        }
        else if(blockType == BlockType.Operator)
        {
            GetComponent<SpriteRenderer>().sprite = Calculator.instance.GetOperatorSprite(operatorType);
        }
    }

    public BlockType GetBlockType()
    {
        return blockType;
    }

    public OperatorType GetOperatorType()
    {
        return operatorType;
    }

    public int GetNumberValue()
    {
        return numberValue;
    }
}
