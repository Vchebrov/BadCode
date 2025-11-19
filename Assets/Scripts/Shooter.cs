using System.Collections;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private Bullet _bullet;
    [SerializeField] private Transform _target;

    private float _timeDelay = 1f;
    private WaitForSeconds _wait;

    private void Start()
    {
        _wait = new WaitForSeconds(_timeDelay);
        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        while (enabled)
        {
            Vector3 direction = (_target.position - transform.position).normalized;
            var bullet = Instantiate(_bullet, transform.position + direction, Quaternion.identity);
            bullet.Initialize(direction);

            yield return _wait;

            Destroy(bullet.gameObject);
        }
    }
}