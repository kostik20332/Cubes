using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerThings : MonoBehaviour
{
    private Camera cam;
    public Vector3 firstRayDirection;
    public Vector3 lastRayDirection;

    public int[] randValue = new int[4];
    public int randValue1;
    public int randValue2;
    public int randValue3;
    public int randValue4;
    private int score = 0;
    public static int cubesMaxAmount = 300;
    public int v2;
    public static int v = 0;
    public static int skinVariant = 0;

    public GameObject[] allCubePrefabs = new GameObject[6];
    public GameObject[] cubePrefabs = new GameObject[3];
    private GameObject[] cubesForChoosing = new GameObject[3];
    public GameObject[] inventoryCubeOne = new GameObject[3];
    public GameObject[] inventoryCubeTwo = new GameObject[3];
    public GameObject[] inventoryCubeThree = new GameObject[3];
    private GameObject[] emphtyCubesForDeleting;
    private GameObject[] emphtyCubesForDisactivation;
    public static GameObject[] allColorCubes = new GameObject[cubesMaxAmount];
    public GameObject cube;
    public GameObject cubesBackGround;
    private GameObject placedCube;
    public GameObject[] placedOnLineCube = new GameObject[3];
    private GameObject cubeForScaling;
    private GameObject previousCube;
    private GameObject emphtyCubeInstance;
    private GameObject smallEmphtyCubeInstance;
    public GameObject cubes;
    private GameObject emphtyCube;
    public GameObject emphtyCubePrefab;
    public GameObject emphtyCubePrefab2;
    public GameObject loseScreen;
    private GameObject temp1;
    private GameObject temp2;
    public GameObject particalEffect;

    public bool cubeMoving = false;
    private bool isCubeChoosed = false;
    private bool isMoveExists = false;
    //private bool emphtyCubesVisibility = false;

    private Material[] materials = new Material[2];
    public Material[] materialPrafabs = new Material[10];
    public Material[] semiTransparent = new Material[1];
    public Material[] transparent = new Material[1];

    private System.Random rnd = new System.Random();
    private EmphtyCubesLogic emphtyCubeScript;
    private CubesLogic script1;
    private CubesLogic script2;
    public TMP_Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        SkinChangin();
        v = 0;
        cam = GetComponent<Camera>();
        materials[1] = materialPrafabs[0];
        SpawningThreeCubesForChoosing();
        SpawningFirstThreeCubes();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "" + score;

        v2 = v;
        if (Input.GetMouseButtonDown(0))//ждём нажатия на экран
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);//создаём луч в точку, куда тыкаем пальцем
            RaycastHit hit;//создаём переменную для информации о луче
            if (Physics.Raycast(ray, out hit))//метод возвращает true или false и передаёт информацию о луче в переменную hit
            {
                GameObject hitObject = hit.transform.gameObject;//помещаем в переменную hitObject объект в который попали
                if (hitObject.tag == "firstLineEmphtyCube" && isCubeChoosed)
                {
                    script1 = cube.GetComponent<CubesLogic>();
                    script2 = placedOnLineCube[0].GetComponent<CubesLogic>();
                    if (script1.shape == script2.shape || script1.color == script2.color)
                    {
                        CubePlacing(hitObject, 0, "firstLineEmphtyCube", "placedOnFirstLineCube");
                    }
                    else
                    {
                        UnScaling();
                        cube = null;
                    }
                }
                else if (hitObject.tag == "secondLineEmphtyCube" && isCubeChoosed)
                {
                    script1 = cube.GetComponent<CubesLogic>();
                    script2 = placedOnLineCube[1].GetComponent<CubesLogic>();
                    if (script1.shape == script2.shape || script1.color == script2.color)
                    {
                        CubePlacing(hitObject, 1, "secondLineEmphtyCube", "placedOnSecondLineCube");
                    }
                    else
                    {
                        UnScaling();
                        cube = null;
                    }
                }
                else if (hitObject.tag == "thirdLineEmphtyCube" && isCubeChoosed)
                {
                    script1 = cube.GetComponent<CubesLogic>();
                    script2 = placedOnLineCube[2].GetComponent<CubesLogic>();
                    if (script1.shape == script2.shape || script1.color == script2.color)
                    {
                        CubePlacing(hitObject, 2, "thirdLineEmphtyCube", "placedOnThirdLineCube");
                    }
                    else
                    {
                        UnScaling();
                        cube = null;
                    }
                }
                else if (hitObject.tag == "firstCubeForChoosing" 
                    || hitObject.tag == "secondCubeForChoosing"
                    || hitObject.tag == "thirdCubeForChoosing")
                {
                    UnScaling();
                    cube = hitObject;
                    Scaling();
                    SpawnAllowedToMoveOnEmphtyCubes();
                }
            }
            else
            {
                UnScaling();
            }
        }
    }

    void CubePlacing(GameObject hitObject,int lineNumber,string emphtyCubeTag, string placedCubeTag)
    {
        emphtyCube = hitObject;
        cube.transform.SetParent(cubes.transform, true);
        cube.transform.position = new Vector3(emphtyCube.transform.position.x, emphtyCube.transform.position.y, emphtyCube.transform.position.z);
        cube.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        cube.transform.localScale = new Vector3(50f, 50f, 50f);
        Instantiate(particalEffect, cube.transform.position, Quaternion.Euler(45f,90f,0f));
        allColorCubes[v++] = cube;
        placedOnLineCube[lineNumber] = cube;
        SpawningSemiTransparentCubes(emphtyCube, emphtyCubeTag);
        Destroy(emphtyCube);
        isCubeChoosed = false;
        SpawnNewCubeForChoosing();
        placedCube.tag = placedCubeTag;
        DeleteAllSemiTransparentCubes();
        cube = null;
        score += 10;
        LoseCheck();
    }

    void Scaling()
    {
        if(cube != null)
        {
            isCubeChoosed = true;
            cube.transform.localScale = new Vector3(13f, 13f, 13f);
        }
    }

    void UnScaling()
    {
        if (cube != null)
        {
            isCubeChoosed = false;
            cube.transform.localScale = new Vector3(10f, 10f, 10f);
            DeleteAllSemiTransparentCubes();
        }
    }

    void SkinChangin()
    {
        int border = skinVariant * 3;
        for (int i = 0; i < 3; i++)
        {
            cubePrefabs[i] = allCubePrefabs[border + i];
        }
    }

    void LoseCheck()
    {
        isMoveExists = false;
        inventoryCubeOne = GameObject.FindGameObjectsWithTag("firstCubeForChoosing");
        inventoryCubeTwo = GameObject.FindGameObjectsWithTag("secondCubeForChoosing");
        inventoryCubeThree = GameObject.FindGameObjectsWithTag("thirdCubeForChoosing");
        for (int i = 0; i < 3; i++)
        {
            switch (i)
            {
                case 0:
                    temp1 = inventoryCubeOne[0];
                    script1 = inventoryCubeOne[0].GetComponent<CubesLogic>();
                    break;
                case 1:
                    temp1 = inventoryCubeTwo[0];
                    script1 = inventoryCubeTwo[0].GetComponent<CubesLogic>();
                    break;
                case 2:
                    temp1 = inventoryCubeThree[0];
                    script1 = inventoryCubeThree[0].GetComponent<CubesLogic>();
                    break;
            }
            script2 = placedOnLineCube[0].GetComponent<CubesLogic>();
            temp2 = placedOnLineCube[0];
            //if ((script1.shape == script2.shape) || (script1.color == script2.color))
            //{
            //    isMoveExists = true;
            //}
            //script2 = placedOnLineCube[1].GetComponent<CubesLogic>();
            //if ((script1.shape == script2.shape) || (script1.color == script2.color))
            //{
            //    isMoveExists = true;
            //}
            //script2 = placedOnLineCube[2].GetComponent<CubesLogic>();
            //if ((script1.shape == script2.shape) || (script1.color == script2.color))
            //{
            //    isMoveExists = true;
            //}

            if ((temp1.name == temp2.name) || (script1.color == script2.color))
            {
                isMoveExists = true;
            }
            script2 = placedOnLineCube[1].GetComponent<CubesLogic>();
            temp2 = placedOnLineCube[1];
            if ((temp1.name == temp2.name) || (script1.color == script2.color))
            {
                isMoveExists = true;
            }
            script2 = placedOnLineCube[2].GetComponent<CubesLogic>();
            temp2 = placedOnLineCube[2];
            if ((temp1.name == temp2.name) || (script1.color == script2.color))
            {
                isMoveExists = true;
            }

            switch (i)
            {
                case 0:
                    if (!isMoveExists)
                    {
                        Debug.Log("first");
                    }
                    break;
                case 1:
                    if (!isMoveExists)
                    {
                        Debug.Log("second");
                    }
                    break;
                case 2:
                    if (!isMoveExists)
                    {
                        Debug.Log("third");
                    }
                    break;
            }
        }
        if (!isMoveExists)
        {
            loseScreen.SetActive(true);
        }
    }

    void SpawnAllowedToMoveOnEmphtyCubes()
    {
        script1 = cube.GetComponent<CubesLogic>();
        script2 = placedOnLineCube[0].GetComponent<CubesLogic>();
        if (script1.shape == script2.shape || script1.color == script2.color)
        {
            SpawningSemiTransparentCubes(placedOnLineCube[0], "firstLineEmphtyCube");
        }
        script2 = placedOnLineCube[1].GetComponent<CubesLogic>();
        if (script1.shape == script2.shape || script1.color == script2.color)
        {
            SpawningSemiTransparentCubes(placedOnLineCube[1], "secondLineEmphtyCube");
        }
        script2 = placedOnLineCube[2].GetComponent<CubesLogic>();
        if (script1.shape == script2.shape || script1.color == script2.color)
        {
            SpawningSemiTransparentCubes(placedOnLineCube[2], "thirdLineEmphtyCube");
        }
    }

    void ShowAllowedToMoveOnEmphtyCubes()
    {
        script1 = cube.GetComponent<CubesLogic>();
        script2 = placedOnLineCube[0].GetComponent<CubesLogic>();
        if (script1.shape == script2.shape || script1.color == script2.color)
        {
            emphtyCubesForDisactivation = GameObject.FindGameObjectsWithTag("firstLineEmphtyCube");
            foreach (GameObject emphtyCubeForDisactivating in emphtyCubesForDisactivation)
            {
                emphtyCubeForDisactivating.GetComponent<MeshRenderer>().materials = semiTransparent;
            }
        }
        script2 = placedOnLineCube[1].GetComponent<CubesLogic>();
        if (script1.shape == script2.shape || script1.color == script2.color)
        {
            
            emphtyCubesForDisactivation = GameObject.FindGameObjectsWithTag("secondLineEmphtyCube");
            foreach (GameObject emphtyCubeForDisactivating in emphtyCubesForDisactivation)
            {
                emphtyCubeForDisactivating.GetComponent<MeshRenderer>().materials = semiTransparent;
            }
        }
        script2 = placedOnLineCube[2].GetComponent<CubesLogic>();
        if (script1.shape == script2.shape || script1.color == script2.color)
        {
            emphtyCubesForDisactivation = GameObject.FindGameObjectsWithTag("thirdLineEmphtyCube");
            foreach (GameObject emphtyCubeForDisactivating in emphtyCubesForDisactivation)
            {
                emphtyCubeForDisactivating.GetComponent<MeshRenderer>().materials = semiTransparent;
            }
        }
    }

    //проверка какой на какой позиции нужно спавнить куб и вызов функции для его спавна
    void SpawnNewCubeForChoosing()
    {
        if(cube.tag == "firstCubeForChoosing")
        {
            SpawningCubeForChoosing(cubesForChoosing[0], 3.45f, -2.3f, 0.83f, "firstCubeForChoosing");
        }
        else if (cube.tag == "secondCubeForChoosing")
        {
            SpawningCubeForChoosing(cubesForChoosing[1], 3.15f, -2.624f, 0.83f, "secondCubeForChoosing");
        }
        else if (cube.tag == "thirdCubeForChoosing")
        {
            SpawningCubeForChoosing(cubesForChoosing[2], 2.839f, -2.954f, 0.83f, "thirdCubeForChoosing");
        }
    }

    //создание первых трёх кубов
    void SpawningFirstThreeCubes()
    {
        for(int i = 0; i < 3; i++)
        {
            randValue[1] = rnd.Next(0, 3);
            while (randValue[0] == randValue[1])
            {
                randValue[1] = rnd.Next(0, 3);
            }
            switch (i)
            {
                case 0:
                    cube = Instantiate(cubePrefabs[randValue[1]], new Vector3(1f, 0f, 0f), Quaternion.Euler(0f, 0f, 0f));
                    allColorCubes[v++] = cube;
                    cube.tag = "placedOnFirstLineCube";
                    placedOnLineCube[0] = cube;
                    SpawningFirstThreeSemiTransparentCubes(cube, "firstLineEmphtyCube");
                    break;
                case 1:
                    cube = Instantiate(cubePrefabs[randValue[1]], new Vector3(0f, 1f, 0f), Quaternion.Euler(0f, 0f, 0f));
                    allColorCubes[v++] = cube;
                    cube.tag = "placedOnSecondLineCube";
                    placedOnLineCube[1] = cube;
                    SpawningFirstThreeSemiTransparentCubes(cube, "secondLineEmphtyCube");
                    break;
                case 2:
                    cube = Instantiate(cubePrefabs[randValue[1]], new Vector3(0f, 0f,1f), Quaternion.Euler(0f, 0f, 0f));
                    allColorCubes[v++] = cube;
                    cube.tag = "placedOnThirdLineCube";
                    placedOnLineCube[2] = cube;
                    SpawningFirstThreeSemiTransparentCubes(cube, "thirdLineEmphtyCube");
                    break;
            }
            randValue[0] = randValue[1];
            cube.transform.SetParent(cubes.transform, false);
            ColorChanging();
            cube = null;
        }
        
    }

    //лпределение где нет куба и вызов функции для создания нового куба на этом месте
    void SpawningThreeCubesForChoosing()
    {
        for (int i = 0; i < 3; i++)
        {
            if (cubesForChoosing[i] == null)
            {
                switch (i)
                {
                    case 0:
                        //SpawningCubeForChoosing(cubesForChoosing[i], -4.85f, 1.75f, 0.35f,"firstCubeForChoosing");
                        SpawningCubeForChoosing(cubesForChoosing[i], 3.45f, -2.3f, 0.83f, "firstCubeForChoosing");
                        break;
                    case 1:
                        //SpawningCubeForChoosing(cubesForChoosing[i], -4.85f, 1.75f, 0f, "secondCubeForChoosing");
                        SpawningCubeForChoosing(cubesForChoosing[i], 3.15f, -2.624f, 0.83f, "secondCubeForChoosing");
                        break;
                    case 2:
                        //SpawningCubeForChoosing(cubesForChoosing[i], -4.85f, 1.75f, -0.35f, "thirdCubeForChoosing");
                        SpawningCubeForChoosing(cubesForChoosing[i], 2.839f, -2.954f, 0.83f, "thirdCubeForChoosing");
                        break;
                }
            }
        }
    }

    //спавн куба для выбора
    void SpawningCubeForChoosing(GameObject cubeForChoosing,float x, float y, float z,string tag)
    {
        randValue[1] = rnd.Next(0, 3);
        while (randValue[0] == randValue[1])
        {
            randValue[1] = rnd.Next(0, 3);
        }
        cubeForChoosing = Instantiate(cubePrefabs[randValue[1]], new Vector3(x, y, z), Quaternion.Euler(45f, 0f, 0f));
        cubeForChoosing.transform.SetParent(cubesBackGround.transform, true);
        cubeForChoosing.transform.localPosition = new Vector3(x, y, z);
        randValue[0] = randValue[1];
        cubeForChoosing.transform.localScale = new Vector3(10f, 10f, 10f);
        cubeForChoosing.tag = tag;
        placedCube = cube;
        cube = cubeForChoosing;
        ColorChanging();
    }

    void DeleteAllSemiTransparentCubes()
    {
        emphtyCubesForDeleting = GameObject.FindGameObjectsWithTag("firstLineEmphtyCube");
        foreach (GameObject emphtyCubeForDeleting in emphtyCubesForDeleting)
        {
            Destroy(emphtyCubeForDeleting);
        }
        emphtyCubesForDeleting = GameObject.FindGameObjectsWithTag("secondLineEmphtyCube");
        foreach (GameObject emphtyCubeForDeleting in emphtyCubesForDeleting)
        {
            Destroy(emphtyCubeForDeleting);
        }
        emphtyCubesForDeleting = GameObject.FindGameObjectsWithTag("thirdLineEmphtyCube");
        foreach (GameObject emphtyCubeForDeleting in emphtyCubesForDeleting)
        {
            Destroy(emphtyCubeForDeleting);
        }
    }

    //создание маленьких и больших полупрозрачных кубов, для определения куда ходить можно, а куда нет
    void SpawningSemiTransparentCubes(GameObject emphtyCube, string tag)
    {
        emphtyCubesForDeleting = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject emphtyCubeForDeleting in emphtyCubesForDeleting)
        {
            Destroy(emphtyCubeForDeleting);
        }
        for (int i = 0; i < 6; i++)
        {
            emphtyCubeInstance = Instantiate(emphtyCubePrefab2, new Vector3(0f, 0f, 0f), Quaternion.Euler(0f, 0f, 0f));
            smallEmphtyCubeInstance = Instantiate(emphtyCubePrefab, new Vector3(0f, 0f, 0f), Quaternion.Euler(0f, 0f, 0f));

            emphtyCubeInstance.transform.SetParent(cubes.transform, false);
            smallEmphtyCubeInstance.transform.SetParent(cubes.transform, false);

            emphtyCubeScript = emphtyCubeInstance.GetComponent<EmphtyCubesLogic>();
            emphtyCubeScript.smallCube = smallEmphtyCubeInstance;
            switch (i)
            {
                case 0:
                    emphtyCubeInstance.transform.localPosition = new Vector3(emphtyCube.transform.localPosition.x, emphtyCube.transform.localPosition.y, emphtyCube.transform.localPosition.z + 1.11f);
                    smallEmphtyCubeInstance.transform.localPosition = new Vector3(emphtyCube.transform.localPosition.x, emphtyCube.transform.localPosition.y, emphtyCube.transform.localPosition.z + 1f);
                    smallEmphtyCubeInstance.tag = tag;
                    break;
                case 1:
                    emphtyCubeInstance.transform.localPosition = new Vector3(emphtyCube.transform.localPosition.x, emphtyCube.transform.localPosition.y + 1.11f, emphtyCube.transform.localPosition.z);
                    smallEmphtyCubeInstance.transform.localPosition = new Vector3(emphtyCube.transform.localPosition.x, emphtyCube.transform.localPosition.y + 1f, emphtyCube.transform.localPosition.z );
                    smallEmphtyCubeInstance.tag = tag;
                    break;
                case 2:
                    emphtyCubeInstance.transform.localPosition = new Vector3(emphtyCube.transform.localPosition.x + 1.11f, emphtyCube.transform.localPosition.y, emphtyCube.transform.localPosition.z);
                    smallEmphtyCubeInstance.transform.localPosition = new Vector3(emphtyCube.transform.localPosition.x + 1f, emphtyCube.transform.localPosition.y, emphtyCube.transform.localPosition.z);
                    smallEmphtyCubeInstance.tag = tag;
                    break;
                case 3:
                    emphtyCubeInstance.transform.localPosition = new Vector3(emphtyCube.transform.localPosition.x, emphtyCube.transform.localPosition.y, emphtyCube.transform.localPosition.z - 1.11f);
                    smallEmphtyCubeInstance.transform.localPosition = new Vector3(emphtyCube.transform.localPosition.x, emphtyCube.transform.localPosition.y, emphtyCube.transform.localPosition.z - 1f);
                    smallEmphtyCubeInstance.tag = tag;
                    break;
                case 4:
                    emphtyCubeInstance.transform.localPosition = new Vector3(emphtyCube.transform.localPosition.x, emphtyCube.transform.localPosition.y - 1.11f, emphtyCube.transform.localPosition.z);
                    smallEmphtyCubeInstance.transform.localPosition = new Vector3(emphtyCube.transform.localPosition.x, emphtyCube.transform.localPosition.y - 1f, emphtyCube.transform.localPosition.z);
                    smallEmphtyCubeInstance.tag = tag;
                    break;
                case 5:
                    emphtyCubeInstance.transform.localPosition = new Vector3(emphtyCube.transform.localPosition.x - 1.11f, emphtyCube.transform.localPosition.y, emphtyCube.transform.localPosition.z);
                    smallEmphtyCubeInstance.transform.localPosition = new Vector3(emphtyCube.transform.localPosition.x - 1f, emphtyCube.transform.localPosition.y, emphtyCube.transform.localPosition.z);
                    smallEmphtyCubeInstance.tag = tag;
                    break;
            }
            emphtyCubeInstance.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    void SpawningFirstThreeSemiTransparentCubes(GameObject emphtyCube, string tag)
    {
        emphtyCubeInstance = Instantiate(emphtyCubePrefab, new Vector3(emphtyCube.transform.localPosition.x * 2f, emphtyCube.transform.localPosition.y * 2f, emphtyCube.transform.localPosition.z * 2f), Quaternion.Euler(0f, 0f, 0f));
        emphtyCubeInstance.transform.SetParent(cubes.transform, false);
        emphtyCubeInstance.tag = tag;
    }

    void ColorChanging()
    {
        if (cube != null)
        {
            randValue[3] = rnd.Next(1, 7);
            while (randValue[2] == randValue[3])
            {
                randValue[3] = rnd.Next(1, 7);
            }
            materials[0] = materialPrafabs[randValue[3]];
            randValue[2] = randValue[3];
            cube.GetComponent<MeshRenderer>().materials = materials;
            script1 = cube.GetComponent<CubesLogic>();
            switch (randValue[3])
            {
                case 1:
                    script1.color = "red";
                    break;
                case 2:
                    script1.color = "green";
                    break;
                case 3:
                    script1.color = "purple";
                    break;
                case 4:
                    script1.color = "brown";
                    break;
                case 5:
                    script1.color = "orange";
                    break;
                case 6:
                    script1.color = "lightGreen";
                    break;
            }
        }
    }
}
