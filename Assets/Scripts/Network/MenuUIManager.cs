using DilmerGames.Core.Singletons;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

namespace Assets.Scripts.Network
{
    public class MenuUIManager : NetworkSingleton<MenuUIManager>
    {
        [SerializeField]
        private Button playAsPlayerButton;

        [SerializeField]
        private Button playAsAdvisorButton;

        [SerializeField]
        private GameObject advisorMenu;

        [SerializeField]
        private GameObject mainMenu;

        [SerializeField]
        private Button advisorEnterButton;

        [SerializeField]
        private Button advisorBackButton;

        [SerializeField]
        private GameObject loadingIcon;

        private void Start()
        {
            mainMenu.SetActive(false);
            advisorMenu.SetActive(false);
            loadingIcon.SetActive(false);

            StartCoroutine(LoadAsynchronously());

            mainMenu.SetActive(true);
            advisorMenu.SetActive(false);

            playAsPlayerButton?.onClick.AddListener(() =>
            {
                // start Host
            });
            playAsAdvisorButton?.onClick.AddListener(() =>
            {
                mainMenu.SetActive(false);
                advisorMenu.SetActive(true);
            });
            advisorEnterButton?.onClick.AddListener(() =>
            {
                //Loading Scene
            });
            advisorBackButton?.onClick.AddListener(() =>
            {
                mainMenu.SetActive(true);
                advisorMenu.SetActive(false);
            });
        }
        private async Task startHostAsync()
        {
            if (RelayManager.Instance.isRelayEnabled)
            {
                await RelayManager.Instance.SetupRelay();
            }

            if (NetworkManager.Singleton.StartHost())
            {
                Debug.Log("Host Started...");
                return;
            }
            else
            {
                Debug.Log("Host could not be Started...");
            }
        }
        IEnumerator LoadAsynchronously()
        {
            loadingIcon.SetActive(true);
            Task task = startHostAsync();

            while (!task.IsCompleted)
            {
                loadingIcon.SetActive(false);
                yield return task;
            }
        }
    }
}