using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class InstantiateBulletsShooting : MonoBehaviour
{
    [SerializeField] float _shootingDelay = 1f;
    [SerializeField] private float number;
    [SerializeField] private Transform _prefab;
    [SerializeField] private Transform Target;

    private WaitForSeconds _wait;

    private void Awake()
    {
        _wait = new WaitForSeconds(_shootingDelay);
    }
    
    private void Start()
    {
        StartCoroutine(_shootingWorker());
    }

    private IEnumerator _shootingWorker()
    {
        while (enabled)
        {
            var _vector3direction = (Target.position - transform.position).normalized;
            var NewBullet = Instantiate(_prefab, transform.position + _vector3direction, Quaternion.identity);

            NewBullet.GetComponent<Rigidbody>().transform.up = _vector3direction;
            NewBullet.GetComponent<Rigidbody>().velocity = _vector3direction * number;

            yield return _wait;
        }
    }
}