using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour
{
	public GameObject shot;
	public Transform[] shotSpawns;
	public float fireRate;
	public float delay;

	void Start ()
	{
        if(fireRate > 0)
		    InvokeRepeating ("Fire", delay, fireRate);
	}

	void Fire ()
	{
        foreach(var shotSpawn in shotSpawns)
        {
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        }
        GetComponent<AudioSource>().Play();
    }
}
