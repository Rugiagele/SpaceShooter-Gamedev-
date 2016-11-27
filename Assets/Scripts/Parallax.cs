using UnityEngine;
using System.Collections;

public class Parallax : MonoBehaviour
{
    public float speed;
    public float start = 0;

    Vector2 offset = new Vector2(0, 0);
    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        offset.y = start + Time.time * speed;
        rend.material.mainTextureOffset = offset;
    }
}