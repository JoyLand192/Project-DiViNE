using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyInfo", menuName = "Project: DiViNE/Data/New EnemyInfo")]
public class EnemyInfo : ScriptableObject
{
    public float MoveSpeed;
    public float MaxHP;
    public float Strength;
    public float AggroRange;
    public float AttackRange;
    public Weapon EnemyWeapon;
    public EnemyLootInfo LootInfo;
}
