using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text HighScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    private int m_highscore;
    private string m_highscoreHolder;
    private bool m_GameOver = false;

    private void Awake()
    {
        LoadPlayerData();
    }
    // Start is called before the first frame update
    void Start()
    {   
        if(m_highscoreHolder != null)
        {
            HighScoreText.text = "High Score : " + m_highscoreHolder + " : " + $"{m_highscore}";
        }
        else
        {
            HighScoreText.text = "High Score : ";
        }
        
        ScoreText.text = MainMenuManager.Instance.GetUserName() + " | " + $"Score : {m_Points}";

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            SavePlayerData();
            if (Input.GetKeyDown(KeyCode.Space))
            {             
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = MainMenuManager.Instance.GetUserName() + " | " + $"Score : {m_Points}";
    }

    void UpdateHighScore()
    {
        m_highscore = m_Points;
        HighScoreText.text = "High Score : " + MainMenuManager.Instance.GetUserName() + " : " + $"{m_highscore}";
    }

    public void GameOver()
    {     
        m_GameOver = true;
        if(m_Points > m_highscore)
        {
            m_highscoreHolder = MainMenuManager.Instance.GetUserName();
            UpdateHighScore();
        }      
        GameOverText.SetActive(true);
    }

    [System.Serializable]
    class SaveData // put all data you want to be able to save here
    {
        public int highscore;
        public string highscoreHolder;
    }

    public void SavePlayerData()
    {
        SaveData data = new SaveData();
        data.highscore = m_highscore;
        data.highscoreHolder = m_highscoreHolder;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadPlayerData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if(File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            m_highscore = data.highscore;
            m_highscoreHolder = data.highscoreHolder;
        }
    }


}
