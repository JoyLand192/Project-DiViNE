using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityMovement : MonoBehaviour
{
    [SerializeField] protected bool isMovable;
    public bool IsMovable
    {
        get => isMovable;
        set => isMovable = value;
    }
}
