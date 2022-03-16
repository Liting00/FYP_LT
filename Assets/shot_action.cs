using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shot_action : MonoBehaviour
{
    public Transform bullet_spawnpoint;
    public GameObject bullet_prefab;
    public float bullet_speed;
    private bool press;

    public Button yourButton;

    void Start()
    {
    }

    private void Update()
    {
        if (yourButton.GetComponent<Button>())
        {
            Button btn = yourButton.GetComponent<Button>();
            press = true;
            btn.onClick.AddListener(TaskOnClick);
        }
    }

    void TaskOnClick()
    {
        if (press)
        {
            var bullet = Instantiate(bullet_prefab, bullet_spawnpoint.position, bullet_spawnpoint.rotation);
            bullet.GetComponent<Rigidbody>().velocity = bullet_spawnpoint.up * bullet_speed;
            press = false;
            Debug.Log("button was press");
       }
    }
}
