using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUG_mobSpawn : MonoBehaviour
{
    [SerializeField] Enemy enemyPrefab;
    [SerializeField] GameObject cr;
    public void Spawn() => Instantiate(enemyPrefab).SetTarget(cr);
}
