using UnityEngine;

public class PlayerBasicWeapon : WeaponBase {
	void Update () {
        if (Input.GetButton("Fire" + _playerId) && Time.time > _nextFire)
            //if (Input.GetButtonDown("Fire" + _playerId) && Time.time > _nextFire)
            {
                _nextFire = Time.time + fireRate * _fireRateCof;
            var shotGo = Instantiate(shotGameObject, shotSpawns[0].position, shotSpawns[0].rotation) as GameObject;
            var missileBase = shotGo.GetComponent<MissileBase>();
            missileBase.missileDamage = missileDamage;
            missileBase.missileSpeed = missileSpeed;
            missileBase.damageSource = _playerId;

            GetComponent<AudioSource>().Play();
        }
	}
}
