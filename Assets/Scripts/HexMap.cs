using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HexMap : MonoBehaviour
{
    public int gridCol = 10;
    public int gridRow = 5;

    public Material[] hexMetarials;
    // Start is called before the first frame update
    void Start()
    {
        generateMap();
    }

    public GameObject hexPrefab;

    public void generateMap()
    {
        for (int column = 0; column < gridCol; column++)
        {
            for (int row = 0; row < gridRow; row++)
            {
                createHexObj(column, row);
            }
        }
    }

    private void createHexObj(int column,int row)
    {
        Hex hex = new Hex(column, row);
        GameObject hexObj = Instantiate(hexPrefab, hex.position(), Quaternion.identity, this.transform);
        hexObj.name = "("+ column + "," + row+")";
        hex.instantiatedObject = hexObj;
        MeshRenderer mr = hexObj.GetComponentInChildren<MeshRenderer>();
        mr.material = hexMetarials[Random.Range(0, hexMetarials.Length)];
    }
}