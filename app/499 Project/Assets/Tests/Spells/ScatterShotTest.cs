using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ScatterShotTest
{
    private PlayerController playerController;
    private UnityEngine.Object playerPrefab;
    private GameObject player;
    private Spells spells;
    private EnemyHealthSystem enemyHealthSystem;


    private UnityEngine.Object templatePrefab;
    private GameObject template;

    private GameObject path;

    private UnityEngine.Object orcPrefab;
    private GameObject orc;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        SceneManager.LoadScene("Test");
        yield return null;

        //spawn and set up the player
        playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Players/Mage_player1.prefab");
        player = GameObject.Instantiate(playerPrefab) as GameObject;

        //Spawn and set up the level template
        templatePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Levels/TestLevel.prefab");
        template = GameObject.Instantiate(templatePrefab) as GameObject;

        // Set up the player controller
        playerController = player.GetComponent<PlayerController>();
        playerController.unitTest = true;
        spells = player.GetComponent<Spells>();
        spells.spellName = "ScatterShot";


        //Set up path so the orc can move and shoot without an error from the path finding algo
        path = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Other/Pathfinder.prefab")) as GameObject;

        //set up and spawn the orc 
        orcPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemies/Orc_cyan.prefab");
        orc = GameObject.Instantiate(orcPrefab, new Vector3(3, 0, 0), Quaternion.identity) as GameObject;
        //restrict the orc from moving 
        orc.GetComponent<EnemyMovement>().movementSpeed = 0;
        enemyHealthSystem = orc.GetComponent<EnemyHealthSystem>();

    }

    [UnityTest]
    public IEnumerator shootScatterShot()
    {
        //Get enemy health before getting hit by scatter shot
        int health = enemyHealthSystem.enemyHealth;

        //Shoot a scatter shot
        spells.scatterShot();

        //Wait for scatter shot to hit enemy
        yield return new WaitForSeconds(0.5f);

        //Check that the enemy took damage from the scatter shot
        Assert.IsTrue(enemyHealthSystem.enemyHealth < health);
    }


    [TearDown]
    public void Teardown()
    {
        // Clean up any objects created during the tests.
        GameObject.Destroy(player);
        Object.Destroy(playerController.gameObject);

        GameObject.Destroy(orc);

        GameObject.Destroy(template);

    }

}
