using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using TMPro;



#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    public String playerName = "Player";
    public int highestScore;
    public String highestScorePlayer;
    public TMP_InputField nameInputField;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            nameInputField.onValueChanged.AddListener(SetPlayerName);
            LoadScoreData();
        }
    }

    public void SetPlayerName(string name)
    {
        playerName = name;
        Debug.Log("Player name set to: " + playerName);
        if (string.IsNullOrEmpty(playerName))
        {
            playerName = "Player";
        }
    }


    public void StartGame()
    {
        // Logic to start the game, e.g., loading the first scene
        SceneManager.LoadScene(1);
        gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        SaveScoreData();
#if UNITY_EDITOR
        // Logic to quit the game
        Debug.Log("Quit Game");
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }


    [Serializable]
    public class SaveData
    {
        public string playerName;
        public int highestScore;

        // Start is called once before the first execution of Update after the MonoBehaviour is created

    }

    public static void LoadScoreData()
    {
        //load from persistent data path
        string path = Path.Combine(Application.persistentDataPath, "save.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            MenuManager.Instance.highestScorePlayer = data.playerName;
            MenuManager.Instance.highestScore = data.highestScore;
        }
    }

    // Update is called once per frame
    public static void SaveScoreData()
    {
        //save to persistent data path
        string path = Path.Combine(Application.persistentDataPath, "save.json");
        SaveData data = new SaveData
        {
            playerName = MenuManager.Instance.highestScorePlayer,
            highestScore = MenuManager.Instance.highestScore
        };
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
    }
        
}
