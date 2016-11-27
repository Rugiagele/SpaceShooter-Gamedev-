using System;
using System.Collections;
using UnityEngine;

public class Wave1 : LevelBase
{
    public GameObject enemyShip;
    public int shipCount;
    public Vector3[] spawnPoints;
    public float delayBetweenSpawns;

    public override void Spawn()
    {
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        while (shipCount > 0)
        {
            foreach (var spawnPoint in spawnPoints)
            {
                var spawnedShip = Instantiate(enemyShip, spawnPoint, Quaternion.identity) as GameObject;
                spawnedShip.GetComponent<ShipBase>().Initialize(gameController);
                spawnedShip.transform.parent = this.transform;
                shipCount--;
                if (shipCount <= 0)
                {
                    break;
                }
            }
            yield return new WaitForSeconds(delayBetweenSpawns);
            spawningEnded = true;
        }
    }
}
