using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, yMin, yMax;
}

public class PlayerController : ShipBase
{
    public float speed;
    public float speedBuffTime;
    public float tilt;
    public int lives;
    public Boundary boundary;
    public float fireRate;
    public GameObject playerExplosion;
    private float _speedCof = 1;

    private bool _isInvulnerable;
    public float invulneravilityTime;

    private int _playerId = 1;
    private Color _playerColor;


    public int GetPlayerId() { return _playerId; }

    void Start()
    {
        GetComponent<PlayerWeaponController>()._playerId = _playerId;
    }

	public void Initialize(GameController gameController, int playerId, Color playerColor) //call this method after player gameobject initialization
	{
		Initialize(gameController);
		_playerColor = playerColor;
		_playerId = playerId;

		GetComponent<MeshRenderer>().materials[0].color = _playerColor;
		_gameController.UpdateLives(_playerId);
	}

    void Update()
    {
		if (_isInvulnerable) {
			invulneravilityTime -= Time.deltaTime;
			if (invulneravilityTime <= 0) {
				invulneravilityTime = 0;
				_isInvulnerable = false;
				GetComponent<MeshRenderer> ().materials [0].color = _playerColor;
			}
		} else {
			if (isTinted && Time.time > tintExpireTime) {
				meshRenderer.materials [0].color = originalColor;
				isTinted = false;
			}
		}
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal" + _playerId);
        float moveVertical = Input.GetAxis("Vertical" + _playerId);

        Vector3 movement = new Vector2(moveHorizontal, moveVertical);
        _rigidbody2d.velocity = movement * speed * _speedCof;

        _rigidbody2d.position = new Vector3
        (
            Mathf.Clamp(_rigidbody2d.position.x, boundary.xMin, boundary.xMax),
            Mathf.Clamp(_rigidbody2d.position.y, boundary.yMin, boundary.yMax)
        );

        transform.rotation = Quaternion.Euler(0.0f, _rigidbody2d.velocity.x * -tilt, 0.0f);
    }

    public void AddSpeed(float speedCof)
    {
		_speedCof = Mathf.Clamp (_speedCof+speedCof, .5f, 3f);
    }

	public void AddInvulnerability(float time)
	{
		_isInvulnerable = true;
		invulneravilityTime += time;
		GetComponent<MeshRenderer>().materials[0].color = _playerColor * new Color(.4f, .7f, .4f);
	}

    public override void ChangeHp(int changeAmount, int playerId = 0)
    {
        if (changeAmount < 0 && _isInvulnerable)
        {
            return;
        }
		if (changeAmount < 0) {
            var audioSources = GetComponents<AudioSource>();
            if (audioSources[1] != null)
            {
                audioSources[1].Play();
            }
            TintOnHit ();
		}
        _shipHp += changeAmount;
        _shipHp = _shipHp > maxShipHp ? maxShipHp : _shipHp;

        if (_shipHp <= 0)
        {
            _shipHp = 0;
            _gameController.UpdateLives(_playerId);
            OnShipDestroy();
            return;
        }
        _gameController.UpdateLives(_playerId);
    }

    public int GetPlayerHp()
    {
        return _shipHp * 100 / maxShipHp;
    }

    protected override void OnShipDestroy()
    {
        Instantiate(playerExplosion, transform.position, transform.rotation);
        Destroy(gameObject);
        _gameController.GameOver();
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyBolt")
        {
            ChangeHp(-other.gameObject.GetComponent<MissileBase>().missileDamage);
            other.GetComponent<MissileBase>().Destroy();
        }
    }
}
