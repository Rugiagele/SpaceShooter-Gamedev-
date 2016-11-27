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
        var body = GetComponent<Rigidbody2D>();
            body.angularDrag = 0;
            body.freezeRotation = true;

        foreach(GameObject vector in vectors)
        {
            body.velocity = vector.transform.position * speed;
            yield return new WaitForSeconds(changeVectorAfter);
        }
    }
}
