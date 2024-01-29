using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class levelUi : MonoBehaviour
{
    private Text levelText;
    GameMaster gameMaster;

    private void Start()
    {
        gameMaster = GameObject.FindObjectOfType<GameMaster>();
        levelText = GetComponent<Text>();

        if (levelText == null)
        {
            Debug.LogError("Text component not found on the 'level' GameObject.");
        }
    }

    //Updates text to display current level
    private void Update()
    {
        if(gameMaster != null)
        {
            int levelNum = gameMaster.currentLevel;
            levelText.text = "LEVEL " + levelNum;
        }
    }
}
