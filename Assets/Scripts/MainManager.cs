using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;
    public string namePlayer = "Name";
    public BestScore bestScore;

    public Text ScoreText;
    private Text bestScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
        bestScoreText = GameObject.Find("BestScore").GetComponent<Text>();
        LoadScore();
        if (PersistenceManager.Instance != null)
        {
            namePlayer = PersistenceManager.Instance.Name;
        }
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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {namePlayer} : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        SaveScore();
        LoadScore();
    }
    void SaveScore()
    {
        string path = Application.persistentDataPath + "/save.json";
        if (bestScore.Score < m_Points)
        {
            bestScore = new BestScore { Name = namePlayer, Score = m_Points };
            File.WriteAllText(path, JsonConvert.SerializeObject(bestScore));
        }
    }
    void LoadScore()
    {
        string path = Application.persistentDataPath + "/save.json";
        if (File.Exists(path))
        {
            bestScore = JsonConvert.DeserializeObject<BestScore>(File.ReadAllText(path));
        } else
        {
            bestScore = new BestScore { Name = "Name", Score = 0 };
            File.WriteAllText(path, JsonConvert.SerializeObject(bestScore));
        }
        bestScoreText.text = $"Best Score : {bestScore.Name} : {bestScore.Score}";
    }
}
