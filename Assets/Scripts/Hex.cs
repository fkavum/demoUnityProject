using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour
{
    public int col;
    public int row;
    public int colorIndex;

    // This is not the real position.

    private GameObject[] neigbours;
    private HexMap _hexMap;
    //[NonSerialized]
    //public GameObject hexObj;

    public void setColandRow(int col, int row)
    {
        this.col = col;
        this.row = row;
        setPosition();
    }

    public void setHexmap(HexMap hexMap)
    {
        this._hexMap = hexMap;
       // this.hexObj = this.gameObject;
    }

    public void setPosition()
    {
        float width = _hexMap.hexRadius * 2;
        var height = _hexMap.HEIGHT_MULTIPLIER * width;

        var yTiling = height;
        float xTiling = width * 0.75f;

        float xPos;
        float yPos;


        xPos = (xTiling * (this.col) + _hexMap.bottomLeftCorner.x);
        if (col % 2 == 0)
        {
            yPos = yTiling * (this.row) + _hexMap.bottomLeftCorner.y;
        }
        else
        {
            yPos = yTiling * (this.row) + yTiling * 0.5f + _hexMap.bottomLeftCorner.y;
        }

        this.gameObject.transform.Translate(new Vector3(
            xPos + xPos * _hexMap.padding,
            yPos + yPos * _hexMap.padding,
            0
        ));
    }


    public void moveHex(int col, int row)
    {
        // change Arraylist index
        _hexMap._hexList[col][row] = this;

        // change Column and Row
        this.col = col;
        this.row = row;
        
        // change real position (When animation finishes it goes in his real place.)
        this.gameObject.transform.position = _hexMap.hexMapCoord[col][row];
    }

 
    
}