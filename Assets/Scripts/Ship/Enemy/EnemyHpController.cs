using UnityEngine;

public class EnemyHpController : ShipBase
{
    public int scoreValue;
    private int _p1Damage;
    private int _p2Damage;

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Boundary" || other.tag == "Enemy" || other.tag == "Powerup")
        {
            return;
        }

        if(other.tag == "Player")
        {
            var playerController = other.GetComponent<PlayerController>();
            playerController.ChangeHp(-damageOnContact);
            ChangeHp(-playerController.damageOnContact, playerController.GetPlayerId());
            return;
        }

        if (other.tag == "Bolt")
        {
            var missileBase = other.gameObject.GetComponent<MissileBase>();
            ChangeHp(-missileBase.missileDamage, missileBase.damageSource);
            other.GetComponent<MissileBase>().Destroy();
            return;
        }
    }

    public override void ChangeHp(int changeAmount, int playerId)
    {
        if(playerId == 1)
        {
            _p1Damage += changeAmount;
        }
        else if (playerId == 2)
        {
            _p2Damage += changeAmount;
        }
        _shipHp += changeAmount;
        _shipHp = _shipHp > maxShipHp ? maxShipHp : _shipHp;
        if (_shipHp <= 0)
        {
            _shipHp = 0;
            OnShipDestroy();
        }
    }

    protected override void OnShipDestroy()
    {
        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }
        int damageDone = _p1Damage + _p2Damage;
        if (damageDone != 0)
        {
            _p1Damage = Mathf.RoundToInt(_p1Damage * maxShipHp / damageDone);
            _p2Damage = Mathf.RoundToInt(_p2Damage * maxShipHp / damageDone);
            int p1Score = scoreValue * Mathf.RoundToInt(_p1Damage / maxShipHp);
            int p2Score = scoreValue - p1Score;
            _gameController.AddScore(p1Score, p2Score);
        }
        if (Random.value < 0.5)
        {
            _gameController.DropPowerupOnKill(gameObject.transform.position);
        }
        Destroy(gameObject);
    }
}
