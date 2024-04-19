using UnityEngine;

public class Snake : MonoBehaviour
{
    public float Speed { get => _speed; }
    [SerializeField] private Tail _tailPrefab;
    [SerializeField] private Transform _head;
    [SerializeField] private float _speed;
    private Tail _tail;

    public void Init(int detailCount)
    {
        _tail = Instantiate(_tailPrefab, transform.position, Quaternion.identity);
        _tail.Init(_head, _speed, detailCount);
    }

    public void SetDetailCount(int detailCount)
    {
        _tail.SetDetailCount(detailCount);
    }

    public void Destroy()
    {
        _tail.Destroy();
        Destroy(gameObject);
    }

    private void Update()
    {
        Move();
    }

    public void SetRotation(Vector3 pointToLook)
    {
        _head.LookAt(pointToLook);
    }

    private void Move()
    {
        transform.position += _head.forward * _speed * Time.deltaTime;
    }
}