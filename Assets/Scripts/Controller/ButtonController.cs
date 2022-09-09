using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Network.Singletons;
using UnityEngine.SceneManagement;
using System;

public class ButtonController : NetworkSingleton<ButtonController>
{
    [SerializeField]
    private Button playerShootButton;

    [SerializeField]
    private Button playerPassButton;

    [SerializeField]
    private Button advisorShootButton;

    [SerializeField]
    private Button advisorPassButton;


    public AudioSource ShootAudio;

    public AudioSource PassAudio;

    public int AdvisorAutoBtnPressed { get; set;}

    private void Update()
    {
        KeyPressed();
    }
    private void KeyPressed()
    {
        if (!GameSettings.ENABLE_SHOOT_PASS_KEYPRESSED || !GameManager.Instance.IsGameStarted)
            return;

        if (Input.GetKeyUp(KeyCode.Space))
        {
            PlayerShoot();
        }
        if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            PlayerPass();
        }
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
            PlayerShoot();
        }
        if (buttonPressed == playerPassButton)
        {
            PlayerPass();
        }
        if (buttonPressed == advisorShootButton)
        {
            AdvisorShoot();
        }
        if (buttonPressed == advisorPassButton)
        {
            AdvisorPass();
        }
    }
    private void PlayerShoot()
    {
        if(ShootAudio != null)
            ShootAudio.Play();

        if (GameManager.Instance.gamestart == false) return;

        // Record Player follow or not follow Shoot advice
        Logger.Instance.advice = AdvisorManager.Instance.adviseTextBox.text;
        Logger.Instance.decision = GameSettings.RED_BUTTON_TEXT;

        //Log Decision and Advice and NPCs
        Logger.Instance.LogGame(GameManager.Instance.NumberOfGames);

        //Destroy Target and change Target
        TargetController.Instance.destroyTarget();
        TargetController.Instance.changeTarget = true;

        //No Advice
        AdvisorManager.Instance.insertAdvise(AdvisorAdvice.NoAdvice);
        AdvisorManager.Instance.updateAdviseClientRpc(AdvisorAdvice.NoAdvice);

        //FOR TESTING
        Logger.Instance.writeToJSON();

        //Reset Logger
        Logger.Instance.resetLogger();

        Debug.Log("Player Shoot Button pressed");
    }
    private void PlayerPass()
    {
        if (PassAudio != null)
            PassAudio.Play();

        if (GameManager.Instance.gamestart == false) return;

        // Record Player follow or not follow Pass advice
        Logger.Instance.advice = AdvisorManager.Instance.adviseTextBox.text;
        Logger.Instance.decision = GameSettings.GREEN_BUTTON_TEXT;

        //Record NPCs
        TargetController.Instance.recordNPCs();

        //Log Decision and Advice and NPCs
        Logger.Instance.LogGame(GameManager.Instance.NumberOfGames);

        //Change Target
        TargetController.Instance.changeTarget = true;

        //No Advice
        AdvisorManager.Instance.insertAdvise(AdvisorAdvice.NoAdvice);
        AdvisorManager.Instance.updateAdviseClientRpc(AdvisorAdvice.NoAdvice);

        //FOR TESTING
        Logger.Instance.writeToJSON();

        //Reset Logger
        Logger.Instance.resetLogger();

        Debug.Log("Player Pass Button pressed");
    }
    //private string AdviceLogger(string playerChoice)
    //{
    //    string currentAdvise = AdvisorManager.Instance.adviseTextBox.text;
    //    if (String.Equals(currentAdvise, playerChoice))
    //        Debug.Log($"Player followed {playerChoice} advice");
    //    else
    //        Debug.Log($"Player did not follow {playerChoice} advice");
    //    return currentAdvise;
    //}
    private void AdvisorShoot()
    {
        if (PassAudio != null)
            PassAudio.Play();

        AdvisorManager.Instance.AutoButtonPressed = false;
        AdvisorManager.Instance.insertAdvise(AdvisorAdvice.Shoot);
        AdvisorManager.Instance.updateAdviseTextServerRpc(AdvisorAdvice.Shoot);
        Debug.Log("Advisor Shoot Button pressed");
    }
    private void AdvisorPass()
    {
        if (PassAudio != null)
            PassAudio.Play();

        AdvisorManager.Instance.AutoButtonPressed = false;
        AdvisorManager.Instance.insertAdvise(AdvisorAdvice.Pass);
        AdvisorManager.Instance.updateAdviseTextServerRpc(AdvisorAdvice.Pass);
        Debug.Log("Advisor Pass Button pressed");

    }
}
