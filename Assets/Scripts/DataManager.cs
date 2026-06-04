using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    static DataManager instance;
    public static DataManager Instance => instance;
    int currentGold;
    public int CurrentCoin
    {
        get => currentGold;
        set
        {
            var diff = value - currentGold;

            currentGold = value;
            OnGoldChanged?.Invoke((currentGold, diff));
        }
    }
    public event System.Action<(int value, int diff)> OnGoldChanged;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);  
        }
        else Destroy(gameObject);
    }
}
