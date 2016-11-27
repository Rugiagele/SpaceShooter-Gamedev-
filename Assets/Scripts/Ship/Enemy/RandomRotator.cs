using UnityEngine;
using System.Collections;

public class RandomRotator : MonoBehaviour 
{
	public float tumble;
    private Vector3 angle;


    void Start()
    {
        angle = Random.insideUnitSphere;
    }

	void FixedUpdate ()
	{
		transform.Rotate(angle * tumble);
	}
}