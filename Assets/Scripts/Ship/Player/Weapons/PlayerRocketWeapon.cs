using UnityEngine;

public class PlayerRocketWeapon : WeaponBase
{
    void Update()
    {
        if (ammoCount <= 0)
        {
            playerWeaponController.SetActive(WeaponType.basic, 0);
        }
        if (Input.GetButton("Fire" + _playerId) && Time.time > _nextFire)
        {
            _nextFire = Time.time + fireRate * _fireRateCof;
            var shotGo = Instantiate(shotGameObject, shotSpawns[0].position, shotSpawns[0].rotation) as GameObject;
            var missileBase = shotGo.GetComponent<MissileBase>();
            missileBase.missileDamage = missileDamage;
            missileBase.missileSpeed = missileSpeed;
            missileBase.damageSource = _playerId;
            GetComponent<AudioSource>().Play();
            ammoCount--;
        }
    }
}
