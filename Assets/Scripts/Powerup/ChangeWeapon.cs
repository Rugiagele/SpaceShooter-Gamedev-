using UnityEngine;
using System.Collections;

public class ChangeWeapon : MonoBehaviour
{
	public WeaponBase.WeaponType type;
	public int ammoCount;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
			other.GetComponent<PlayerWeaponController> ().SetActive (type, ammoCount);
            var audioSources = other.GetComponents<AudioSource>();

            if (audioSources != null)
            {
                audioSources[0].Play();
            }
            Destroy(gameObject);
        }
    }
}
