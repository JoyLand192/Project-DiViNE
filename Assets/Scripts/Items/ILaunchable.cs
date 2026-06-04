using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILaunchable
{
    public Tween CurrentLaunchTween { get; }
    public void Launch(float power, float duration);
}
