using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class OptionsMenuTest
{
    [UnityTest]
    public IEnumerator OptionsMenuTest_BackToPauseMenu()
    {
        var gameObject = new GameObject();
        var optionsMenu = gameObject.AddComponent<OptionsMenu>();
        var pauseMenu = new GameObject();
        optionsMenu.pauseMenu = pauseMenu;

        optionsMenu.Back();

        Assert.IsFalse(optionsMenu.gameObject.activeSelf);
        Assert.IsTrue(pauseMenu.activeSelf);
        yield return null;
    }
}

    
     /* working on here
     
     [UnityTest] 
     
     public IEnumerator OptionsMenu_SetMusicVolume()
    {

        var gameObject = new GameObject();
        var slider = gameObject.AddComponent<Slider>();
        var optionsMenu = gameObject.AddComponent<OptionsMenu>();

        
 if (musicVolumeSlider != null)
        {
            musicVolumeSlider.value = volume;
        }
        else
        {
            Debug.LogError("musicVolumeSlider is not initialized in OptionsMenu.");
        }
        var volume = 0.5f;

        optionsMenu.SetMusicVolume(volume);

        Assert.AreEqual(volume, slider.value);
        yield return null;
    }/*/



