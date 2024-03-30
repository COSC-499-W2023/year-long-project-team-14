using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor; 
using UnityEngine.SceneManagement;

public class RainbowTest
{
    private GameObject player;
    private PlayerController playerController;
    private Spells spells;
    private GameObject template;
    private SpriteRenderer spriteRenderer;
    private int rngTest1 = 0;
    private int rngTest2 = 0;
    private GameObject path;


    [UnitySetUp]
    public IEnumerator Setup()
    {
        //Load into test scene
        SceneManager.LoadScene("Test");
        yield return null;

        //Spawn in the level template 
        template = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Levels/TestLevel.prefab")) as GameObject;
    
        //Spawn and set up the player
        UnityEngine.Object playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Players/Mage_player1.prefab"); 
        player = GameObject.Instantiate(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity) as GameObject;
        playerController = player.GetComponent<PlayerController>();
        spells = player.GetComponent<Spells>();
        // Below will be used to check that the sprite changes.
        spriteRenderer = player.GetComponent<SpriteRenderer>();
        playerController.unitTest = true; //this prevents some code from running in PlayerController that requires user input

        //This is just incase one of the AI is spawned 
        path = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Other/Pathfinder.prefab")) as GameObject;

    }

    [UnityTest]
    public IEnumerator Rainbow()
    {
        
        //Cast the rainbow spell
        spells.RainbowSpell();

        rngTest1 = spells.rngSP; 

        //Cast the rainbow spell again to store the second rng to ensure that a different spell was cast 
        spells.RainbowSpell();

        rngTest2 = spells.rngSP; 
    
        //Ensure both of the rng values have been changed
        Assert.IsTrue(rngTest1 != 0 && rngTest2 != 0);

        //Check that the first and second rng values are not the same meaning a different spell was cast. If they are than see below 
        Assert.IsTrue(rngTest1 != rngTest2 );  
        
        //Recast the rainbow spell to get a different rng value (meaning different spell used).
        spells.RainbowSpell();
        
        rngTest2 = spells.rngSP; 

        //Check that the first rng value is not the same as the third rng value now (as the first two rng vlaues were the same). 
        Assert.IsTrue(rngTest1 != rngTest2 ); 

        yield return null;
    }

    [UnityTearDown]
    public IEnumerator Teardown()
    {
        //Load into test scene for the next test
        SceneManager.LoadScene("Test");
        yield return null;
    }
}