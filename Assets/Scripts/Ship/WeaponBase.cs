using UnityEngine;
using System.Collections;

public abstract class WeaponBase : MonoBehaviour
{
    public int ammoCount;
    public int missileDamage;
    public float missileSpeed;
    public float fireRate;
    public Transform[] shotSpawns;
    public GameObject shotGameObject;
    public enum WeaponType { basic, automatic, shotgun, rocket, piercing };
    public WeaponType weaponType;
    public PlayerWeaponController playerWeaponController;
    protected float _fireRateCof = 1;
    protected float _nextFire;
    public int _playerId;

    public virtual void Fire()
    {
        if (GetComponent<AudioSource>() != null)
        {
            GetComponent<AudioSource>().Play();
        }
        foreach (var shotSpawn in shotSpawns)
        {
            var shot = Instantiate(shotGameObject, shotSpawn.position, shotSpawn.rotation) as GameObject;
            var missileBase = shot.GetComponent<MissileBase>();
            missileBase.missileDamage = missileDamage;
            missileBase.missileSpeed = missileSpeed;
        }
    }

	public void updateAmmo(int ammo)
	{
		GameController controller = playerWeaponController._gameController;
		controller.updateAmmo(_playerId, ammo.ToString());
	}
}
