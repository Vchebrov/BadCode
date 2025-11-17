using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Rigidbody))]
public class Shootr : MonoBehaviour
{
    [SerializeField] float _shootingDelay = 0.5f;
    [SerializeField] private float _shootingForce = 10f;
    [SerializeField] private Transform _bulletPrefab;
    [SerializeField] private Transform _target;
    [SerializeField] private float _bulletSpawnOffset = 2f; // Отступ от стрелка

    private WaitForSeconds _wait;
    private ObjectPool<Transform> _pool;
    private Collider _shooterCollider; // Коллайдер стрелка

    private void Awake()
    {
        _wait = new WaitForSeconds(_shootingDelay);
        _shooterCollider = GetComponent<Collider>(); // Получаем коллайдер стрелка
        
        _pool = new ObjectPool<Transform>(
            createFunc: () => Instantiate(_bulletPrefab),
            actionOnGet: (bullet) => InitializeGetAction(bullet),
            actionOnRelease: (bullet) => 
            {
                bullet.gameObject.SetActive(false);
                // Сбрасываем игнор коллизий при возврате в пул
                if (bullet.TryGetComponent<Collider>(out var bulletCollider))
                {
                    Physics.IgnoreCollision(_shooterCollider, bulletCollider, false);
                }
            },
            actionOnDestroy: (bullet) => Destroy(bullet.gameObject),
            collectionCheck: true,
            defaultCapacity: 10,
            maxSize: 16
        );
    }

    private void Start()
    {
        StartCoroutine(ShootingLoop());
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
        // РАССЧИТЫВАЕМ НАПРАВЛЕНИЕ И ПОЗИЦИЮ
        Vector3 direction = (_target.position - transform.position).normalized;
        Vector3 spawnPosition = transform.position + direction * _bulletSpawnOffset;
        
        // ПЕРЕМЕЩАЕМ ПУЛЮ С ОТСТУПОМ ОТ СТРЕЛКА
        bullet.position = spawnPosition;
        
        // АКТИВИРУЕМ ПУЛЮ
        bullet.gameObject.SetActive(true);
        
        // ИГНОРИРУЕМ КОЛЛИЗИИ МЕЖДУ ПУЛЕЙ И СТРЕЛКОМ
        if (bullet.TryGetComponent<Collider>(out var bulletCollider))
        {
            Physics.IgnoreCollision(_shooterCollider, bulletCollider, true);
        }
        
        // ЗАПУСКАЕМ ДВИЖЕНИЕ ПУЛИ
        StartCoroutine(BulletMovement(bullet, direction));
    }
    
    private IEnumerator BulletMovement(Transform bullet, Vector3 direction)
    {
        if (_target == null || bullet == null) yield break;
        
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb == null) yield break;
        
        // НАСТРАИВАЕМ НАПРАВЛЕНИЕ ПУЛИ
        bullet.forward = direction;
        
        // ИСПОЛЬЗУЕМ AddForce ИЛИ velocity ДЛЯ РЕЗКОГО СТАРТА
        rb.velocity = direction * _shootingForce;
        
        // ДОБАВЛЯЕМ КОРУТИНУ ДЛЯ ВОЗВРАТА ПУЛИ
        yield return new WaitForSeconds(5f);
        
        if (bullet.gameObject.activeInHierarchy)
        {
            _pool.Release(bullet);
        }
    }

    public void ReturnBulletToPool(Transform bullet)
    {
        if (bullet != null && bullet.gameObject.activeInHierarchy)
        {
            _pool.Release(bullet);
        }
    }
}