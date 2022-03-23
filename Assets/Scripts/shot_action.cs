using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shot_action : MonoBehaviour
{
    public Button shotButton;
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
    }

    void shotButton_click()
    {
        if (press)
        {
            Destroy(GameObject.FindWithTag("button"));
            Debug.Log("Button pressed");
        }
    }

}
