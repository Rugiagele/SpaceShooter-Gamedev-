﻿using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;

public class Level : MonoBehaviour
{
    public GameObject[] enemies;
    public int spawnWaves;
    public float delayBetweenSpawn;
    public GameController gameController;

    public IEnumerator Spawn()
    {
            
        for(int i = 0; i < spawnWaves; i++) { 
            foreach (var enemy in enemies)
            {
                var e = Instantiate(enemy, enemy.transform.position, Quaternion.identity) as GameObject;
                e.GetComponent<ShipBase>().Initialize(gameController);
            }
            yield return new WaitForSeconds(delayBetweenSpawn);
        }

        while (isInProgress())
            yield return new WaitForSeconds(0.1f);

        yield return new WaitForSeconds(3.0f);
    }

    public bool isInProgress()
    {
        bool found = false;
        foreach (var enemy in enemies)
        {
            found = GameObject.Find(enemy.name + "(Clone)") != null;
        }
        
        return found;
    }


}