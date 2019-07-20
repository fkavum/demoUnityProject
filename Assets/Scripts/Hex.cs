using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex
{

   public int col;
   public int row;
   public float xPos;
   public float yPos;
   
   public GameObject instantiatedObject;
   private GameObject[] neigbours;
   private HexMap _hexMap;


   public Hex(int col,int row , HexMap hexMap )
   {
      this._hexMap = hexMap;
      this.col = col;
      this.row = row;
      //this.sumFactor = -(col + row);
   }

   public Vector3 position()
   {
      const float radius = 1f;
      const float width = radius * 2;
      var height = _hexMap.HEIGHT_MULTIPLIER * width;

      var yTiling = height;
      const float xTiling = width * 0.75f;

      xPos = (xTiling * (this.col) + _hexMap.bottomLeftCorner.x);
      if (col % 2 == 0)
      {
         yPos = yTiling*(this.row)+_hexMap.bottomLeftCorner.y;
      }
      else
      {
         yPos = yTiling*(this.row) + yTiling*0.5f + _hexMap.bottomLeftCorner.y;
      }

      return new Vector3(
         xPos+xPos*_hexMap.padding,
         yPos+yPos*_hexMap.padding,
         0
         );
      
   }
}
