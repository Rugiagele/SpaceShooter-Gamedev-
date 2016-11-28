using UnityEngine;
using System.Collections;

[System.Serializable]
public class PathPart
{
    public Vector2 destination;
    public float time;
    public bool toggleWeapon = false;
    public bool isMoving = true;
}

public class Trajectory : MonoBehaviour {

    public PathPart[] trajectory;
    public bool repeat;
    public int repeatFrom = 1;

}
