using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] CR currentCR;
    public CR CurrentCR
    {
        get
        {
            if (currentCR == null) currentCR = FindAnyObjectByType<CR>();
            return currentCR;
        }
        set => currentCR = value;
    }
    [SerializeField] BulletPool bulletPool;
    public BulletPool BulletPool
    {
        get
        {
            if (bulletPool == null) bulletPool = FindAnyObjectByType<BulletPool>();
            return bulletPool;
        }
    }
    [SerializeField] DamageTextPool dtPool;
    public DamageTextPool DamageTextPool
    {
        get
        {
            if (dtPool == null) dtPool = FindAnyObjectByType<DamageTextPool>();
            return dtPool;
        }
    }
    public event System.Action<EnemyInfo> OnEnemyDeath;
    public void EnemyDeath(EnemyInfo info) => OnEnemyDeath?.Invoke(info);
}
