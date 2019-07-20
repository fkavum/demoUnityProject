using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Transform hexPrefab;
    public int gridWidth = 11;
    public int gridHeight = 11;

    float hexWidth = 1.732f;
    float hexHeight = 2.0f;
    public float gap = 0.0f;

    Vector3 startPos;


    private void Start()
    {
        AddGap();
        CalcStartPos();
        CreateGrid();
    }
    private void AddGap()
    {
        hexWidth += hexWidth * gap;
        hexHeight += hexHeight * gap;
    }

    private void CalcStartPos()
    {
        startPos = Vector3.zero;
    }

    private void CreateGrid()
    {

        for(int y=0; y < gridHeight; y++)
        {


        }



    }

   

   
}
