using Colyseus;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MultiplayerManager : ColyseusManager<MultiplayerManager>
{
#region Server

    private const string GameRoomName = "state_handler";

    private ColyseusRoom<State> _room;
    private bool _addEnemy = true;

    public void Leave() => _room?.Leave();

    public void SendMessage(string key, Dictionary<string, object> data)
    {
        _room.Send(key, data);
    }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        InitializeClient();
        Connection();
    }

    private async void Connection()
    {
        _room = await client.JoinOrCreate<State>(GameRoomName);
        _room.OnStateChange += OnChange;
    }

    private void OnChange(State state, bool isFirstState)
    {
        if (!isFirstState) return;
        _room.OnStateChange -= OnChange;

        state.players.ForEach((key, player) =>
        {
            if (key == _room.SessionId) CreatePlayer(player);
            else CreateEnemy(key, player);
        });

        _addEnemy = false;
        _room.State.players.OnAdd(CreateEnemy);
        _room.State.players.OnRemove(RemoveEnemy);
        _addEnemy = true;
    }

    protected override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
        Leave();
    }

    #endregion

    #region Player
    [SerializeField] private PlayerAim _playerAim;
    [SerializeField] private Controller _controllerPrefab;
    [SerializeField] private Snake _snakePrefab;

    private void CreatePlayer(Player player)
    {
        Vector3 position = new Vector3(player.x, 0, player.z);
        Quaternion quaternion = Quaternion.identity;

        Snake snake = Instantiate(_snakePrefab, position, quaternion);
        snake.Init(player.d);

        var aim = Instantiate(_playerAim, position, quaternion);
        aim.Init(snake.Speed);

        Controller controller = Instantiate(_controllerPrefab);
        controller.Init(aim, player, snake);
    }

    #endregion

    #region Enemy

    Dictionary<string, EnemyController> _enemies = new Dictionary<string, EnemyController>();

    private void CreateEnemy(string key, Player player)
    {
        if (!_addEnemy) return;
        Vector3 position = new Vector3(player.x, 0, player.z);

        Snake snake = Instantiate(_snakePrefab, position, Quaternion.identity);
        snake.Init(player.d);

        var enemyController = snake.AddComponent<EnemyController>();
        enemyController.Init(snake, player);
        _enemies.Add(key, enemyController);
    }

    private void RemoveEnemy(string key, Player value)
    {
        if (!_enemies.ContainsKey(key))
        {
            Debug.LogError("Ошибка удаления");
            return;
        }

        EnemyController enemyController = _enemies[key];
        _enemies.Remove(key);
        enemyController.Destroy();
    }

    #endregion
}