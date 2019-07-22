using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{
    // This Class not just a selector but also mover.

    // Selector Prefab Comes here
    public GameObject selector;

    // Selected Selector Obj Comes here when clicked.
    private GameObject selectedCircle;

    // This Obj holds the selected HexInformation
    private triggerCollector triggeredObjs;

    // Selected object shape
    private bool tripleSelected;
    private bool starSelected;


    public Camera cam;
    private HexMap _hexMap;
    private InputManager _inputManager;
    public bool rotating = false;
    private ThreeHexRotator _threeHexRotator;

    private void Start()
    {
        _inputManager = this.gameObject.GetComponent<InputManager>();
        _threeHexRotator = this.gameObject.GetComponent<ThreeHexRotator>();
        _threeHexRotator.setSelector(this);
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
                if (tripleSelected)
                {
                    _threeHexRotator.swipeSelectedRight(triggeredObjs._selectedHexes);
                }
            }

            if (_inputManager.inputState == InputManager.inputEnum.SwipedLeft)
            {
                if (tripleSelected)
                {
                    _threeHexRotator.swipeSelectedLeft(triggeredObjs._selectedHexes);
                }
            }

        }
        _inputManager.inputState = InputManager.inputEnum.None;

    }


    private void initializeSelectedCircle()
    {
        tripleSelected = false;
        starSelected = false;
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
            // results goes to Rotator obj.
            setOriginPoint(3);
            checkSideAndSort();

        }
    }
    
    // ------------------------------------

    private void setOriginPoint(int hexCount)
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

        if(hexCount == 3){
        _threeHexRotator.originPoint = new Vector3(posx / 3f, posy / 3f, 0);
        }
    }

    // This is only set the "rightSided" variable. and Sort the selecteds
    private void checkSideAndSort()
    {
        // Sorting by looking their col*10 + row values.
        triggeredObjs._selectedHexes.Sort((x, y) =>
            (x.GetComponent<Hex>().col * 10 + x.GetComponent<Hex>().row).CompareTo(
                y.GetComponent<Hex>().col * 10 + y.GetComponent<Hex>().row));


        if (triggeredObjs._selectedHexes[0].GetComponent<Hex>().col ==
            triggeredObjs._selectedHexes[1].GetComponent<Hex>().col)
        {
            //Debug.Log("Right Çıkıntılı");
            _threeHexRotator.rightSided = true;
        }
        else if (triggeredObjs._selectedHexes[1].GetComponent<Hex>().col ==
                 triggeredObjs._selectedHexes[2].GetComponent<Hex>().col)

        {
//            Debug.Log("Lef Çıkıntı");
            _threeHexRotator.rightSided = false;
        }
        else
        {
            Debug.Log("Unexpected select occured.");
        }
    }


}