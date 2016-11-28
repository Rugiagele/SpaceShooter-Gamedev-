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
			if (other.GetComponent<AudioSource>() != null)
			{
				other.GetComponent<AudioSource>().Play();
			}
            Destroy(gameObject);
        }
    }
}
