using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [field: SerializeField] public ChangeSkins Skins { get; private set; }
    public float Speed { get => _speed; }

    [SerializeField] private MeshRenderer[] _headMeshRenderer;
    [SerializeField] private Tail _tailPrefab;
    [SerializeField] private Transform _head;
    [SerializeField] private float _speed;
    private Tail _tail;
    private List<MeshRenderer> _snakeMeshRenderers;

    public void Init(int detailCount, int colorID)
    {
        _tail = Instantiate(_tailPrefab, transform.position, Quaternion.identity);
        _tail.Init(_head, _speed, detailCount);

        _snakeMeshRenderers = new List<MeshRenderer>();

        for (int i = 0; i < _headMeshRenderer.Length; i++)
        {
            _snakeMeshRenderers.Add(_headMeshRenderer[i]);
        }

        for (int i = 0; i < detailCount + 1; i++)
        {
            for(int j = 0; j < _tail.Details[i].DetailMeshRenderers.Length; j++)
            {
                _snakeMeshRenderers.Add(_tail.Details[i].DetailMeshRenderers[j]);
            }
        }

        Skins.Init(_snakeMeshRenderers);
    }

    public void SetRotation(Vector3 pointToLook)
    {
        _head.LookAt(pointToLook);
    }

    public void ChangeSkins(int index)
    {
        Skins.SetSkin(index);
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

    private void Move()
    {
        transform.position += _head.forward * _speed * Time.deltaTime;
    }
}