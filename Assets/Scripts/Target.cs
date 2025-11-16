using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;

    [SerializeField] private Transform[] Places;

    private int _index = 0;

    private void Update()
    {
        if (Places.Length == 0) return;

        if (_index < 0 || _index >= Places.Length)
        {
            _index = 0;
            return;
        }

        var currentTarget = Places[_index];

        if (currentTarget == null)
        {
            GetNextPoint();
            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            currentTarget.position,
            _speed * Time.deltaTime
        );

        if (transform.position == currentTarget.position)
        {
            GetNextPoint();
        }
    }

    private void GetNextPoint()
    {
        _index++;

        if (_index >= Places.Length)
            _index = 0;
    }
}