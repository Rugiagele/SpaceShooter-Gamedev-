using System;
using System.Collections;
using UnityEngine;

public class Boss : LevelBase
{
    public GameObject enemyShip;
    public Trajectory trajectory;
    public int[] jumpPoint;
    public int[] jumpPointEnd;
    public int centerPoint;

    public override void Spawn()
    {
        var spawnedShip = Instantiate(enemyShip, trajectory.trajectory[0].destination, Quaternion.identity) as GameObject;
        spawnedShip.GetComponent<ShipBase>().Initialize(gameController);
        var moverScript = spawnedShip.AddComponent<BossMover>();
        moverScript.trajectory = trajectory;
        moverScript.jumpPoint = jumpPoint;
        moverScript.jumpPointEnd = jumpPointEnd;
        moverScript.centerPoint = centerPoint;
        spawnedShip.transform.parent = this.transform;
        spawningEnded = true;
    }
}
