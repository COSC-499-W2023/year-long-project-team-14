using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor; 
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TransitionTest
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
        pathfinder = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Other/Pathfinder.prefab")) as GameObject;
        gameMaster = (GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Other/GameMaster.prefab")) as GameObject).GetComponent<GameMaster>();
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
        port = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Interactables/Portal.prefab")) as GameObject;
        pathfinder = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Other/Pathfinder.prefab")) as GameObject;
        gameMaster = (GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Other/GameMaster.prefab")) as GameObject).GetComponent<GameMaster>();
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

    [UnityTest]
    public IEnumerator ShopTransitionTest()
    {
        //Load into game scene
        SceneManager.LoadScene("GameScene");
        yield return null;

        //Get the game master
        gameMaster = GameObject.FindWithTag("GameMaster").GetComponent<GameMaster>();
        gameMaster.unitTest = true;
        yield return null;

        //Get access to ladder and level
        Ladder ladder = GameObject.FindWithTag("Ladder").GetComponent<Ladder>();
        GameObject level = gameMaster.level;

        //Set level to 4
        gameMaster.currentLevel = 4;

        //Set player life to 1
        gameMaster.healthSystem1.life = 1;

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
        yield return new WaitForSeconds(0.6f);

        //Check that player health did not increase
        Assert.IsTrue(gameMaster.healthSystem1.life == 1);

        //Check that it transitioned to the shop and the level counter didn't increase
        Assert.IsTrue(gameMaster.currentLevel == 4);
        Assert.IsTrue(gameMaster.level != level);
        Assert.IsTrue(gameMaster.inShop == true);

        //Get a list of all 3 spells that spawned in
        GameObject spell = null;
        List<GameObject> spells = new List<GameObject>();

        spell = GameObject.FindWithTag("Fireball");
        if(spell != null)
            spells.Add(spell);
        spell = null;

        spell = GameObject.FindWithTag("SeekingOrb");
        if(spell != null)
            spells.Add(spell);
        spell = null;

        spell = GameObject.FindWithTag("Freeze");
        if(spell != null)
            spells.Add(spell);
        spell = null;

        spell = GameObject.FindWithTag("lightning");
        if(spell != null)
            spells.Add(spell);
        spell = null;

        spell = GameObject.FindWithTag("Shield");
        if(spell != null)
            spells.Add(spell);
        spell = null;

        GameObject[] s = GameObject.FindGameObjectsWithTag("summonChad");
        for(int i = 0; i < s.Length; i++)
            spells.Add(s[i]);
        spell = null;

        Debug.Log(spells.Count);

        // spell = GameObject.FindWithTag("summonChad");
        // if(spell != null)
        //     spells.Add(spell);
        // spell = null;

        // spell = GameObject.FindWithTag("RageMage");
        // if(spell != null)
        //     spells.Add(spell);
        // spell = null;

        // spell = GameObject.FindWithTag("Rainbow");
        // if(spell != null)
        //     spells.Add(spell);
        // spell = null;

        // spell = GameObject.FindWithTag("ScatterShot");
        // if(spell != null)
        //     spells.Add(spell);
        // spell = null;

        // spell = GameObject.FindWithTag("Laser");
        // if(spell != null)
        //     spells.Add(spell);
        // spell = null;

        //Check that there are 3 spells
        Assert.IsTrue(spells.Count == 3);

        //Check that they arent the same
        Assert.IsTrue(spells[0] != spells[1]);
        Assert.IsTrue(spells[0] != spells[2]);
        Assert.IsTrue(spells[2] != spells[1]);

        //Transition to next level
        gameMaster.LevelComplete();
        yield return null;
        yield return new WaitForSeconds(0.5f);

        //Check that player health increased
        Assert.IsTrue(gameMaster.healthSystem1.life == 2);

        //Check that it transitioned out of the shop and the level counter increased
        Assert.IsTrue(gameMaster.currentLevel == 5);
        Assert.IsTrue(gameMaster.level != level);
        Assert.IsTrue(gameMaster.inShop == false);
    }

    [UnityTearDown]
    public IEnumerator Teardown()
    {
        //Reload the scene for the next test
        SceneManager.LoadScene("Test");
        yield return null;
    }
}
