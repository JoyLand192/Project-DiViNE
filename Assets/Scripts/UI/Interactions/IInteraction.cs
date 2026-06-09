using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteraction
{
    public Vector3 CurrentPosition { get; }
    public bool IsAvailable { get; }
    public bool IsDisplayingUI { get; set; }
    public bool IsDestroyed { get; set; }
    public void Interact(CR cr);
    public event System.Action<IInteraction> OnDestroyed;
}
