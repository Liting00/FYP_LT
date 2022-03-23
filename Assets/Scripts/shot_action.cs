using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shot_action : MonoBehaviour
{
    public Button shotButton;
    public Button passButton;
    private bool press;
    void Start()
    {
    }

    private void Update()
    {
        if (shotButton.GetComponent<Button>())
        {
            Button btn = shotButton.GetComponent<Button>();
            press = true;
            btn.onClick.AddListener(shotButton_click);
        }
        if (shotButton.GetComponent<Button>())
        {
            Button btn = shotButton.GetComponent<Button>();
            press = true;
            btn.onClick.AddListener(passButton_click);
        }
    }

    void shotButton_click()
    {
        if (press)
        {
            GameObject tController = GameObject.Find("TargetController");
            TargetController targetController = tController.GetComponent<TargetController>();
            Destroy(GameObject.FindWithTag("HostileNPC"));
            Debug.Log("Button pressed");
        }
    }
    void passButton_click()
    {
        if (press)
        {
            GameObject tController = GameObject.Find("TargetController");
            TargetController targetController = tController.GetComponent<TargetController>();
            targetController.changeTarget = true;
            Debug.Log("Button pressed");
        }
    }

}
