using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkinChanging : MonoBehaviour
{
    public static int score = 0;
    public TMP_Text scoreText;
    public GameObject[] skinPrefabs = new GameObject[9];

    void Awake()
    {
        LoadGame();
    }

    void Start()
    {
        //if()
    }

    void Update()
    {
        //scoreText.text = "" + score;
    }

    void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath
          + "/MySaveData.dat");
        SaveData data = new SaveData();
        data.savedScore = score;
        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Game data saved!");
    }

    void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath
          + "/MySaveData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file =
              File.Open(Application.persistentDataPath
              + "/MySaveData.dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();
            score = data.savedScore;
            Debug.Log("Game data loaded!");
        }
        else
        {
            score = 0;
            Debug.LogError("There is no save data!");
        }
    }

    public void ChangeOnFirstSkin()
    {
        PlayerThings.skinVariant = 1;
    }

    public void ChangeOnDefaultSkin()
    {
        PlayerThings.skinVariant = 0;
    }

    public void Activating(GameObject other)
    {
        other.gameObject.SetActive(true);
    }

    public void DisActivating(GameObject other)
    {
        other.gameObject.SetActive(false);
    }
}

[Serializable]
class SaveData
{
    public int savedScore;
}