using UnityEngine;
using System.Collections;

public class GiveLives : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            var playerController = other.GetComponent<PlayerController>();
            playerController.ChangeLives(1);
            Destroy(gameObject);
        }
    }
}
