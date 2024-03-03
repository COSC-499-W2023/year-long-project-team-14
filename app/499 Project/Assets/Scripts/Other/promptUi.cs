using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class promptUi : MonoBehaviour
{
    [SerializeField] private GameObject image;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI contentText;
    // Start is called before the first frame update
    void Start()
    {
        image.SetActive(false);
    }

    public bool isDisplayed = false;
    public void SetUp(string text1, string text2)
    {
        titleText.text = text1;
        contentText.text = text2;
        image.SetActive(true);
        isDisplayed = true;
    }
    public void Open()
    {
        image.SetActive(true);
        isDisplayed = true;
    }
    public void Close()
    {
        image.SetActive(false);
        isDisplayed = false;
    }
}
