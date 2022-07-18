using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartupManager : MonoBehaviour
{
    private int nextSceneIndex;

    [SerializeField]
    private GameObject loadingIcon;

    [SerializeField]
    private TextMeshProUGUI playInGameUI;

    // Start is called before the first frame update
    void Start()
    {
        nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        StartCoroutine(InitialLoad());
    }
    IEnumerator InitialLoad()
    {
        playInGameUI.gameObject.SetActive(false);
        loadingIcon.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        playInGameUI.gameObject.SetActive(true);
        loadingIcon.gameObject.SetActive(false);
        SceneManager.LoadScene(nextSceneIndex);
    }
}
