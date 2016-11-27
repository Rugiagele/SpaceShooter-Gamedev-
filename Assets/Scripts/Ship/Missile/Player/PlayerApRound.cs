using UnityEngine;

public class PlayerApRound : MissileBase
{

    public int lives = 3;

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
