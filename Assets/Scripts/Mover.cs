using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour
{
	public float speed;
    public bool randomizeSpeed = false;

	void Start ()
	{
		GetComponent<Rigidbody>().velocity = transform.forward * speed * (randomizeSpeed?(Random.value/2+0.75f):1);
	}
}
