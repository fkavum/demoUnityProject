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
                hexMapCoord[column].Add(_hexList[column][row].gameObject.transform.position);
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
            Instantiate(hexPrefab, new Vector3(0, 0, 0), Quaternion.identity, this.transform.Find("Hexagons"));
        Hex hex = hexObj.GetComponent<Hex>();
        hex.setHexmap(this);
        hex.setColandRow(column, row);
        hexObj.name = "c" + column + "_r" + row;

        // Modification. 
        int rand = Random.Range(1, hexMetarials.Length);
        MeshRenderer mr = hexObj.GetComponentInChildren<MeshRenderer>();
        mr.material = hexMetarials[rand];
        hex.colorIndex = rand;
        // 2d array that holds hexes
        _hexList[column].Add(hex);
    }

    public void breakTriples()
    {
        List<Hex> breakList = new List<Hex>();
        for (int column = 0; column < gridCol; column++)
        {
            for (int row = 0; row < gridRow; row++)
            {
                if (isInBreakList(column, row))
                {
                    //Debug.Log(column +" "+row +"should be added");
                    breakList.Add(_hexList[column][row]);
                }
            }
        }


        foreach (Hex breakHex in breakList)
        {
            breakHex.gameObject.transform.Translate(new Vector3(0, 10, 0));
        }
    }

    private bool isInBreakList(int col, int row)
    {
        int colorIndex = _hexList[col][row].colorIndex;

        int c0,c1,c2,c3,c4,c5,r0,r1,r2,r3,r4,r5;

        c2 = col;
        c3 = col;
        r2 = row - 1;
        r3 = row + 1;
        c0 = col - 1;
        c1 = col - 1;
        c4 = col + 1;
        c5 = col + 1;

        if (col % 2 == 0)
        {
            r0 = row - 1;
            r4 = row - 1;
            r1 = row;
            r5 = row;
        }
        else
        {
            r0 = row;
            r4 = row;
            r1 = row + 1;
            r5 = row + 1;
        }
        
        
        // 22    11 (col-1,row-1 -- 0) 12 (col-1,row -- 1) 21 (col,row-1 -- 2) 23 (col, row+1 -- 3 ) 31 (col +1 , row-1 -- 4) 32 (col+1 ,row -- 5)
        // 0 - 1 
        if (c0 >= 0 && r0 >= 0 && c1 >=0 && r1 < gridRow )
        {
            if (_hexList[c0][r0].colorIndex == _hexList[c1][r1].colorIndex &&
                colorIndex == _hexList[c1][r1].colorIndex)
            {
                Debug.Log(col + " " +row + "with  0 - 1" );
                return true;
            }
        }
        if (c0 >= 0 && r0 >= 0 && r2 >= 0)
        {
            // 0 -2 
            if (_hexList[c0][r0].colorIndex == _hexList[c2][r2].colorIndex &&
                colorIndex == _hexList[c2][r2].colorIndex)
            {
                Debug.Log(col + " " +row + "with  0 - 2" );
                return true;
            }
        }

        if (c1 >= 0 && r1 < gridRow && r3 < gridRow)
        {
            // 1 - 3
            if (_hexList[c1][r1].colorIndex == _hexList[c3][r3].colorIndex &&
                colorIndex == _hexList[c1][r1].colorIndex)
            {
                Debug.Log(col + " " +row + "with  1 - 3" );
                return true;
            }
        }

        if (r2 >= 0 && r4 >= 0 && c4 < gridRow)
        {
            // 2  -4 
            if (_hexList[c2][r2].colorIndex == _hexList[c4][r4].colorIndex &&
                colorIndex == _hexList[c2][r2].colorIndex)
            {
                Debug.Log(col + " " +row + "with  2 - 4" );
                return true;
            }
        }

        if (r3 < gridRow && r5 < gridRow && c5 < gridRow)
        {
            // 3 - 5
            if (_hexList[c3][r3].colorIndex == _hexList[c5][r5].colorIndex &&
                colorIndex == _hexList[c3][r3].colorIndex)
            {
                Debug.Log(col + " " +row + "with  3 - 5" );
                return true;
            }
        }

        if ( r5 < gridRow && c5 < gridRow && r4 >= 0 && c4 < gridRow)
        {
            //4 - 5 
            if (_hexList[c4][r4].colorIndex == _hexList[c5][r5].colorIndex &&
                colorIndex == _hexList[c5][r5].colorIndex)
            {
                Debug.Log(col + " " +row + "with  4 - 5" );
                return true;
            }
        }

        return false;
    }
}