using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public abstract class ShipBase : MonoBehaviour
{
    public int maxShipHp;
    public int damageOnContact = 100;
    public GameObject explosion;
    protected int _shipHp;
	public GameController _gameController;
	private float tintTime = .4f;
	public float tintExpireTime;
	public Color tintColor = new Color (1, 0, 0);
	public Color originalColor;
    protected Rigidbody2D _rigidbody2d;
	public MeshRenderer meshRenderer;
	public bool isTinted = false;

    // Use this for initialization
    void Awake()
    {
        _shipHp = maxShipHp;
        _rigidbody2d = GetComponent<Rigidbody2D>();
        if(explosion == null)
        {
            Debug.LogWarning(gameObject.name + " has no explosion prefab set");
        }
    }

    public void Initialize(GameController gameController)
    {
        _gameController = gameController; // override on player controller - to pass playerId and Tint.
    }

    public abstract void ChangeHp(int changeAmount, int playerId = 0);

    protected abstract void OnShipDestroy();

    public abstract void OnTriggerEnter2D(Collider2D other);

	public virtual void TintOnHit()
	{
        if (isTinted!=true)
        {
            isTinted = true;
            originalColor = meshRenderer.materials[0].color;
            meshRenderer.materials[0].color *= tintColor;
        }
		tintExpireTime = Time.time + tintTime;
	}
}
