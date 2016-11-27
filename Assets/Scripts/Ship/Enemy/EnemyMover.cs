using UnityEngine;
using System.Collections;

[System.Serializable]
public class PathPart
{
    public Vector2 destination;
    public float time;
    public bool toggleWeapon = false;
    public bool isMoving = true;
}

public class EnemyMover : MonoBehaviour
{
    public EnemyWeaponController[] weapons;
    public PathPart[] trajectory;
    private float nextMove = 0;
    private int step = 0;
    private Rigidbody2D _rigidbody2d;
    public bool repeat;
    public int repeatFrom = 0;


    void Start()
    {
        weapons = GetComponents<EnemyWeaponController>();
        _rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (step > trajectory.Length)
        {
            if (repeat)
            {
                step = repeatFrom;
            }
            else
            {
                _rigidbody2d.velocity = new Vector2();
                return;
            }
        }
        if (Time.time >= nextMove)
        {
            nextMove = Time.time + trajectory[step].time;
            if (trajectory[step].toggleWeapon)
            {
                foreach (var weapon in weapons)
                {
                    weapon.enabled = !weapon.enabled;
                }
            }
            if (trajectory[step].isMoving)
            {
                if (trajectory[step].time <= 0)
                {
                    Debug.LogWarning("trajectory time must be above 0");
                    _rigidbody2d.velocity = new Vector2();
                    step++;
                    return;
                }
                var direction = new Vector2(transform.position.x - trajectory[step].destination.x, transform.position.y - trajectory[step].destination.y);
                var speed = direction.magnitude / trajectory[step].time;
                _rigidbody2d.velocity = -direction.normalized * speed;
            }
            step++;
        }
    }
}
