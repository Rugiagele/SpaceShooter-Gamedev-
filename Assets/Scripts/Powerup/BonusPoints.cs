using UnityEngine;
using System.Collections;

public class BonusPoints : MonoBehaviour
{
    public int scoreValue;
    private GameController gameController;

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
        if (other.tag == "Player")
        {
            gameController.AddScore(scoreValue, 1); //TODO: pass player Id
			if (other.GetComponent<AudioSource>() != null)
			{
				other.GetComponent<AudioSource>().Play();
			}
            Destroy(gameObject);
        }
    }
}