using System;
using System.Collections;
using UnityEngine;

public class Wave1 : LevelBase
{
    public GameObject enemyShip;
    public int shipCount;
    public Vector3[] spawnPoints;
    public float delayBetweenSpawns;
    public PathPart[] trajectories;
    public bool repeat;
    public int repeatFrom;

    public override void Spawn()
    {
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        while (shipCount > 0)
        {
            var spawnedShip = Instantiate(enemyShip, spawnPoints[0], Quaternion.identity) as GameObject;
            spawnedShip.GetComponent<ShipBase>().Initialize(gameController);
            var moverScript = spawnedShip.AddComponent<EnemyMover>();
            moverScript.repeat = repeat;
            moverScript.repeatFrom = repeatFrom;
            moverScript.trajectory = trajectories;
            spawnedShip.transform.parent = this.transform;
            shipCount--;
            if (shipCount <= 0)
            {
                break;
            }
            yield return new WaitForSeconds(delayBetweenSpawns);
        }
        spawningEnded = true;
    }
}
