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


    public GameObject shot;
    public int missileDamage = 20;
    public float missileSpeed = 20;
    public Transform shotSpawn;
    public float fireRate;
    public GameObject playerExplosion;

    private float _nextFire;
    private float _speedCof = 1;
    private float _fireRateCof = 1;

    private bool _isInvulnerable;
    public float invulneravilityTime;

    private int _playerId = 1;
    private Color _playerColor;


    public int GetPlayerId() { return _playerId; }

    void Update() //move weapon logic to another sript. probably speed buffs/invulnerability - in my opinion should stay here
    {
        if (Input.GetButton("Fire" + _playerId) && Time.time > _nextFire)
        {
            _nextFire = Time.time + fireRate * _fireRateCof;
            var shotGo = Instantiate(shot, shotSpawn.position, shotSpawn.rotation) as GameObject;
            var missileBase = shotGo.GetComponent<MissileBase>();
            missileBase.missileDamage = missileDamage;
            missileBase.missileSpeed = missileSpeed;
            missileBase.damageSource = _playerId;

            GetComponent<AudioSource>().Play();
        }
        if (speedBuffTime > 0)
        {
            speedBuffTime -= Time.deltaTime;
            if (speedBuffTime <= 0)
            {
                _speedCof = 1;
                _fireRateCof = 1;
                speedBuffTime = 0;
            }
        }
        if (_isInvulnerable)
        {
            invulneravilityTime -= Time.deltaTime;
            if (invulneravilityTime <= 0)
            {
                invulneravilityTime = 0;
                _isInvulnerable = false;
                GetComponent<MeshRenderer>().materials[0].color = _playerColor;
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

    public void AddSpeed(float speedTime, float speedCof)
    {
        speedBuffTime += speedTime;
        _speedCof = speedCof;
        _fireRateCof = 0.5f;
    }

    public void Initialize(GameController gameController, int playerId, Color playerColor) //call this method after player gameobject initialization
    {
        Initialize(gameController);
        _playerColor = playerColor;
        _playerId = playerId;

        GetComponent<MeshRenderer>().materials[0].color = _playerColor;
        _gameController.UpdateLives(_playerId);
    }

    public override void ChangeHp(int changeAmount, int playerId = 0)
    {
        if (changeAmount < 0 && _isInvulnerable)
        {
            return;
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
        }
    }
}
