using UnityEngine;
using System.Collections;

public class BonusSpeed : MonoBehaviour
{
    public float speedCof;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerController>().AddSpeed(speedCof);
            var audioSources = other.GetComponents<AudioSource>();

            if (audioSources != null)
			{
                audioSources[0].Play();
			}
            Destroy(gameObject);
        }
    }
}
