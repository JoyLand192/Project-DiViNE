using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public abstract EntityMovement Movement { get; }
    public abstract EntityStatus Status { get; }
    public abstract SpriteRenderer Graphic { get; }
}
