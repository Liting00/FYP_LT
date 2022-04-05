using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public Button playerShootButton;
    public Button playerPassButton;
    public Button advisorShootButton;
    public Button advisorPassButton;


    TargetController targetController;
    AdvisorManager advisorManager;
    private void Awake()
    {
        Debug.Log("Button Controller is called");
    }
    private void Start()
    {
        GameObject tController = GameObject.Find("Target Controller");
        targetController = tController.GetComponent<TargetController>();
        GameObject AManager = GameObject.Find("Advisor Manager");
        advisorManager = AManager.GetComponent<AdvisorManager>();
    }
    void OnEnable()
    {
        //Register Button Events
        playerShootButton.onClick.AddListener(() => buttonCallBack(playerShootButton));
        playerPassButton.onClick.AddListener(() => buttonCallBack(playerPassButton));
        advisorShootButton.onClick.AddListener(() => buttonCallBack(advisorShootButton));
        advisorPassButton.onClick.AddListener(() => buttonCallBack(advisorPassButton));
        //showAdvisorButton.onClick.AddListener(() => buttonCallBack(showAdvisorButton));
    }
    private void buttonCallBack(Button buttonPressed)
    {
        if (buttonPressed == playerShootButton)
        {  
            targetController.destroyTarget();
            targetController.changeTarget = true;
            advisorManager.advisorBox.SetActive(false);
            Debug.Log("Player Shoot Button pressed");
        }
        if (buttonPressed == playerPassButton)
        {
            targetController.changeTarget = true;
            advisorManager.advisorBox.SetActive(false);
            Debug.Log("Player Pass Button pressed");
        }
        if (buttonPressed == advisorShootButton)
        {
            advisorManager.insertAdvise("Shoot");
            Debug.Log("Advisor Shoot Button pressed");
        }
        if (buttonPressed == advisorPassButton)
        {
            advisorManager.insertAdvise("Pass");
            Debug.Log("Advisor Pass Button pressed");
        }
        /*if (buttonPressed == showAdvisorButton)
        {
            
            if (advisorManager.advisorBox.activeInHierarchy)
                advisorManager.advisorBox.SetActive(false);
            else
            {
                advisorManager.getAdvise();
                advisorManager.advisorBox.SetActive(true);
            }
            Debug.Log("Show Advisor Button pressed");
        }*/
    }
}
