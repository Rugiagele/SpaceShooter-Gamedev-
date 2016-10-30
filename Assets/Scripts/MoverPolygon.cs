using UnityEngine;
using System.Collections;

public class MoverPolygon : MonoBehaviour
{
    public float speed;
	public float changeVectorAfter;
    public GameObject[] vectors;

	void Start ()
	{
        StartCoroutine(Moving());
        
	}

    IEnumerator Moving()
    {
        var body = GetComponent<Rigidbody>();

        foreach(GameObject vector in vectors)
        {
            body.velocity = vector.transform.position * speed;
            body.angularVelocity = Vector3.zero;
            body.angularDrag = 0;
            body.freezeRotation = true;
            yield return new WaitForSeconds(changeVectorAfter);
            //body.velocity;
        }
    }
}
