using UnityEngine;

public class PlayerApRound : MissileBase
{

    public int lives = 3;

    protected override void Mover()
    {
        _rigidbody2d.velocity = transform.up * missileSpeed;
    }

    public override void Destroy()
    {
        lives--;
        missileDamage /= 2;
        if (lives <= 0)
        {
            Destroy(gameObject);
        }
    }
}
