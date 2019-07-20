using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{
    public GameObject selector;
    public Camera cam;
    private GameObject selectedCircle;
    private HexMap _hexMap;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            initializeSelectedCircle();
            Invoke("getSelectedHex", 0.03f);
        }
    }


    private void initializeSelectedCircle()
    {
        Vector3 point = cam.ScreenToWorldPoint(Input.mousePosition);
        Destroy(selectedCircle);
        selectedCircle = null;
        selectedCircle = Instantiate(selector, new Vector3(point.x, point.y, 0), Quaternion.identity, this.transform);
    }

    private void getSelectedHex()
    {
        triggerCollector triggeredObjs = selectedCircle.transform.GetComponentInChildren<triggerCollector>();
        if (triggeredObjs._selectedHexes.Count == 3)
        {
            foreach (var selectedHex in triggeredObjs._selectedHexes)
            {
                if (_hexMap == null)
                {
                    _hexMap = selectedHex.transform.parent.transform.parent.GetComponent<HexMap>();
                }
                
                MeshRenderer mr = selectedHex.GetComponentInChildren<MeshRenderer>();
                mr.material =_hexMap.hexMetarials[0];
            }
        }
    }
}