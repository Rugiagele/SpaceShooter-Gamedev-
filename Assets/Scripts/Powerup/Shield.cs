using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour
{
    public float time;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
			other.GetComponent<PlayerController>().AddInvulnerability (time);
            var audioSources = other.GetComponents<AudioSource>();

            if (audioSources != null)
            {
                audioSources[0].Play();
            }
            Destroy(gameObject);
        }
    }
}
