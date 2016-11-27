using UnityEngine;
using System.Collections;

public class PlayerAutomaticWeapon : WeaponBase
{
    void Update()
    {
        if(ammoCount <= 0)
        {
            playerWeaponController.SetActive(WeaponType.basic, 0);
        }
        if (Input.GetButton("Fire" + _playerId) && Time.time > _nextFire)
        {
            ammoCount--;
            _nextFire = Time.time + fireRate * _fireRateCof;
            int source = Random.Range(0, shotSpawns.Length);
            var shotGo = Instantiate(shotGameObject, shotSpawns[source].position, shotSpawns[source].rotation) as GameObject;
            shotGo.transform.Rotate(0, 0, Random.Range(-1f, 1f));
            var missileBase = shotGo.GetComponent<MissileBase>();
            missileBase.missileDamage = missileDamage;
            missileBase.missileSpeed = missileSpeed;
            missileBase.damageSource = _playerId;

            GetComponent<AudioSource>().Play();
        }
    }
}
