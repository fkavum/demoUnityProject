using UnityEngine;

public class InputManager : MonoBehaviour
{
    Vector2 firstPressPos;
    Vector2 secondPressPos;

    Vector2 currentSwipe;
    // State Machines

    public enum inputEnum
    {
        None,
        Clicked,
        SwipedLeft,
        SwipedRight,
        SwipedUp,
        SwipedDown
    }

    //public StateMachine<inputEnum> inputState;
    public inputEnum inputState = inputEnum.None;
    private readonly float swipeTreshold = 0.5f;
    private void Awake()
    {
        //inputState = new StateMachine<inputEnum>(this.gameObject);
    }

    private void Update()
    {
        Swipe();
    }

    public void Swipe()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //save began touch 2d point
            firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }

        if (Input.GetMouseButtonUp(0))
        {
            //save ended touch 2d point
            secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            firstPressPos = Camera.main.ScreenToWorldPoint(firstPressPos);
            secondPressPos = Camera.main.ScreenToWorldPoint(secondPressPos);
            
            //create vector from the two points
            currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
//            Debug.Log(currentSwipe);
//            Debug.Log(swipeTreshold);
            if (currentSwipe.x > -swipeTreshold && currentSwipe.x < swipeTreshold && currentSwipe.y > -swipeTreshold && currentSwipe.y < swipeTreshold )
            {
//                Debug.Log("Clicked");
                inputState = inputEnum.Clicked;
            }
            else
            {
                //normalize the 2d vector
                currentSwipe.Normalize();

                //swipe upwards
                if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                {
//                    Debug.Log("up");
                    inputState = inputEnum.SwipedUp;
                }

                //swipe down
                if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                {
//                    Debug.Log("down");

                
                    inputState = inputEnum.SwipedDown;
                }

                //swipe left
                if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
//                    Debug.Log("left");

                    inputState = inputEnum.SwipedLeft;
                }

                //swipe right
                if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
//                    Debug.Log("right");

                    inputState = inputEnum.SwipedRight;
                }
            }
        }
    }


    public void SwipeTouch()
    {
        if (Input.touches.Length > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                //save began touch 2d point
                firstPressPos = new Vector2(t.position.x, t.position.y);
            }

            if (t.phase == TouchPhase.Ended)
            {
                //save ended touch 2d point
                secondPressPos = new Vector2(t.position.x, t.position.y);

                //create vector from the two points
                currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

                //normalize the 2d vector
                currentSwipe.Normalize();

                //swipe upwards
                if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                {
                    Debug.Log("up swipe");
                }

                //swipe down
                if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                {
                    Debug.Log("down swipe");
                }

                //swipe left
                if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    Debug.Log("left swipe");
                }

                //swipe right
                if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    Debug.Log("right swipe");
                }
            }
        }
    }
}