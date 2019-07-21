using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerCollector : MonoBehaviour
{

    public List<GameObject> _selectedHexes = new List<GameObject>();
    private void OnTriggerEnter(Collider other)
    {
//        Debug.Log("Something triggered me: "+other.transform.parent.gameObject);
        _selectedHexes.Add(other.transform.parent.gameObject);

    }
}
