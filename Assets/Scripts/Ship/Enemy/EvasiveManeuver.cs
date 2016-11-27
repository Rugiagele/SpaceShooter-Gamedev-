using UnityEngine;
using System.Collections;

public class EvasiveManeuver : MonoBehaviour
{
	public Boundary boundary;
	public float tilt;
	public float dodge;
	public float smoothing;
	public Vector2 startWait;
	public Vector2 maneuverTime;
	public Vector2 maneuverWait;

	private float currentSpeed;
	private float targetManeuver;
	private GameController gc;

    void Start ()
	{
		currentSpeed = GetComponent<Rigidbody2D>().velocity.y;
		StartCoroutine(Evade());
		gc = GetComponent<ShipBase>()._gameController;
	}
	
	IEnumerator Evade ()
	{
		yield return new WaitForSeconds (Random.Range (startWait.x, startWait.y));
		while (true)
		{
			float direction = getDirection();
			targetManeuver = direction;
			yield return new WaitForSeconds (Random.Range (maneuverTime.x, maneuverTime.y));
			targetManeuver = 0;
			yield return new WaitForSeconds (Random.Range (maneuverWait.x, maneuverWait.y));
		}
	}

	float getDirection()
	{
		if (gc.isMultiplayer) {
			if (Random.value < .5f) {
				return gc.p1Controller.transform.position.x;
			} else {
				return gc.p2Controller.transform.position.x;
			}
		} else {
			return gc.p1Controller.transform.position.x;
		}
	}

	void FixedUpdate ()
	{
		float newManeuver = Mathf.MoveTowards (GetComponent<Rigidbody2D>().velocity.x, targetManeuver, smoothing * Time.deltaTime);
		GetComponent<Rigidbody2D>().velocity = new Vector2 (newManeuver, currentSpeed);
		GetComponent<Rigidbody2D>().position = new Vector2
		(
			Mathf.Clamp(GetComponent<Rigidbody2D>().position.x, boundary.xMin, boundary.xMax), 
			Mathf.Clamp(GetComponent<Rigidbody2D>().position.y, boundary.yMin, boundary.yMax)
		);
		
	//	GetComponent<Rigidbody2D>().Rotate(GetComponent<Rigidbody2D>().velocity.x * -tilt);
	}
}
