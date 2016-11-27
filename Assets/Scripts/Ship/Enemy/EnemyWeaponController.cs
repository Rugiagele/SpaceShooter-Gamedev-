using UnityEngine;
using System.Collections;

public class EnemyWeaponController : WeaponBase
{
    public int shotCount = 1;
    public float timeBetweenShots = 0;
    public float delay;
    private int shotsRemaining;
    private float nextFireTime;

    void Start()
    {
        nextFireTime = Time.time + delay;
    }

    void Update()
    {
        if (nextFireTime < Time.time)
        {
            nextFireTime = Time.time + 1 / fireRate;
            StartCoroutine(InitiateFire());
        }
    }

    IEnumerator InitiateFire()
    {
        for (int i = 0; i < shotCount; i++)
        {
            Fire();
            yield return new WaitForSeconds(timeBetweenShots);
        }
    }
}
