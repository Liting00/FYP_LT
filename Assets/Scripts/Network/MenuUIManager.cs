using DilmerGames.Core.Singletons;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;

namespace Assets.Scripts.Network
{
    public class MenuUIManager : NetworkSingleton<MenuUIManager>
    {
        public static new MenuUIManager Instance;

        [SerializeField]
        private Button playAsShooterButton;

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
        private TMP_InputField inputCodeText;

        [SerializeField]
        private GameObject loadingIcon;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            mainMenu.SetActive(false);
            advisorMenu.SetActive(false);
            loadingIcon.SetActive(false);

            mainMenu.SetActive(true);
            advisorMenu.SetActive(false);

            playAsShooterButton?.onClick.AddListener(() =>
            {
                StartCoroutine(LoadPlayerAsynchronously());
                LevelManager.Instance.NewGame(LevelManager.PlayerState.Shooter);
                Debug.Log("Play as Player");
            });
            playAsAdvisorButton?.onClick.AddListener(() =>
            {
                mainMenu.SetActive(false);
                advisorMenu.SetActive(true);
            });
            advisorEnterButton?.onClick.AddListener(() =>
            {
                StartCoroutine(LoadAdvisorAsynchronously());
                LevelManager.Instance.NewGame(LevelManager.PlayerState.Advisor);
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
                Debug.Log("Shooter Host could not be Started...");
            }
        }
        private async Task startClientAsync()
        {
            if (RelayManager.Instance.isRelayEnabled && !string.IsNullOrEmpty(inputCodeText.text))
            {
                await RelayManager.Instance.JoinRelay(inputCodeText.text);
            }
            else
            {
                Debug.Log("Empty Input Code. Client could not be Started");
                return;
            }

            if (NetworkManager.Singleton.StartClient())
            {
                Debug.Log("Client Started...");
            }
            else
            {
                Debug.Log("Advisor Client could not be Started...");
            }
        }
        IEnumerator LoadPlayerAsynchronously()
        {
            loadingIcon.SetActive(true);
            Task task = startHostAsync();
            while (!task.IsCompleted)
            {
                Debug.Log(task.Status);

                yield return null;
            }
            loadingIcon.SetActive(false);
        }
        IEnumerator LoadAdvisorAsynchronously()
        {
            loadingIcon.SetActive(true);
            Task task = startClientAsync();
            while (!task.IsCompleted)
            {
                Debug.Log(task.Status);

                yield return null;
            }
            loadingIcon.SetActive(false);
        }
    }
}