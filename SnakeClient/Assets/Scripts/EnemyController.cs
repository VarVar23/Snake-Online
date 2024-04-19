using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Player _player;
    private Snake _snake;
    private Vector3 _position;

    public void Init(Snake snake, Player player)
    {
        _player = player;
        _snake = snake;
        player.OnChange(OnChange); // 9 минут, 6.1
        player.OnDChange(OnDChange); //
        player.OnXChange(OnXChange); //
        player.OnZChange(OnZChange); //
        player.OnColorIDChange(OnColorIdChange);
    }

    private void OnColorIdChange(int currentValue, int previousValue)
    {
        _snake.ChangeSkins(currentValue);
    }

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