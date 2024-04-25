using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Building;
using Core.UI;
using GameResult;
using Loading;
using UnityEngine;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    [SerializeField]
    private List<Vector2Int> _boardSizeLevel = new List<Vector2Int>();

    //[SerializeField]
    private Vector2Int _boardSize;

    [SerializeField]
    private GameBoard _gameBoard;

    [SerializeField]
    private DefenderHud _defenderHud;

    [SerializeField]
    private TilesBuilder _tilesBuilder;

    [SerializeField]
    private GameResultWindow _gameResultWindow;

    [SerializeField]
    private PrepareGamePanel _prepareGamePanel;

    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private GameTileContentFactory _contentFactory;

    [SerializeField]
    private WarFactory _warFactory;

    [SerializeField]
    private EnemyFactory _enemyFactory;

    [SerializeField]
    private List<GameScenario> _scenarios = new List<GameScenario>();

    [SerializeField, Range(0, 100)]
    private List<int> _startingPlayerHealthsLevel = new List<int>();

    [SerializeField, Range(5f, 40f)]
    private List <float> _prepareTimesLevel = new List<float>();

    private int _startingPlayerHealth;
    private float _prepareTime;

    private bool _scenarioInProcess;
    private GameScenario _scenario;
    private GameScenario.State _activeScenario;
    private CancellationTokenSource _prepareCancellation;

    private readonly GameBehaviorCollection _enemies = new GameBehaviorCollection();
    private readonly GameBehaviorCollection _nonEnemies = new GameBehaviorCollection();

    private static Game _instance;

    private int _playerHealth;
    private int PlayerHealth
    {
        get => _playerHealth;
        set
        {
            _playerHealth = Mathf.Max(0, value);
            _defenderHud.UpdatePlayerHealth(_playerHealth, _startingPlayerHealth);
        }
    }

    public IEnumerable<GameObjectFactory> Factories => new GameObjectFactory[3]{_contentFactory,
        _warFactory, _enemyFactory};

    private void OnEnable()
    {
        _instance = this;
    }

    private void Awake()
    {
        ActiveConfigLevel();
    }

    private void ActiveConfigLevel()
    {
        int currentLevel = LevelMenu.StartLevelNumber;
        //switch (currentLevel)
        //{
        //    case 1:
        //        _boardSize = _boardSizeLevel[0];
        //        _prepareTime = _prepareTimesLevel[0];
        //        _scenario = _scenarios[0];
        //        break;
        //    case 2:
        //        _boardSize = _boardSizeLevel[1];
        //        _prepareTime = _prepareTimesLevel[1];
        //        _scenario = _scenarios[1];
        //        break;
        //    case 3:
        //        _boardSize = _boardSizeLevel[2];
        //        _prepareTime = _prepareTimesLevel[2];
        //        _scenario = _scenarios[2];
        //        break;
        //    case 4:
        //        _boardSize = _boardSizeLevel[3];
        //        _prepareTime = _prepareTimesLevel[3];
        //        _scenario = _scenarios[3];
        //        break;
        //    case 5:
        //        _boardSize = _boardSizeLevel[4];
        //        _prepareTime = _prepareTimesLevel[4];
        //        _scenario = _scenarios[4];
        //        break;
        //    case 6:
        //        _boardSize = _boardSizeLevel[5];
        //        _prepareTime = _prepareTimesLevel[5];
        //        _scenario = _scenarios[5];
        //        break;
        //}

        _boardSize = _boardSizeLevel[currentLevel - 1];
        _startingPlayerHealth = _startingPlayerHealthsLevel[currentLevel - 1];
        _prepareTime = _prepareTimesLevel[currentLevel - 1];
        _scenario = _scenarios[currentLevel - 1];
    }

    private void Start()
    {
        _defenderHud.PauseClicked += OnPauseClicked;
        _defenderHud.QuitGame += GoToMainMenu;
        _gameBoard.Initialize(_boardSize, _contentFactory);
        _tilesBuilder.Initialize(_contentFactory, _camera, _gameBoard);
        BeginNewGame();
    }

    private void OnPauseClicked(bool isPaused)
    {
        Time.timeScale = isPaused ? 0f : 1f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            BeginNewGame();
        }

        if (_scenarioInProcess)
        {
            var waves = _activeScenario.GetWaves();
            _defenderHud.UpdateScenarioWaves(waves.currentWave, waves.wavesCount);
            if (PlayerHealth <= 0)
            {
                _scenarioInProcess = false;
                _gameResultWindow.Show(GameResultType.Defeat, BeginNewGame, GoToMainMenu);
            }
            if (_activeScenario.Progress() == false && _enemies.IsEmpty)
            {
                _scenarioInProcess = false;
                _gameResultWindow.Show(GameResultType.Victory, BeginNewGame, GoToMainMenu);
            }
        }

        _enemies.GameUpdate();
        Physics.SyncTransforms();
        _gameBoard.GameUpdate();
        _nonEnemies.GameUpdate();
    }

    public static void SpawnEnemy(EnemyFactory factory, EnemyType enemyType)
    {
        var spawnPoint = _instance._gameBoard.GetSpawnPoint(Random.Range(0, _instance._gameBoard.SpawnPointCount));
        var enemy = factory.Get(enemyType);
        enemy.SpawnOn(spawnPoint);
        _instance._enemies.Add(enemy);
    }

    public static Shell SpawnShell()
    {
        var shell = _instance._warFactory.Shell;
        _instance._nonEnemies.Add(shell);
        return shell;
    }

    public static Arrow SpawnArrow()
    {
        var shell = _instance._warFactory.Arrow;
        _instance._nonEnemies.Add(shell);
        return shell;
    }

    public static Explosion SpawnExplosion()
    {
        var shell = _instance._warFactory.Explosion;
        _instance._nonEnemies.Add(shell);
        return shell;
    }

    public static void EnemyReachedDestination()
    {
        _instance.PlayerHealth--;
    }

    private async void BeginNewGame()
    {
        Cleanup();
        _tilesBuilder.Enable();
        PlayerHealth = _startingPlayerHealth;

        try
        {
            _prepareCancellation?.Dispose();
            _prepareCancellation = new CancellationTokenSource();
            if (await _prepareGamePanel.Prepare(_prepareTime, _prepareCancellation.Token))
            {
                _activeScenario = _scenario.Begin();
                _scenarioInProcess = true;
            }
        }
        catch (TaskCanceledException e) { }
    }

    public void Cleanup()
    {
        _tilesBuilder.Disable();
        _scenarioInProcess = false;
        _prepareCancellation?.Cancel();
        _prepareCancellation?.Dispose();
        _enemies.Clear();
        _nonEnemies.Clear();
        _gameBoard.Clear();
    }

    private void GoToMainMenu()
    {
        var operations = new Queue<ILoadingOperation>();
        operations.Enqueue(new ClearGameOperation(this));
        LoadingScreen.Instance.Load(operations);
    }
}
