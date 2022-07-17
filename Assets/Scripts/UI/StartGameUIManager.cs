using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGameUIManager: MonoBehaviour
{
    [SerializeField]
    public Button nextButton;

    [SerializeField]
    public Button backButton;

    [SerializeField]
    public TextMeshProUGUI tutorialMessage;

    [SerializeField]
    public Sprite[] TutorialMsg;

    [SerializeField]
    public Image Image;

    private int nextSceneIndex;
    private int i = 0;

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
            i++;
            // tutorialMessage.text = "Hi";
            Debug.Log("Next Button Pressed");
            if(i >= TutorialMsg.Length)
                SceneManager.LoadScene(nextSceneIndex);
            else
                Image.GetComponent<Image>().sprite = TutorialMsg[i];

        }
        if (buttonPressed == backButton)
        {
            i--;
            // tutorialMessage.text = "Hi";
            Debug.Log("Next Button Pressed");
            Image.GetComponent<Image>().sprite = TutorialMsg[i];
        }
    }
}
