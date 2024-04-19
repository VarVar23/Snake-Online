using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private float _cameraOffsetY;
    [SerializeField] private Transform _cursor;
    private MultiplayerManager _multiplayerManager;
    private Camera _camera;
    private Snake _snake;
    private Player _player;
    private PlayerAim _playerAim;

    private Plane _plane;

    public void Init(PlayerAim aim, Player player, Snake snake)
    {
        _playerAim = aim;
        _player = player;
        _snake = snake;
        _camera = Camera.main;
        _plane = new Plane(Vector3.up, Vector3.zero);
        _multiplayerManager = MultiplayerManager.Instance;

        _snake.AddComponent<CameraManager>().Init(_cameraOffsetY);

        player.OnChange(OnChange); // 
        player.OnDChange(OnDChange); //
        player.OnXChange(OnXChange); //
        player.OnZChange(OnZChange); //
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            MoveCursor();
            _playerAim.SetTargetDirection(_cursor.position);
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            int random = Random.Range(0, _snake.Skins.GetCountMaterials());
            _snake.Skins.SetSkin(random);
            _snake.Skins.Send(random);
        }

        SendMove();
    }

    private void SendMove()
    {
        _playerAim.GetMoveInfo(out Vector3 position);

        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "x", position.x },
            { "z", position.z },
        };

        _multiplayerManager.SendMessage("move", data);
    }

    private void MoveCursor()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        _plane.Raycast(ray, out float distance);
        Vector3 point = ray.GetPoint(distance);

        _cursor.position = point;
    }

    private Vector3 _position;

    private void OnChange()//
    {
        _position = _snake.transform.position;
    }

    private void OnZChange(float currentValue, float previousValue)//
    {
        _position.z = currentValue;
        _snake.SetRotation(_position);
    }

    private void OnXChange(float currentValue, float previousValue)// 
    {
        _position.x = currentValue;
        _snake.SetRotation(_position);
    }

    private void OnDChange(byte currentValue, byte previousValue)//
    {
        _snake.SetDetailCount(currentValue);
    }

    public void Destroy()
    {
        _snake.Destroy();
    }
}