using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class StartManager : MonoBehaviour
{
    public static StartManager Instance;

    public int highScore;

    public string highScorePlayerName;
    
    public string playerName;

    public TMPro.TextMeshProUGUI playerNameInputUI;

    public TMPro.TextMeshProUGUI highScoreTextUI;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        Load();
        SetHighScoreText();

    }

    private void SetHighScoreText()
    {
        highScoreTextUI.text = $"High Score by {highScorePlayerName} at {highScore}";
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnEndEdit(string input)
    {
        
        Debug.Log("Input: '" + input + "'");
        playerName = input;
    }
    
    public void OnButtonQuit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void OnButtonStart()
    {
        SceneManager.LoadScene(1);
    }

    [System.Serializable]
    class SavedData
    {
        public int score;
        public string name;
    }

    public static void Save(int score, string name)
    {
        SavedData data = new SavedData
        {
            score = score,
            name = name
        };
        string json = JsonUtility.ToJson(data);
        string dataPath = Application.persistentDataPath;
        string dataFile = "/savefile.json";
        Debug.Log("Writing to:" + dataPath + dataFile);
        File.WriteAllText(dataPath + dataFile, json);
        
        Instance.Load();
    }

    public void Load()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        Debug.Log($"Loading data from: {path}.");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SavedData data = JsonUtility.FromJson<SavedData>(json);
            highScore = data.score;
            highScorePlayerName = data.name;
        }
        else
        {
            highScore = 0;
            highScorePlayerName = "None";
        }
    }
}
