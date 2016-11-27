using UnityEngine;
using System.Collections;
using System.Linq;

public class PlayerWeaponController : MonoBehaviour
{
    public WeaponBase[] playerWeapons;
    public int _playerId;
    private WeaponBase _activeWeapon;

    void Start()
    {
        foreach(var playerWeapon in playerWeapons)
        {
            playerWeapon.playerWeaponController = this;
            playerWeapon._playerId = _playerId;
			if(playerWeapon.weaponType == WeaponBase.WeaponType.basic)
            {
                _activeWeapon = playerWeapon;
                //_activeWeapon.ammoCount = 50;
                playerWeapon.enabled = true;
            }
            else
            {
                playerWeapon.enabled = false;
            }
        }
    }

    public void SetActive(WeaponBase.WeaponType weaponType, int ammo)
    {
        _activeWeapon.enabled = false;
		foreach (var playerWeapon in playerWeapons)
        {
            playerWeapon.playerWeaponController = this;
            playerWeapon._playerId = _playerId;
            if (playerWeapon.weaponType == weaponType)
            {
                _activeWeapon = playerWeapon;
                _activeWeapon.enabled = true;
                _activeWeapon.ammoCount = ammo;
				return;
            }
        }
    }

}
