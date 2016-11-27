using UnityEngine;

public abstract class MissileBase : MonoBehaviour {

    public int missileDamage;
    public float missileSpeed;
    public int damageSource = 0; // 1 - player1, 2 - player2, 0 - enemy;
    protected Rigidbody2D _rigidbody2d;
    void Start()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        Mover();
    }
    protected abstract void Mover();
}
