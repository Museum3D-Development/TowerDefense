using System.Collections.Generic;
using Common;
using Loading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    [HideInInspector]
    public static int StartLevelNumber = 1;

    [SerializeField]
    private Button _startLevelGameBtn1;

    [SerializeField]
    private Button _startLevelGameBtn2;

    [SerializeField]
    private Button _startLevelGameBtn3;

    [SerializeField]
    private Button _startLevelGameBtn4;

    [SerializeField]
    private Button _startLevelGameBtn5;

    [SerializeField]
    private Button _startLevelGameBtn6;

    [SerializeField]
    private Button _exitBtn;

    private void Start()
    {
        _startLevelGameBtn1.onClick.AddListener(OnStartGameBtnClicked);
        _startLevelGameBtn2.onClick.AddListener(OnStartGameBtnClicked);
        _startLevelGameBtn3.onClick.AddListener(OnStartGameBtnClicked);
        _startLevelGameBtn4.onClick.AddListener(OnStartGameBtnClicked);
        _startLevelGameBtn5.onClick.AddListener(OnStartGameBtnClicked);
        _startLevelGameBtn6.onClick.AddListener(OnStartGameBtnClicked);
        _exitBtn.onClick.AddListener(OnExitGameBtnClicked);
    }

    public void GetLevelNumber(int levelNumber)
    {
        StartLevelNumber = levelNumber;
    }

    private void OnStartGameBtnClicked()
    {
        var loadingOperations = new Queue<ILoadingOperation>();
        loadingOperations.Enqueue(new GameLoadingOperation());
        LoadingScreen.Instance.Load(loadingOperations);
    }

    private void OnExitGameBtnClicked()
    {
        var loadingOperations = new Queue<ILoadingOperation>();
        loadingOperations.Enqueue(new MenuLoadingOperation());
        LoadingScreen.Instance.Load(loadingOperations);
    }
}
