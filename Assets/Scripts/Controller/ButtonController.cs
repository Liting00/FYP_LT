using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    [SerializeField]
    private Button playerShootButton;

    [SerializeField]
    private Button playerPassButton;

    [SerializeField]
    private Button advisorShootButton;

    [SerializeField]
    private Button advisorPassButton;

    private void Awake()
    {
        Debug.Log("Button Controller is called");
    }
    private void Start()
    {
        //GameObject AManager = GameObject.Find("Advisor Manager");
    }
    void OnEnable()
    {
        //Register Button Events
        playerShootButton.onClick.AddListener(() => buttonCallBack(playerShootButton));
        playerPassButton.onClick.AddListener(() => buttonCallBack(playerPassButton));
        advisorShootButton.onClick.AddListener(() => buttonCallBack(advisorShootButton));
        advisorPassButton.onClick.AddListener(() => buttonCallBack(advisorPassButton));
    }
    private void buttonCallBack(Button buttonPressed)
    {
        if (buttonPressed == playerShootButton)
        {
            TargetController.Instance.destroyTarget();
            TargetController.Instance.changeTarget = true;
            //string advise = AdvisorManager.Instance.Advise(AdvisorAdvice.NoAdvice);
            AdvisorManager.Instance.insertAdvise(AdvisorAdvice.NoAdvice);
            AdvisorManager.Instance.updateAdviseClientRpc(AdvisorAdvice.NoAdvice);
            Debug.Log("Player Shoot Button pressed");
        }
        if (buttonPressed == playerPassButton)
        {
            TargetController.Instance.changeTarget = true;
            AdvisorManager.Instance.insertAdvise(AdvisorAdvice.NoAdvice);
            AdvisorManager.Instance.updateAdviseClientRpc(AdvisorAdvice.NoAdvice);
            Debug.Log("Player Pass Button pressed");
        }
        if (buttonPressed == advisorShootButton)
        {
            AdvisorManager.Instance.insertAdvise(AdvisorAdvice.Shoot);
            AdvisorManager.Instance.updateAdviseTextServerRpc(AdvisorAdvice.Shoot);
            Debug.Log("Advisor Shoot Button pressed");
        }
        if (buttonPressed == advisorPassButton)
        {
            AdvisorManager.Instance.insertAdvise(AdvisorAdvice.Pass);
            AdvisorManager.Instance.updateAdviseTextServerRpc(AdvisorAdvice.Pass);
            Debug.Log("Advisor Pass Button pressed");
        }
    }
}
