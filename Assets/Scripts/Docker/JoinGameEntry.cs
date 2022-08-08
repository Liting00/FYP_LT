using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinGameEntry : MonoBehaviour
{
    public Button enterBtn;
    public TextMeshProUGUI enterBtnText;
    public string joinCode;

    private void Start()
    {
        Button btn = enterBtn.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
        Debug.Log("Start Join Game Entry");

        joinCode = RelayManager.Instance.JoinCode;
    }
    private void TaskOnClick()
    {
        Debug.Log("You have clicked the button!");

        EnterCode();
    }
    public void SetJoinCodeData()
    {
        if (joinCode == null || joinCode == "")
            enterBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Empty";
        else
            enterBtn.GetComponentInChildren<TextMeshProUGUI>().text = joinCode;
    }

    private void EnterCode()
    {
        //TODO: Enter the code onto inputTextfield
        //GameObject.Find("InputCodeTextField").GetComponent<InputField>().text = joinCode;
    }
}
