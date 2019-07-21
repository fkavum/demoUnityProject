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

            Hex selectedHexScript = selectedHex.GetComponent<Hex>();
            
            MeshRenderer mr = selectedHex.GetComponentInChildren<MeshRenderer>();
            mr.material =_hexMap.hexMetarials[0];
        }
        //List<GameObject> SortedList = triggeredObjs._selectedHexes.OrderBy(o=>o.getComponent<Hex>().).ToList();
        triggeredObjs._selectedHexes.Sort((x, y) => (x.GetComponent<Hex>().col*10 + x.GetComponent<Hex>().row).CompareTo(y.GetComponent<Hex>().col*10 + y.GetComponent<Hex>().row));
        if (triggeredObjs._selectedHexes[0].GetComponent<Hex>().col == triggeredObjs._selectedHexes[1].GetComponent<Hex>().col)
        {
            Debug.Log("Right Çıkıntılı");
            
            /*
             42    51
             41
             
             swipe Right için 41 -> 42 ,  42 -> 51, 51 -> 41
                                0   1     1     2   2     0
             */
            
            
        }else if(triggeredObjs._selectedHexes[1].GetComponent<Hex>().col == triggeredObjs._selectedHexes[2].GetComponent<Hex>().col )

        {
            Debug.Log("Lef Çıkıntı");
            
            /*
             
            23 33
               32             
             
             Swipe Right için 23-> 33 , 33 -> 32 , 32->23
                               0   2     2     1   1   0
             */
            
        }
        else
        {
            Debug.Log("Unexpected select occured.");
        }
    }
    
    
}


/*
class HexComperor : IComparer<GameObject>
{ 
    public int Compare(GameObject x, GameObject y)
    {
        int a = x.GetComponent<Hex>().col * 10 + x.row;
        int b = y.col * 10 + y.row;
        if (a == 0 || b == 0) 
        { 
            return 0; 
        }  
        // CompareTo() method 
        return a.CompareTo(b);

    } 
} 

*/