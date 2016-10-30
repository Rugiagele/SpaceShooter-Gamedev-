using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    public GameObject[] powerUps;
    public GameObject playerPrefab;
    public GameObject player2Prefab;

    public Level[] levels;
    public float startWait;
    

    public Text gameOverText;
    public Text restartText;
    public Text scoreText;
    public Text highScoreText;
    public Text player1LivesText;
    public Text player2LivesText;
    public Toggle multiplayerToggle;

    public Color player1Color;
    public Color player2Color;

    public bool isMultiplayer;
    public PlayerController p1Controller;
    public PlayerController p2Controller;

    private bool gameOver;
    private int score;
    private int highScore;
    private bool initializing = true;

    void Awake()
    {
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
            p1Controller.gameController = this;
            p1Controller.playerId = 1;
            player1LivesText.text = "Player 1 Lives: " + p1Controller.lives;
            player1.GetComponent<MeshRenderer>().materials[0].color = player1Color;
            p1Controller.playerColor = player1Color;

            p2Controller = player2.GetComponent<PlayerController>();
            p2Controller.gameController = this;
            p2Controller.playerId = 2;
            player2LivesText.text = "Player 2 Lives: " + p2Controller.lives;
            player2.GetComponent<MeshRenderer>().materials[0].color = player2Color;
            p2Controller.playerColor = player2Color;
        }
        else
        {
            var player1 = Instantiate(playerPrefab, new Vector3(0, -1, 0), Quaternion.identity) as GameObject;
            p1Controller = player1.GetComponent<PlayerController>();
            p1Controller.gameController = this;
            p1Controller.playerId = 1;
            player1LivesText.text = "Player 1 Lives: " + p1Controller.lives;
            p1Controller.playerColor = new Color(1, 1, 1);
        }
        gameOver = false;
        restartText.text = "Press 'R' for Restart";
        gameOverText.text = "";
        score = 0;
        UpdateScore();
        StartCoroutine(SpawnWaves());
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


    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (gameOver && Time.timeScale == 0)
        {
            HighScoreUpdate();
        }
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (!gameOver)
        {
            foreach (Level level in levels)
            {
                yield return StartCoroutine(level.Spawn());
                DropPowerup();
                //powerUps[];
            }
            YouWin();
        }
    }

    public void DropPowerup()
    {
        int luckyNumber = Random.Range(0, powerUps.Length);
        GameObject powerUp = powerUps[luckyNumber];
        Instantiate(powerUp, powerUp.transform.position, Quaternion.identity);
    }

    public void UpdateLives(int lives, int playerId)
    {
        if (lives == 0)
        {
            GameOver();
        }
        if (playerId == 1)
        {
            player1LivesText.text = "Player 1 Lives: " + lives;
            return;
        }
        player2LivesText.text = "Player 2 Lives: " + lives;

    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
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
        gameOverText.text = text;
        gameOver = true;
        StartCoroutine(ScaleTime(1, 0, 0.75f));
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