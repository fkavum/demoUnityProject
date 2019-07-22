using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{
    // This Class not just a selector but also mover.

    // Selector Prefab Comes here
    public GameObject selector;

    public int rotationSpeed = 120;

    // Selected Selector Obj Comes here when clicked.
    private GameObject selectedCircle;

    // This Obj holds the selected HexInformation
    private triggerCollector triggeredObjs;

    // Selected object shape
    private bool rightSided;
    private bool tripleSelected;
    private bool starSelected;


    public Camera cam;
    private HexMap _hexMap;
    private InputManager _inputManager;
    private Vector3 originPoint;
    private bool rotating = false;
    private ThreeHexRotator _threeHexRotator;

    private void Start()
    {
        _inputManager = this.gameObject.GetComponent<InputManager>();
        _threeHexRotator = this.gameObject.GetComponent<ThreeHexRotator>();
    }

    void Update()
    {
       
        //if(rotate){
        //triggeredObjs._selectedHexes[0].transform.RotateAround(originPoint,Vector3.forward,40*Time.deltaTime);}
//        Debug.Log(_inputManager.inputState);
        if (!rotating)
        {
//            Debug.Log(_inputManager.inputState);
            if (_inputManager.inputState == InputManager.inputEnum.Clicked)
            {
                initializeSelectedCircle();
                Invoke("getSelectedHex", 0.03f);
            }

            if (_inputManager.inputState == InputManager.inputEnum.SwipedRight)
            {
                if (triggeredObjs != null)
                {
                    swipeSelectedRight();
                }
            }

            if (_inputManager.inputState == InputManager.inputEnum.SwipedLeft)
            {
                if (triggeredObjs != null)
                {
                    swipeSelectedLeft();
                }
            }

        }
        _inputManager.inputState = InputManager.inputEnum.None;

    }


    private void initializeSelectedCircle()
    {
        Vector3 point = cam.ScreenToWorldPoint(Input.mousePosition);
        Destroy(selectedCircle);
        selectedCircle = null;
        selectedCircle = Instantiate(selector, new Vector3(point.x, point.y, 0), Quaternion.identity, this.transform);
    }

    // When the selector circle initialize this method gets the trşggered hexes from the triggerCollector.
    private void getSelectedHex()
    {
        triggeredObjs = selectedCircle.transform.GetComponentInChildren<triggerCollector>();

        if (_hexMap == null && triggeredObjs._selectedHexes.Count > 0)
        {
            _hexMap = triggeredObjs._selectedHexes[0].transform.parent.transform.parent.GetComponent<HexMap>();
        }

        if (triggeredObjs._selectedHexes.Count == 3)
        {
            tripleSelected = true;
            starSelected = false;
            tripleHex();
        }
    }

    private void tripleHex()
    {
        float posx = 0f;
        float posy = 0f;

        foreach (var selectedHex in triggeredObjs._selectedHexes)
        {
            posx += selectedHex.transform.position.x;
            posy += selectedHex.transform.position.y;

            /*
            Hex selectedHexScript = selectedHex.GetComponent<Hex>();
            MeshRenderer mr = selectedHex.GetComponentInChildren<MeshRenderer>();
            mr.material = _hexMap.hexMetarials[0];*/
        }

        originPoint = new Vector3(posx / 3f, posy / 3f, 0);

        checkSelectedsShape();
    }

    // This is only set the "rightSided" variable. and Sort the selecteds
    private void checkSelectedsShape()
    {
        // Sorting by looking their col*10 + row values.
        triggeredObjs._selectedHexes.Sort((x, y) =>
            (x.GetComponent<Hex>().col * 10 + x.GetComponent<Hex>().row).CompareTo(
                y.GetComponent<Hex>().col * 10 + y.GetComponent<Hex>().row));


        if (triggeredObjs._selectedHexes[0].GetComponent<Hex>().col ==
            triggeredObjs._selectedHexes[1].GetComponent<Hex>().col)
        {
            //Debug.Log("Right Çıkıntılı");
            rightSided = true;
        }
        else if (triggeredObjs._selectedHexes[1].GetComponent<Hex>().col ==
                 triggeredObjs._selectedHexes[2].GetComponent<Hex>().col)

        {
//            Debug.Log("Lef Çıkıntı");
            rightSided = false;
        }
        else
        {
            Debug.Log("Unexpected select occured.");
        }
    }


    private IEnumerator Rotate(GameObject hexToRotate, float speed, float angle, Vector3 rotateAxis, int col, int row)
    {
        rotating = true;
        for (float t = 0; t < angle; t += speed)
        {
            rotating = true;
            hexToRotate.transform.RotateAround(originPoint, rotateAxis, speed);
            yield return null;
        }

        hexToRotate.GetComponent<Hex>().moveHex(col, row);
        rotating = false;
    }

    private void swipeSelectedRight()
    {
        int r0 = triggeredObjs._selectedHexes[0].GetComponent<Hex>().row;
        int r1 = triggeredObjs._selectedHexes[1].GetComponent<Hex>().row;
        int r2 = triggeredObjs._selectedHexes[2].GetComponent<Hex>().row;
        int c0 = triggeredObjs._selectedHexes[0].GetComponent<Hex>().col;
        int c1 = triggeredObjs._selectedHexes[1].GetComponent<Hex>().col;
        int c2 = triggeredObjs._selectedHexes[2].GetComponent<Hex>().col;

        //Debug.Log("Selected: " + c0+r0 + " , " + c1+r1 + " , " + c2+r2 );

        /* Right Sided
            42    51
            41
            
            swipe Right için 41 -> 42 ,  42 -> 51, 51 -> 41
                               0   1     1     2   2     0
            */

        if (rightSided)
        {
            // 0 -> 1
            StartCoroutine(Rotate(triggeredObjs._selectedHexes[0], this.rotationSpeed, 120, Vector3.back, c1, r1));

            // 1 -> 2
            StartCoroutine(Rotate(triggeredObjs._selectedHexes[1], this.rotationSpeed, 120, Vector3.back, c2, r2));

            // 2 -> 0
            StartCoroutine(Rotate(triggeredObjs._selectedHexes[2], this.rotationSpeed, 120, Vector3.back, c0, r0));
            return;
        }

        // 0 -> 2
        StartCoroutine(Rotate(triggeredObjs._selectedHexes[0], this.rotationSpeed, 120, Vector3.back, c2, r2));

        // 2 -> 1
        StartCoroutine(Rotate(triggeredObjs._selectedHexes[2], this.rotationSpeed, 120, Vector3.back, c1, r1));

        // 1 -> 0
        StartCoroutine(Rotate(triggeredObjs._selectedHexes[1], this.rotationSpeed, 120, Vector3.back, c0, r0));

        /* Left Sided
          
         23 33
            32             
          
          Swipe Right için 23-> 33 , 33 -> 32 , 32->23
                            0   2     2     1   1   0
          */
    }

    private void swipeSelectedLeft()
    {
        int r0 = triggeredObjs._selectedHexes[0].GetComponent<Hex>().row;
        int r1 = triggeredObjs._selectedHexes[1].GetComponent<Hex>().row;
        int r2 = triggeredObjs._selectedHexes[2].GetComponent<Hex>().row;
        int c0 = triggeredObjs._selectedHexes[0].GetComponent<Hex>().col;
        int c1 = triggeredObjs._selectedHexes[1].GetComponent<Hex>().col;
        int c2 = triggeredObjs._selectedHexes[2].GetComponent<Hex>().col;

        Debug.Log("Selected: " + c0 + r0 + " , " + c1 + r1 + " , " + c2 + r2);

        /* Right Sided
            42    51
            41
            
            swipe Right için 41 -> 42 ,  42 -> 51, 51 -> 41
                               0   1     1     2   2     0
            */

        if (rightSided)
        {
            // 0 -> 1
            StartCoroutine(Rotate(triggeredObjs._selectedHexes[1], this.rotationSpeed, 120, Vector3.forward, c0, r0));

            // 1 -> 2
            StartCoroutine(Rotate(triggeredObjs._selectedHexes[2], this.rotationSpeed, 120, Vector3.forward, c1, r1));

            // 2 -> 0
            StartCoroutine(Rotate(triggeredObjs._selectedHexes[0], this.rotationSpeed, 120, Vector3.forward, c2, r2));
            return;
        }

        // 0 -> 2
        StartCoroutine(Rotate(triggeredObjs._selectedHexes[2], this.rotationSpeed, 120, Vector3.forward, c0, r0));

        // 2 -> 1
        StartCoroutine(Rotate(triggeredObjs._selectedHexes[1], this.rotationSpeed, 120, Vector3.forward, c2, r2));

        // 1 -> 0
        StartCoroutine(Rotate(triggeredObjs._selectedHexes[0], this.rotationSpeed, 120, Vector3.forward, c1, r1));

        /* Left Sided
          
         23 33
            32             
          
          Swipe Right için 23-> 33 , 33 -> 32 , 32->23
                            0   2     2     1   1   0
          */
    }
}