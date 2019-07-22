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


    public List<List<Hex>> _hexList = new List<List<Hex>>();
    public List<List<Vector3>> hexMapCoord = new List<List<Vector3>>();
    void Start()
    {
        generateMap();
        generateCoordinates();
        Debug.Log("Map Generated Successfully!");
    }

    private void generateCoordinates()
    {
        for (int column = 0; column < gridCol; column++)
               {
                   hexMapCoord.Add(new List<Vector3>());
                   for (int row = 0; row < gridRow; row++)
                   {
                       hexMapCoord[column].Add( _hexList[column][row].gameObject.transform.position);
                   }
               }
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

        // Initialization
        GameObject hexObj =
            Instantiate(hexPrefab, new Vector3(0,0,0), Quaternion.identity, this.transform.Find("Hexagons"));
        Hex hex = hexObj.GetComponent<Hex>();
        hex.setHexmap(this);
        hex.setColandRow(column,row);
        hexObj.name = "c" + column + "_r" + row;
        
        // Modification. Add color index
        MeshRenderer mr = hexObj.GetComponentInChildren<MeshRenderer>();
    
        mr.material = hexMetarials[Random.Range(1, hexMetarials.Length)];

        // 2d array that holds hexes
        _hexList[column].Add(hex);
    }
    

}