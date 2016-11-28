using System;
using System.Collections;
using UnityEngine;

public class Wave1 : LevelBase
{
    public GameObject[] enemyShips;
    public int[] trajectoryShipCount;
    //public Vector3[] spawnPoints;
    public float[] trajectoryDelayBetweenSpawns;
    public Trajectory[] trajectories;
    public bool spawnSingleShipType = true;

    public override void Spawn()
    {
        for (int i = 0; i< trajectories.Length; i++)
        {
            StartCoroutine(SpawnWave(i));
        }
    }

    private IEnumerator SpawnWave(int trajectoryId)
    {
        var shipCount = spawnSingleShipType ? trajectoryShipCount[0] : trajectoryShipCount[trajectoryId];
        var delayBetweenSpawns = spawnSingleShipType ? trajectoryDelayBetweenSpawns[0] : trajectoryDelayBetweenSpawns[trajectoryId];
        while (shipCount > 0)
        {
                var spawnedShip = Instantiate(spawnSingleShipType ? enemyShips[0] : enemyShips[trajectoryId], trajectories[trajectoryId].trajectory[0].destination, Quaternion.identity) as GameObject;
                spawnedShip.GetComponent<ShipBase>().Initialize(gameController);
                var moverScript = spawnedShip.AddComponent<EnemyMover>();
                moverScript.trajectory = trajectories[trajectoryId];
                spawnedShip.transform.parent = this.transform;
            shipCount--;
            yield return new WaitForSeconds(delayBetweenSpawns);
        }
        spawningEnded = true;
    }
}
