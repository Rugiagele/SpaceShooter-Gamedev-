﻿using UnityEngine;
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
            var audioSources = other.GetComponents<AudioSource>();

            if (audioSources != null)
            {
                audioSources[0].Play();
            }
            Destroy(gameObject);
        }
    }
}
