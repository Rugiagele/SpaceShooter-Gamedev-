public class EnemySimpleMissile : MissileBase {

    protected override void Mover()
    {
        _rigidbody2d.velocity = transform.up * missileSpeed;
    }
}
