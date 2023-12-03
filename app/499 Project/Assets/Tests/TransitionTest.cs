using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor; 
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TransitionTest : MonoBehaviour
{
    GameMaster gameMaster;
    GameObject pathfinder;
    GameObject port;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        //Reload test scene
        SceneManager.LoadScene("Test");
        yield return null;
    }

    [UnityTest]
    public IEnumerator LadderTransitionTest()
    {
        //Instantiate GameMaster and Pathfinder
        pathfinder = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Pathfinder.prefab")) as GameObject;
        gameMaster = (GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/GameMaster.prefab")) as GameObject).GetComponent<GameMaster>();
        gameMaster.unitTest = true;
        yield return null;

        //Get access to ladder and level
        Ladder ladder = GameObject.FindWithTag("Ladder").GetComponent<Ladder>();
        GameObject level = gameMaster.level;

        //Set player life to 0
        gameMaster.healthSystem1.life = 0;

        //Check that the ladder is locked at the start of the level
        Assert.IsTrue(!ladder.exitUnlocked);

        //Kill all enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for(int i = 0; i < enemies.Length; i++)
            enemies[i].GetComponent<EnemyHealthSystem>().Die();
        yield return null;

        //Check that the ladder is unlocked once all enemies are dead
        Assert.IsTrue(ladder.exitUnlocked);

        //Transition to next level
        gameMaster.LevelComplete();
        yield return null;

        //Check that it transitioned to the next level
        Assert.IsTrue(gameMaster.currentLevel == 2);
        Assert.IsTrue(gameMaster.level != level);

        //Check if player health got reset
        Assert.IsTrue(gameMaster.healthSystem1.life > 0);
    }

    [UnityTest]
    public IEnumerator PortalTransitionTest()
    {
        //Instantiate Portal, GameMaster, and Pathfinder
        port = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Portal.prefab")) as GameObject;
        pathfinder = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Pathfinder.prefab")) as GameObject;
        gameMaster = (GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/GameMaster.prefab")) as GameObject).GetComponent<GameMaster>();
        gameMaster.unitTest = true;
        yield return null;

        //Get access to portal and level
        Portal portal = port.GetComponent<Portal>();
        GameObject level = gameMaster.level;

        //Set player life to 0
        gameMaster.healthSystem1.life = 0;

        //Check that the portal is locked at the start of the level
        Assert.IsTrue(!portal.portalActive);

        //Kill all enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for(int i = 0; i < enemies.Length; i++)
            enemies[i].GetComponent<EnemyHealthSystem>().Die();
        yield return null;

        //Check that the portal is unlocked once all enemies are dead
        Assert.IsTrue(portal.portalActive);

        //Transition to next level
        gameMaster.LevelComplete();
        yield return null;

        //Check that it transitioned to the next level
        Assert.IsTrue(gameMaster.currentLevel == 2);
        Assert.IsTrue(gameMaster.level != level);
        
        //Check if player health got reset
        Assert.IsTrue(gameMaster.healthSystem1.life > 0);
    }

    [UnityTearDown]
    public IEnumerator Teardown()
    {
        //Reload the scene for the next test
        SceneManager.LoadScene("Test");
        yield return null;
    }
}
