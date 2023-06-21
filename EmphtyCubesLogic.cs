using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmphtyCubesLogic : MonoBehaviour
{
    public GameObject smallCube;
    public Material[] semiTransparent = new Material[1];

    void OnTriggerStay(Collider other)
    { 
        if(other.gameObject.tag == "cube" 
            || other.gameObject.tag == "placedOnFirstLineCube" 
            || other.gameObject.tag == "placedOnSecondLineCube" 
            || other.gameObject.tag == "placedOnThirdLineCube")
        {
            //Destroy(this.gameObject);
            //Destroy(smallCube);
            Destroy(smallCube);
            Destroy(this.gameObject);
        }
        else
        {
            if(smallCube != null)
            {
                smallCube.GetComponent<MeshRenderer>().materials = semiTransparent;
            }
            Destroy(this.gameObject);
        }
    }
}
