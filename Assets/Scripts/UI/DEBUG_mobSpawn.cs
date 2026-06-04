using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUG_mobSpawn : MonoBehaviour
{
    [SerializeField] Enemy enemyPrefab;
    [SerializeField] CR cr;
    public void Spawn(int count)
    {
        for (int i = 0; i < count; i++) Instantiate(enemyPrefab);
    }
    public void GiveHP(float value) => cr.Status.TakeHeal(value);
    public void FuckYourCoins(int value) => DataManager.Instance.CurrentCoin += value;
}
