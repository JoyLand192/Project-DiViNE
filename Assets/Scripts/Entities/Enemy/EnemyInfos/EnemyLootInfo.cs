using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[CreateAssetMenu(fileName = "EnemyLootInfo", menuName = "Project: DiViNE/Data/New EnemyLootInfo")]
public class EnemyLootInfo : ScriptableObject
{
    [Range(0, 100)] public float DropChance;
    public float MinCoinAmount;
    public float MaxCoinAmount;
    public List<Loot> Loots = new();
    public Loot Roll()
    {
        var dropped = Random.Range(0, 100f);
        if (dropped <= DropChance)
        {
            float total = Loots.Sum(l => l.Weight);
            var roll = Random.Range(0, total);
            foreach (var loot in Loots)
            {
                roll -= loot.Weight;
                if (roll < 0) return loot;
            }
        }
        return null;
    }
}
