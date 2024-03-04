using Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private GameObject retryView;
        [SerializeField] private GameObject startView;

        [SerializeField] private TextMeshProUGUI coinsCount;
        [SerializeField] private TextMeshProUGUI distanceCount;

        [SerializeField] private Button retryButton;
        [SerializeField] private Button startButton;

        [SerializeField] private GameController controller;
    
        private void Awake()
        {
            retryButton.onClick.AddListener(OnRetryButton);
            startButton.onClick.AddListener(OnStartButton);
        }

        private void Start()
        {
            controller.OnPlayerDie += GameLost;
            controller.PlayerData.onPlayerDataChanges += UpdateUserData;
        }

        private void UpdateUserData()
        {
            coinsCount.text = controller.PlayerData.Coins.ToString();
            distanceCount.text = $"{((int)controller.PlayerData.Distance).ToString()} m";
        }

        private void OnRetryButton()
        {
            SceneManager.LoadScene(0);
        } 
    
        private void OnStartButton()
        {
            controller.ChangePauseState(false);
            retryView.SetActive(false);
            startView.SetActive(false);
        }

        private void GameLost()
        {
            retryView.SetActive(true);
        }
    }
}
