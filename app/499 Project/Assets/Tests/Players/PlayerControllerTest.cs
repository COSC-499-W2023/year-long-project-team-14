using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;
using UnityEngine.InputSystem;

public class PlayerControllerTests
{
    private PlayerController playerController;
    private GameObject playerObject;

    [SetUp]
    public void Setup()
    {
        //spawn and set up the player
        playerObject = new GameObject();
        playerController = playerObject.AddComponent<PlayerController>();
        
    }

    [UnityTest]
    public IEnumerator MoveDirection()
    {
        // Simulate move input.
        playerController.SetMoveDirection(Vector2.up);

        // Check if moveDirection is set correctly.
        Assert.AreEqual(playerController.GetMoveDirection(), Vector2.up);

        yield return null;
    }

    [UnityTest]
    public IEnumerator AimDirection()
    {
        // Simulate aim input.
        playerController.SetAimDirection(Vector2.right);

        // Check if aimDirection is set correctly.
        Assert.AreEqual(playerController.GetAimDirection(), Vector2.right);

        yield return null;
    }

    [TearDown]
    public void Teardown()
    {
        // Clean up any objects created during the tests.
        Object.Destroy(playerController.gameObject);
        GameObject.Destroy(playerObject);
    }
}
