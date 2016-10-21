using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
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

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        GetComponent<Rigidbody>().velocity = movement * speed * speedCof;

        GetComponent<Rigidbody>().position = new Vector3
        (
            Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
        );

        GetComponent<Rigidbody>().rotation = Quaternion.Euler(0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);
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
