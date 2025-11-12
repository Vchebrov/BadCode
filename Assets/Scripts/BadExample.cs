using UnityEngine;

public class Patrol : MonoBehaviour
{
    [SerializeField] public Transform PlacePoint;
    [SerializeField] private float _speed = 10f;
    [SerializeField] private Transform[] Places;

    private int _index;

    private void Awake()
    {
        Places = new Transform[PlacePoint.childCount];
    }

    private void Start()
    {
        for (int i = 0; i < PlacePoint.childCount; i++)
        {
            Places[i] = PlacePoint.GetChild(i).GetComponent<Transform>();
        }
    }

    private void Update()
    {
        var _pointByNumberInArray = Places[_index];

        transform.position = Vector3.MoveTowards
        (
            transform.position,
            _pointByNumberInArray.position,
            _speed * Time.deltaTime
        );

        if (transform.position == _pointByNumberInArray.position)
        {
            GetNextPoint();
        }
    }

    private void GetNextPoint()
    {
        _index++;

        if (_index == Places.Length)
            _index = 0;
    }
}