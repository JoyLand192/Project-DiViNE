using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRShooter : MonoBehaviour
{
    [SerializeField] protected BulletPool bulletPool;
    [SerializeField] protected SpriteRenderer weaponGraphic;
    [SerializeField] protected Bullet testBulletPrefab_T;
    protected bool isShootable;
    public bool IsShootable
    {
        get => isShootable;
        set => isShootable = value;
    }
    protected void Update()
    {
        if (Input.GetMouseButtonDown(0)) Shoot();
    }
    protected void Shoot()
    {
        var bullet = Instantiate(testBulletPrefab_T);
        var direction = (Vector2)(Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        var normalizedDirection = direction.normalized;

        bullet.Launch(transform.position, normalizedDirection, 25, 0.5f);
    }
}
