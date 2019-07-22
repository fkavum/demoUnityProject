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
    private bool rightSided;
    private bool tripleSelected;
    private bool starSelected;
    public Camera cam;
    private HexMap _hexMap;
    private InputManager _inputManager;
    private Vector3 originPoint;
    private bool rotate = false;
    private void Start()
    {
        _inputManager = this.gameObject.GetComponent<InputManager>();
    }

    void Update()
    {
        //if(rotate){
        //triggeredObjs._selectedHexes[0].transform.RotateAround(originPoint,Vector3.forward,40*Time.deltaTime);}
//        Debug.Log(_inputManager.inputState);
        if (_inputManager.inputState == InputManager.inputEnum.Clicked)
        {
            initializeSelectedCircle();
            Invoke("getSelectedHex", 0.03f);
        }

        if (_inputManager.inputState == InputManager.inputEnum.SwipedRight)
        {
            swipeSelectedRight();
        }

        if (_inputManager.inputState == InputManager.inputEnum.SwipedLeft)
        {
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
            Hex selectedHexScript = selectedHex.GetComponent<Hex>();
            MeshRenderer mr = selectedHex.GetComponentInChildren<MeshRenderer>();
            mr.material = _hexMap.hexMetarials[0];
        }
        
        originPoint = new Vector3(posx/3f,posy/3f,0);
        
        
        
        rotate = true;
        
     
      
        
        
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
        StartCoroutine( Rotate( new Vector3(0, 0, 90), 2f,120 ) ) ;
    }
    
    public GameObject objectToRotate;
    private bool rotating ;
    
    private IEnumerator Rotate( Vector3 angles, float speed,float angle)
    {
        for( float t = 0 ; t < angle ; t+= speed )
        {
            triggeredObjs._selectedHexes[0].transform.RotateAround(originPoint,Vector3.forward, speed );
            yield return null;
        }
    }
 


    private void swipeSelectedRight()
    {
        /* Right Sided
            42    51
            41
            
            swipe Right için 41 -> 42 ,  42 -> 51, 51 -> 41
                               0   1     1     2   2     0
            */

        if (rightSided)
        {
            // 0 -> 1
         
            
            // 1 -> 2

            
            // 2 -> 0
            return;
        }
        
        

        /* Left Sided
          
         23 33
            32             
          
          Swipe Right için 23-> 33 , 33 -> 32 , 32->23
                            0   2     2     1   1   0
          */
    }
}
/*
 
    //triggeredObjs._selectedHexes[0].transform.RotateAround(originPoint,Vector3.forward, 90 );
private IEnumerator Rotate( Vector3 angles, float speed,float angle)
{
objectToRotate = triggeredObjs._selectedHexes[0];
rotating = true ;
Quaternion startRotation = objectToRotate.transform.rotation ;
Quaternion endRotation = Quaternion.Euler( angles ) * startRotation ;
for( float t = 0 ; t < angle ; t+= speed )
{
    triggeredObjs._selectedHexes[0].transform.RotateAround(originPoint,Vector3.forward, speed );
    //objectToRotate.transform.rotation = Quaternion.Lerp( startRotation, endRotation, t / duration ) ;
    yield return null;
}
//objectToRotate.transform.rotation = endRotation  ;
// rotating = false;
}
*/

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