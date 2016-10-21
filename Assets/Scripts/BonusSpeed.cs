using UnityEngine;
using System.Collections;

public class BonusSpeed : MonoBehaviour
{
    public float speedTime;
    public float speedCof;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerController>().AddSpeed(speedTime, speedCof);
            Destroy(gameObject);
        }
    }
}
