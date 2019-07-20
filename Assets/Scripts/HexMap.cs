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
    public Vector2 bottomLeftCorner = new Vector2(-4f,-4f);
    
    // About individual hexes
    public float padding = 0.1f;
    public readonly float HEIGHT_MULTIPLIER = Mathf.Sqrt(3) / 2;
    public Material[] hexMetarials;


    List<List<Hex>> _hexList = new List<List<Hex>>();
    
    void Start()
    {
        generateMap();
        Debug.Log("Map Generated Successfully!");
    }

    public GameObject hexPrefab;

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

    private void createHexObj(int column,int row)
    {
        Hex hex = new Hex(column, row,this);
        GameObject hexObj = Instantiate(hexPrefab, hex.position(), Quaternion.identity, this.transform.Find("Hexagons"));
        hexObj.name = "c"+ column + "_r" + row;
        hex.instantiatedObject = hexObj;
        MeshRenderer mr = hexObj.GetComponentInChildren<MeshRenderer>();
        mr.material = hexMetarials[Random.Range(0, hexMetarials.Length)];
        
        // 2d array that holds hexes
        _hexList[column].Add(hex);
    }
}