using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeChecking : MonoBehaviour
{
    public static event OnSwipeInput SwipeEvent;
    public delegate void OnSwipeInput(Vector2 direction);

    private Vector2 tapPosition;
    private Vector2 tapPosition2;
    private Vector2 previousTapPosition;
    private Vector2 newTapPosition;
    private Vector2 delta;
    private Vector2 swipeDelta;

    private float deadZone = 80f;
    private float tapDeadZone = 30f;

    private bool isSwiping;
    private bool isMobile;
    //private bool isSwipingForJump = false;

    private float tapTime;
    private float timeTouchBegan;
    private bool touchDidMove = false;
    private float tapTimeThreshold = 0.3f;

    //public GameObject ball;

    void Awake()
    {
        SwipeEvent = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        isMobile = Application.isMobilePlatform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMobile)
        {

            if (Input.GetMouseButtonDown(0))
            {
                isSwiping = true;
                tapPosition = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                ResetSwipe();
            }
        }
        else
        {
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    isSwiping = true;
                    tapPosition = Input.GetTouch(0).position;
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Canceled || Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    ResetSwipe();
                }
            }
        }
        CheckSwipe();
    }

    private void CheckSwipe()
    {
        swipeDelta = Vector2.zero;

        if (isSwiping)
        {
            if (!isMobile && Input.GetMouseButton(0))
            {
                swipeDelta = (Vector2)Input.mousePosition - tapPosition;
            }
            else if (Input.touchCount > 0)
            {
                swipeDelta = Input.GetTouch(0).position - tapPosition;
            }
        }

        if (swipeDelta.magnitude > deadZone)
        {
            if (SwipeEvent != null)
            {
                if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
                {
                    SwipeEvent(swipeDelta.x > 0 ? Vector2.right : Vector2.left);
                }
                else
                {
                    SwipeEvent(swipeDelta.y > 0 ? Vector2.up : Vector2.down);
                }
            }
            ResetSwipe();
        }
    }

    private void ResetSwipe()
    {
        isSwiping = false;

        tapPosition = Vector2.zero;
        swipeDelta = Vector2.zero;
    }
}
