﻿using System;
using System.Collections;
using System.Collections.Generic;
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

    public bool animate = false;
    void Start()
    {
        generateMap();
        generateCoordinates();
        Debug.Log("Map Generated Successfully!");

       /* while (breakTriples())
        {
            Debug.Log("we need to break triples");
        }*/
       StartCoroutine(deleteGarbages());
    }

    private IEnumerator deleteGarbages()
    {
        while (breakTriples())
        {
            yield return new WaitForSeconds(0.1f);
            Debug.Log("we need to break triples");
        }
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
        //rand = Random.Range(1, 4);
        MeshRenderer mr = hexObj.GetComponentInChildren<MeshRenderer>();
        mr.material = hexMetarials[rand];
        hex.colorIndex = rand;
        // 2d array that holds hexes
        _hexList[column].Add(hex);
    }

    public bool breakTriples()
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

        if (breakList.Count == 0)
        {
            return false;
        }

        foreach (Hex breakHex in breakList)
        {
            breakHex.gameObject.transform.Translate(new Vector3(0, 10, 0));

        }
        fillEmptyGrids(breakList);

        return true;
    }

    private bool isInBreakList(int col, int row)
    {
        int colorIndex = _hexList[col][row].colorIndex;

        int c0, c1, c2, c3, c4, c5, r0, r1, r2, r3, r4, r5;

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

        //Debug.Log("Checking: " + col + "" + row);
        // 22    11 (col-1,row-1 -- 0) 12 (col-1,row -- 1) 21 (col,row-1 -- 2) 23 (col, row+1 -- 3 ) 31 (col +1 , row-1 -- 4) 32 (col+1 ,row -- 5)
        // 0 - 1 
        if (c0 >= 0 && r0 >= 0 && c1 >= 0 && r1 < gridRow)
        {
            if (_hexList[c0][r0].colorIndex == _hexList[c1][r1].colorIndex &&
                colorIndex == _hexList[c1][r1].colorIndex)
            {
                Debug.Log(col + " " + row + "with  0 - 1");
                return true;
            }
        }

        if (c0 >= 0 && r0 >= 0 && r2 >= 0)
        {
            // 0 -2 
            if (_hexList[c0][r0].colorIndex == _hexList[c2][r2].colorIndex &&
                colorIndex == _hexList[c2][r2].colorIndex)
            {
                Debug.Log(col + " " + row + "with  0 - 2");
                return true;
            }
        }

        if (c1 >= 0 && r1 < gridRow && r3 < gridRow)
        {
            // 1 - 3
            if (_hexList[c1][r1].colorIndex == _hexList[c3][r3].colorIndex &&
                colorIndex == _hexList[c1][r1].colorIndex)
            {
                Debug.Log(col + " " + row + "with  1 - 3");
                return true;
            }
        }

        if (r2 >= 0 && r4 >= 0 && c4 < gridCol)
        {
            // 2  -4 
            if (_hexList[c2][r2].colorIndex == _hexList[c4][r4].colorIndex &&
                colorIndex == _hexList[c2][r2].colorIndex)
            {
                Debug.Log(col + " " + row + "with  2 - 4");
                return true;
            }
        }

        if (r3 < gridRow && r5 < gridRow && c5 < gridCol)
        {
            // 3 - 5
            if (_hexList[c3][r3].colorIndex == _hexList[c5][r5].colorIndex &&
                colorIndex == _hexList[c3][r3].colorIndex)
            {
                Debug.Log(col + " " + row + "with  3 - 5");
                return true;
            }
        }

        if (r5 < gridRow && c5 < gridCol && r4 >= 0 && c4 < gridCol)
        {
            //4 - 5 
            if (_hexList[c4][r4].colorIndex == _hexList[c5][r5].colorIndex &&
                colorIndex == _hexList[c5][r5].colorIndex)
            {
                Debug.Log(col + " " + row + "with  4 - 5");
                return true;
            }
        }

        return false;
    }


    private void fillEmptyGrids(List<Hex> breakList)
    {
        // Sorting by looking their col*10 + row values.
        breakList.Sort((y, x) =>
            (x.GetComponent<Hex>().row).CompareTo(y.GetComponent<Hex>().row));
        foreach (Hex breakHex in breakList)
        {

            fillEmptyGrid(breakHex.col,breakHex.row);
        }
        foreach (Hex breakHex in breakList)
        {

            Destroy(breakHex.gameObject);
        }
    }

    private void fillEmptyGrid(int emptyGridCol, int emptyGridRow)
    {
        
        if (emptyGridRow == gridRow - 1)
        {
            newHexHasArrived(emptyGridCol,emptyGridRow);
            return;
        }

        Hex hexGoesDown = _hexList[emptyGridCol][emptyGridRow + 1];
        
        
        // change row and column
        hexGoesDown.row = emptyGridRow;
        // change hexmap
        _hexList[emptyGridCol][emptyGridRow] = hexGoesDown;
        // Move the real location
        StartCoroutine(wait(hexGoesDown, hexMapCoord[emptyGridCol][emptyGridRow]));
       // hexGoesDown.transform.position = hexMapCoord[emptyGridCol][emptyGridRow];}
        // Recursively goes down
        fillEmptyGrid(emptyGridCol, emptyGridRow + 1);


    }

    public IEnumerator wait(Hex hexGoesDown,Vector3 hexMapCoor)
    {
        Debug.Log("Let Break Things.");
        float distance = 1f;
        if (!animate)
        {
            distance = 0f;}
        
        while (distance > 0.1f)
        {
            if(hexGoesDown != null){
            hexGoesDown.gameObject.transform.position = Vector3.MoveTowards(hexGoesDown.gameObject.transform.position, hexMapCoor, 0.05f);
            distance = Vector3.Distance(hexGoesDown.gameObject.transform.position, hexMapCoor);
            yield return null;}
            else
            {
                distance = 0f;
            }

        }
        if(hexGoesDown != null){
        hexGoesDown.transform.position = hexMapCoor;}
    }

    private void newHexHasArrived(int col,int row)
    {
        // Initialization
        GameObject hexObj =
            Instantiate(hexPrefab, new Vector3(hexMapCoord[col][row].x, hexMapCoord[col][row].y + 2f, 0), Quaternion.identity, this.transform.Find("Hexagons"));
        Hex hex = hexObj.GetComponent<Hex>();
        hex.setHexmap(this);
        hex.setColandRowAnimatively(col, row);
        hexObj.name = "c" + col + "_r" + row;

        // Modification. 
        int rand = Random.Range(1, hexMetarials.Length);
        //rand = Random.Range(1, 4);
        MeshRenderer mr = hexObj.GetComponentInChildren<MeshRenderer>();
        mr.material = hexMetarials[rand];
        hex.colorIndex = rand;
        // 2d array that holds hexes
        _hexList[col][row] = hex;
        
        Debug.Log("I'll fill the " + row + " later");
    }
}