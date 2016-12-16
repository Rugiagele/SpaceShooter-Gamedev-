using UnityEngine;
using System.Collections;
using System.Linq;

public class PlayerWeaponController : MonoBehaviour
{
    public WeaponBase[] playerWeapons;
    public int _playerId;
    public WeaponBase _activeWeapon;
	public GameController _gameController;

	public void Initialize(GameController gameController)
	{
		_gameController = gameController; // override on player controller - to pass playerId and Tint.
	}

    void Start()
    {
        foreach(var playerWeapon in playerWeapons)
        {
            playerWeapon.playerWeaponController = this;
            playerWeapon._playerId = _playerId;
			if(playerWeapon.weaponType == WeaponBase.WeaponType.basic)
            {
                _activeWeapon = playerWeapon;
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
				if (weaponType != WeaponBase.WeaponType.basic) {
					_gameController.updateAmmo(_playerId, ammo.ToString());
				} else {
					_gameController.updateAmmo(_playerId, "*");
				}	
				
				return;
            }
        }
    }

}
