using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameMaster : MonoBehaviour
{
    public int playerCount = 1;
    public int player1ControlScheme = 0;
    public int player2ControlScheme = 1;

    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;
    public GameObject player1Prefab;
    public GameObject player2Prefab;

    public GameObject levelTemplate;
    private Transform player1Spawn;
    private Transform player2Spawn;

    GameObject level;

    void Start()
    {
        playerCount = PlayerPrefs.GetInt("playerCount");
        print(playerCount);
        Setup();
    }

    public void Setup()
    {
        level = Instantiate(levelTemplate, transform.position, Quaternion.identity);

        SetupPlayers();
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
                if(player1ControlScheme == 0)
                    player2.GetComponent<PlayerInput>().SwitchCurrentControlScheme("Gamepad", gamepads[0]);
                else if(gamepads.Length > 1)
                    player2.GetComponent<PlayerInput>().SwitchCurrentControlScheme("Gamepad", gamepads[1]);
            }
        }
    }
   
}
