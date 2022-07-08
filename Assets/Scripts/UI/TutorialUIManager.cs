using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialUIManager : MonoBehaviour
{
    [SerializeField]
    public Button nextButton;

    [SerializeField]
    public Button backButton;

    [SerializeField]
    public TextMeshProUGUI tutorialMessage;

    private int nextSceneIndex;

    private void Start()
    {
        nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
    }
    void OnEnable()
    {
        //Register Button Events
        nextButton.onClick.AddListener(() => buttonCallBack(nextButton));
        backButton.onClick.AddListener(() => buttonCallBack(backButton));
    }
    private void buttonCallBack(Button buttonPressed)
    {
        if (buttonPressed == nextButton)
        {
            tutorialMessage.text = "Hi";
            Debug.Log("Next Button Pressed");
            SceneManager.LoadScene(nextSceneIndex);

        }
    }
}
