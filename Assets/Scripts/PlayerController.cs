using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, yMin, yMax;
}

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float speedBuffTime;
    public float tilt;
    public int lives;
    public Boundary boundary;

    public int playerId = 1;

    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;

    private float nextFire;
    private float speedCof = 1;
    private float fireRateCof = 1;
    public GameController gameController;

    public bool isInvulnerable;
    public float invulneravilityTime;
    public Color playerColor;
    public GameObject playerExplosion;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetButton("Fire" + playerId) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate*fireRateCof;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            GetComponent<AudioSource>().Play();
        }
        if (speedBuffTime > 0)
        {
            speedBuffTime -= Time.deltaTime;
            if (speedBuffTime <= 0)
            {
                speedCof = 1;
                fireRateCof = 1;
                speedBuffTime = 0;
            }
        }
        if (isInvulnerable)
        {
            invulneravilityTime -= Time.deltaTime;
            if (invulneravilityTime <= 0)
            {
                invulneravilityTime = 0;
                isInvulnerable = false;
                GetComponent<MeshRenderer>().materials[0].color = playerColor;
            }
        }
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal" + playerId);
        float moveVertical = Input.GetAxis("Vertical" + playerId);

        Vector3 movement = new Vector2(moveHorizontal, moveVertical);
        rb.velocity = movement * speed * speedCof;

        rb.position = new Vector3
        (
            Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
            Mathf.Clamp(rb.position.y, boundary.yMin, boundary.yMax)
        );

        transform.rotation = Quaternion.Euler(0.0f, rb.velocity.x * -tilt, 0.0f);
    }

    public void AddSpeed(float speedTime, float speedCof)
    {
        speedBuffTime += speedTime;
        this.speedCof = speedCof;
        this.fireRateCof = 0.5f;
    }

    public void ChangeLives(int lifeChange)
    {
        
        if (lifeChange < 0)
        {
            if (isInvulnerable)
            {
                return;
            }
            isInvulnerable = true;
            invulneravilityTime = 1;
            GetComponent<MeshRenderer>().materials[0].color = playerColor * new Color(0.4f,0.7f,0.4f);

        }
        lives = lives + lifeChange;
        gameController.UpdateLives(lives, playerId);

        if(lives<1)
        {
            Instantiate(playerExplosion, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
