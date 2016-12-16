using UnityEngine;
using System.Collections;

public class EnemyMover : MonoBehaviour
{
    public EnemyWeaponController[] weapons;
    
    private float nextMove = 0;
    private int step = 1;
    private Rigidbody2D _rigidbody2d;

    public Trajectory trajectory;
	private float tilt = 3;

    void Start()
    {
        weapons = GetComponents<EnemyWeaponController>();
        _rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Time.time >= nextMove)
        {
            if(step>=trajectory.trajectory.Length)
            {
                if (trajectory.repeat && trajectory.repeatFrom < step)
                {
                    step = trajectory.repeatFrom;
                }
                else
                {
                    _rigidbody2d.velocity = new Vector2();
                    return;
                }
            }
            if (step < trajectory.trajectory.Length)
            {
                nextMove = Time.time + trajectory.trajectory[step].time;
                if (trajectory.trajectory[step].toggleWeapon)
                {
                    foreach (var weapon in weapons)
                    {
                        weapon.enabled = !weapon.enabled;
                    }
                }
                if (trajectory.trajectory[step].isMoving)
                {
                    if (trajectory.trajectory[step].time <= 0)
                    {
                        Debug.LogWarning("trajectory time must be above 0");
                        _rigidbody2d.velocity = new Vector2();
                        step++;
                        return;
                    }
                    var direction = new Vector2(transform.position.x - trajectory.trajectory[step].destination.x, transform.position.y - trajectory.trajectory[step].destination.y);
                    var speed = direction.magnitude / trajectory.trajectory[step].time;
                    _rigidbody2d.velocity = -direction.normalized * speed;                    
                }
                else
                {
                    _rigidbody2d.velocity = new Vector2();
                }
                step++;
            }
        }
    }

	void FixedUpdate()
	{
		transform.rotation = Quaternion.Euler (0.0f, _rigidbody2d.velocity.x * -tilt, 0.0f);
	}

}
