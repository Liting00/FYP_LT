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
            string advise = AdvisorManager.Instance.Advise(AdvisorManager.AdvisorAdvice.NoAdvice);
            AdvisorManager.Instance.insertAdvise(AdvisorManager.AdvisorAdvice.NoAdvice);
            AdvisorManager.Instance.updateAdviseClientRpc(AdvisorManager.AdvisorAdvice.NoAdvice);
            Debug.Log("Player Shoot Button pressed");
        }
        if (buttonPressed == playerPassButton)
        {
            TargetController.Instance.changeTarget = true;
            string advise = AdvisorManager.Instance.Advise(AdvisorManager.AdvisorAdvice.NoAdvice);
            AdvisorManager.Instance.insertAdvise(AdvisorManager.AdvisorAdvice.NoAdvice);
            AdvisorManager.Instance.updateAdviseClientRpc(AdvisorManager.AdvisorAdvice.NoAdvice);
            Debug.Log("Player Pass Button pressed");
        }
        if (buttonPressed == advisorShootButton)
        {
            string advise = AdvisorManager.Instance.Advise(AdvisorManager.AdvisorAdvice.Shoot);
            AdvisorManager.Instance.insertAdvise(AdvisorManager.AdvisorAdvice.Shoot);
            AdvisorManager.Instance.updateAdviseTextServerRpc(AdvisorManager.AdvisorAdvice.Shoot);
            Debug.Log("Advisor Shoot Button pressed");
        }
        if (buttonPressed == advisorPassButton)
        {
            string advise = AdvisorManager.Instance.Advise(AdvisorManager.AdvisorAdvice.Pass);
            AdvisorManager.Instance.insertAdvise(AdvisorManager.AdvisorAdvice.Pass);
            AdvisorManager.Instance.updateAdviseTextServerRpc(AdvisorManager.AdvisorAdvice.Pass);
            Debug.Log("Advisor Pass Button pressed");
        }
    }
}
