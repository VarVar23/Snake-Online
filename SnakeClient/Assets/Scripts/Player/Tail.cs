using System.Collections.Generic;
using UnityEngine;

public class Tail : MonoBehaviour
{
    [SerializeField] public Detail _detailPrefab;
    [SerializeField] private float _detailDistance;
    public List<Detail> Details { get; private set; } = new List<Detail>();

    private Transform _head;

    private List<Vector3> _positionHistory = new List<Vector3>();
    private List<Quaternion> _rotationHistory = new List<Quaternion>();
    private float _snakeSpeed;

    public void Init(Transform head, float speed, int detailCount)
    {
        _head = head;
        _snakeSpeed = speed;
        Details.Add(GetComponent<Detail>());
        _positionHistory.Add(_head.position);
        _rotationHistory.Add(_head.rotation);
        _positionHistory.Add(transform.position);
        _rotationHistory.Add(transform.rotation);

        SetDetailCount(detailCount);
    }

    public void SetDetailCount(int detailCount)
    {
        if (detailCount == Details.Count - 1) return;

        int diff = (Details.Count - 1) - detailCount;

        if(diff < 1)
        {
            for(int i = 0; i < -diff; i++)
            {
                AddDetail();
            }
        }
        else
        {
            for (int i = 0; i < diff; i++)
            {
                RemoveDetail();
            }
        }
    }

    private void AddDetail()
    {
        Vector3 position = Details[Details.Count - 1].transform.position;
        Quaternion rotation = Details[Details.Count - 1].transform.rotation;
        var detail = Instantiate(_detailPrefab, position, rotation);
        Details.Insert(0, detail);
        _positionHistory.Add(position);
        _rotationHistory.Add(rotation);
    }

    private void RemoveDetail()
    {
        if(Details.Count <= 1 )
        {
            Debug.LogError("Нет детали для удаления");
            return;
        }

        var detail = Details[0];
        Details.Remove(detail);
        Destroy(detail.gameObject);
        _positionHistory.RemoveAt(_positionHistory.Count - 1);
        _rotationHistory.RemoveAt(_rotationHistory.Count - 1);
    }

    private void Update()
    {
        float distance = (_head.position - _positionHistory[0]).magnitude;

        while (distance > _detailDistance)
        {
            Vector3 direction = (_head.position - _positionHistory[0]).normalized;

            _positionHistory.Insert(0, _positionHistory[0] + direction * _detailDistance);
            _positionHistory.RemoveAt(_positionHistory.Count - 1);

            _rotationHistory.Insert(0, _head.rotation);
            _rotationHistory.RemoveAt(_rotationHistory.Count - 1);

            distance -= _detailDistance;
        }


        for(int i = 0; i < Details.Count; i++)
        {
            float percent = distance / _detailDistance;
            Details[i].transform.position = Vector3.Lerp(_positionHistory[i + 1], _positionHistory[i], percent);
            Details[i].transform.rotation = Quaternion.Lerp(_rotationHistory[i + 1], _rotationHistory[i], percent);
        }
    }

    public void Destroy()
    {
        for(int i = 0; i < Details.Count;i++)
        {
            Destroy(Details[i].gameObject);
        }
    }
}