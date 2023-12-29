using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultBlock : MonoBehaviour
{
    [SerializeField]
    private int number;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = Calculator.instance.GetResultSprite(number);
    }

    public int GetNumber()
    {
        return number;
    }

}
