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
            Destroy(gameObject);
        }
    }
}
