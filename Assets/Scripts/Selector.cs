using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{
    // Selector Prefab Comes here
    public GameObject selector;
    
    // Selected Selector Obj Comes here when clicked.
    private GameObject selectedCircle;

    // This Obj holds the selected HexInformation
    private triggerCollector triggeredObjs;
    
    public Camera cam;
    private HexMap _hexMap;
    private InputManager _inputManager;
    private void Start()
    {
        _inputManager = this.gameObject.GetComponent<InputManager>();
    }

    void Update()
    {
        
//        Debug.Log(_inputManager.inputState);
        if (_inputManager.inputState == InputManager.inputEnum.Clicked)
        {
            initializeSelectedCircle();
            Invoke("getSelectedHex", 0.03f);
            _inputManager.inputState = InputManager.inputEnum.None;
        }
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
        
        if ( _hexMap == null && triggeredObjs._selectedHexes.Count > 0 )
        {
            _hexMap = triggeredObjs._selectedHexes[0].transform.parent.transform.parent.GetComponent<HexMap>();
        }
        if (triggeredObjs._selectedHexes.Count == 3)
        {
            tripleHex();
        }
    }

    private void tripleHex()
    {
        foreach (var selectedHex in triggeredObjs._selectedHexes)
        {
           
            MeshRenderer mr = selectedHex.GetComponentInChildren<MeshRenderer>();
            mr.material =_hexMap.hexMetarials[0];
        }
    }
    
    
}