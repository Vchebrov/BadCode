using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private Transform[] _wayPoints;

    private int _index = 0;
    private Transform _currentTarget;

    private void Start()
    {
        if (_wayPoints.Length == 0)
            return;

        if (_index < 0 || _index >= _wayPoints.Length)
        {
            _index = 0;
            return;
        }

        _currentTarget = _wayPoints[_index];
    }

    private void Update()
    {
        _currentTarget = _wayPoints[_index];

        transform.position = Vector3.MoveTowards(
            transform.position,
            _currentTarget.position,
            _speed * Time.deltaTime
        );

        if (transform.position == _currentTarget.position)
        {
            SelectNextPoint();
        }
    }

    private void SelectNextPoint()
    {
        _index = ++_index % _wayPoints.Length;
    }
}