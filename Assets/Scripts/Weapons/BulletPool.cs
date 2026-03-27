using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class BulletPool : MonoBehaviour
{
    Dictionary<Bullet, Queue<Bullet>> bulletPools = new();
    public Bullet GetBullet(Bullet prefab, CRShooter shooter, float damage)
    {
        if (!bulletPools.ContainsKey(prefab)) bulletPools.Add(prefab, new Queue<Bullet>());

        var bullet = bulletPools[prefab].Count > 0 ? bulletPools[prefab].Dequeue() : Instantiate(prefab, transform);
        bullet.gameObject.SetActive(true);
        bullet.Hitable = true;
        bullet.Key = prefab;
        bullet.Damage = damage;
        bullet.Shooter = shooter;

        return bullet;
    }
    public void Return(Bullet bullet)
    {
        if (bullet == null || bullet.Key == null) return;

        if (!bulletPools.ContainsKey(bullet.Key)) bulletPools.Add(bullet.Key, new Queue<Bullet>());

        bullet.Reset();
        bullet.gameObject.SetActive(false);
        bulletPools[bullet.Key].Enqueue(bullet);
    }
}
