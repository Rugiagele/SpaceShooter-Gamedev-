using UnityEngine;
using System.Collections;
using System;

public class LevelManager : MonoBehaviour
{

    public LevelBase[] levels;
    public GameController gameController;
    public int currentLevelId = 0;
    public bool levelInProgress = false;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (levelInProgress)
        {
            return;
        }
        if (levels.Length <= currentLevelId)
        {
            gameController.YouWin();
            enabled = false;
            return;
        }
        Debug.Log("levelcalled");
        levels[currentLevelId].gameController = gameController;
        levels[currentLevelId].gameObject.SetActive(true);
        levelInProgress = true;
    }

    public void LevelEnded()
    {
        levelInProgress = false;
        currentLevelId++;
    }
}
