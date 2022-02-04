using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text HighScoreText;
    public GameObject GameOverText;
    
    private bool _mStarted = false;
    private int _mPoints;
    
    private bool _mGameOver = false;

    private string _playerName;

    private int _highScore;

    private string _highScoreName;
    // Start is called before the first frame update
    void Start()
    {
        GetPlayerName();
        _highScore = StartManager.Instance.highScore;
        _highScoreName = StartManager.Instance.highScorePlayerName;
        UpdateHighScoreText();
        AddPoint(0);
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

    void GetPlayerName()
    {
        _playerName = StartManager.Instance.playerName;
    }
    void UpdateHighScoreText()
    {
        HighScoreText.text = $"Best Score by {_highScoreName} at {_highScore}";
    }
    
    private void Update()
    {
        if (!_mStarted)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _mStarted = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (_mGameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        _mPoints += point;
        ScoreText.text = $"{_playerName}'s score : {_mPoints}";
        if (_mPoints > _highScore)
        {
            _highScore = _mPoints;
            _highScoreName = _playerName;
            UpdateHighScoreText();
        }
    }

    public void GameOver()
    {
        _mGameOver = true;
        StartManager.Save(_highScore, _highScoreName);
        GameOverText.SetActive(true);
    }
}
