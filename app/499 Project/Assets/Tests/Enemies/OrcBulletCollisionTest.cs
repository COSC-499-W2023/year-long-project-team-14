using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor; 
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class OrcBulletCollisionTest : MonoBehaviour
{

    private EnemyAttack orcShooter;
    private UnityEngine.Object orcPrefab;
    private GameObject orc;
    private  GameObject[] bullets;
    

    private UnityEngine.Object templatePrefab;
    private GameObject template;

    private GameObject path;


    [SetUp]
    public void Setup()
    {
        //Set up path so the orc can move and shoot without an error from the path finding algo
        path = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Other/Pathfinder.prefab")) as GameObject;

        //spawn and set up the orc
        orcPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemies/Orc_cyan.prefab"); 
        orc = GameObject.Instantiate(orcPrefab) as GameObject;
        orcShooter = orc.GetComponent<EnemyAttack>();
        orcShooter.bulletSpeed = 20;
       
       //Spawn and set up the level template
        templatePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Levels/TestLevel.prefab"); 
        template = GameObject.Instantiate(templatePrefab) as GameObject;

    }


    [UnityTest]
    public IEnumerator orcBulletCTest()
    {
        // Make the orc shoot 
        orcShooter.Shoot(orcShooter.lineRenderer1);

        //Store the bullet in a variable
        bullets = GameObject.FindGameObjectsWithTag("EnemyBullet");

        //Let the bullet travel 
        yield return new WaitForSeconds(0.5f); 

        // Check if the bullet is within the level template.
        Assert.IsTrue(bullets[0].transform.position.y < 6.5 && bullets[0].transform.position.y > -7.5 && bullets[0].transform.position.x < 11.5 && bullets[0].transform.position.x > -11.5); 
        //^ test all walls since the wall prefab uses the same collider, so no matter which wall the bullet collides with it is essentially the same as hitting any other one in essence testing all walls.
    }

    [TearDown]
    public void Teardown()
    {
        // Clean up any objects created during the tests.
        GameObject.Destroy(orc);
        Object.Destroy(orcShooter.gameObject);

        GameObject.Destroy(template);

        for(int i = 0; i < bullets.Length; i++)
            GameObject.Destroy(bullets[i]);

        GameObject.Destroy(path);
    }
}
