using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor; 
using UnityEngine.InputSystem;

public class OrcMovementTest : MonoBehaviour
{
    private GameObject orc;
    private EnemyMovement enemyMovement;

    private GameObject template;
    private GameObject path;

    private GameObject cam;

    [SetUp]
    public void Setup()
    {

        //Spawn in a level with a wall that the enemy has to navigate around
        template = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Levels/TestLevel2.prefab")) as GameObject;

        //This allows the orc to move and avoid obstacles
        path = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Other/Pathfinder.prefab")) as GameObject;

        //Spawn and set up the orc
        orc = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemies/Orc_cyan.prefab"), new Vector3(-10, 0, 0), Quaternion.identity) as GameObject;
        enemyMovement = orc.GetComponent<EnemyMovement>();
        enemyMovement.movementSpeed = 30;
       
    }

    [UnityTest]
    public IEnumerator OrcMoveTest()
    {
        //Set the orcs target position
        enemyMovement.NewTarget(10f, 0f);

        //Keep track of the target position
        Vector2 previousTargetPosition = enemyMovement.targetPosition;

        //Make the orc move immediately
        yield return null;
        enemyMovement.waitTime = 0;

        //Wait for the orc to move through the level to get to the target position
        yield return new WaitUntil(() => orc.transform.position.x > 9);

        //Check if the the orc reached the target position 
        Assert.IsTrue(orc.transform.position.x >= 9); 

        yield return new WaitForSeconds(0.1f);

        //Check that the enemy has a new target position after reaching its previous one
        Assert.IsFalse(enemyMovement.targetPosition == previousTargetPosition);
    }

    [TearDown]
    public void Teardown()
    {
        //Clean up any objects created during the tests
        GameObject.Destroy(orc);
        GameObject.Destroy(template);
        GameObject.Destroy(path);
    }
}
