using UnityEngine;
using System.Collections;

public class BossMover : MonoBehaviour
{
    public EnemyWeaponController[] weapons;
    
    private float nextMove = 0;
    private int step = 1;
    private Rigidbody2D _rigidbody2d;

    public Trajectory trajectory;
    public int centerPoint;
    
    public int[] jumpPoint;
    public int[] jumpPointEnd;
    private int exitPoint;

    void Start()
    {
        weapons = GetComponents<EnemyWeaponController>();
        _rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Time.time >= nextMove)
        {
            if(step == centerPoint)
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
                if(step==centerPoint)
                {
                    int selectedPathId = Random.Range(0, jumpPoint.Length);
                    step = jumpPoint[selectedPathId];
                    exitPoint = jumpPointEnd[selectedPathId];
                }
                else if(step==exitPoint)
                {
                    step = centerPoint;
                }
                else
                {
                    step++;
                }
            }
        }
    }
}
