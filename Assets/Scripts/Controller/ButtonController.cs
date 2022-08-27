using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Network.Singletons;
using UnityEngine.SceneManagement;

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

    [SerializeField]
    private Button advisorAutoButton;

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
        advisorAutoButton.onClick.AddListener(() => buttonCallBack(advisorAutoButton));
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
        if (buttonPressed == advisorAutoButton)
        {
            AdvisorAuto();
        }
    }
    private void PlayerShoot()
    {
        if(ShootAudio != null)
            ShootAudio.Play();

        TargetController.Instance.destroyTarget();
        TargetController.Instance.changeTarget = true;
        //string advise = AdvisorManager.Instance.Advise(AdvisorAdvice.NoAdvice);
        AdvisorManager.Instance.insertAdvise(AdvisorAdvice.NoAdvice);
        AdvisorManager.Instance.updateAdviseClientRpc(AdvisorAdvice.NoAdvice);
        if (AdvisorManager.Instance.adviseTextBox.text == AdvisorManager.Instance.Advise(AdvisorAdvice.Shoot))
        {
            Debug.Log("Player followed shoot advice");
        }
        Debug.Log("Player Shoot Button pressed");
    }
    private void PlayerPass()
    {
        if (PassAudio != null)
            PassAudio.Play();

        TargetController.Instance.changeTarget = true;
        AdvisorManager.Instance.insertAdvise(AdvisorAdvice.NoAdvice);
        AdvisorManager.Instance.updateAdviseClientRpc(AdvisorAdvice.NoAdvice);
        Debug.Log("Player Pass Button pressed");
    }
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

    private void AdvisorAuto()
    {
        if (PassAudio != null)
            PassAudio.Play();

        AdvisorAutoBtnPressed += 1;
        AdvisorManager.Instance.AutoButtonPressed = true;
        Debug.Log("Advisor Auto Button pressed" + AdvisorAutoBtnPressed);
    }
}
