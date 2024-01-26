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

    [SerializeField] private AudioSource transitionSound;

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
        //Play transition sound
        transitionSound.Play();

        //Wait for screen to fade out and then destroy current level and bullets
        if (fadeAnim != null)
        {
            fadeAnim.Play("ScreenFadeOut");
            yield return new WaitForSecondsRealtime(0.5f);
        }
        Destroy(level);
        GameObject[] playerBullets = GameObject.FindGameObjectsWithTag("Player_bullet");
        GameObject[] enemyBullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
        for(int i = 0; i < playerBullets.Length; i++) Destroy(playerBullets[i]);
        for(int i = 0; i < enemyBullets.Length; i++) Destroy(enemyBullets[i]);
        yield return null;

        //Start to fade back in
        if(fadeAnim != null)
            fadeAnim.Play("ScreenFadeIn");

        //Move players out of the way
        player1.transform.position = new Vector3(1000, 0, 0);
        if(playerCount > 1)
            player2.transform.position = new Vector3(1000, 0, 0);

        //Increase level count and spawn in new level or end game if on last level
        currentLevel++;

        if(currentLevel <= levels.Length)
        {
            //Spawn in new level
            level = Instantiate(levels[currentLevel-1], transform.position, Quaternion.identity);
            AstarPath.active.Scan();
            
            //Set player positions to new start positions
            player1Spawn = GameObject.FindWithTag("Player1Spawn").GetComponent<Transform>();
            player1.transform.position = player1Spawn.position;
            if(playerCount > 1)
            {   
                player2Spawn = GameObject.FindWithTag("Player2Spawn").GetComponent<Transform>();
                player2.transform.position = player2Spawn.position;
            }

            //Give players health
            if(healthSystem1.life < 3 && !healthSystem1.dead)
            {
                healthSystem1.life++;
                healthSystem1.SetHeartsActive();
            }

            if(playerCount > 1 && healthSystem2.life < 3 && !healthSystem2.dead)
            {
                healthSystem2.life++;
                healthSystem2.SetHeartsActive();
            }
            
            //Respawn player if dead
            if(healthSystem1.dead)
                RespawnPlayer(healthSystem1);
            
            if(playerCount > 1 && healthSystem2.dead)
                RespawnPlayer(healthSystem2);
        }
        else
        {
            //TODO: display win screen and end game
            print("YOU WIN!!!");

            //Uploads score to leaderboard
            leaderboardManager.SubmitScore((int)(Math.Round(gameTime, 2) * 100), null);
        }
    }

    //Brings player back to life
    public void RespawnPlayer(healthSystem hs)
    {
        hs.life = 1;
        hs.SetHeartsActive();
        hs.dead = false;
        hs.rb.bodyType = RigidbodyType2D.Dynamic;
        hs.gameObject.layer = LayerMask.NameToLayer("Player");
        hs.playerController.playerCenter.SetActive(true);        
        hs.spriteRenderer.sortingOrder = 10;
        hs.gameOverMenu.playercount++;
        hs.spriteRenderer.color = new Color(1, 1, 1, 1);
        hs.animator.SetTrigger("isHit");
        hs.isInvic = false;
    }

    //Set up controls for players
    public void SetupPlayers()
    {
        if(player1 != null && player2 != null)
        {
            Gamepad[] gamepads = Gamepad.all.ToArray();

            player1Spawn = GameObject.FindWithTag("Player1Spawn").GetComponent<Transform>();
            player1.transform.position = player1Spawn.position;
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
                player2.transform.position = player2Spawn.position;
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
            else
                player2.SetActive(false);
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
