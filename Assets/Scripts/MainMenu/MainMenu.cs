using System.Collections.Generic;
using Loading;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private Button _startGameBtn;

        [SerializeField]
        private Button _vkLinkBtn;

        [SerializeField]
        private Button _exitGameBtn;

        private void Start()
        {
            _startGameBtn.onClick.AddListener(OnStartGameBtnClicked);
            _vkLinkBtn.onClick.AddListener(OnVkLinkBtnClicked);
            _exitGameBtn.onClick.AddListener(OnExitGameBtnClicked);
        }

        private void OnStartGameBtnClicked()
        {
            var loadingOperations = new Queue<ILoadingOperation>();
            loadingOperations.Enqueue(new GameLoadingOperation());
            LoadingScreen.Instance.Load(loadingOperations);
        }

        private void OnVkLinkBtnClicked()
        {
            Application.OpenURL("https://vk.com/museum_cdtt");
        }

        private void OnExitGameBtnClicked()
        {
            Application.Quit();
        }
    }
}