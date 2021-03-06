using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGameUIManager: MonoBehaviour
{
    [SerializeField]
    private Button nextButton;

    [SerializeField]
    private Button backButton;

    [SerializeField]
    private TextMeshProUGUI tutorialMessage;

    [SerializeField]
    private Sprite[] TutorialMsg;

    [SerializeField]
    private Sprite BackgroundSprite;

    [SerializeField]
    private Image Image;

    [SerializeField]
    private Image BackgroundImage;

    private int nextSceneIndex;
    private int i = 0;

    private void Start()
    {
        nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        //BackgroundImage.GetComponent<Image>().sprite = BackgroundSprite;
        Image.GetComponent<Image>().sprite = TutorialMsg[i];

        backButton.gameObject.SetActive(false);
        Image.gameObject.SetActive(true);
        //BackgroundImage.gameObject.SetActive(true);
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
            backButton.gameObject.SetActive(true);

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

            if(i==0)
                backButton.gameObject.SetActive(false);
        }
    }
}
