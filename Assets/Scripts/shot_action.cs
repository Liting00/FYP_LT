using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shot_action : MonoBehaviour
{
    public Transform[] bullet_spawnpoint;
    public GameObject bullet_prefab;
    public float bullet_speed;
    private bool press;
    private int count = 0;
    private int mod;
    private int random_index;

    public Button shotButton;
    public Button passButton;

    void Start()
    {
    }

    private void Update()
    {

        mod = count % 4;
        Debug.Log("This is the mod value" + mod);
        if (shotButton.GetComponent<Button>())
        {
            Button btn = shotButton.GetComponent<Button>();
            press = true;
            btn.onClick.AddListener(shotButton_click);
        }
        else if (passButton.GetComponent<Button>())
        {
            Button btn = passButton.GetComponent<Button>();
            btn.onClick.AddListener(passButton_click);
        }

    }

    void shotButton_click()
    {
        if (press)
        {
            if (mod == 0)
            {
                var bullet = Instantiate(bullet_prefab, bullet_spawnpoint[0].position, bullet_spawnpoint[0].rotation);
                bullet.GetComponent<Rigidbody>().velocity = bullet_spawnpoint[0].up * bullet_speed;
                press = false;
            }
            else if (mod == 1)
            {
                var bullet = Instantiate(bullet_prefab, bullet_spawnpoint[1].position, bullet_spawnpoint[1].rotation);
                bullet.GetComponent<Rigidbody>().velocity = bullet_spawnpoint[1].up * bullet_speed;
                press = false;
            }
            else if (mod == 2)
            {
                var bullet = Instantiate(bullet_prefab, bullet_spawnpoint[2].position, bullet_spawnpoint[2].rotation);
                bullet.GetComponent<Rigidbody>().velocity = bullet_spawnpoint[2].up * bullet_speed;
                press = false;
            }
            else if (mod == 3)
            {
                var bullet = Instantiate(bullet_prefab, bullet_spawnpoint[3].position, bullet_spawnpoint[3].rotation);
                bullet.GetComponent<Rigidbody>().velocity = bullet_spawnpoint[3].up * bullet_speed;
                press = false;
            }
            count++;
        }
    }

    void passButton_click()
    {
        count++;
    }
}
