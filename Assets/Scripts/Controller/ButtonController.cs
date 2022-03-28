using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public Button shootButton;
    public Button passButton;
    public Button showAdvisorButton;

    [SerializeField] public GameObject textBox;

    void OnEnable()
    {
        //Register Button Events
        shootButton.onClick.AddListener(() => buttonCallBack(shootButton));
        passButton.onClick.AddListener(() => buttonCallBack(passButton));
        showAdvisorButton.onClick.AddListener(() => buttonCallBack(showAdvisorButton));
    }
    private void buttonCallBack(Button buttonPressed)
    {
        if (buttonPressed == shootButton)
        {
            GameObject tController = GameObject.Find("Target Controller");
            TargetController targetController = tController.GetComponent<TargetController>();
            targetController.destroyTarget();
            targetController.changeTarget = true;
            Debug.Log("Shoot Button pressed");
        }
        if (buttonPressed == passButton)
        {
            GameObject tController = GameObject.Find("Target Controller");
            TargetController targetController = tController.GetComponent<TargetController>();
            targetController.changeTarget = true;
            Debug.Log("Pass Button pressed");
        }
        if (buttonPressed == showAdvisorButton)
        {
            if (textBox.activeInHierarchy)
                textBox.SetActive(false);
            else
                textBox.SetActive(true);
            Debug.Log("Show Advisor Button pressed");
        }
    }
}
