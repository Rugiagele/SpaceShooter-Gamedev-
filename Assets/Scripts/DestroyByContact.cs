using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour
{
    public GameObject explosion;
    public int scoreValue;
    private GameController gameController;
    public bool destroyableByShots = true;

    void Start()
    {
        GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Boundary" || other.tag == "Enemy" || other.tag == "Powerup")
        {
            return;
        }

        if (other.tag == "Bolt" && !destroyableByShots)
        {
            return;
        }

        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }

        if (other.tag == "Player")
        {
            var playerController = other.GetComponent<PlayerController>();
            playerController.ChangeLives(-1);
            gameController.AddScore(scoreValue);
            Destroy(gameObject);
            return;
        }

        if (other.tag == "Bolt")
        {
            if (Random.value < 0.5)
                gameController.DropPowerupOnKill(gameObject.transform.position);

            gameController.AddScore(scoreValue);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}