using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubesLogic : MonoBehaviour
{
    public string color;
    public string shape;
    // Start is called before the first frame update
    void Start()
    {
        if(this.gameObject.name == "square" 
            || this.gameObject.name == "square(Clone)"
            || this.gameObject.name == "square2"
            || this.gameObject.name == "square2(Clone)")
        {
            shape = "square";
        }
        else if (this.gameObject.name == "triangle" 
            || this.gameObject.name == "triangle(Clone)"
            || this.gameObject.name == "triangle2"
            || this.gameObject.name == "triangle2(Clone)")
        {
            shape = "triangle";
        }
        else if (this.gameObject.name == "circle" 
            || this.gameObject.name == "circle(Clone)"
            || this.gameObject.name == "circle2"
            || this.gameObject.name == "circle2(Clone)")
        {
            shape = "circle";
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if(PlayerThings.)
    }
}
