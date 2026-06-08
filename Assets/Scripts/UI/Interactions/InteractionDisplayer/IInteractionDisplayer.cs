using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractionDisplayer : MonoBehaviour
{
    public abstract bool TweenExist { get; }
    public abstract void Initialize();
    public abstract void Pause();
    public abstract void Resume();
    public abstract void Stop();
}
