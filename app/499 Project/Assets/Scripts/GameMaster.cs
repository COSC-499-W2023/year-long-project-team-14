using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Pathfinding;

public class GameMaster : MonoBehaviour
{
    public LeaderboardManager leaderboardManager;
    public float gameTime = 0;
    public Animator fadeAnim;
    public int playerCount = 1;
    public int difficulty = 1;
    public int player1ControlScheme = 0;
    public int player2ControlScheme = 1;

    public int currentLevel = 1;

    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;
    public healthSystem healthSystem1;
    public healthSystem healthSystem2;

    public GameObject player1Prefab;
    public GameObject player2Prefab;

    public GameObject levelTemplate;
    public GameObject level1;
    public GameObject[] levels;
    private Transform player1Spawn;
    private Transform player2Spawn;

    public GameObject level;

    public bool unitTest = false;

    void Start() //Sets everything up
    {
        level = Instantiate(level1, transform.position, Quaternion.identity);
        AstarPath.active.Scan();
        playerCount = PlayerPrefs.GetInt("playerCount");
        SetupPlayers();
        StartCoroutine(UpdateGrid());
    }

    void Update()
    {
        //Increment game timer
        gameTime += Time.deltaTime;
    }

    public void LevelComplete()
    {
        StartCoroutine(NextLevel());
    }
    public IEnumerator NextLevel()
    {
        //wait for screen to fade out and then destroy current level
        if(fadeAnim != null)
        {
            fadeAnim.Play("ScreenFadeOut");
            yield return new WaitForSecondsRealtime(0.5f);
        }
        Destroy(level);
        yield return null;
        if(fadeAnim != null)
            fadeAnim.Play("ScreenFadeIn");

        //move players out of the way
        player1.transform.position = new Vector3(1000, 0, 0);
        if(playerCount > 1)
            player2.transform.position = new Vector3(1000, 0, 0);

        //increase level count and spawn in new level or end game if on last level
        currentLevel++;

        if(currentLevel <= levels.Length)
        {
            //spawn in new level
            level = Instantiate(levels[currentLevel-1], transform.position, Quaternion.identity);
            AstarPath.active.Scan();
            
            //set player positions to new start positions
            player1Spawn = GameObject.FindWithTag("Player1Spawn").GetComponent<Transform>();
            player1.transform.position = player1Spawn.position;
            if(playerCount > 1)
            {   
                player2Spawn = GameObject.FindWithTag("Player2Spawn").GetComponent<Transform>();
                player2.transform.position = player2Spawn.position;
            }

            //reset player health
            healthSystem1.life = healthSystem1.maxLife;
            healthSystem1.SetHeartsActive();

            if(playerCount > 1)
            {
                healthSystem2.life = healthSystem2.maxLife;
                healthSystem2.SetHeartsActive();
            }
            
            //TODO: revive player if dead
        }
        else
        {
            //TODO: display win screen and end game
            print("YOU WIN!!!");

            //uploads score to leaderboard
            leaderboardManager.SubmitScore((int)(Math.Round(gameTime, 2) * 100), null);
        }
    }

    public void SetupPlayers()
    {
        Gamepad[] gamepads = Gamepad.all.ToArray();

        player1Spawn = GameObject.FindWithTag("Player1Spawn").GetComponent<Transform>();
        player1 = Instantiate(player1Prefab, player1Spawn.position, Quaternion.identity);
        healthSystem1 = player1.GetComponent<healthSystem>();

        if(player1ControlScheme == 0) //keyboard
        {
            player1.GetComponent<PlayerInput>().SwitchCurrentControlScheme("Keyboard&Mouse", Keyboard.current, Mouse.current);
        }
        else //controller
        {
            player1.GetComponent<PlayerInput>().SwitchCurrentControlScheme("Gamepad", gamepads[0]);
        }

        if(playerCount > 1)
        {
            player2Spawn = GameObject.FindWithTag("Player2Spawn").GetComponent<Transform>();
            player2 = Instantiate(player2Prefab, player2Spawn.position, Quaternion.identity);
            healthSystem2 = player2.GetComponent<healthSystem>();

            if(player2ControlScheme == 0 && player1ControlScheme == 1) //keyboard
            {
                player2.GetComponent<PlayerInput>().SwitchCurrentControlScheme("Keyboard&Mouse", Keyboard.current, Mouse.current);
            }
            else //controller
            {
                if(player1ControlScheme == 0 && gamepads.Length > 0)
                    player2.GetComponent<PlayerInput>().SwitchCurrentControlScheme("Gamepad", gamepads[0]);
                else if(gamepads.Length > 1)
                    player2.GetComponent<PlayerInput>().SwitchCurrentControlScheme("Gamepad", gamepads[1]);
            }
        }
    }

    //Constantly updates pathfinding grid so enemies know where they can and cannot go
    public IEnumerator UpdateGrid()
    {
        GraphUpdateObject guo = new GraphUpdateObject(GetComponent<Collider2D>().bounds);
        guo.updatePhysics = true;
        AstarPath.active.UpdateGraphs(guo);
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(UpdateGrid());
    }
}
