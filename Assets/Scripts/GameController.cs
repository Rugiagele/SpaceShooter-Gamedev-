using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class GameController : MonoBehaviour
{
    public GameObject[] powerUps;
    public GameObject playerPrefab;
    public GameObject player2Prefab;

    public Text gameOverText;
    public Text restartText;
    public Text scoreText;
    public Text highScoreText;
    public Text player1LivesText;
    public Text player2LivesText;
    public Text player1ScoreText;
    public Text player2ScoreText;
    public Toggle multiplayerToggle;

    public Color player1Color;
    public Color player2Color;

    public bool isMultiplayer;
    public PlayerController p1Controller;
    public PlayerController p2Controller;

    public GameObject pauseMenu;
    public Text resumeButton;

    private bool gameOver;
    private int score;
    private int score1;
    private int score2;
    private int highScore;
    private bool initializing = true;
    public bool isPaused = false;

    void Awake()
    {
        isMultiplayer = ApplicationModel.TwoPlayers;
        GameObject[] gameControllerObjects = GameObject.FindGameObjectsWithTag("GameController");
        foreach (var gameControllerObject in gameControllerObjects)
        {
            if (gameControllerObject != this.gameObject)
            {
                isMultiplayer = gameControllerObject.GetComponent<GameController>().isMultiplayer;
                Destroy(gameControllerObject);
                break;
            }
        }

        multiplayerToggle.isOn = isMultiplayer;
        if (isMultiplayer)
        {
            var player1 = Instantiate(playerPrefab, new Vector3(-2, -1, 0), Quaternion.identity) as GameObject;
            var player2 = Instantiate(player2Prefab, new Vector3(2, -1, 0), Quaternion.identity) as GameObject;
            p1Controller = player1.GetComponent<PlayerController>();
            p1Controller.Initialize(this, 1, player1Color);
            player1LivesText.text = "Player 1 status: " + p1Controller.GetPlayerHp() + "%";

            p2Controller = player2.GetComponent<PlayerController>();
            p2Controller.Initialize(this, 2, player2Color);
            player2LivesText.text = "Player 2 status: " + p2Controller.GetPlayerHp() + "%";
            player1ScoreText.text = "Score: 0";
            player2ScoreText.text = "Score: 0";
        }
        else
        {
            var player1 = Instantiate(playerPrefab, new Vector3(0, -1, 0), Quaternion.identity) as GameObject;
            p1Controller = player1.GetComponent<PlayerController>();
            p1Controller.Initialize(this, 1, new Color(1, 0.2f, 0.2f));
            player1LivesText.text = "Player status: " + p1Controller.GetPlayerHp() + "%";
        }
        gameOver = false;
        isPaused = false;
        restartText.text = "Press 'R' for Restart";
        gameOverText.text = "";
        score = 0;
        UpdateScore();
        if (isMultiplayer)
        {
            highScore = PlayerPrefs.GetInt("HighScoreMulti", 0);
        }
        else
        {
            highScore = PlayerPrefs.GetInt("HighScore", 0);
        }
        highScoreText.text = "HighScore: " + highScore;
        DontDestroyOnLoad(this.gameObject);
        Time.timeScale = 1;
        initializing = false;
        resumeButton.text = "Resume";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (Input.GetKeyDown(KeyCode.P)|| Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        if(isPaused)
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
            if(gameOver)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    public void DropPowerupOnKill(Vector3 position)
    {
        Vector3 spawnPosition = position;
        int luckyNumber = UnityEngine.Random.Range(0, powerUps.Length);
        GameObject powerUp = powerUps[luckyNumber];
        Instantiate(powerUp, spawnPosition, Quaternion.identity);
    }

    public void UpdateLives(int playerId)
    {
        if (playerId == 1)
        {
            if (isMultiplayer)
            {
                player1LivesText.text = "Player 1 status: " + p1Controller.GetPlayerHp() + "%";
            }
            else
            {
                player1LivesText.text = "Player status: " + p1Controller.GetPlayerHp() + "%";

            }
        }
        else if (playerId == 2)
        {
            player2LivesText.text = "Player 2 status: " + p2Controller.GetPlayerHp() + "%";
        }
    }

    public void AddScore(int p1Score, int p2Score)
    {
        score += p1Score + p2Score;
        if (isMultiplayer)
        {
            score1 += p1Score;
            score2 += p2Score;
            player1ScoreText.text = "Score: " + score1;
            player2ScoreText.text = "Score: " + score2;
        }
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
        if (isMultiplayer)
        {
            //display individual score
        }
    }

    void HighScoreUpdate()
    {
        if (highScore < score)
        {
            highScore = score;
            highScoreText.text = "High Score: " + highScore;
            if (isMultiplayer)
            {
                PlayerPrefs.SetInt("HighScoreMulti", highScore);
            }
            else
            {
                PlayerPrefs.SetInt("HighScore", highScore);
            }
        }
    }

    public void GameOver()
    {
        GameOver("Game Over!");
    }

    public void YouWin()
    {
        GameOver("You beat the game! Gratz. Now try and beat your score.");
    }

    private void GameOver(string text)
    {
        resumeButton.text = "Play again";
        gameOverText.text = text;
        gameOver = true;
        StartCoroutine(ScaleTime(1, 0, 0.75f));
        HighScoreUpdate();
    }

    public void ToggleMultiplayer()
    {
        if (initializing)
        {
            return;
        }
        isMultiplayer = !isMultiplayer;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator ScaleTime(float start, float end, float time)
    {
        float lastTime = Time.realtimeSinceStartup;
        float timer = 0.0f;

        while (timer < time)
        {
            Time.timeScale = Mathf.Lerp(start, end, timer / time);
            timer += (Time.realtimeSinceStartup - lastTime);
            lastTime = Time.realtimeSinceStartup;
            yield return null;
        }

        Time.timeScale = end;
    }
}