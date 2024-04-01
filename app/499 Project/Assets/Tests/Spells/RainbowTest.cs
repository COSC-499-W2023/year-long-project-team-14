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
        playerController.unitTest = true; //this prevents some code from running in PlayerController that requires user input

        //This is just incase one of the AI is spawned 
        path = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Other/Pathfinder.prefab")) as GameObject;

    }

    [UnityTest]
    public IEnumerator Rainbow()
    {
        
        //Cast the rainbow spell
        spells.RainbowSpell();

        //Store the rng value in rngTest1.
        rngTest1 = spells.rngSP; 

        //Cast the rainbow spell again to store the second rng to ensure that a different spell was cast 
        spells.RainbowSpell();
        
        //Store the rng value in rngTest2.
        rngTest2 = spells.rngSP; 
    
        //Ensure that both of the rng values have been changed from the default.
        if(rngTest1 != 0 && rngTest2 != 0){
            //If they have been changed from the default then test if they are the same value. This is a 1/9 * 1/9 chance of happening. 
            if(rngTest1 != rngTest2){
                // If they are not the same then pass the test as both rng values are not the default and are different from one another.
                Assert.IsTrue(rngTest1 != rngTest2 );
            }else{
               //If the first two rng values are not equal then recast spell to get a different rng value. The odds of this value being the same as the first are a 1/9 * 1/9 * 1/9. Using this strategy has taken most of the randomness out of the test.
                spells.RainbowSpell();
        
                rngTest2 = spells.rngSP; 

                Assert.IsTrue(rngTest1 != rngTest2 ); 
            }
        }else{
            // This will make the test fail as the rng values have not been changed from the default. 
            Assert.IsTrue(rngTest1 != 0 && rngTest2 != 0);
        }

        //Just a default so all code paths return a value
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