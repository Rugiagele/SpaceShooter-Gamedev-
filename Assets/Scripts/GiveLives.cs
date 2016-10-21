using UnityEngine;
using System.Collections;

public class GiveLives : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            var playerController = other.GetComponent<PlayerController>();
            playerController.ChangeLives(1);
            Destroy(gameObject);
        }
    }
}
