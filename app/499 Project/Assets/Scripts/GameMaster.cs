using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameMaster : MonoBehaviour
{
    public Animator fadeAnim;
    public int playerCount = 1;
    public int player1ControlScheme = 0;
    public int player2ControlScheme = 1;

    public int currentLevel = 1;

    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;
    public GameObject player1Prefab;
    public GameObject player2Prefab;

    public GameObject levelTemplate;
    public GameObject[] levels;
    private Transform player1Spawn;
    private Transform player2Spawn;

    public GameObject level;

    void Start()
    {
        playerCount = PlayerPrefs.GetInt("playerCount");
        SetupPlayers();
    }

    public void LevelComplete()
    {
        StartCoroutine(NextLevel());
    }
    public IEnumerator NextLevel()
    {
        //wait for screen to fade out and then destroy current level
        fadeAnim.Play("ScreenFadeOut");
        yield return new WaitForSecondsRealtime(0.5f);
        Destroy(level);
        fadeAnim.Play("ScreenFadeIn");
        yield return new WaitForSecondsRealtime(0.1f);
        

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

            //TODO:

            //revive player if dead

            //maybe give players a life at the end of each level unless they already have 3

            //update the level counter UI
        }
        else
        {
            //TODO:
            
            //display win screen and end game

            print("YOU WIN!!!");
        }
    }

    public void SetupPlayers()
    {
        Gamepad[] gamepads = Gamepad.all.ToArray();

        player1Spawn = GameObject.FindWithTag("Player1Spawn").GetComponent<Transform>();
        player1 = Instantiate(player1Prefab, player1Spawn.position, Quaternion.identity);

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
   
}
