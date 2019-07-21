using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class HexMap : MonoBehaviour
{
    public int gridCol = 10;
    public int gridRow = 5;
    public Vector2 bottomLeftCorner = new Vector2(-4f, -4f);
    public readonly float hexRadius = 1f;

    // About individual hexes
    public float padding = 0.1f;
    public readonly float HEIGHT_MULTIPLIER = Mathf.Sqrt(3) / 2;
    public Material[] hexMetarials;
    public GameObject hexPrefab;


    List<List<Hex>> _hexList = new List<List<Hex>>();

    void Start()
    {
        generateMap();
        Debug.Log("Map Generated Successfully!");
    }


    public void generateMap()
    {
        for (int column = 0; column < gridCol; column++)
        {
            _hexList.Add(new List<Hex>());
            for (int row = 0; row < gridRow; row++)
            {
                createHexObj(column, row);
            }
        }
    }

    private void createHexObj(int column, int row)
    {
        Hex hex = new Hex(column, row, this);
        GameObject hexObj =
            Instantiate(hexPrefab, hex.position(), Quaternion.identity, this.transform.Find("Hexagons"));
        hexObj.name = "c" + column + "_r" + row;
        hex.instantiatedObject = hexObj;
        MeshRenderer mr = hexObj.GetComponentInChildren<MeshRenderer>();
        mr.material = hexMetarials[Random.Range(1, hexMetarials.Length)];

        // 2d array that holds hexes
        _hexList[column].Add(hex);
    }
    /*
    private void initializeSelectors()
    {
        Instantiate(selectorPrefab, new Vector3(selectorStartPosL.x, selectorStartPosL.y, 0),
            Quaternion.identity,
            this.transform.Find("Selector"));
        
      Instantiate(selectorPrefab, new Vector3(selectorStartPosR.x, selectorStartPosR.y, 0),
            Quaternion.identity,
            this.transform.Find("Selector"));

      int selectorRowCount = gridRow - 1;
      int selectorColCount = gridCol - 1;
        

    }
    private void setSelectorStartPos()
    {
        Vector2 firstHexPos = new Vector2(bottomLeftCorner.x * (1f + padding), bottomLeftCorner.y * (1f + padding));
        selectorStartPosL = new Vector2(firstHexPos.x + hexRadius / 2f + padding / 2f,
            firstHexPos.y + HEIGHT_MULTIPLIER * hexRadius + padding / 2f
        );
        selectorStartPosR = new Vector2(selectorStartPosL.x + hexRadius/2f + padding / 2f,
            selectorStartPosL.y + HEIGHT_MULTIPLIER*hexRadius + padding / 2f
        );
    }*/

}