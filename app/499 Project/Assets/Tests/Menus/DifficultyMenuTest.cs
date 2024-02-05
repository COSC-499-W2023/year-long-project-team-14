using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

public class DifficultyMenuTest : MonoBehaviour
{
    [UnitySetUp]
    public IEnumerator Setup()
    {
        SceneManager.LoadScene("Menu");
        yield return null;
    }

    [UnityTest]
    public IEnumerator DifficultyMenu_BackButton()
    {
        SceneManager.LoadScene("Menu");
        yield return null;
        
    }
}
