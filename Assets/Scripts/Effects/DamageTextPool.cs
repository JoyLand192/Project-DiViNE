using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextPool : MonoBehaviour
{
    [SerializeField] DamageText damageTextPrefab;
    Queue<DamageText> pool = new();
    public DamageText Get() => pool.Count > 0 ? pool.Dequeue() : Instantiate(damageTextPrefab, transform);
    public void Return(DamageText damage)
    {
        if (damage == null) return;

        damage.gameObject.SetActive(false);
        pool.Enqueue(damage);
    }
}
