using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public class HighScores : MonoBehaviour
{
    private HsData _hsData;

    public void Awake()
    {
        LoadHighScores();
    }

    public void LoadHighScores()
    {
        if (File.Exists(Application.persistentDataPath + "/highscores.rt"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/highscores.rt", FileMode.Open);
            _hsData = (HsData) bf.Deserialize(file);
            file.Close();
        }
        else
        {
            _hsData = new HsData();
        }
    }

    public void SaveHighScores()
    {
        BinaryFormatter bf = new BinaryFormatter(); 
        FileStream file = File.Create(Application.persistentDataPath 
                                      + "/highscores.rt");
        bf.Serialize(file, _hsData);
        file.Close();
        Debug.Log("Game data saved!");
    }

    private void OnDestroy()
    {
        SaveHighScores();
    }
}

[Serializable]
public class HsData
{
    [SerializeField] private List<HighScore> _scores;
    public List<HighScore> Scores => _scores;

    public HsData()
    {
        _scores = new List<HighScore>();
    }
}

[Serializable]
public class HighScore
{
    private int value;
    private string player;
}