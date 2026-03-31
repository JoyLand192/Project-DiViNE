using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] DamageTextPool dtPool;
    float cd = 0.3f;
    float timer;
    void Update()
    {
        if (timer > 0) timer -= Time.deltaTime;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("CR") && timer <= 0)
        {
            timer = cd;
            collision.gameObject.GetComponent<CRStatus>().TakeDamage(damage, dtPool);
        }
    }
}
