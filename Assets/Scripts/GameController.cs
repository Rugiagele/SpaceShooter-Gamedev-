using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class GameController : MonoBehaviour
{
    public GameObject[] hazards;
    public GameObject[] powerUps;
    public GameObject playerPrefab;

    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;


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
            var player1 = Instantiate(playerPrefab, new Vector3(-2, 0, 0), Quaternion.identity) as GameObject;
            var player2 = Instantiate(playerPrefab, new Vector3(2, 0, 0), Quaternion.identity) as GameObject;
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
            var player1 = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
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
            for (int i = 0; i < hazardCount; i++)
            {
                if (UnityEngine.Random.value < 0.10f)
                {
                    GameObject powerUp = powerUps[UnityEngine.Random.Range(0, powerUps.Length)];
                    Instantiate(powerUp, new Vector3(UnityEngine.Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z), Quaternion.identity);
                }
                GameObject hazard = hazards[UnityEngine.Random.Range(0, hazards.Length)];
                Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }
            spawnWait *= 0.8f;
            hazardCount += 2;
            yield return new WaitForSeconds(waveWait);
            waveWait *= 0.8f;
        }
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
        gameOverText.text = "Game Over!";
        gameOver = true;
        StartCoroutine(ScaleTime(1,0,0.75f));
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