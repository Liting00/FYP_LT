using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinGameEntry : MonoBehaviour
{
    public Button enterBtn;
    public GameObject TextBox;
    public string joinCode { get; set; }

    private void Start()
    {
        Button btn = enterBtn.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
        Debug.Log("Join Game Entry");
    }
    private void TaskOnClick()
    {
        Debug.Log("You have Enter Btn Join Code:" + joinCode);

        //EnterCode();
    }
    public void SetInputField()
    {
        if (joinCode == null || joinCode == "")
            enterBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Empty";
        else
            enterBtn.GetComponentInChildren<TextMeshProUGUI>().text = joinCode;
    }

    private void EnterCode()
    {
        GameObject inputCodeTextField = GameObject.Find("InputCodeTextField");
        Graphic graphic = inputCodeTextField.GetComponent<InputField>().placeholder;
        ((TextMeshProUGUI)graphic).text = joinCode;
        InputField inputField = inputCodeTextField.GetComponent<InputField>();
    }
}
