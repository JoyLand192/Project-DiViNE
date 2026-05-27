using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUG_mobSpawn : MonoBehaviour
{
    [SerializeField] Enemy enemyPrefab;
    [SerializeField] CR cr;
    public void Spawn()
    {
        for (int i = 0; i < 10; i++) Instantiate(enemyPrefab);
    }
    public void GiveHP(float value) => cr.Status.TakeHeal(value);
}
