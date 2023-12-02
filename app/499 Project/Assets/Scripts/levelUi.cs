using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class levelUi : MonoBehaviour
{
    private Text levelText;

    private void Start()
    {
        levelText = GetComponent<Text>();

        if (levelText == null)
        {
            Debug.LogError("Text component not found on the 'level' GameObject.");
        }
    }

    private void Update()
    {
        GameMaster gameMaster = GameObject.FindObjectOfType<GameMaster>();

        if (gameMaster != null)
        {
            int levelNum = gameMaster.currentLevel;
            levelText.text = "LEVEL " + levelNum;
        }
    }
}
