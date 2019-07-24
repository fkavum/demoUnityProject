using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeHexRotator : MonoBehaviour
{
    public int rotationSpeed = 12;
    public bool rightSided;
    public Vector3 originPoint;

    private Selector _selector;

    public void swipeSelectedRight(List<GameObject> triggeredObjs)
    {
        rotateAnimationally(triggeredObjs, true);
    }

    public void swipeSelectedLeft(List<GameObject> triggeredObjs)
    {
        rotateAnimationally(triggeredObjs, false);

    }


    private void rotateAnimationally(List<GameObject> triggeredObjs, bool rotateClockwise)
    {
        int r0 = triggeredObjs[0].GetComponent<Hex>().row;
        int r1 = triggeredObjs[1].GetComponent<Hex>().row;
        int r2 = triggeredObjs[2].GetComponent<Hex>().row;
        int c0 = triggeredObjs[0].GetComponent<Hex>().col;
        int c1 = triggeredObjs[1].GetComponent<Hex>().col;
        int c2 = triggeredObjs[2].GetComponent<Hex>().col;

        Debug.Log("Swipe Started.");
        //Debug.Log("Selected: " + c0+r0 + " , " + c1+r1 + " , " + c2+r2 );
        if (rotateClockwise)
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
                StartCoroutine(Rotate(triggeredObjs[0], this.rotationSpeed, 120, Vector3.back, c1, r1));

                // 1 -> 2
                StartCoroutine(Rotate(triggeredObjs[1], this.rotationSpeed, 120, Vector3.back, c2, r2));

                // 2 -> 0
                StartCoroutine(Rotate(triggeredObjs[2], this.rotationSpeed, 120, Vector3.back, c0, r0));
                return;
            }

            // 0 -> 2
            StartCoroutine(Rotate(triggeredObjs[0], this.rotationSpeed, 120, Vector3.back, c2, r2));

            // 2 -> 1
            StartCoroutine(Rotate(triggeredObjs[2], this.rotationSpeed, 120, Vector3.back, c1, r1));

            // 1 -> 0
            StartCoroutine(Rotate(triggeredObjs[1], this.rotationSpeed, 120, Vector3.back, c0, r0));

            /* Left Sided
              
             23 33
                32             
              
              Swipe Right için 23-> 33 , 33 -> 32 , 32->23
                                0   2     2     1   1   0
              */
        }
        else
        {
            if (rightSided)
            {
                // 0 -> 1
                StartCoroutine(Rotate(triggeredObjs[1], this.rotationSpeed, 120, Vector3.forward, c0, r0));

                // 1 -> 2
                StartCoroutine(Rotate(triggeredObjs[2], this.rotationSpeed, 120, Vector3.forward, c1, r1));

                // 2 -> 0
                StartCoroutine(Rotate(triggeredObjs[0], this.rotationSpeed, 120, Vector3.forward, c2, r2));
                return;
            }

            // 0 -> 2
            StartCoroutine(Rotate(triggeredObjs[2], this.rotationSpeed, 120, Vector3.forward, c0, r0));

            // 2 -> 1
            StartCoroutine(Rotate(triggeredObjs[1], this.rotationSpeed, 120, Vector3.forward, c2, r2));

            // 1 -> 0
            StartCoroutine(Rotate(triggeredObjs[0], this.rotationSpeed, 120, Vector3.forward, c1, r1));
        }
    }


    private IEnumerator Rotate(GameObject hexToRotate, float speed, float angle, Vector3 rotateAxis, int col,
        int row)
    {
        _selector.rotating = true;
        for (float t = 0; t < angle; t += speed)
        {
            _selector.rotating = true;
            hexToRotate.transform.RotateAround(originPoint, rotateAxis, speed);
            yield return null;
        }

        hexToRotate.GetComponent<Hex>().moveHex(col, row);
        _selector.rotating = false;
    }

    public void setSelector(Selector selector)
    {
        _selector = selector;
    }
}