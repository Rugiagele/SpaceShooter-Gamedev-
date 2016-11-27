using UnityEngine;

public class PlayerRocket : MissileBase
{

    public GameObject explosion;
    public float explosionRadius;

    public override void Destroy()
    {
        Instantiate(explosion, transform.position, transform.rotation);
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var hit in hitColliders)
        {
            if (hit.gameObject.tag == "Enemy")
            {
				hit.GetComponent<ShipBase>().ChangeHp(
					Mathf.RoundToInt(
						-missileDamage * 
						(Mathf.Sqrt(
							Mathf.Pow(transform.position.x + hit.transform.position.x, 2) + 
							Mathf.Pow(transform.position.y + hit.transform.position.y, 2)) 
							/ explosionRadius)), 
					damageSource);
            }
        }
        Destroy(gameObject);
    }
}
