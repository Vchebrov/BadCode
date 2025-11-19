using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float _shootingForce = 50f;
    private Rigidbody _rigidbody;
    private WaitForSeconds _wait;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Initialize(Vector3 direction)
    {
        _rigidbody.velocity = direction * _shootingForce;
        transform.up = direction;
    }   
}
