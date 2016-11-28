using UnityEngine;
using System.Collections;

public class GiveLives : MonoBehaviour
{
    public int lives = 10;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            var playerController = other.GetComponent<PlayerController>();
            playerController.ChangeHp(lives);
			if (other.GetComponent<AudioSource>() != null)
			{
				other.GetComponent<AudioSource>().Play();
			}
            Destroy(gameObject);
        }
    }
}
