 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    public Vector3 fingerPositionAxis1;
    public Vector3 fingerPositionAxis2;
    public Vector3 firstFingerPosition;
    public Vector3 direction;
    public Vector3 directionFinal;
    public float rotatingingSpeed = 0.05f;
    public float distance;
    public float firstMinDistance;
    public float mainMinDistance;
    private float scalingSpeed = 0f;
    private float scalingSpeedCoef = 15f;
    private float speed;
    private float distanceToObject = 6f;
    public GameObject cubes;
    //public GameObject mainCamera;

    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    firstFingerPosition = new Vector3(-Input.mousePosition.y + Input.mousePosition.x, Input.mousePosition.x, Input.mousePosition.y + Input.mousePosition.x);
        //}

        //if (Input.GetMouseButton(0))
        //{
        //    fingerPositionAxis1 = new Vector3(-Input.mousePosition.y + Input.mousePosition.x, Input.mousePosition.x, Input.mousePosition.y + Input.mousePosition.x);
        //    directionFinal = firstFingerPosition - fingerPositionAxis1;
        //    cubes.transform.Rotate(directionFinal * rotatingingSpeed, Space.World);
        //    firstFingerPosition = fingerPositionAxis1;
        //}
    }

    void LateUpdate()
    {
        firstMinDistance = 10f;
        for (int i = 0; i < PlayerThings.v; i++)
        {
            //distance = MathF.Sqrt(Mathf.Pow(PlayerThings.allColorCubes[i].transform.position.x - this.transform.position.x,2) + Mathf.Pow(PlayerThings.allColorCubes[i].transform.position.y - this.transform.position.y, 2));
            distance = MathF.Sqrt(Mathf.Pow(PlayerThings.allColorCubes[i].transform.position.x - this.transform.position.x,2));
            if (distance < firstMinDistance)
            {
                firstMinDistance = distance;
            }
        }
        mainMinDistance = firstMinDistance;

        if (mainMinDistance < distanceToObject)
        {
            scalingSpeed = mainMinDistance - distanceToObject;
            //this.transform.Translate(new Vector3(scalingSpeed * scalingSpeedCoef, -scalingSpeed * scalingSpeedCoef * 0.5f, 0f) * Time.deltaTime, Space.World);
            this.transform.Translate(new Vector3(scalingSpeed * scalingSpeedCoef, 0f, 0f) * Time.deltaTime, Space.World);
        }
        else
        {
            scalingSpeed = mainMinDistance - distanceToObject;
            //this.transform.Translate(new Vector3(scalingSpeed * scalingSpeedCoef, -scalingSpeed * scalingSpeedCoef * 0.5f, 0f) * Time.deltaTime, Space.World);
            this.transform.Translate(new Vector3(scalingSpeed * scalingSpeedCoef, 0f, 0f) * Time.deltaTime, Space.World);
        }
    }
}
