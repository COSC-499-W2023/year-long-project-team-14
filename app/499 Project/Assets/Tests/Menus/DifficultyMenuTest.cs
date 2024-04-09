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
/*
// will add the following tests when the diffulty levels are implemented

    [UnityTest]
    public IEnumerator DifficultyMenu_BackButton()
    {
       // SceneManager.LoadScene("Menu");
      //  yield return null;
        
    }

    [UnityTest]

    public IEnumerator DifficultyMenu_EasyButton()
    {
        //SceneManager.LoadScene("Menu");
        //yield return new WaitForSeconds(1);
        //GameObject.Find("easy").GetComponent<Button>().onClick.Invoke();
        //yield return new WaitForSeconds(1);
        //Assert.AreEqual(SceneManager.GetActiveScene().name, "Game");
        //yield return null;
        
            }

    [UnityTest]
    public IEnumerator DifficultyMenu_MediumButton()
    {
       // SceneManager.LoadScene("Menu");
       // yield return new WaitForSeconds(1);
         // GameObject.Find("medium").GetComponent<Button>().onClick.Invoke();
        // yield return new WaitForSeconds(1);
        // Assert.AreEqual(SceneManager.GetActiveScene().name, "Game");
        // yield return null;


    }

    [UnityTest]
    public IEnumerator DifficultyMenu_HardButton()
    {
        //SceneManager.LoadScene("Menu");
        // yield return new WaitForSeconds(1);
        // GameObject.Find("hard").GetComponent<Button>().onClick.Invoke();
        // yield return new WaitForSeconds(1);
        // Assert.AreEqual(SceneManager.GetActiveScene().name, "Game");
 // yield return null;
    }

    [UnityTest]
    public IEnumerator DifficultyMenu_ExtremeButton()
    {
        //SceneManager.LoadScene("Menu");
        // yield return new WaitForSeconds(1);
        // GameObject.Find("extreme").GetComponent<Button>().onClick.Invoke();
        // yield return new WaitForSeconds(1);
        // Assert.AreEqual(SceneManager.GetActiveScene().name, "Game");
        // yield return null;
    }
*/
}
