using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Pathfinding;
using UnityEngine.EventSystems;

public class GameMaster : MonoBehaviour
{
    public LeaderboardManager leaderboardManager;
    public float gameTime = 0;
    public Animator fadeAnim;
    public int playerCount = 1;
    public int player1ControlScheme = 0;
    public int player2ControlScheme = 1;

    public int currentLevel = 1;
    public bool inShop = false;

    public GameObject WinMenu;

    public PauseMenu pauseMenu;
    public WinMenu winMenu;
    public GameOverMenu gameOverMenu;
    public ControlMenu controlMenu;
    public GameObject inputField;

    public GameObject player1;
    public GameObject player2;
    public healthSystem healthSystem1;
    public healthSystem healthSystem2;

    public GameObject player1Prefab;
    public GameObject player2Prefab;

    public GameObject shopLevel;
    public GameObject levelTemplate;
    public GameObject level1;
    public GameObject[] levels;
    private Transform player1Spawn;
    private Transform player2Spawn;

    public GameObject level;

    public bool stopTimer = false;
    public bool unitTest = false;

    [SerializeField] private AudioSource transitionSound;

    void Start() //Sets everything up
    {
        level = Instantiate(level1, transform.position, Quaternion.identity);
        playerCount = PlayerPrefs.GetInt("playerCount");
        SetupPlayers();
        StartCoroutine(UpdateGrid());
        StartCoroutine(SelectMenuButton());
    }

    void Update()
    {
        //Increment game timer
        if(!stopTimer)
            gameTime += Time.deltaTime;
    }

    //Go to next level
    public void LevelComplete()
    {
        StartCoroutine(NextLevel());
    }
    public IEnumerator NextLevel()
    {
        //Play transition sound
        transitionSound.Play();

        //Make players invincible while transitioning
        healthSystem1.isInvic = true;
        if(playerCount > 1)
            healthSystem2.isInvic = true;

        if(currentLevel < levels.Length)
        {
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

            if(currentLevel % 2 == 0 && !inShop) //Go to shop
            {
                level = Instantiate(shopLevel, transform.position, Quaternion.identity);
                inShop = true;
            }
            else //Go to next level
            {
                if(inShop)
                    inShop = false;

                //Increase level count
                currentLevel++;

                //Spawn in new level
                level = Instantiate(levels[currentLevel-1], transform.position, Quaternion.identity);
                AstarPath.active.Scan();
            }
            
            //Set player positions to new start positions
            player1Spawn = GameObject.FindWithTag("Player1Spawn").GetComponent<Transform>();
            player1.transform.position = player1Spawn.position;
            if(playerCount > 1)
            {   
                player2Spawn = GameObject.FindWithTag("Player2Spawn").GetComponent<Transform>();
                player2.transform.position = player2Spawn.position;
            }

            //Give players health
            if(!inShop)
            {
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
            }
            
            //Respawn player if dead
            if(healthSystem1.dead)
                RespawnPlayer(healthSystem1);
            
            if(playerCount > 1 && healthSystem2.dead)
                RespawnPlayer(healthSystem2);

            //Make player not invincible
            healthSystem1.isInvic = false;
            if(playerCount > 1)
                healthSystem2.isInvic = false;
        }
        else
        {
            stopTimer = true;

            //Fade out
            if (fadeAnim != null)
                fadeAnim.Play("ScreenFadeOut");
            yield return new WaitForSecondsRealtime(0.5f);

            //Display win screen and end game
            WinMenu.SetActive(true);
            winMenu.winMenu = true;
            SelectButton(winMenu.restartButton);
            player1.SetActive(false);
            player2.SetActive(false);
            GameObject portal = GameObject.FindWithTag("Portal");
            if(portal != null)
                Destroy(portal);

            //Fade back in
            if (fadeAnim != null)
                fadeAnim.Play("ScreenFadeIn");
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
        hs.animator.SetBool("IsDead", false);
        hs.isInvic = false;
    }

    //Set up controls for players
    public void SetupPlayers()
    {
        if(player1 == null)
        {
            player1 = Instantiate(player1Prefab, new Vector3(0, 0, 0), Quaternion.identity);
        }

        player1Spawn = GameObject.FindWithTag("Player1Spawn").GetComponent<Transform>();
        player1.transform.position = player1Spawn.position;
        healthSystem1 = player1.GetComponent<healthSystem>();

        if(!unitTest)
            StartCoroutine(Player1Controls());

        if(playerCount > 1)
        {
            player2Spawn = GameObject.FindWithTag("Player2Spawn").GetComponent<Transform>();
            player2.transform.position = player2Spawn.position;
            healthSystem2 = player2.GetComponent<healthSystem>();

            if(!unitTest)
                StartCoroutine(Player2Controls());
        }
        else if(player2 != null)
            player2.SetActive(false);
    }

    //Set player 1 controls
    public IEnumerator Player1Controls()
    {
        if(!gameOverMenu.gameOverMenu && !winMenu.winMenu)
        {
            Gamepad[] gamepads = Gamepad.all.ToArray();

            if(playerCount > 1 && gamepads.Length > 1)
            {
                player1.GetComponent<PlayerInput>().SwitchCurrentControlScheme("Gamepad", Keyboard.current, Mouse.current, gamepads[1]);
            }
            else if(playerCount < 2 && gamepads.Length > 0)
            {
                player1.GetComponent<PlayerInput>().SwitchCurrentControlScheme("Gamepad", Keyboard.current, Mouse.current, gamepads[0]);
            }
            else
            {
                player1.GetComponent<PlayerInput>().SwitchCurrentControlScheme("Gamepad", Keyboard.current, Mouse.current);
            }
        }

        yield return new WaitForSeconds(1);

        StartCoroutine(Player1Controls());
    }

    //Set player 2 controls
    public IEnumerator Player2Controls()
    {
        if(!gameOverMenu.gameOverMenu && !winMenu.winMenu)
        {
            Gamepad[] gamepads = Gamepad.all.ToArray();

            if(gamepads.Length > 0)
            {
                player2.GetComponent<PlayerInput>().SwitchCurrentControlScheme("Gamepad", gamepads[0]);
            }
            else
                player2.GetComponent<PlayerInput>().SwitchCurrentControlScheme("Touch");
        }

        yield return new WaitForSeconds(1);

        StartCoroutine(Player2Controls());
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

    public void SelectButton(GameObject button)
    {
        if(Gamepad.all.Count > 0)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(button);
        }
    }

    public IEnumerator SelectMenuButton() //Selects a button depending on which menu you are in when using a controller and no buttons are already selected
    {
        if(!unitTest)
        {
            if(Gamepad.all.Count > 0) 
            {
                if(EventSystem.current.currentSelectedGameObject == null)
                {
                    if(pauseMenu.pauseMenu)
                    {
                        EventSystem.current.SetSelectedGameObject(pauseMenu.resumeButton);
                    }
                    else if(leaderboardManager.lbMenu)
                    {
                        EventSystem.current.SetSelectedGameObject(leaderboardManager.menuButton);
                    }
                    else if(winMenu.winMenu)
                    {
                        EventSystem.current.SetSelectedGameObject(winMenu.restartButton);
                    }
                    else if(gameOverMenu.gameOverMenu)
                    {
                        EventSystem.current.SetSelectedGameObject(gameOverMenu.restartButton);
                    }
                    else if(controlMenu.controlMenu)
                    {
                        EventSystem.current.SetSelectedGameObject(controlMenu.backButton);
                    }
                }
            }
            else if(EventSystem.current.currentSelectedGameObject != inputField)
                EventSystem.current.SetSelectedGameObject(null);
                        
            yield return new WaitForSecondsRealtime(1f);
            StopCoroutine(SelectMenuButton());
            StartCoroutine(SelectMenuButton());
        }
    }
}
