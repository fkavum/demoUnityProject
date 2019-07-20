using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex
{

   public int col;
   public int row;
   public float xPos;
   public float yPos;
   public float padding = 0.1f;
   public GameObject instantiatedObject;
   private GameObject[] neigbours;
   
   public Vector2 bottomLeftCorner = new Vector2(-4f,-4f);

   private readonly float HEIGHT_MULTIPLIER = Mathf.Sqrt(3) / 2;

   public Hex(int col,int row )
   {
      this.col = col;
      this.row = row;
      //this.sumFactor = -(col + row);
   }

   public Vector3 position()
   {
      float radius = 1f;
      float width = radius * 2;
      float height = HEIGHT_MULTIPLIER * width;

      float yTiling = height;
      float xTiling = width * 0.75f;

      xPos = (xTiling * (this.col) + bottomLeftCorner.x);
      if (col % 2 == 0)
      {
         yPos = yTiling*(this.row)+bottomLeftCorner.y;
      }
      else
      {
         yPos = yTiling*(this.row) + yTiling*0.5f + bottomLeftCorner.y;
      }

      return new Vector3(
         xPos+xPos*padding,
         yPos+yPos*padding,
         0
         
         );
      
      
   }
}
