using System;
using UnityEngine;

public class CollisionMonitor : MonoBehaviour
{
    public event Action<Transform> Touched;

    private void OnCollisionEnter(Collision other)
    {
        Touched?.Invoke(other.transform);
    }
}