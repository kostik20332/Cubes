using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RatatingByArrows : MonoBehaviour
{
    private int verticalArrowTaps = 0;
    private int horizontalArrowTaps = 1;

    private float angle;
    private float x;
    private float rotationZ;
    private float rotationY;
    private float rotationX;
    private float rotatingSpeed = 0.001f;
    private float downAngle = 90f;
    private float leftAngle = 90f;
    private float upAngle = -90f;

    public GameObject cubes;
    public GameObject[] buttons = new GameObject[4];
    Coroutine coroutine;
    Quaternion rotation;

    private bool directionPlus = false;
    private bool cubesRotating = false;

    void Start()
    {
        SwipeChecking.SwipeEvent += OnSwipe;
    }

    private void OnSwipe(Vector2 direction)
    {
        //Vector3 dir = direction == Vector2.up ? Vector3.forward : direction == Vector2.down ? Vector3.back : (Vector3)direction;
        if (direction == Vector2.up && !cubesRotating)
        {
            DownArrow();
        }
        else if (direction == Vector2.down && !cubesRotating)
        {
            UpArrow();
        }
        else if (direction == Vector2.left && !cubesRotating)
        {
            RightArrow();
        }
        else if (direction == Vector2.right && !cubesRotating)
        {
            LeftArrow();
        }
    }

    void ButtonsDisactivation()
    {
        foreach (GameObject button in buttons)
        {
            button.GetComponent<Button>().enabled = false;
        }
    }

    void ButtonsActivation()
    {
        foreach (GameObject button in buttons)
        {
            button.GetComponent<Button>().enabled = true;
        }
    }

    public void DownArrow()
    {
        if(verticalArrowTaps == 1)
        {
            verticalArrowTaps--;
            ButtonsDisactivation();
            cubesRotating = true;
            StartCoroutine(Rotating(0f, 0f, -1f));
        }
    }

    public void UpArrow()
    {
        if(verticalArrowTaps == 0)
        {
            verticalArrowTaps++;
            ButtonsDisactivation();
            cubesRotating = true;
            StartCoroutine(Rotating(0f, 0f, 1f));
        }
    }

    public void LeftArrow()
    {
        if(horizontalArrowTaps == 1)
        {
            horizontalArrowTaps--;
            ButtonsDisactivation();
            cubesRotating = true;
            StartCoroutine(Rotating(0f, -1f, 0f));
        }
    }

    public void RightArrow()
    {
        if(horizontalArrowTaps == 0)
        {
            horizontalArrowTaps++;
            ButtonsDisactivation();
            cubesRotating = true;
            StartCoroutine(Rotating(0f, 1f, 0f));
        }
    }

    void Update()
    {
        rotationZ = cubes.transform.eulerAngles.z;
        rotationY = cubes.transform.eulerAngles.y;
        rotationX = cubes.transform.eulerAngles.x;
    }

    IEnumerator Rotating(float x, float y, float z)
    {
        float difference = 0;
        for (int i = 0; i < 90; i++)
        {
            cubes.transform.Rotate(x, y, z, Space.World);
            if(x > 0)
            {
                difference += x;
            }
            else if (x > 0)
            {
                difference += y;
            }
            else if (x > 0)
            {
                difference += z;
            }
            yield return new WaitForSeconds(0.001f);
        }
        ButtonsActivation();
        cubesRotating = false;
    }
}
