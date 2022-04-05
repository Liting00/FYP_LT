using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public Button shootButton;
    public Button passButton;
    public Button showAdvisorButton;


    TargetController targetController;
    AdvisorManager advisorManager;

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
        shootButton.onClick.AddListener(() => buttonCallBack(shootButton));
        passButton.onClick.AddListener(() => buttonCallBack(passButton));
        //showAdvisorButton.onClick.AddListener(() => buttonCallBack(showAdvisorButton));
    }
    private void buttonCallBack(Button buttonPressed)
    {
        if (buttonPressed == shootButton)
        {  
            targetController.destroyTarget();
            targetController.changeTarget = true;
            advisorManager.advisorBox.SetActive(false);
            Debug.Log("Shoot Button pressed");
        }
        if (buttonPressed == passButton)
        {
            targetController.changeTarget = true;
            advisorManager.advisorBox.SetActive(false);
            Debug.Log("Pass Button pressed");
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
