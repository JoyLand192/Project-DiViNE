using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public event Action<Collider2D> OnEnter;
    public event Action<Collider2D> OnExit;
    void OnDestroy()
    {
        OnEnter = null;
        OnExit = null;
    }
    private void OnTriggerEnter2D(Collider2D other) => OnEnter?.Invoke(other);
    private void OnTriggerExit2D(Collider2D other) => OnExit?.Invoke(other);
}
