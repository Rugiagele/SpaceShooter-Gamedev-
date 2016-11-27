using UnityEngine;
using System.Collections;

public abstract class WeaponBase : MonoBehaviour
{
    public int missileDamage;
    public float missileSpeed;
    public float fireRate;
    public Transform[] shotSpawns;
    public GameObject shotGameObject;
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
}
