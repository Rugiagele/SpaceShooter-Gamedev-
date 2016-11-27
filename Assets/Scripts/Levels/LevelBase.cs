using UnityEngine;
using System.Collections.Generic;

public abstract class LevelBase : MonoBehaviour
{

    public bool levelFinished = false;
    public bool spawningEnded = false;
    public LevelManager _levelManager;
    public GameController gameController;
    // Use this for initialization
    void Start()
    {
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount == 0 && spawningEnded && !levelFinished)
        {
            levelFinished = true;
            _levelManager.LevelEnded();
        }
    }
    public abstract void Spawn();
}
