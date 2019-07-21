using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex:MonoBehaviour
{
   [NonSerialized]
   public int col;
   [NonSerialized]
   public int row;
   
   
   // This is not the real position.
   private float xPos;
   private float yPos;
   
   private GameObject[] neigbours;
   private HexMap _hexMap;

/*
   public Hex(int col,int row , HexMap hexMap )
   {
      this._hexMap = hexMap;
      this.col = col;
      this.row = row;
      //this.sumFactor = -(col + row);
   }*/

   public void setColandRow(int col,int row)
   {
      this.col = col;
      this.row = row;
      setPosition();
   }

   public void setHexmap(HexMap hexMap)
   {
      this._hexMap = hexMap;
   }

   public void setPosition()
   {
      float width = _hexMap.hexRadius * 2;
      var height = _hexMap.HEIGHT_MULTIPLIER * width;

      var yTiling = height;
      float xTiling = width * 0.75f;

      xPos = (xTiling * (this.col) + _hexMap.bottomLeftCorner.x);
      if (col % 2 == 0)
      {
         yPos = yTiling*(this.row)+_hexMap.bottomLeftCorner.y;
      }
      else
      {
         yPos = yTiling*(this.row) + yTiling*0.5f + _hexMap.bottomLeftCorner.y;
      }
      
      this.gameObject.transform.Translate(new Vector3(
         xPos+xPos*_hexMap.padding,
         yPos+yPos*_hexMap.padding,
         0
         ));

      /*
      return new Vector3(
         xPos+xPos*_hexMap.padding,
         yPos+yPos*_hexMap.padding,
         0
         );
      */
   }
}
