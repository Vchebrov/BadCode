using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Rigidbody))]
public class Shooter : MonoBehaviour
{
    [SerializeField] float _shootingDelay = 0.5f;
    [SerializeField] private float _shootingForce = 50f;
    [SerializeField] private Transform _bulletPrefab;
    [SerializeField] private Transform _target;
    [SerializeField] private CollisionMonitor _collisionMonitor;

    private WaitForSeconds _wait;
    private ObjectPool<Transform> _pool;

    private void Awake()
    {
        _wait = new WaitForSeconds(_shootingDelay);

        _pool = new ObjectPool<Transform>(
            createFunc: () => Instantiate(_bulletPrefab),
            actionOnGet: (bullet) => InitializeGetAction(bullet),
            actionOnRelease: (bullet) => bullet.gameObject.SetActive(false),
            actionOnDestroy: Destroy,
            collectionCheck: true,
            defaultCapacity: 10,
            maxSize: 16
        );
    }

    private void Start()
    {
        StartCoroutine(ShootingLoop());
    }

    private void OnEnable()
    {
        if (_collisionMonitor != null)
        {
            _collisionMonitor.Touched += OnReleaseBullet;
        }
        else
        {
            Debug.LogError("CollisionMonitor стены не назначен в инспекторе");
        }
    }

    private void OnDisable()
    {
        _collisionMonitor.Touched -= OnReleaseBullet;
    }

    private IEnumerator ShootingLoop()
    {
        while (enabled)
        {
            if (_target != null)
            {
                _pool.Get();
            }

            yield return _wait;
        }
    }

    private void InitializeGetAction(Transform bullet)
    {
        bullet.position = transform.position;

        bullet.gameObject.SetActive(true);

        StartCoroutine(BulletMovement(bullet));
    }

    private IEnumerator BulletMovement(Transform bullet)
    {
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        Vector3 direction = (_target.position - transform.position).normalized;

        bullet.forward = direction;
        rb.velocity = direction * _shootingForce;

        yield return _wait;
    }

    private void OnReleaseBullet(Transform bullet)
    {
        _pool.Release(bullet);
    }
}