using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class LootDrops
{
    const string CoinPrefabsDirectoryLocation = "ItemPrefabs/VCoins";
    const string CoinGainEffectsDirectoryLocation = "ItemPrefabs/VCoinsGainEffect";
    const string WeaponDropPrefabLocation = "ItemPrefabs/WeaponDrop/ItemDrop";
    const int MaxCoinValue = 20;
    static List<Coin> CoinPrefabs;
    static List<ParticleSystem> CoinGainEffects;
    public static WeaponDrop WeaponDropPrefab { get; private set; }
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void LoadPrefabs()
    {
        CoinPrefabs = Resources.LoadAll<Coin>(CoinPrefabsDirectoryLocation)
            .OrderBy(c => c.gameObject.name)
            .ToList();
        CoinGainEffects = Resources.LoadAll<ParticleSystem>(CoinGainEffectsDirectoryLocation)
            .OrderBy(c => c.gameObject.name)
            .ToList();
        WeaponDropPrefab = Resources.Load<WeaponDrop>(WeaponDropPrefabLocation);
    }
    public static Coin GetCoinObjectByIndex(int index) => CoinPrefabs[Mathf.Clamp(index, 0, CoinPrefabs.Count - 1)];
    public static Coin GetCoinObject(float coinValue)
    {
        var divider = MaxCoinValue / (float)(CoinPrefabs.Count - 1);
        int index = Mathf.FloorToInt(coinValue / divider);

        return GetCoinObjectByIndex(index);
    }
    public static ParticleSystem GetCoinEffectByIndex(int index) => CoinGainEffects[Mathf.Clamp(index, 0, CoinGainEffects.Count - 1)];
    public static ParticleSystem GetCoinEffect(float coinValue)
    {
        var divider = MaxCoinValue / (float)(CoinGainEffects.Count - 1);
        int index = Mathf.FloorToInt(coinValue / divider);

        return GetCoinEffectByIndex(index);
    }
}
